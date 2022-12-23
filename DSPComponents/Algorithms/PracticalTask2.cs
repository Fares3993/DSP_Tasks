﻿﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DSPAlgorithms.Algorithms
{
    public class PracticalTask2 : Algorithm
    {
        public String SignalPath { get; set; }
        public float Fs { get; set; }
        public float miniF { get; set; }
        public float maxF { get; set; }
        public float newFs { get; set; }
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            Signal InputSignal = LoadSignal(SignalPath);
            FIR bpf =new FIR();
            bpf.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.BAND_PASS;
            bpf.InputTimeDomainSignal = InputSignal;
            bpf.InputTransitionBand = 500;
            bpf.InputStopBandAttenuation = 50;
            bpf.InputFS = Fs;
            bpf.InputF1 = miniF;
            bpf.InputF2 = maxF;
            bpf.Run();
            Signal Filtered_Signal = bpf.OutputYn;
            Sampling Resample = new Sampling();
            DC_Component DC_Remover = new DC_Component();
            Signal Resampled_Signal;
            if (newFs>=2*maxF)
            {
                Resample.InputSignal = Filtered_Signal;
                Resample.M = M;
                Resample.L = L;
                Resample.Run();

                Resampled_Signal = Resample.OutputSignal;
                DC_Remover.InputSignal = Resampled_Signal;
                //DC_Remover.Run();
            }
            else
            {
                Console.WriteLine("newFs is not valid");
                DC_Remover.InputSignal= Filtered_Signal;
                //DC_Remover.Run();
            }
            DC_Remover.Run();
            Signal Removed_DC = DC_Remover.OutputSignal;
            Normalizer Normalizer = new Normalizer();
            Normalizer.InputSignal = Removed_DC;
            Normalizer.InputMaxRange = 1;
            Normalizer.InputMinRange = -1;
            Normalizer.Run();

            Signal Normalized_Signal = Normalizer.OutputNormalizedSignal;
            DiscreteFourierTransform DFT = new DiscreteFourierTransform();
            DFT.InputTimeDomainSignal = Normalized_Signal;
            DFT.InputSamplingFrequency = newFs;
            DFT.Run();
            OutputFreqDomainSignal = DFT.OutputFreqDomainSignal;

        }

        public Signal LoadSignal(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(stream);

            var sigType = byte.Parse(sr.ReadLine());
            var isPeriodic = byte.Parse(sr.ReadLine());
            long N1 = long.Parse(sr.ReadLine());

            List<float> SigSamples = new List<float>(unchecked((int)N1));
            List<int> SigIndices = new List<int>(unchecked((int)N1));
            List<float> SigFreq = new List<float>(unchecked((int)N1));
            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));

            if (sigType == 1)
            {
                SigSamples = null;
                SigIndices = null;
            }

            for (int i = 0; i < N1; i++)
            {
                if (sigType == 0 || sigType == 2)
                {
                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
                }
                else
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            if (!sr.EndOfStream)
            {
                long N2 = long.Parse(sr.ReadLine());

                for (int i = 0; i < N2; i++)
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            stream.Close();
            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
        }
    }
}
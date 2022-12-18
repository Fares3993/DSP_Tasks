using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }
        public List<Complex> Convolved = new List<Complex>();
        public List<float> Amplitudes = new List<float>();
        public List<float> PhaseShifts = new List<float>();
        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            Signal temp_signal1 = new Signal(
                new List<float>(InputSignal1.Samples),
                new List<int>(InputSignal1.SamplesIndices),
                InputSignal1.Periodic && true);
            Signal temp_signal2 = new Signal(
                new List<float>(InputSignal2.Samples),
                new List<int>(InputSignal2.SamplesIndices),
                InputSignal2.Periodic && true);

            int l = temp_signal1.Samples.Count + temp_signal2.Samples.Count - 1;

            for (int idx = temp_signal1.Samples.Count; idx < l; idx++)
            {
                temp_signal1.Samples.Add(0);
            }
            for (int idx = temp_signal2.Samples.Count; idx < l; idx++)
            {
                temp_signal2.Samples.Add(0);
            }

            DiscreteFourierTransform DFT1 = new DiscreteFourierTransform();
            DFT1.InputTimeDomainSignal = temp_signal1;
            DFT1.Run();
            List<float> Amplitudes1 = new List<float>(DFT1.OutputFreqDomainSignal.FrequenciesAmplitudes);
            List<float> PhaseShifts1 = new List<float>(DFT1.OutputFreqDomainSignal.FrequenciesPhaseShifts);

            DiscreteFourierTransform DFT2 = new DiscreteFourierTransform();
            DFT2.InputTimeDomainSignal = temp_signal2;
            DFT2.Run();
            List<float> Amplitudes2 = new List<float>(DFT2.OutputFreqDomainSignal.FrequenciesAmplitudes);
            List<float> PhaseShifts2 = new List<float>(DFT2.OutputFreqDomainSignal.FrequenciesPhaseShifts);
            double Real1, Imaginary1, Real2, Imaginary2;
            for (int i = 0; i < DFT1.OutputFreqDomainSignal.Samples.Count; i++)
            {
                Real1 = Amplitudes1[i] * Math.Cos(PhaseShifts1[i]);
                Imaginary1 = Amplitudes1[i] * Math.Sin(PhaseShifts1[i]);
                Real2 = Amplitudes2[i] * Math.Cos(PhaseShifts2[i]);
                Imaginary2 = Amplitudes2[i] * Math.Sin(PhaseShifts2[i]);

                Convolved.Add(Complex.Multiply( new Complex(Real1,Imaginary1) , new Complex(Real2, Imaginary2)));
                Amplitudes.Add((float)Convolved[i].Magnitude);
                PhaseShifts.Add((float)Convolved[i].Phase);
            }
            InverseDiscreteFourierTransform IDFT = new InverseDiscreteFourierTransform();
            IDFT.InputFreqDomainSignal = new Signal(false,new List<float>(),new List<float>(Amplitudes), new List<float>(PhaseShifts));
            IDFT.Run();

            OutputConvolvedSignal = IDFT.OutputTimeDomainSignal;

            int start = InputSignal1.SamplesIndices[0] + InputSignal2.SamplesIndices[0];
            List<int> indices = new List<int>();

            OutputConvolvedSignal.SamplesIndices.ForEach(x => indices.Add(start++));
            OutputConvolvedSignal.SamplesIndices = indices;
        }
    }
}

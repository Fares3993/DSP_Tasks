using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }
        public List<Complex> Convolved = new List<Complex>();
        public List<float> Amplitudes = new List<float>();
        public List<float> PhaseShifts = new List<float>();
        public override void Run()
        {
            List<float> s1 = InputSignal1.Samples;
            List<float> s2;
            float p = 0;
            OutputNormalizedCorrelation = new List<float>();
            if (InputSignal2 == null)
            {
                InputSignal2 = InputSignal1;
                
            }
            s2 = InputSignal2.Samples;
            float p1 = 0;
            float p2 = 0;

            for (int i = 0; i < s1.Count; i++)
            {
                p1 += (float)Math.Pow(s1[i], 2);
                p2 += (float)Math.Pow(s2[i], 2);
            }

            p += p1 * p2;
            p = (float)Math.Sqrt(p);
            p /= s1.Count;
            DiscreteFourierTransform DFT1 = new DiscreteFourierTransform();
            DFT1.InputTimeDomainSignal = InputSignal1;
            DFT1.Run();
            List<float> Amplitudes1 = new List<float>(DFT1.OutputFreqDomainSignal.FrequenciesAmplitudes);
            List<float> PhaseShifts1 = new List<float>(DFT1.OutputFreqDomainSignal.FrequenciesPhaseShifts);

            DiscreteFourierTransform DFT2 = new DiscreteFourierTransform();
            DFT2.InputTimeDomainSignal = InputSignal2;
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

                Convolved.Add(Complex.Multiply(Complex.Conjugate( new Complex(Real1,Imaginary1)) , new Complex(Real2, Imaginary2)));
                Amplitudes.Add((float)Convolved[i].Magnitude);
                PhaseShifts.Add((float)Convolved[i].Phase);
            }
            InverseDiscreteFourierTransform IDFT = new InverseDiscreteFourierTransform();
            IDFT.InputFreqDomainSignal = new Signal(false, new List<float>(), new List<float>(Amplitudes), new List<float>(PhaseShifts));
            IDFT.Run();
            for(int i = 0; i < DFT2.OutputFreqDomainSignal.Samples.Count;i++)
            {
                IDFT.OutputTimeDomainSignal.Samples[i] /= DFT2.OutputFreqDomainSignal.Samples.Count;
                OutputNormalizedCorrelation.Add(IDFT.OutputTimeDomainSignal.Samples[i] / p);
            }
            OutputNonNormalizedCorrelation = IDFT.OutputTimeDomainSignal.Samples;
            
        }
    }
}
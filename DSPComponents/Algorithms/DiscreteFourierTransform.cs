using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;
using System.IO;
namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }
        
        public override void Run()
        {

            OutputFreqDomainSignal = new Signal(InputTimeDomainSignal.Samples, false);
            OutputFreqDomainSignal.FrequenciesAmplitudes = new List<float>();
            OutputFreqDomainSignal.FrequenciesPhaseShifts = new List<float>();
            OutputFreqDomainSignal.Frequencies=new List<float>();
            int N_Samples = InputTimeDomainSignal.Samples.Count;
            float Xn;
            double A , PhaseShift;
            Complex e, j;
            Complex[] X = new Complex[N_Samples];
            for (int k = 0; k < N_Samples; k++)
            {
                for (int n = 0; n < N_Samples; n++)
                {
                    Xn = InputTimeDomainSignal.Samples[n];
                    j = Complex.ImaginaryOne;
                    e = Complex.Exp(-j * 2 * Math.PI * k * n / N_Samples);
                    X[k] += Xn * e;
                }
  
                A = (float)X[k].Magnitude;
                OutputFreqDomainSignal.FrequenciesAmplitudes.Add((float)A);

                PhaseShift = (float)X[k].Phase;
                OutputFreqDomainSignal.FrequenciesPhaseShifts.Add((float)PhaseShift);
                OutputFreqDomainSignal.Frequencies.Add((float)Math.Round(2*Math.PI*InputSamplingFrequency)/N_Samples);
            } 
        }
    }
}
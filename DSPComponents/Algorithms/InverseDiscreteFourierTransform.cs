using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.IO;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }



        List<float> Amplitudes = new List<float>();
        List<float> PhaseShifts = new List<float>();
        List<float> X = new List<float>();
        public override void Run()
        {
            int N_Frequencies = InputFreqDomainSignal.FrequenciesAmplitudes.Count;
            Amplitudes = new List<float>(InputFreqDomainSignal.FrequenciesAmplitudes);
            PhaseShifts = new List<float>(InputFreqDomainSignal.FrequenciesPhaseShifts);
            for (int n = 0; n < N_Frequencies; n++)
            {
                Complex Xn = 0;
                for (int k = 0; k < N_Frequencies; k++)
                {
                    double Real = Amplitudes[k] * Math.Cos(PhaseShifts[k]);
                    double Imaginary = Amplitudes[k] * Math.Sin(PhaseShifts[k]);
                    Complex Xk = new Complex(Real, Imaginary);
                    Complex j = Complex.ImaginaryOne;
                    Complex e = Complex.Exp(j * 2 * Math.PI * n * k / N_Frequencies);
                    Xn += Xk * e;
                }
                Xn = Xn / N_Frequencies;
                X.Add((float)Xn.Real);
            }
            OutputTimeDomainSignal = new Signal(X, false);
        }
    }
}

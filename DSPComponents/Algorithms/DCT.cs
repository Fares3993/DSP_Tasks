using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }
        public override void Run()
        {
            List<float> out_samples = new List<float>();
            int N = InputSignal.Samples.Count;
            double T1, T2, T3;
            for (int k = 0; k < N ;k++)
            {
                double Sum = 0;
                for (int n = 0; n < N; n++)
                {
                     T1 = Math.PI / (4 * N);
                     T2 = 2 * n - 1;
                     T3 = 2 * k - 1;
                    Sum += InputSignal.Samples[n] * Math.Cos(T1* T2 * T3);
                }
                out_samples.Add( (float)(Math.Sqrt(2.0f/N)*Sum));
            }
            OutputSignal=new Signal(out_samples,false);
        }
    }
}

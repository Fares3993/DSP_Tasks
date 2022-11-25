using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();

            List<float> s1 = InputSignal1.Samples;
            List<float> s2;
            float p = 0;

            if (InputSignal2 == null)
            {
                for (int i = 0; i < s1.Count; i++)
                {
                    p += (float) Math.Pow(s1[i], 2);
                }

                p /= s1.Count;

                s2 = new List<float>(InputSignal1.Samples);
            }
            else
            {
                s2 = InputSignal2.Samples;  

                float p1 = 0;
                float p2 = 0;

                for (int i = 0; i < s1.Count; i++)
                {
                    p1 += (float)Math.Pow(s1[i], 2);
                    p2 += (float)Math.Pow(s2[i], 2);
                }

                p += p1 * p2;
                p = (float) Math.Sqrt(p);
                p /= s1.Count;
            }

            for (int i = 0; i < s1.Count; i++)
            {
                OutputNonNormalizedCorrelation.Add(0);

                for (int j = 0; j < s1.Count; j++)
                {
                    OutputNonNormalizedCorrelation[i] += s1[j] * s2[j];
                }

                OutputNonNormalizedCorrelation[i] /= s1.Count;
                OutputNormalizedCorrelation.Add(OutputNonNormalizedCorrelation[i] / p);

                if (InputSignal1.Periodic)
                    s2.Add(s2[0]);
                else
                    s2.Add(0);

                s2.RemoveAt(0);
            }
        }
    }
}
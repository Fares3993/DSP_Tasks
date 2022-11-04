using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class SinCos: Algorithm
    {
        public string type { get; set; }
        public float A { get; set; }
        public float PhaseShift { get; set; }
        public float AnalogFrequency { get; set; }
        public float SamplingFrequency { get; set; }
        public List<float> samples { get; set; }
        public override void Run()
        {
            samples = new List<float>();
            if (type.Equals("sin"))
            {
                for (int n = 0; n < SamplingFrequency; n++)
                {
                    float Xn = A * (float)Math.Sin((2 * Math.PI * (AnalogFrequency / SamplingFrequency)) * n + PhaseShift);
                    samples.Add(Xn);
                }
            }
            if (type.Equals("cos"))
            {
                for (int n = 0; n < SamplingFrequency; n++)
                {
                    float value = A * (float)Math.Cos((2 * Math.PI * (AnalogFrequency / SamplingFrequency)) * n + PhaseShift);
                    samples.Add(value);
                }
            }

        }
    }
}

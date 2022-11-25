using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class TimeDelay:Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public float InputSamplingPeriod { get; set; }
        public float OutputTimeDelay { get; set; }

        public override void Run()
        {
            DirectCorrelation corr = new DirectCorrelation();
            
            corr.InputSignal1 = InputSignal1;
            corr.InputSignal2 = InputSignal2;
            corr.Run();
            List<float> values = corr.OutputNormalizedCorrelation;

            float SamplingRate = 1 / InputSamplingPeriod;
            float max = float.MinValue;

            for (int i = 0; i < SamplingRate; i++)
            {
                if (values[i] > max)
                {
                    max = values[i];
                    OutputTimeDelay = i;
                }
            }
        }
    }
}

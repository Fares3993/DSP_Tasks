using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Normalizer : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputMinRange { get; set; }
        public float InputMaxRange { get; set; }
        public Signal OutputNormalizedSignal { get; set; }

        public override void Run()
        {
            List<float> outSamples = new List<float>();
            float oldMin = float.PositiveInfinity;
            float oldMax = float.NegativeInfinity;

            InputSignal.Samples.ForEach(sample => {
                if (sample < oldMin) oldMin = sample;
                if (sample > oldMax) oldMax = sample;
            });

            InputSignal.Samples.ForEach(sample => {
                outSamples.Add(
                        (sample - oldMin) / (oldMax - oldMin) * (InputMaxRange - InputMinRange) + InputMinRange
                    );
            });

            OutputNormalizedSignal = new Signal(outSamples, false);
        }
    }
}

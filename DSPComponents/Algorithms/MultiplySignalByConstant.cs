using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MultiplySignalByConstant : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputConstant { get; set; }
        public Signal OutputMultipliedSignal { get; set; }

        public override void Run()
        {
            List<float> outSamples = new List<float>();

            InputSignal.Samples.ForEach(sample => outSamples.Add(InputConstant * sample));

            OutputMultipliedSignal = new Signal(outSamples, false);
        }
    }
}

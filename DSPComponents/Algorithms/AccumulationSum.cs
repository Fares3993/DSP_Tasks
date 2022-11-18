using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;


namespace DSPAlgorithms.Algorithms
{
    public class AccumulationSum : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            List<float> inputSamples = InputSignal.Samples;
            List<float> outSamples = new List<float>();

            for (int i = 0; i < inputSamples.Count; i++)
            {
                float o = 0;
                
                for (int j = 0; j < i + 1; j++)
                {
                    o += inputSamples[j];
                }

                outSamples.Add(o);
            }

            OutputSignal = new Signal(outSamples, false);
        }
    }
}

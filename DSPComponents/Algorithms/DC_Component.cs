using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DC_Component: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            float sum = 0, mean = 0;
            List<float> Out = new List<float>();
            foreach (float sample in InputSignal.Samples)
            {
                sum += sample;
            }
            mean = sum / InputSignal.Samples.Count;
            foreach (float sample in InputSignal.Samples)
            {
                Out.Add(sample - mean);
            }
            OutputSignal = new Signal(Out, InputSignal.Periodic);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Adder : Algorithm
    {
        public List<Signal> InputSignals { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            int max = 0;
            List<float> output = new List<float>();

            foreach (Signal signal in InputSignals)
            {
                if (signal.Samples.Count > max)
                    max = signal.Samples.Count;
            }
            {
                //if (InputSignals[0].Samples.Count > InputSignals[1].Samples.Count)
                //{
                //    max = InputSignals[0].Samples.Count;
                //}
                //else
                //{
                //    max = InputSignals[1].Samples.Count;
                //}
            }
            for (int i = 0; i < max; i++)
            {

                output.Add(0);
                foreach (Signal signal in InputSignals)
                {
                    if (signal.Samples.Count < max) signal.Samples.Add(0);
                    output[i] += signal.Samples[i];
                }
                {
                    //if (InputSignals[0].Samples.Count < max)
                    //    InputSignals[0].Samples[i] = 0;
                    //if (InputSignals[1].Samples.Count < max)
                    //    InputSignals[1].Samples[i] = 0;

                    //output.Add(InputSignals[0].Samples[i] + InputSignals[1].Samples[i] );
                }
            }


            OutputSignal = new Signal(output, false);

        }
    }
}
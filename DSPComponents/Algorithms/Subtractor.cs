using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Subtractor : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputSignal { get; set; }

        /// <summary>
        /// To do: Subtract Signal2 from Signal1 
        /// i.e OutSig = Sig1 - Sig2 
        /// </summary>
        public override void Run()
        {
            {
                //int max;
                //List<float> output = new List<float>();

                //if (InputSignal1.Samples.Count > InputSignal2.Samples.Count)
                //{
                //    max = InputSignal1.Samples.Count;
                //}
                //else
                //{
                //    max = InputSignal2.Samples.Count;
                //}

                //for (int i = 0; i < max; i++)
                //{
                //    if (InputSignal1.Samples.Count < max)
                //        InputSignal1.Samples[i] = 0;
                //    if (InputSignal2.Samples.Count < max)
                //        InputSignal2.Samples[i] = 0;

                //    output.Add(InputSignal1.Samples[i] - InputSignal2.Samples[i]);
                //}
                //OutputSignal = new Signal(output, false);
            }
            MultiplySignalByConstant multiplySignal = new MultiplySignalByConstant();
            multiplySignal.InputSignal = InputSignal2;
            multiplySignal.InputConstant = -1;
            multiplySignal.Run();

            Adder adder = new Adder();
            adder.InputSignals = new List<Signal>();
            adder.InputSignals.Add(InputSignal1);
            adder.InputSignals.Add(multiplySignal.OutputMultipliedSignal);
            adder.Run();

            OutputSignal = adder.OutputSignal;

        }
    }
}
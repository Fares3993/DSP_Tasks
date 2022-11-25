using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()

        {

            int start = 0;
            int size = InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1;

            List<float> samples = new List<float>();
            List<int> indices = new List<int>();

            if (InputSignal1.SamplesIndices[0] < InputSignal2.SamplesIndices[0])
            {
                start = InputSignal1.SamplesIndices[0];
            }

            if (InputSignal2.SamplesIndices[0] < 0)
            {
                for (int i = InputSignal2.SamplesIndices[0]; i < 0; i++)
                {
                    InputSignal1.Samples.RemoveAt(InputSignal1.Samples.Count - 1);
                    InputSignal1.SamplesIndices.RemoveAt(InputSignal1.SamplesIndices.Count - 1);
                    start--;
                    size--;
                }
            }

            for (int i = 0; i < size; i++)
            {
                float sample = 0;
                for (int j = 0; j < InputSignal1.Samples.Count; j++)
                {
                    try
                    {
                        sample += InputSignal1.Samples[j] * InputSignal2.Samples[i - j];
                    }
                    catch
                    {
                        sample += 0;
                    }
                }
                indices.Add(start + i);
                samples.Add(sample);
            }



            OutputConvolvedSignal = new Signal(samples, indices, false);

        }
    }
}

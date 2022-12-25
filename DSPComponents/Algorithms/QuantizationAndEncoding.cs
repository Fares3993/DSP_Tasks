using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }

        public override void Run()
        {
            int levels, numOfBits;
            float delta;
            float max = float.NegativeInfinity, min = float.PositiveInfinity;
            Dictionary<float, float> intervals = new Dictionary<float, float>();
            List<float> outSamples = new List<float>();

            if (InputLevel > 0)
            {
                levels = InputLevel;
                numOfBits = (int)Math.Log(InputLevel, 2);
            }
            else
            {
                levels = (int)Math.Pow(2, InputNumBits);
                numOfBits = InputNumBits;
            }

            InputSignal.Samples.ForEach(sample =>
            {
                if (sample > max) max = sample;
                if (sample < min) min = sample;
            });

            delta = (max - min) / levels;

            for (int i = 0; i < levels; i++)
            {
                intervals.Add(min, (float)Math.Round(min += delta, 3));
            }

            OutputQuantizedSignal = new Signal(outSamples, false);
            OutputIntervalIndices = new List<int>();
            OutputEncodedSignal = new List<string>();
            OutputSamplesError = new List<float>();

            InputSignal.Samples.ForEach((sample) => {
                for (int i = 0; i < intervals.Count; i++)
                {
                    float key = intervals.ElementAt(i).Key;
                    float value = intervals.ElementAt(i).Value;

                    if (sample >= key && sample <= value)
                    {
                        float mid = (key + value) / 2;
                        OutputQuantizedSignal.Samples.Add(mid);
                        OutputIntervalIndices.Add(i + 1);
                        OutputEncodedSignal.Add(Convert.ToString(i, 2).PadLeft(numOfBits, '0'));
                        OutputSamplesError.Add(mid - sample);
                        break;
                    }
                }
            });
            

        }
    }
}

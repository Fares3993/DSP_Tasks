using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Derivatives: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal FirstDerivative { get; set; }
        public Signal SecondDerivative { get; set; }

        public override void Run()
        {
            List<float> InputSamples = InputSignal.Samples;
            List<float> FOutSamples = new List<float>();
            List<float> SOutSamples = new List<float>() { 0 };

            for (int i = 0; i < InputSamples.Count; i++)
            {
                try
                {
                    FOutSamples.Add(InputSamples[i + 1] - InputSamples[i]);
                    SOutSamples.Add(InputSamples[i + 1] - 2 * InputSamples[i] + InputSamples[i - 1]);
                }
                catch { }
            }

            FirstDerivative = new Signal(FOutSamples, false);
            SecondDerivative = new Signal(SOutSamples, false);
        }
    }
}

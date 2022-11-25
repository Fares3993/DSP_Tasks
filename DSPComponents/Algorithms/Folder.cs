using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputFoldedSignal { get; set; }

        public override void Run()
        {
            bool periodic = !InputSignal.Periodic;

            List<float> Samples = new List<float>();
            List<int> NewIndex = new List<int>();
            
            int count = InputSignal.Samples.Count;
            
            for(int i = 1; i <= count; i++)
            {
                Samples.Add(InputSignal.Samples[count-i]);
                NewIndex.Add(InputSignal.SamplesIndices[count - i] * -1);
            }
            OutputFoldedSignal = new Signal(Samples, NewIndex, periodic);
        }
    }
}

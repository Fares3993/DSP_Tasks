using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Shifter : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int ShiftingValue { get; set; }
        public Signal OutputShiftedSignal { get; set; }
        
        public int Fsign = 1;
        public override void Run()
        {
            List<int> NewIndex = new List<int>();

            if (InputSignal.Periodic)
                Fsign = -1;
            
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                NewIndex.Add(InputSignal.SamplesIndices[i] - (ShiftingValue * Fsign));
            }

            OutputShiftedSignal = new Signal(InputSignal.Samples, NewIndex, InputSignal.Periodic);
        }
    }
}

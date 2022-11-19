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
            if (/*Folder.FoldedCount % 2 != 0*/InputSignal.Samples[0] == 1)
                Fsign = -1;
            //else if (Folder.FoldedCount % 2 == 0)
            //    Fsign = 1;
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                InputSignal.SamplesIndices[i] = InputSignal.SamplesIndices[i] + (ShiftingValue*Fsign);
                NewIndex.Add(InputSignal.SamplesIndices[i]);
            }
            OutputShiftedSignal = new Signal(InputSignal.Samples, NewIndex, false);
        }
    }
}

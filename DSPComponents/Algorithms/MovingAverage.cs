﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MovingAverage : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int InputWindowSize { get; set; }
        public Signal OutputAverageSignal { get; set; }
 
        public override void Run()
        {
            List<float> OutSamples = new List<float>();

            for (int i = 0; i < InputSignal.Samples.Count - InputWindowSize + 1; i++)
            {
                float average = 0;
                for (int j = 0; j < InputWindowSize; j++)
                {
                    average += InputSignal.Samples[i + j];
                }
                OutSamples.Add(average/InputWindowSize); 
            }
            OutputAverageSignal = new Signal(OutSamples, false);
        }
    }
}

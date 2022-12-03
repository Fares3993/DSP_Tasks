﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }
        public List<float> Values = new List<float>();
        public override void Run()
        {
            int N = InputSignal.Samples.Count;
            
            for(int k = 0; k < N ;k++)
            {
                double sum = 0;
                for (int i = 0; i < N; i++)
                {
                    sum+=InputSignal.Samples[i] * (Math.Cos((Math.PI / (4 * N)) * (2 * i - 1) * (2 * k - 1)));
                }
                Values.Add( (float)(Math.Sqrt(2.0f/N)*sum));
            }
            OutputSignal=new Signal(Values,false);
        }
    }
}

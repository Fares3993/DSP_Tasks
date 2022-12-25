﻿using DSPAlgorithms.DataStructures;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }




        public override void Run()
        {
            if (M <= 0 && L <= 0) { 
                // Error
                return;
            }
            OutputSignal = UpSampling();

            FIR filter = new FIR();
            filter.InputFilterType = FILTER_TYPES.LOW;
            filter.InputFS = 8000;
            filter.InputStopBandAttenuation = 50;
            filter.InputCutOffFrequency = 1500;
            filter.InputTransitionBand = 500;
            filter.InputTimeDomainSignal = OutputSignal;
            filter.Run();
            
            OutputSignal = filter.OutputYn;

            OutputSignal = DownSampling();
        }
        private Signal UpSampling()
        {            
            List<float> samples = InputSignal.Samples;

            List<float> out_samples = new List<float>();
            List<int> out_indices = new List<int>();

            int first_index = InputSignal.SamplesIndices[0] - 1;

            if (L > 0)
            {
                samples.ForEach(x => {
                    out_samples.Add(x);
                    out_indices.Add(++first_index);
                    for (int i = 1; i < L; i++)
                    {
                        out_samples.Add(0);
                        out_indices.Add(++first_index);
                    }
                });
                
                // remove the last zeros added after the last sample
                for (int i = 1; i < L; i++)
                {
                    out_samples.RemoveAt(out_samples.Count - 1);
                    out_indices.RemoveAt(out_indices.Count - 1);
                }

                return new Signal(out_samples, out_indices, false);
            }
            return InputSignal;
        }
        private Signal DownSampling()
        {
            List<float> samples = OutputSignal.Samples;

            List<float> out_samples = new List<float>();
            List<int> out_indices = new List<int>();

            int first_index = OutputSignal.SamplesIndices[0] - 1;

            if (M > 0)
            {
                int temp = -1;
                samples.ForEach(x => {
                    
                    temp++;
                    if (temp % M == 0)
                    {
                        out_samples.Add(x);
                        out_indices.Add(++first_index);
                    }
                });
                return new Signal(out_samples, out_indices, false);
            }
            return OutputSignal;
        }
    }

}
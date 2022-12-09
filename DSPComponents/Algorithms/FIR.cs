using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; }
        public float? InputCutOffFrequency { get; set; }
        public float? InputF1 { get; set; }
        public float? InputF2 { get; set; }
        public float InputStopBandAttenuation { get; set; }
        public float InputTransitionBand { get; set; }
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; }

        public override void Run()
        {
            List<float> inputs = InputTimeDomainSignal.Samples;
            List<float> idealResponses = new List<float>(); // Hd(n)
            List<float> impulseResponses = new List<float>(); // W(n)

            for(int i = 0; i < inputs.Count; i++)
            {
                inputs[i] = inputs[i] / InputFS;
                idealResponses.Add(IdealResponse(i));
                //impulseResponses.Add(ImpulseResponse(i));
            }
            OutputHn = new Signal(idealResponses, false);
        }

        private float IdealResponse(int n)
        {
            if (InputFilterType == FILTER_TYPES.LOW)
            {
                float cutOff = (float)(InputCutOffFrequency + (InputTransitionBand / 2));
                if (n == 0)
                {
                    return 2 * cutOff;
                }
                else
                {
                    return (float)
                        (2 * cutOff * Math.Sin(n * 2 * Math.PI * cutOff) 
                        / (n * 2 * Math.PI * cutOff));
                }
            }
            else if (InputFilterType == FILTER_TYPES.HIGH)
            {
                float cutOff = (float)(InputCutOffFrequency + InputTransitionBand / 2);
                if (n == 0)
                {
                    return 1 - (2 * cutOff);
                }
                else
                {
                    return (float)
                        (-2 * cutOff * Math.Sin(n * 2 * Math.PI * cutOff) 
                        / (n * 2 * Math.PI * cutOff));
                }
            }
            return 0;
        }

        private float ImpulseResponse(int n)
        {
            float TransitionWidth = InputTransitionBand / InputFS;

            if (InputStopBandAttenuation < 21)
            {
                return 1;
            }
            else if (InputStopBandAttenuation < 53)
            {
                float N = (float)Math.Round(3.3 / TransitionWidth);

                if (N % 2 == 0)
                    N++;
                return (float)(0.54 + 0.64 * Math.Cos(2 * Math.PI * n / N));
            }
            return 0;
        }
    }
}

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
            List<float> H_List = new List<float>();
            List<int> H_Indecies = new List<int>();

            Func<int, float> idealResonse = (n) => 0;
            if (InputFilterType == FILTER_TYPES.LOW)
            {
                float cutOff = (float)((InputCutOffFrequency + (InputTransitionBand / 2)) / InputFS);
                idealResonse = (n) => LowIdealResponse(n, cutOff);
            }
            else if (InputFilterType == FILTER_TYPES.HIGH)
            {
                float cutOff = (float)((InputCutOffFrequency - (InputTransitionBand / 2)) / InputFS);
                idealResonse = (n) => HighIdealResponse(n, cutOff);
            }
            else if (InputFilterType == FILTER_TYPES.BAND_PASS)
            {
                float f1 = (float)((InputF1 - (InputTransitionBand / 2)) / InputFS);
                float f2 = (float)((InputF2 + (InputTransitionBand / 2)) / InputFS);
                idealResonse = (n) => BandPassIdealResponse(n, f1, f2);
            }
            else if (InputFilterType == FILTER_TYPES.BAND_STOP)
            {
                float f1 = (float)((InputF1 + (InputTransitionBand / 2)) / InputFS);
                float f2 = (float)((InputF2 - (InputTransitionBand / 2)) / InputFS);
                idealResonse = (n) => BandRejectIdealResponse(n, f1, f2);
            }


            float TransitionWidth = InputTransitionBand / InputFS;
            int N = 0;
            Func<int, float> impulseResponse = n => 0;
            if (InputStopBandAttenuation <= 21)
            {
                N = (int)Math.Round(0.9 / TransitionWidth); 
                if (N % 2 == 0)
                    N++;
                impulseResponse = (n) => RectangularImpulseRespnse(n, N);
            }
            else if (InputStopBandAttenuation <= 44)
            {
                N = (int)Math.Round(3.1 / TransitionWidth);
                if (N % 2 == 0)
                    N++;
                impulseResponse = (n) => HanningImpulseRespnse(n, N);
            }
            else if (InputStopBandAttenuation <= 53)
            {
                N = (int)Math.Round(3.3 / TransitionWidth);
                if (N % 2 == 0)
                    N++;
                impulseResponse = (n) => HammingImpulseRespnse(n, N);
            }
            else if (InputStopBandAttenuation <= 74)
            {
                N = (int)Math.Round(5.5 / TransitionWidth);
                if (N % 2 == 0)
                    N++;
                impulseResponse = (n) => BlackmanImpulseRespnse(n, N);
            }


            for (int i = 0; i <= (N - 1) / 2; i++)
            {
                float Hd = idealResonse(i);
                float w = impulseResponse(i);
                H_List.Add(Hd*w);
                H_Indecies.Add(i);
            }
            for (int i = -1; i >= -(N - 1) / 2; i--)
            {
                H_List.Insert(0, H_List[-(2 * i + 1)]);
                H_Indecies.Insert(0, i);
            }
            OutputHn = new Signal(H_List, H_Indecies, false);

            FastConvolution conv = new FastConvolution();
            conv.InputSignal1 = InputTimeDomainSignal;
            conv.InputSignal2 = OutputHn;
            conv.Run();
            OutputYn = conv.OutputConvolvedSignal;
        }

        #region Window Functions for Impulse Responses
        private float RectangularImpulseRespnse(int n, int N)
        {
            return 1;
        }

        private float HanningImpulseRespnse(int n, int N)
        {
            return (float)(0.5 + 0.5 * Math.Cos(2 * Math.PI * n / N));
        }

        private float HammingImpulseRespnse(int n, int N)
        {
            return (float)(0.54 + 0.46 * Math.Cos(2 * Math.PI * n / N));
        }

        private float BlackmanImpulseRespnse(int n, int N)
        {
            return (float)
                (0.42 
                + 0.5 * Math.Cos(2 * Math.PI * n / (N - 1)) 
                + 0.08 * Math.Cos(4 * Math.PI * n / (N - 1)));
        }
        #endregion

        #region Filter Ideal Responses
        private float LowIdealResponse(int n, float CutOff)
        {
            if (n == 0)
            {
                return 2 * CutOff;
            }
            else
            {
                return (float)
                    (2 * CutOff * Math.Sin(n * 2 * Math.PI * CutOff)
                    / (n * 2 * Math.PI * CutOff));
            }
        }

        private float HighIdealResponse(int n, float CutOff)
        {
            if (n == 0)
            {
                return 1 - (2 * CutOff);
            }
            else
            {
                return (float)
                    (-2 * CutOff * Math.Sin(n * 2 * Math.PI * CutOff)
                    / (n * 2 * Math.PI * CutOff));
            }
        }

        private float BandPassIdealResponse(int n, float f1, float f2)
        {
            if (n == 0)
            {
                return 2 * (f2 - f1);
            }
            else
            {
                return (float)
                    (2 * f2 * Math.Sin(n * 2 * Math.PI * f2) / (n * 2 * Math.PI * f2)
                    -2 * f1 * Math.Sin(n * 2 * Math.PI * f1) / (n * 2 * Math.PI * f1));
            }
        }

        private float BandRejectIdealResponse(int n, float f1, float f2)
        {
            if (n == 0)
            {
                return 1 - (2 * (f2 - f1));
            }
            else
            {
                return (float)
                    (2 * f1 * Math.Sin(n * 2 * Math.PI * f1) / (n * 2 * Math.PI * f1)
                    -2 * f2 * Math.Sin(n * 2 * Math.PI * f2) / (n * 2 * Math.PI * f2));
            }
        }
        #endregion
    }
}

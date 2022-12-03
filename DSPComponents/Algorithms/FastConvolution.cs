using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }
        public List<Complex> L = new List<Complex>();
        public List<float> Amplitudes = new List<float>();
        public List<float> PhaseShifts = new List<float>();
        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()

        {

            int length = InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1;
            for (int i = InputSignal1.Samples.Count; i < length; i++)
            {
                InputSignal1.Samples.Add(0);
            }
            for (int j = InputSignal2.Samples.Count; j < length; j++)
            {
                InputSignal2.Samples.Add(0);
            }

            DiscreteFourierTransform DFT1 = new DiscreteFourierTransform();
            DFT1.InputTimeDomainSignal =InputSignal1;
            DFT1.Run();
            List<float> Amplitudes1 = new List<float>(DFT1.OutputFreqDomainSignal.FrequenciesAmplitudes);
            List<float> PhaseShifts1 = new List<float>(DFT1.OutputFreqDomainSignal.FrequenciesPhaseShifts);



            DiscreteFourierTransform DFT2 = new DiscreteFourierTransform();
            DFT2.InputTimeDomainSignal = InputSignal2;
            DFT2.Run();

            List<float> Amplitudes2 = new List<float>(DFT2.OutputFreqDomainSignal.FrequenciesAmplitudes);
            List<float> PhaseShifts2 = new List<float>(DFT2.OutputFreqDomainSignal.FrequenciesPhaseShifts);

            for (int i = 0; i < DFT1.OutputFreqDomainSignal.Samples.Count; i++)
            {
                L.Add(Complex.Multiply( Complex.FromPolarCoordinates(Amplitudes1[i], PhaseShifts1[i]) , Complex.FromPolarCoordinates(Amplitudes2[i], PhaseShifts2[i])));
                Amplitudes.Add((float)L[i].Magnitude);
                PhaseShifts.Add((float)L[i].Phase);
            }
            InverseDiscreteFourierTransform IDFT = new InverseDiscreteFourierTransform();
            IDFT.InputFreqDomainSignal = new Signal(false,new List<float>(),new List<float>(Amplitudes), new List<float>(PhaseShifts));
            IDFT.Run();
            OutputConvolvedSignal = IDFT.OutputTimeDomainSignal;

        }
    }
}

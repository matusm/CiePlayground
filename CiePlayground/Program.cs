using System;
using System.Globalization;
using System.Threading;
using At.Matus.BevMetrology;
using At.Matus.StatisticPod;

namespace CiePlayground
{
    class Program
    {
        public static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            SpectralQuantity lamp = SpectralQuantity.LoadFromCsv("SN_7-1108_Kalibrierung_FEL1000_2022_intpl1.csv");
            Console.WriteLine($"CCT = {lamp.ColorTemperature.Cct:F3} K  ({lamp.ColorTemperature.ChomaticityDifference:F9}   {lamp.ColorTemperature.Status})");
            Console.WriteLine();

            // basic MC simulation
            StatisticPod sp = new StatisticPod("CCT / K");
            SpectralQuantity lampR;
            for (int i = 0; i < 50; i++)
            {
<<<<<<< HEAD
=======
                lampR = lamp.Randomize(0, 0.005);
                Console.WriteLine($"CCT = {lampR.ColorTemperature.Cct:F3} K  ({lampR.ColorTemperature.ChomaticityDifference:F9}   {lampR.ColorTemperature.Status})");
>>>>>>> 7b873f2963754d79ba96cbae6ea00be84cce30d6
                sp.Update(lampR.ColorTemperature.Cct);
            }
            Console.WriteLine();
            Console.WriteLine(sp);

        }

    }

}
    

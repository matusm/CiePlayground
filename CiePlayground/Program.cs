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

            SpectralQuantity lamp = SpectralQuantity.LoadFromCsv("SN_7-1108_Kalibrierung_FEL1000_2022_noU.csv");

            Console.WriteLine($"CCT of original spectrum: {lamp.CCT:F1} K ({lamp.ColorTemperature.ChomaticityDifference:F9} -> {lamp.ColorTemperature.Status})");
            Console.WriteLine($"Distribution temperature of original spectrum: {lamp.TD:F1} K");

            // basic MC simulation
        //    StatisticPod sp = new StatisticPod("CCT / K");
        //    SpectralQuantity lampR;
        //    for (int i = 0; i < 5; i++)
        //    {
        //        lampR = lamp.Randomize(0, 0.006);
        //        Console.WriteLine($"{i,5}   {lampR.ColorTemperature.Cct:F1} K   ({lampR.ColorTemperature.ChomaticityDifference:F9} -> {lampR.ColorTemperature.Status})");
        //        sp.Update(lampR.ColorTemperature.Cct);
        //    }

        //    Console.WriteLine();
        //    Console.WriteLine($"CCT of original spectrum: {lamp.ColorTemperature.Cct:F1} K ({lamp.ColorTemperature.ChomaticityDifference:F9} -> {lamp.ColorTemperature.Status})");
        //    Console.WriteLine($"CCT of MC spectra:        {sp.AverageValue:F1} ({sp.StandardDeviation:F1}) K");

        }

    }

}
    

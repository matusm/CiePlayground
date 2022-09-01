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

            SpectralQuantity lamp = SpectralQuantity.LoadFromCsv("SN_8421_Kalibrierung_FEL1000_2022_noU.csv");

            Console.WriteLine($"Name:                     {lamp.Name}");
            Console.WriteLine($"Number of values:         {lamp.NumberOfValues}");
            Console.WriteLine($"Minimum wavelength:       {lamp.MinWavelength} nm");
            Console.WriteLine($"Maximum wavelength:       {lamp.MaxWavelength} nm");
            Console.WriteLine($"Color temperature:        {lamp.ColorTemperature.CCT} K [{lamp.ColorTemperature.ChomaticityDifference:F9}]");
            Console.WriteLine($"Distribution temperature: {lamp.DistributionTemperature.TD} K [{lamp.DistributionTemperature.RelativeDeviation*100:F1} %]");
            Console.WriteLine();

            // basic MC simulation
            CovariancePod sp = new CovariancePod();
            SpectralQuantity lampR;
            for (int i = 0; i <= 10; i++)
            {
                lampR = lamp.Randomize(0, 0.006);
                if(i%10==0)
                    Console.Write("|");
                else
                    Console.Write("-");
                sp.Update(lampR.CCT, lampR.TD);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine($"CCT of MC spectra: {sp.AverageValueOfX:F1} ({sp.StandardDeviationOfX:F1}) K");
            Console.WriteLine($"T_D of MC spectra: {sp.AverageValueOfY:F1} ({sp.StandardDeviationOfY:F1}) K");
            Console.WriteLine($"correlation coefficient: {sp.CorrelationCoefficient:F4}");


        }

    }

}
    

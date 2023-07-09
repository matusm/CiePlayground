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

            SpectralQuantity lamp = SpectralQuantity.FromCieIlluminantD50();

            BevCie.Validate();

            Console.WriteLine();


            //SpectralQuantity lamp = SpectralQuantity.LoadFromCsv("SN_7-1108_Kalibrierung_FEL1000_2022_noU.csv");

            Console.WriteLine($"Name:                          {lamp.Name}");
            Console.WriteLine($"Number of values:              {lamp.NumberOfValues}");
            Console.WriteLine($"Minimum wavelength:            {lamp.MinWavelength} nm");
            Console.WriteLine($"Maximum wavelength:            {lamp.MaxWavelength} nm");
            Console.WriteLine($"Color temperature:             {lamp.CCT:F3} K");
            Console.WriteLine($"Color temperature stat:        {lamp.ColorTemperature.Status}");
            Console.WriteLine($"Color temperature cd:          {lamp.ColorTemperature.ChomaticityDifference:F4}");
            Console.WriteLine($"Distribution temperature:      {lamp.TD:F3} K");
            Console.WriteLine($"Distribution temperature stat: {lamp.DistributionTemperature.Status}");
            Console.WriteLine($"Distribution temperature dev:  {lamp.DistributionTemperature.RelativeDeviation*100:F0} %");
            Console.WriteLine();

            //// basic MC simulation
            //CovariancePod sp = new CovariancePod();
            //SpectralQuantity lampR;
            //for (int i = 0; i <= 100; i++)
            //{
            //    lampR = lamp.Randomize(0, 0.006);
            //    if(i%10==0)
            //        Console.Write("|");
            //    else
            //        Console.Write("-");
            //    sp.Update(lampR.CCT, lampR.TD);
            //}
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine($"CCT of MC spectra: {sp.AverageValueOfX:F1} ({sp.StandardDeviationOfX:F1}) K");
            //Console.WriteLine($"T_D of MC spectra: {sp.AverageValueOfY:F1} ({sp.StandardDeviationOfY:F1}) K");
            //Console.WriteLine($"correlation coefficient: {sp.CorrelationCoefficient:F4}");

        }

    }

}
    

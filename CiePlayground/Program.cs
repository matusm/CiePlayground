using System;
using System.Globalization;
using System.IO;
using System.Threading;
using At.Matus.BevMetrology;
using At.Matus.StatisticPod;

namespace CiePlayground
{
    class Program
    {
        public static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            int cctMin = 1900;
            int cctMax = 3900;
            int deltaCct = 100;
            double constUnc = 0.001;
            double relUnc = 0.025;
            int drawsMonteCarlo = 1_000_000;

            int numberOfTemperatures = (cctMax - cctMin) / deltaCct;
            int TemperatureForIndex(int i) => cctMin + i * deltaCct;

            string[] filenames = {
                "LMT_P30SCT_06A5572.csv",
                "LMT_L1000_09A415.csv",
                "PRC_980430.csv",
                "IL_SED033_4624.csv",
                "LMT_P15F0T_109508.csv",
                "LMT_P30SCT_0498202.csv",
                "LMT_P30SCT_04A0681.csv",
                "Osram_7218.csv",
                "VL-3701-2_52365.csv"
            };

            foreach (var filename in filenames)
            {
                Console.WriteLine($"reading {filename}");

                string baseFilename = Path.GetFileNameWithoutExtension(filename);
                SpectralQuantity photometer = SpectralQuantity.LoadFromCsv(filename);
                photometer.AddValue(300, photometer.GetValueFor(photometer.MinWavelength));
                photometer.AddValue(1000, photometer.GetValueFor(photometer.MaxWavelength));

                Console.WriteLine($"Monte Carlo start with n = {drawsMonteCarlo}");

                StatisticPod[] monteCarloArray = new StatisticPod[numberOfTemperatures + 1];
                for (int i = 0; i <= numberOfTemperatures; i++)
                {
                    monteCarloArray[i] = new StatisticPod($"{TemperatureForIndex(i)}"); // this is a hack!
                }

                // Monte Carlo draws
                for (int i = 0; i < drawsMonteCarlo; i++)
                {
                    SpectralQuantity randomizedPhotometer = photometer.Randomize(constUnc, relUnc);
                    for (int j = 0; j <= numberOfTemperatures; j++)
                    {
                        double ccf = BevCie.CcfStar(randomizedPhotometer.GetValueFor, TemperatureForIndex(j));
                        monteCarloArray[j].Update(ccf);
                    }
                }

                Console.WriteLine("Monte Carlo done");

                string outFilename = baseFilename + $"_CCF_MC.csv";
                using (StreamWriter writer = new StreamWriter(outFilename))
                {
                    string csvHeader = $"T/K , ccf* , ccf*_avr , ccf*_min , ccf*_max , ccf*_stdev";
                    writer.WriteLine(csvHeader);
                    foreach (var dsp in monteCarloArray)
                    {
                        int t = int.Parse(dsp.Name);
                        double ccf0 = BevCie.CcfStar(photometer.GetValueFor, t);
                        string csvLine = $"{t} , {ccf0:F6} , {dsp.AverageValue:F6} , {dsp.MinimumValue:F6} , {dsp.MaximumValue:F6} , {dsp.StandardDeviation:F6}";
                        writer.WriteLine(csvLine);
                    }
                }
                Console.WriteLine($"{outFilename} created");

                outFilename = baseFilename + $"_CCF_4fit.csv";
                using (StreamWriter writer = new StreamWriter(outFilename))
                {
                    string csvHeader = $"T-2856 / K , ccf*-1 , u , s";
                    writer.WriteLine(csvHeader);
                    foreach (var dsp in monteCarloArray)
                    {
                        int t = int.Parse(dsp.Name); // second step of the hack
                        int T1 = t - 2856;
                        double ccf = BevCie.CcfStar(photometer.GetValueFor, t);
                        double u = (dsp.MaximumValue - dsp.MinimumValue) / Math.Sqrt(12.0);
                        string csvLine = $"{T1,-4} , {ccf - 1,9:F6} , {u:F6} , {dsp.StandardDeviation:F6}";
                        writer.WriteLine(csvLine);
                    }
                }

                Console.WriteLine($"{outFilename} created");
                Console.WriteLine();

            }
        }
    }
}
    

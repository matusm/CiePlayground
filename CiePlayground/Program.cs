using System;
using System.Globalization;
using System.Threading;
using At.Matus.BevMetrology;

namespace CiePlayground
{
    class Program
    {
        public static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            SpectralQuantity lamp1spline = SpectralQuantity.LoadFromCsv("SN_7-1108_Kalibrierung_FEL1000_2022_intpl1.csv");

            lamp1spline.CalculateColor();


            double uLamp = lamp1spline.ColorCoordinates.uPrime;
            double vLamp = lamp1spline.ColorCoordinates.vPrime;

            Console.WriteLine($"Lamp: uPrime: {uLamp:F6} vPrime: {vLamp:F6} ");
            Console.WriteLine();


            double distanceMin = double.PositiveInfinity;
            double Tlamp = double.NaN;

            for (double T = 3000; T <= 3300; T = T+0.1)
            {
                SpectralQuantity plank = new SpectralQuantity(T.ToString());
                for (int l = 360; l <= 830; l++)
                {
                    plank.AddValue(l, BevCie.LPlanck(T, l));
                }
                plank.CalculateColor();

                double distance = Distance(plank.ColorCoordinates.uPrime, plank.ColorCoordinates.vPrime, uLamp, vLamp);

                if (distance < distanceMin)
                {
                    distanceMin = distance;
                    Tlamp = T;
                }
            }

            Console.WriteLine($"CCT = {Tlamp:F1} K ({distanceMin:F7})");

        }

        public static double Distance(double up, double vp, double u, double v)
        {
            double us = (up - u) * (up - u);
            double vs = (vp - v) * (vp - v);
            return Math.Sqrt(us + vs * (4 / 9));
        }
    }

}
    

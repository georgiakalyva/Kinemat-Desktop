using System;

namespace Kinemat.Windows.Controls.Book
{
    internal class CurlCalculator
    {
        public static double CurlX { get; set; }

        public static double CurlY { get; set; }

        public static double GetCurlX(double x, double y)
        {
            double d = Math.Atan2(y, x);
            CurlCalculator.CurlX = Math.Sqrt(x * x + y * y) / 2.0 / Math.Cos(d);
            return CurlCalculator.CurlX;
        }

        public static double GetCurlY(double x, double y)
        {
            double a = Math.Atan2(y, x);
            CurlCalculator.CurlY = Math.Sqrt(x * x + y * y) / 2.0 / Math.Sin(a);
            return CurlCalculator.CurlY;
        }
    }
}

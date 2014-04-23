using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project
{
    public static class PublicRandom
    {
        private static Random rdn;

        private static Random Rdn
        {
            get
            {
                if (rdn == null)
                    rdn = new Random();
                return PublicRandom.rdn;
            }
        }

        public static double NextDouble()
        {
            return Rdn.NextDouble();
        }

        public static double NextDouble(double min)
        {
            double d = 0;
            while ((d = Rdn.NextDouble()) < min) ;
            return d;
        }
        
        public static double NextDouble(double min, double max)
        {
            double d = 0;
            while ((d = Rdn.NextDouble()) < min || d > max);
            return d;
        }

        public static int Next(int maxValue)
        {
            return Rdn.Next(maxValue);
        }

        public static int Next(int minValue, int maxValue)
        {
            return Rdn.Next(minValue, maxValue);
        }

        public static int Next()
        {
            return Rdn.Next();
        }


    }
}

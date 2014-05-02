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

        /// <summary>
        /// Retorna um double positivo aleatório entre 0.0 e 1.0.
        /// </summary>
        /// <returns>Um double positivo entre 0.0 e 1.0.</returns>
        public static double NextDouble()
        {
            return Rdn.NextDouble();
        }

        /// <summary>
        /// Retorna um double aleatório positivo maior ou igual ao valor especificado e menor ou igual a 1.0.
        /// </summary>
        /// <param name="min">O menor valor aceito.</param>
        /// <returns>Um double positivo maior ou igual a min e menor ou igual a 1.0.</returns>
        public static double NextDouble(double min)
        {
            double d = 0;
            while ((d = Rdn.NextDouble()) < min) ;
            return d;
        }

        /// <summary>
        /// Retorna um double aleatório positivo entre os valores especificados (sendo o máximo 1.0).
        /// </summary>
        /// <param name="min">O menor valor aceito.</param>
        /// <param name="max">O maior valor aceito (no máximo 1.0).</param>
        /// <returns>Um double positivo maior ou igual a min e menor ou igual a max.</returns>
        public static double NextDouble(double min, double max = 1.0)
        {
            if (min > max)
                throw new ArgumentOutOfRangeException("max", "O valor de max(" + max +") não pode ser superior ao valor de min(" + min + ")");
            double d = 0;
            while ((d = Rdn.NextDouble()) < min || d > max);
            return d;
        }

        /// <summary>
        /// Retorna um inteiro aleatório positivo menor do que o valor especificado.
        /// </summary>
        /// <param name="maxValue">Valor máximo permitido.</param>
        /// <returns>Um inteiro positivo entre 0 e maxValue.</returns>
        public static int Next(int maxValue)
        {
            return Rdn.Next(maxValue);
        }

        /// <summary>
        /// Retorna um inteiro aleatório positivo contido entre os valores especificados.
        /// </summary>
        /// <param name="minValue">O menor valor aceito.</param>
        /// <param name="maxValue">O máximo valor aceito + 1.</param>
        /// <returns>Um inteiro positivo entre minValue e maxValue.</returns>
        public static int Next(int minValue, int maxValue)
        {
            return Rdn.Next(minValue, maxValue);
        }

        /// <summary>
        /// Retorna um inteiro positivo.
        /// </summary>
        /// <returns>Um inteiro positivo.</returns>
        public static int Next()
        {
            return Rdn.Next();
        }


    }
}

using System;
using System.Collections.Generic;
using System.Numerics;

namespace CryptoExam.Core.NumberTheory
{
    public class EuclidStep
    {
        public BigInteger A { get; set; }
        public BigInteger B { get; set; }
        public BigInteger Q { get; set; }
        public BigInteger R { get; set; }
        public BigInteger X { get; set; }
        public BigInteger Y { get; set; }
    }

    public static class ExtendedEuclidService
    {
        public static (BigInteger gcd, BigInteger x, BigInteger y, List<EuclidStep> steps) CalculateWithSteps(BigInteger a, BigInteger m)
        {
            var steps = new List<EuclidStep>();
            BigInteger x0 = 1, y0 = 0, x1 = 0, y1 = 1;
            BigInteger originalA = a;
            BigInteger originalM = m;

            while (m != 0)
            {
                BigInteger q = a / m;
                BigInteger r = a % m;
                
                var step = new EuclidStep
                {
                    A = a,
                    B = m,
                    Q = q,
                    R = r,
                    X = x0 - q * x1,
                    Y = y0 - q * y1
                };
                steps.Add(step);

                a = m;
                m = r;

                BigInteger tempX = x1;
                BigInteger tempY = y1;
                x1 = x0 - q * x1;
                y1 = y0 - q * y1;
                x0 = tempX;
                y0 = tempY;
            }

            return (a, x0, y0, steps);
        }

        public static BigInteger? GetInverse(BigInteger a, BigInteger m)
        {
            var result = CalculateWithSteps(a, m);
            if (result.gcd != 1) return null;
            
            BigInteger inv = result.x % m;
            if (inv < 0) inv += m;
            return inv;
        }
    }
}

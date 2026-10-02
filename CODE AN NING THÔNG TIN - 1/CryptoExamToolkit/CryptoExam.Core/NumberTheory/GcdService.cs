using System;
using System.Numerics;

namespace CryptoExam.Core.NumberTheory
{
    public static class GcdService
    {
        public static BigInteger Calculate(BigInteger a, BigInteger b)
        {
            a = BigInteger.Abs(a);
            b = BigInteger.Abs(b);
            while (b != 0)
            {
                BigInteger temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
    }
}

// File: CryptoExam.Core/NumberTheory/GcdService.cs
using System;
using System.Numerics;

namespace CryptoExam.Core.NumberTheory
{
    /// <summary>
    /// Tính GCD bằng thuật toán Euclid.
    /// </summary>
    public static class GcdService
    {
        /// <summary>
        /// Tính GCD(a, b) bằng Euclid đệ quy.
        /// </summary>
        public static BigInteger Gcd(BigInteger a, BigInteger b)
        {
            a = BigInteger.Abs(a);
            b = BigInteger.Abs(b);
            while (b != 0)
            {
                BigInteger t = b;
                b = a % b;
                a = t;
            }
            return a;
        }

        /// <summary>
        /// Kiểm tra a và m có nguyên tố cùng nhau không (gcd = 1).
        /// </summary>
        public static bool AreCoprime(BigInteger a, BigInteger m)
        {
            return Gcd(a, m) == 1;
        }
    }
}

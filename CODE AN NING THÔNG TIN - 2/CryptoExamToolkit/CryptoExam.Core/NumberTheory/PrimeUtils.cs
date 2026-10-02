// File: CryptoExam.Core/NumberTheory/PrimeUtils.cs
using System;
using System.Numerics;

namespace CryptoExam.Core.NumberTheory
{
    /// <summary>
    /// Tiện ích kiểm tra số nguyên tố.
    /// </summary>
    public static class PrimeUtils
    {
        /// <summary>
        /// Kiểm tra n có phải số nguyên tố không (Miller-Rabin đơn giản).
        /// Đủ dùng cho các số trong bài thi.
        /// </summary>
        public static bool IsPrime(BigInteger n)
        {
            if (n < 2) return false;
            if (n == 2 || n == 3 || n == 5) return true;
            if (n % 2 == 0 || n % 3 == 0) return false;

            // Trial division đến sqrt(n)
            for (BigInteger i = 5; i * i <= n; i += 6)
            {
                if (n % i == 0 || n % (i + 2) == 0) return false;
            }
            return true;
        }
    }
}

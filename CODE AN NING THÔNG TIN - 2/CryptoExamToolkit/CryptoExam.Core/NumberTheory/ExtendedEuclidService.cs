// File: CryptoExam.Core/NumberTheory/ExtendedEuclidService.cs
using System;
using System.Collections.Generic;
using System.Numerics;

namespace CryptoExam.Core.NumberTheory
{
    /// <summary>
    /// Thuật toán Euclid mở rộng:
    ///   Tìm x, y sao cho: a*x + m*y = gcd(a, m)
    ///   Nếu gcd = 1: x = a^(-1) mod m
    ///
    /// Test vector:
    ///   550^-1 mod 1759 = 355
    ///   7^-1   mod 192  = 55
    ///   17^-1  mod 3120 = 2753
    ///   53^-1  mod 3120 = 2237
    /// </summary>
    public static class ExtendedEuclidService
    {
        /// <summary>
        /// Record lưu kết quả từng bước.
        /// </summary>
        public record EuclidStep
        {
            public BigInteger A { get; init; }
            public BigInteger B { get; init; }
            public BigInteger Quotient { get; init; }
            public BigInteger Remainder { get; init; }
            public BigInteger X { get; init; }
            public BigInteger Y { get; init; }
        }

        /// <summary>
        /// Kết quả đầy đủ của Extended Euclid.
        /// </summary>
        public record ExtendedGcdResult
        {
            public BigInteger A { get; init; }
            public BigInteger M { get; init; }
            public BigInteger Gcd { get; init; }
            public BigInteger X { get; init; }  // a*X + m*Y = gcd
            public BigInteger Y { get; init; }
            public BigInteger? ModularInverse { get; init; } // null nếu không tồn tại
            public List<EuclidStep> Steps { get; init; } = new();
        }

        /// <summary>
        /// Tính Extended GCD với các bước chi tiết.
        /// </summary>
        public static ExtendedGcdResult Calculate(BigInteger a, BigInteger m)
        {
            BigInteger aOrig = a, mOrig = m;
            var steps = new List<EuclidStep>();

            BigInteger oldR = a, r = m;
            BigInteger oldS = 1, s = 0;
            BigInteger oldT = 0, t = 1;

            while (r != 0)
            {
                BigInteger quotient = oldR / r;
                BigInteger rem = oldR % r;

                steps.Add(new EuclidStep
                {
                    A = oldR,
                    B = r,
                    Quotient = quotient,
                    Remainder = rem,
                    X = oldS,
                    Y = oldT
                });

                (oldR, r) = (r, rem);
                (oldS, s) = (s, oldS - quotient * s);
                (oldT, t) = (t, oldT - quotient * t);
            }

            BigInteger gcd = oldR;
            BigInteger x = oldS;
            BigInteger y = oldT;

            BigInteger? inverse = null;
            if (gcd == 1)
            {
                // Đảm bảo x dương trong mod m
                inverse = ((x % mOrig) + mOrig) % mOrig;
            }

            return new ExtendedGcdResult
            {
                A = aOrig,
                M = mOrig,
                Gcd = gcd,
                X = x,
                Y = y,
                ModularInverse = inverse,
                Steps = steps
            };
        }

        /// <summary>
        /// Lấy nhanh modular inverse. Ném exception nếu không tồn tại.
        /// </summary>
        public static BigInteger GetInverse(BigInteger a, BigInteger m)
        {
            var result = Calculate(a, m);
            if (result.ModularInverse == null)
                throw new ArithmeticException(
                    $"Không tồn tại nghịch đảo: gcd({a}, {m}) = {result.Gcd} ≠ 1");
            return result.ModularInverse.Value;
        }
    }
}

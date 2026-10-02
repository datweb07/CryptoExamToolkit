// File: CryptoExam.Core/NumberTheory/ModularArithmetic.cs
using System;
using System.Collections.Generic;
using System.Numerics;

namespace CryptoExam.Core.NumberTheory
{
    /// <summary>
    /// Tính toán modular arithmetic, đặc biệt modular exponentiation.
    /// Dùng BigInteger.ModPow - KHÔNG dùng Math.Pow.
    ///
    /// Test:
    ///   64^7 mod 221   = 38
    ///   38^55 mod 221  = 64
    ///   112^7 mod 221  = 5
    ///   5^55 mod 221   = 112
    /// </summary>
    public static class ModularArithmetic
    {
        /// <summary>
        /// Tính base^exponent mod modulus bằng BigInteger.ModPow.
        /// </summary>
        public static BigInteger ModPow(BigInteger baseVal, BigInteger exponent, BigInteger modulus)
        {
            if (modulus <= 0) throw new ArgumentException("Modulus phải > 0.");
            if (exponent < 0) throw new ArgumentException("Exponent phải >= 0.");
            return BigInteger.ModPow(baseVal, exponent, modulus);
        }

        /// <summary>
        /// Tính base^exponent mod modulus với từng bước Square-and-Multiply.
        /// Trả về danh sách các bước để hiển thị.
        /// </summary>
        public record SquareMultiplyStep
        {
            public int BitPosition { get; init; }    // vị trí bit (từ MSB)
            public int BitValue { get; init; }        // 0 hoặc 1
            public string Operation { get; init; } = ""; // "SQUARE" hoặc "SQUARE+MULTIPLY"
            public BigInteger Result { get; init; }   // giá trị hiện tại
        }

        public static (BigInteger result, List<SquareMultiplyStep> steps)
            ModPowWithSteps(BigInteger baseVal, BigInteger exponent, BigInteger modulus)
        {
            var steps = new List<SquareMultiplyStep>();
            if (exponent == 0) return (1, steps);

            // Chuyển exponent sang binary để square-and-multiply
            string expBinary = Convert.ToString((long)exponent, 2);

            BigInteger current = 1;
            for (int i = 0; i < expBinary.Length; i++)
            {
                int bit = expBinary[i] - '0';

                // Bước square
                current = (current * current) % modulus;
                string op = "SQUARE";

                if (bit == 1)
                {
                    // Bước multiply
                    current = (current * baseVal) % modulus;
                    op = "SQUARE + MULTIPLY";
                }

                steps.Add(new SquareMultiplyStep
                {
                    BitPosition = i,
                    BitValue = bit,
                    Operation = op,
                    Result = current
                });
            }

            return (current, steps);
        }
    }
}

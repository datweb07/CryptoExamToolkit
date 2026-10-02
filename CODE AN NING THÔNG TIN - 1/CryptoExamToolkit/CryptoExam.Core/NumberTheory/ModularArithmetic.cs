using System.Numerics;

namespace CryptoExam.Core.NumberTheory
{
    public static class ModularArithmetic
    {
        public static BigInteger ModPow(BigInteger baseValue, BigInteger exponent, BigInteger modulus)
        {
            return BigInteger.ModPow(baseValue, exponent, modulus);
        }
    }
}

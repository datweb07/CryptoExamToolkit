using System.Numerics;

namespace CryptoExam.Core.NumberTheory;

public static class GcdService
{
    public static BigInteger Calculate(BigInteger a, BigInteger b) => BigInteger.GreatestCommonDivisor(a, b);
}

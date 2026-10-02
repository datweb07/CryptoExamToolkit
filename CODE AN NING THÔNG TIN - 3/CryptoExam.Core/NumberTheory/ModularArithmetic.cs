using System.Numerics;

namespace CryptoExam.Core.NumberTheory;

public sealed record SquareMultiplyStep(int Step, BigInteger Exponent, BigInteger ResultBefore, BigInteger BaseBefore, bool Multiply, BigInteger ResultAfter, BigInteger BaseAfter);

public static class ModularArithmetic
{
    public static BigInteger Normalize(BigInteger value, BigInteger modulus) => (value % modulus + modulus) % modulus;

    public static BigInteger ModPow(BigInteger value, BigInteger exponent, BigInteger modulus)
    {
        if (exponent < 0) throw new ArgumentOutOfRangeException(nameof(exponent));
        if (modulus <= 0) throw new ArgumentOutOfRangeException(nameof(modulus));
        return BigInteger.ModPow(Normalize(value, modulus), exponent, modulus);
    }

    public static IReadOnlyList<SquareMultiplyStep> TraceModPow(BigInteger value, BigInteger exponent, BigInteger modulus)
    {
        if (exponent < 0 || modulus <= 0) throw new ArgumentOutOfRangeException();
        var result = BigInteger.One % modulus;
        var current = Normalize(value, modulus);
        var remaining = exponent;
        var steps = new List<SquareMultiplyStep>();
        var step = 1;
        while (remaining > 0)
        {
            var beforeResult = result;
            var beforeBase = current;
            var multiply = !remaining.IsEven;
            if (multiply) result = result * current % modulus;
            current = current * current % modulus;
            steps.Add(new(step++, remaining, beforeResult, beforeBase, multiply, result, current));
            remaining >>= 1;
        }
        return steps;
    }
}

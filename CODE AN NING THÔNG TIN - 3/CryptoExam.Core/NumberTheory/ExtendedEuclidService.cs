using System.Numerics;

namespace CryptoExam.Core.NumberTheory;

public sealed record EuclidDivisionStep(BigInteger Dividend, BigInteger Divisor, BigInteger Quotient, BigInteger Remainder);
public sealed record ExtendedEuclidResult(BigInteger Gcd, BigInteger X, BigInteger Y, IReadOnlyList<EuclidDivisionStep> Steps)
{
    public BigInteger? Inverse(BigInteger modulus) => Gcd == BigInteger.One ? ModularArithmetic.Normalize(X, modulus) : null;
}

public static class ExtendedEuclidService
{
    public static ExtendedEuclidResult Solve(BigInteger a, BigInteger b)
    {
        var oldR = a; var r = b;
        var oldS = BigInteger.One; var s = BigInteger.Zero;
        var oldT = BigInteger.Zero; var t = BigInteger.One;
        var steps = new List<EuclidDivisionStep>();
        while (r != 0)
        {
            var quotient = oldR / r;
            var remainder = oldR - quotient * r;
            steps.Add(new(oldR, r, quotient, remainder));
            (oldR, r) = (r, remainder);
            (oldS, s) = (s, oldS - quotient * s);
            (oldT, t) = (t, oldT - quotient * t);
        }
        if (oldR < 0) return new(-oldR, -oldS, -oldT, steps);
        return new(oldR, oldS, oldT, steps);
    }

    public static BigInteger ModularInverse(BigInteger value, BigInteger modulus)
    {
        if (modulus <= 1) throw new ArgumentOutOfRangeException(nameof(modulus), "Modulus phải > 1.");
        var result = Solve(value, modulus);
        if (result.Gcd != 1) throw new InvalidOperationException($"NO MODULAR INVERSE because gcd({value},{modulus}) = {result.Gcd}");
        return ModularArithmetic.Normalize(result.X, modulus);
    }
}

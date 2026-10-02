using System.Numerics;
using CryptoExam.Core.NumberTheory;

namespace CryptoExam.Core.RSA;

public static class RsaService
{
    public static RsaKeyPair CalculateKeys(BigInteger p, BigInteger q, BigInteger e)
    {
        if (p <= 1 || q <= 1) throw new ArgumentOutOfRangeException("p/q phải > 1.");
        var n = p * q;
        var phi = (p - 1) * (q - 1);
        var gcd = BigInteger.GreatestCommonDivisor(e, phi);
        if (gcd != 1) throw new InvalidOperationException($"e không khả nghịch: gcd(e, phi) = {gcd}");
        return new(p, q, n, phi, e, ExtendedEuclidService.ModularInverse(e, phi), gcd);
    }

    public static BigInteger Encrypt(BigInteger message, BigInteger e, BigInteger n) => ValidateMessageAndPow(message, e, n);
    public static BigInteger Decrypt(BigInteger cipher, BigInteger d, BigInteger n) => ValidateMessageAndPow(cipher, d, n);
    public static BigInteger SignStandard(BigInteger message, BigInteger d, BigInteger n) => ValidateMessageAndPow(message, d, n);
    public static BigInteger VerifyStandard(BigInteger signature, BigInteger e, BigInteger n) => ValidateMessageAndPow(signature, e, n);
    public static BigInteger SignUeh(BigInteger message, BigInteger eA, BigInteger n) => ValidateMessageAndPow(message, eA, n);
    public static BigInteger VerifyUeh(BigInteger signature, BigInteger dA, BigInteger n) => ValidateMessageAndPow(signature, dA, n);

    public static RsaWalkthroughResult Walkthrough(BigInteger p, BigInteger q, BigInteger e, BigInteger message)
    {
        var keys = CalculateKeys(p, q, e);
        var cipher = Encrypt(message, keys.E, keys.N);
        var standardSignature = SignStandard(message, keys.D, keys.N);
        var uehSignature = SignUeh(message, keys.E, keys.N);
        return new(keys, message, cipher, Decrypt(cipher, keys.D, keys.N), standardSignature,
            VerifyStandard(standardSignature, keys.E, keys.N), uehSignature, VerifyUeh(uehSignature, keys.D, keys.N));
    }

    private static BigInteger ValidateMessageAndPow(BigInteger value, BigInteger exponent, BigInteger n)
    {
        if (n <= 1) throw new ArgumentOutOfRangeException(nameof(n));
        if (value < 0 || value >= n) throw new ArgumentOutOfRangeException(nameof(value), $"Giá trị phải nằm trong [0, n-1] (n={n}).");
        return ModularArithmetic.ModPow(value, exponent, n);
    }
}

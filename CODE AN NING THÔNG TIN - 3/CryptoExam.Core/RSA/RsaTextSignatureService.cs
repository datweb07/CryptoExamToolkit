using System.Numerics;
using CryptoExam.Core.NumberTheory;

namespace CryptoExam.Core.RSA;

/// <summary>
/// RSA Digital Signature tren van ban (ky tung ky tu).
/// Ho tro 2 encoding: ASCII va A=0..Z=25.
/// Mode UEH: S = M^eA mod n, Verify: M' = S^dA mod n
/// </summary>
public static class RsaTextSignatureService
{
    public enum TextEncoding { Ascii, A0Z25 }

    public sealed record CharSignEntry(int Index, char Char, BigInteger MessageValue, string Formula, BigInteger Signature);
    public sealed record CharVerifyEntry(int Index, BigInteger Signature, BigInteger RecoveredValue, char RecoveredChar);

    public sealed record RsaTextSignResult(
        string OriginalText,
        TextEncoding Encoding,
        BigInteger[] MessageValues,
        CharSignEntry[] SignEntries,
        BigInteger[] Signatures);

    public sealed record RsaTextVerifyResult(
        BigInteger[] InputSignatures,
        CharVerifyEntry[] VerifyEntries,
        string RecoveredText,
        BigInteger[] RecoveredValues,
        bool Valid);

    /// <summary>Ky van ban (UEH mode: S = M^eA mod n).</summary>
    public static RsaTextSignResult SignText(string text, BigInteger eA, BigInteger n, TextEncoding encoding)
    {
        text = text.ToUpperInvariant();
        var messageValues = GetMessageValues(text, encoding, n);
        var entries = new CharSignEntry[text.Length];
        var sigs = new BigInteger[text.Length];
        for (var i = 0; i < text.Length; i++)
        {
            var m = messageValues[i];
            var s = RsaService.SignUeh(m, eA, n);
            entries[i] = new(i + 1, text[i], m, $"{m}^{eA} mod {n}", s);
            sigs[i] = s;
        }
        return new(text, encoding, messageValues, entries, sigs);
    }

    /// <summary>Xac thuc danh sach chu ky (UEH mode: M' = S^dA mod n).</summary>
    public static RsaTextVerifyResult VerifyText(BigInteger[] signatures, BigInteger dA, BigInteger n,
        TextEncoding encoding, string? originalText = null)
    {
        var verifyEntries = new CharVerifyEntry[signatures.Length];
        var recoveredValues = new BigInteger[signatures.Length];
        var recoveredChars = new char[signatures.Length];
        for (var i = 0; i < signatures.Length; i++)
        {
            var recovered = RsaService.VerifyUeh(signatures[i], dA, n);
            var ch = ValueToChar(recovered, encoding);
            verifyEntries[i] = new(i + 1, signatures[i], recovered, ch);
            recoveredValues[i] = recovered;
            recoveredChars[i] = ch;
        }
        var recoveredText = new string(recoveredChars);
        var valid = originalText == null || recoveredText.Equals(originalText.ToUpperInvariant(), StringComparison.Ordinal);
        return new(signatures, verifyEntries, recoveredText, recoveredValues, valid);
    }

    // =========================================================
    // Helpers
    // =========================================================

    private static BigInteger[] GetMessageValues(string text, TextEncoding encoding, BigInteger n)
    {
        var result = new BigInteger[text.Length];
        for (var i = 0; i < text.Length; i++)
        {
            var c = text[i];
            BigInteger m = encoding switch
            {
                TextEncoding.Ascii => (int)c,
                TextEncoding.A0Z25 => (c - 'A'),
                _ => throw new ArgumentOutOfRangeException(nameof(encoding))
            };
            if (m < 0 || m >= n)
                throw new ArgumentException($"Ky tu '{c}' co gia tri M={m} >= n={n}. RSA textbook yeu cau 0 <= M < n.");
            result[i] = m;
        }
        return result;
    }

    private static char ValueToChar(BigInteger value, TextEncoding encoding) => encoding switch
    {
        TextEncoding.Ascii => (value >= 0 && value <= 127) ? (char)(int)value : '?',
        TextEncoding.A0Z25 => (value >= 0 && value <= 25) ? (char)('A' + (int)value) : '?',
        _ => '?'
    };

    // =========================================================
    // Exam Helpers
    // =========================================================

    /// <summary>Tim 2 so nguyen to gan N nhat theo quy uoc: prev < N < next.</summary>
    public static (BigInteger? Prev, BigInteger N, BigInteger? Next, bool NIsItself) FindNearbyPrimes(BigInteger birthday)
    {
        bool nIsPrime = PrimeUtils.IsPrime(birthday);
        BigInteger? prev = null, next = null;
        for (var i = birthday - 1; i > 1; i--)
        {
            if (PrimeUtils.IsPrime(i)) { prev = i; break; }
        }
        for (var i = birthday + 1; i < birthday + 1000; i++)
        {
            if (PrimeUtils.IsPrime(i)) { next = i; break; }
        }
        return (prev, birthday, next, nIsPrime);
    }

    /// <summary>Tim danh sach e hop le cho phi(n): 1 < e < phi, gcd(e,phi)=1.</summary>
    public static List<BigInteger> FindValidE(BigInteger phi, int maxCount = 20)
    {
        var result = new List<BigInteger>();
        for (var e = new BigInteger(2); e < phi && result.Count < maxCount; e++)
        {
            if (BigInteger.GreatestCommonDivisor(e, phi) == 1)
                result.Add(e);
        }
        return result;
    }

    /// <summary>Kiem tra e co hop le voi phi khong.</summary>
    public static (bool Valid, BigInteger Gcd) CheckE(BigInteger e, BigInteger phi)
    {
        var gcd = BigInteger.GreatestCommonDivisor(e, phi);
        return (gcd == 1, gcd);
    }
}

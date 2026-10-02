using System.Numerics;

namespace CryptoExam.Core.RSA;

public sealed record RsaListItem(int Index, BigInteger Input, BigInteger Output);

public static class RsaSignatureService
{
    public static IReadOnlyList<RsaListItem> SignUeh(IEnumerable<BigInteger> messages, BigInteger eA, BigInteger n) =>
        messages.Select((m, i) => new RsaListItem(i + 1, m, RsaService.SignUeh(m, eA, n))).ToList();

    public static IReadOnlyList<RsaListItem> VerifyUeh(IEnumerable<BigInteger> signatures, BigInteger dA, BigInteger n) =>
        signatures.Select((s, i) => new RsaListItem(i + 1, s, RsaService.VerifyUeh(s, dA, n))).ToList();
}

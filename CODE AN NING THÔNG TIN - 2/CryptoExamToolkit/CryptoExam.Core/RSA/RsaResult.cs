// File: CryptoExam.Core/RSA/RsaResult.cs
using System.Numerics;

namespace CryptoExam.Core.RSA
{
    /// <summary>
    /// Kết quả sign/verify RSA cho một giá trị.
    /// </summary>
    public record RsaResult
    {
        public BigInteger Message { get; init; }
        public BigInteger Signature { get; init; }
        public BigInteger Verified { get; init; }
        public bool IsValid => Message == Verified;
    }
}

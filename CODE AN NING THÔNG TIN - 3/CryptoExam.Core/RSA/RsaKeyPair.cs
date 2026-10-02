using System.Numerics;

namespace CryptoExam.Core.RSA;

public sealed record RsaKeyPair(BigInteger P, BigInteger Q, BigInteger N, BigInteger Phi, BigInteger E, BigInteger D, BigInteger Gcd);
public sealed record RsaWalkthroughResult(RsaKeyPair Keys, BigInteger Message, BigInteger Ciphertext, BigInteger Decrypted, BigInteger StandardSignature, BigInteger StandardVerified, BigInteger UehSignature, BigInteger UehVerified);

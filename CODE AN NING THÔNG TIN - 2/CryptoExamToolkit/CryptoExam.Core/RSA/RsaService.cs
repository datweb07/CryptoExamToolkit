// File: CryptoExam.Core/RSA/RsaService.cs
using System;
using System.Numerics;
using CryptoExam.Core.NumberTheory;

namespace CryptoExam.Core.RSA
{
    /// <summary>
    /// Standard RSA:
    ///   public key  = (e, n)
    ///   private key = (d, n)
    ///
    ///   Encrypt: C = M^e mod n
    ///   Decrypt: M = C^d mod n
    ///
    ///   Sign:    S = M^d mod n   (dùng private key)
    ///   Verify:  M'= S^e mod n   (dùng public key)
    /// </summary>
    public static class RsaService
    {
        /// <summary>Standard RSA Encrypt: C = M^e mod n</summary>
        public static BigInteger Encrypt(BigInteger message, BigInteger e, BigInteger n)
            => ModularArithmetic.ModPow(message, e, n);

        /// <summary>Standard RSA Decrypt: M = C^d mod n</summary>
        public static BigInteger Decrypt(BigInteger ciphertext, BigInteger d, BigInteger n)
            => ModularArithmetic.ModPow(ciphertext, d, n);

        /// <summary>Standard RSA Sign: S = M^d mod n (private key)</summary>
        public static BigInteger Sign(BigInteger message, BigInteger d, BigInteger n)
            => ModularArithmetic.ModPow(message, d, n);

        /// <summary>Standard RSA Verify: M' = S^e mod n (public key)</summary>
        public static BigInteger Verify(BigInteger signature, BigInteger e, BigInteger n)
            => ModularArithmetic.ModPow(signature, e, n);
    }
}

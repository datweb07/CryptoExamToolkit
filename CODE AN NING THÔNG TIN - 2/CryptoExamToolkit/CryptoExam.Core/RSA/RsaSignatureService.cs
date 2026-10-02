// File: CryptoExam.Core/RSA/RsaSignatureService.cs
using System;
using System.Numerics;
using CryptoExam.Core.NumberTheory;

namespace CryptoExam.Core.RSA
{
    /// <summary>
    /// UEH/Slide RSA Signature Mode.
    ///
    /// Theo slide môn học:
    ///   eA = private key (dùng để KÝ)
    ///   dA = eA^-1 mod phi(n) = public key (dùng để XÁC THỰC)
    ///
    ///   Sign:   S = M^eA mod n   (ký bằng eA)
    ///   Verify: M'= S^dA mod n   (xác thực bằng dA)
    ///
    /// KHÁC với Standard RSA:
    ///   - Standard: Sign = M^d (d private), Verify = S^e (e public)
    ///   - UEH: Sign = M^eA (eA là private), Verify = S^dA (dA là public)
    ///
    /// Test vector:
    ///   p=13, q=17, eA=7, n=221, phi=192, dA=55
    ///   M=64  -> S=38  -> Verify=64
    ///   M=112 -> S=5   -> Verify=112
    ///   M=97  -> S=7   -> Verify=97
    /// </summary>
    public static class RsaSignatureService
    {
        /// <summary>
        /// UEH Sign: S = M^eA mod n
        /// </summary>
        public static BigInteger UehSign(BigInteger message, BigInteger eA, BigInteger n)
            => ModularArithmetic.ModPow(message, eA, n);

        /// <summary>
        /// UEH Verify: M' = S^dA mod n
        /// </summary>
        public static BigInteger UehVerify(BigInteger signature, BigInteger dA, BigInteger n)
            => ModularArithmetic.ModPow(signature, dA, n);
    }
}

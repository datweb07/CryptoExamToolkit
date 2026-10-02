// File: CryptoExam.Core/RSA/RsaKeyPair.cs
using System;
using System.Numerics;
using CryptoExam.Core.NumberTheory;

namespace CryptoExam.Core.RSA
{
    /// <summary>
    /// RSA Key Pair tính từ p, q, e.
    ///
    /// Standard RSA convention:
    ///   public key  = (e, n)  - dùng để mã hóa hoặc verify signature
    ///   private key = (d, n)  - dùng để giải mã hoặc ký
    ///
    /// UEH/Slide convention:
    ///   eA = private key  (dùng để ký, gọi là eA trong slide)
    ///   dA = public key   (dA = eA^-1 mod phi, dùng để xác thực)
    ///   -> Lưu ý: trong slide, eA đóng vai trò private và dA đóng vai trò public
    /// </summary>
    public class RsaKeyPair
    {
        public BigInteger P { get; }
        public BigInteger Q { get; }
        public BigInteger N { get; }
        public BigInteger Phi { get; }   // phi(n) = (p-1)(q-1)
        public BigInteger E { get; }     // exponent e (standard: public; UEH: private = eA)
        public BigInteger D { get; }     // d = e^-1 mod phi (standard: private; UEH: public = dA)
        public BigInteger GcdEPhi { get; }

        public RsaKeyPair(BigInteger p, BigInteger q, BigInteger e)
        {
            P = p;
            Q = q;
            N = p * q;
            Phi = (p - 1) * (q - 1);
            E = e;
            GcdEPhi = GcdService.Gcd(e, Phi);

            if (GcdEPhi != 1)
                throw new ArgumentException(
                    $"gcd(e={e}, phi={Phi}) = {GcdEPhi} ≠ 1. " +
                    "e phải nguyên tố cùng nhau với phi(n).");

            D = ExtendedEuclidService.GetInverse(e, Phi);
        }
    }
}

using System;
using System.Numerics;
using CryptoExam.Core.NumberTheory;

namespace CryptoExam.Core.RSA
{
    public static class RsaService
    {
        public static RsaKeyPair GenerateKeys(BigInteger p, BigInteger q, BigInteger e)
        {
            BigInteger n = p * q;
            BigInteger phi = (p - 1) * (q - 1);
            
            var euclid = ExtendedEuclidService.CalculateWithSteps(e, phi);
            
            BigInteger d = euclid.x % phi;
            if (d < 0) d += phi;

            return new RsaKeyPair
            {
                P = p,
                Q = q,
                N = n,
                Phi = phi,
                E = e,
                D = d,
                Gcd = euclid.gcd
            };
        }

        // --- Standard Convention ---
        public static BigInteger EncryptStandard(BigInteger m, BigInteger e, BigInteger n)
        {
            return BigInteger.ModPow(m, e, n);
        }

        public static BigInteger DecryptStandard(BigInteger c, BigInteger d, BigInteger n)
        {
            return BigInteger.ModPow(c, d, n);
        }
        
        public static BigInteger SignStandard(BigInteger m, BigInteger d, BigInteger n)
        {
            return BigInteger.ModPow(m, d, n);
        }

        public static BigInteger VerifyStandard(BigInteger s, BigInteger e, BigInteger n)
        {
            return BigInteger.ModPow(s, e, n);
        }

        // --- UEH Convention ---
        // Slide uses eA as private key for signing, dA as public key for verification.
        // Sign: s = m^eA mod n
        // Verify: m' = s^dA mod n
        public static BigInteger SignUeh(BigInteger m, BigInteger eA, BigInteger n)
        {
            return BigInteger.ModPow(m, eA, n);
        }

        public static BigInteger VerifyUeh(BigInteger s, BigInteger dA, BigInteger n)
        {
            return BigInteger.ModPow(s, dA, n);
        }
    }
}

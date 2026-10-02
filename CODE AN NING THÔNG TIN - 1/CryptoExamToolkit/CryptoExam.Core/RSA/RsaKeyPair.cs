using System.Numerics;

namespace CryptoExam.Core.RSA
{
    public class RsaKeyPair
    {
        public BigInteger P { get; set; }
        public BigInteger Q { get; set; }
        public BigInteger N { get; set; }
        public BigInteger Phi { get; set; }
        public BigInteger E { get; set; }
        public BigInteger D { get; set; }
        public BigInteger Gcd { get; set; }
    }
}

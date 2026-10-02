namespace CryptoExam.Core.DES
{
    public static class TripleDesService
    {
        // EDE scheme: C = E_K1(D_K2(E_K1(P)))
        public static string EncryptEde(string plainTextHex, string k1Hex, string k2Hex)
        {
            var t1 = DesCipher.EncryptTrace(plainTextHex, k1Hex);
            var t2 = DesCipher.DecryptTrace(t1.CipherTextHex, k2Hex);
            var t3 = DesCipher.EncryptTrace(t2.CipherTextHex, k1Hex);
            return t3.CipherTextHex;
        }

        public static string DecryptEde(string cipherTextHex, string k1Hex, string k2Hex)
        {
            var t1 = DesCipher.DecryptTrace(cipherTextHex, k1Hex);
            var t2 = DesCipher.EncryptTrace(t1.CipherTextHex, k2Hex);
            var t3 = DesCipher.DecryptTrace(t2.CipherTextHex, k1Hex);
            return t3.CipherTextHex;
        }
        
        // EDE3 scheme: C = E_K3(D_K2(E_K1(P)))
        public static string EncryptEde3(string plainTextHex, string k1Hex, string k2Hex, string k3Hex)
        {
            var t1 = DesCipher.EncryptTrace(plainTextHex, k1Hex);
            var t2 = DesCipher.DecryptTrace(t1.CipherTextHex, k2Hex);
            var t3 = DesCipher.EncryptTrace(t2.CipherTextHex, k3Hex);
            return t3.CipherTextHex;
        }

        public static string DecryptEde3(string cipherTextHex, string k1Hex, string k2Hex, string k3Hex)
        {
            var t1 = DesCipher.DecryptTrace(cipherTextHex, k3Hex);
            var t2 = DesCipher.EncryptTrace(t1.CipherTextHex, k2Hex);
            var t3 = DesCipher.DecryptTrace(t2.CipherTextHex, k1Hex);
            return t3.CipherTextHex;
        }
    }
}

namespace CryptoExam.Core.Classical
{
    public static class DoubleTranspositionCipher
    {
        public static string Encrypt(string text, string key1, string key2)
        {
            string pass1 = ColumnarTranspositionCipher.Encrypt(text, key1);
            return ColumnarTranspositionCipher.Encrypt(pass1, key2);
        }

        public static string Decrypt(string cipherText, string key1, string key2)
        {
            string pass1 = ColumnarTranspositionCipher.Decrypt(cipherText, key2);
            return ColumnarTranspositionCipher.Decrypt(pass1, key1);
        }
    }
}

namespace CryptoExam.Core.Classical;

public static class DoubleTranspositionCipher
{
    public static string Encrypt(string plaintext, string key1, string key2) =>
        new ColumnarTranspositionCipher(key2).Encrypt(new ColumnarTranspositionCipher(key1).Encrypt(plaintext));

    public static string Decrypt(string ciphertext, string key1, string key2) =>
        new ColumnarTranspositionCipher(key1).Decrypt(new ColumnarTranspositionCipher(key2).Decrypt(ciphertext));
}

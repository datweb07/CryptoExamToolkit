using CryptoExam.Core.Common;

namespace CryptoExam.Core.Classical;

public static class CaesarCipher
{
    public static string Encrypt(string plaintext, int key, bool preserveSpaces = true) =>
        TextUtils.TransformLetters(plaintext, value => value + key, preserveSpaces);

    public static string Decrypt(string ciphertext, int key, bool preserveSpaces = true) =>
        TextUtils.TransformLetters(ciphertext, value => value - key, preserveSpaces);

    public static IReadOnlyDictionary<int, string> BruteForce(string ciphertext, bool preserveSpaces = true) =>
        Enumerable.Range(1, 25).ToDictionary(key => key, key => Decrypt(ciphertext, key, preserveSpaces));
}

using CryptoExam.Core.Common;

namespace CryptoExam.Core.Classical;

public static class OneTimePadCipher
{
    public static string Encrypt(string plaintext, string key) => Transform(plaintext, key, 1);
    public static string Decrypt(string ciphertext, string key) => Transform(ciphertext, key, -1);

    private static string Transform(string text, string key, int direction)
    {
        var message = TextUtils.LettersOnly(text);
        var normalizedKey = TextUtils.LettersOnly(key);
        if (message.Length != normalizedKey.Length)
            throw new ArgumentException("OTP yêu cầu key có cùng số chữ cái với message; key không được lặp.");
        return string.Concat(message.Select((c, i) => (char)('A' + TextUtils.Mod(c - 'A' + direction * (normalizedKey[i] - 'A'), 26))));
    }
}

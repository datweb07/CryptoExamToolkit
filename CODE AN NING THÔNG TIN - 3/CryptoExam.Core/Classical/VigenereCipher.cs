using System.Text;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.Classical;

public static class VigenereCipher
{
    public static string ExpandKey(string text, string key)
    {
        var letters = TextUtils.LettersOnly(text);
        var normalizedKey = TextUtils.LettersOnly(key);
        if (normalizedKey.Length == 0) throw new ArgumentException("Khóa phải có ít nhất một chữ cái.");
        return string.Concat(Enumerable.Range(0, letters.Length).Select(i => normalizedKey[i % normalizedKey.Length]));
    }

    public static string Encrypt(string plaintext, string key, bool preserveSpaces = true) => Transform(plaintext, key, 1, preserveSpaces);
    public static string Decrypt(string ciphertext, string key, bool preserveSpaces = true) => Transform(ciphertext, key, -1, preserveSpaces);

    private static string Transform(string text, string key, int direction, bool preserveSpaces)
    {
        var normalizedKey = TextUtils.LettersOnly(key);
        if (normalizedKey.Length == 0) throw new ArgumentException("Khóa phải có ít nhất một chữ cái.");
        var result = new StringBuilder();
        var keyIndex = 0;
        foreach (var c in text.ToUpperInvariant())
        {
            if (c is >= 'A' and <= 'Z')
            {
                var shift = normalizedKey[keyIndex++ % normalizedKey.Length] - 'A';
                result.Append((char)('A' + TextUtils.Mod(c - 'A' + direction * shift, 26)));
            }
            else if (preserveSpaces && char.IsWhiteSpace(c)) result.Append(c);
        }
        return result.ToString();
    }
}

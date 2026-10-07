using System.Text;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.Classical;

public sealed class MonoalphabeticCipher
{
    public string PlainAlphabet { get; }
    public string CipherAlphabet { get; }

    public MonoalphabeticCipher(string plainAlphabet, string cipherAlphabet)
    {
        PlainAlphabet = Validate(plainAlphabet, nameof(plainAlphabet));
        CipherAlphabet = Validate(cipherAlphabet, nameof(cipherAlphabet));
    }

    public string Encrypt(string text, bool preserveSpaces = true) => Transform(text, PlainAlphabet, CipherAlphabet, preserveSpaces);
    public string Decrypt(string text, bool preserveSpaces = true) => Transform(text, CipherAlphabet, PlainAlphabet, preserveSpaces);
    public IReadOnlyList<(char Plain, char Cipher)> Mapping() => PlainAlphabet.Zip(CipherAlphabet).ToList();

    private static string Validate(string alphabet, string name)
    {
        var value = TextUtils.LettersOnly(alphabet);
        if (value.Length != 26 || value.Distinct().Count() != 26)
            throw new ArgumentException("Bảng chữ cái phải gồm đúng 26 chữ A-Z, không trùng.", name);
        return value;
    }

    private static string Transform(string text, string source, string destination, bool preserveSpaces)
    {
        var result = new StringBuilder();
        foreach (var c in text.ToUpperInvariant())
        {
            var index = source.IndexOf(c);
            if (index >= 0) result.Append(destination[index]);
            else if (preserveSpaces && char.IsWhiteSpace(c)) result.Append(c);
        }
        return result.ToString();
    }
}

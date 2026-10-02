using System.Text;

namespace CryptoExam.Core.Common;

public static class TextUtils
{
    public const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public static string LettersOnly(string input, bool mergeJ = false)
    {
        var result = new StringBuilder();
        foreach (var character in input.ToUpperInvariant())
            if (character is >= 'A' and <= 'Z')
                result.Append(mergeJ && character == 'J' ? 'I' : character);
        return result.ToString();
    }

    public static string TransformLetters(string input, Func<int, int> transform, bool preserveSpaces)
    {
        var result = new StringBuilder();
        foreach (var character in input.ToUpperInvariant())
        {
            if (character is >= 'A' and <= 'Z')
                result.Append((char)('A' + Mod(transform(character - 'A'), 26)));
            else if (preserveSpaces && char.IsWhiteSpace(character))
                result.Append(character);
        }
        return result.ToString();
    }

    public static int Mod(int value, int modulus) => (value % modulus + modulus) % modulus;
}

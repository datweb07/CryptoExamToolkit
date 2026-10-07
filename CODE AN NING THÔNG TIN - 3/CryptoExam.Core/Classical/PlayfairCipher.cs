using System.Text;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.Classical;

public sealed class PlayfairCipher
{
    private readonly char[,] _matrix = new char[5, 5];
    private readonly Dictionary<char, (int Row, int Column)> _positions = new();
    public string MatrixText => string.Join(Environment.NewLine, Enumerable.Range(0, 5).Select(r => string.Join(' ', Enumerable.Range(0, 5).Select(c => _matrix[r, c]))));

    public PlayfairCipher(string key)
    {
        var characters = (TextUtils.LettersOnly(key, true) + "ABCDEFGHIKLMNOPQRSTUVWXYZ").Distinct().ToArray();
        for (var i = 0; i < 25; i++)
        {
            _matrix[i / 5, i % 5] = characters[i];
            _positions[characters[i]] = (i / 5, i % 5);
        }
        _positions['J'] = _positions['I'];
    }

    public IReadOnlyList<string> PreparePairs(string plaintext)
    {
        var text = TextUtils.LettersOnly(plaintext, true);
        var pairs = new List<string>();
        for (var i = 0; i < text.Length;)
        {
            var first = text[i++];
            char second;
            if (i >= text.Length) second = 'X';
            else if (text[i] == first) second = first == 'X' ? 'Q' : 'X';
            else second = text[i++];
            pairs.Add($"{first}{second}");
        }
        return pairs;
    }

    public string Encrypt(string plaintext) => string.Concat(PreparePairs(plaintext).Select(pair => TransformPair(pair, 1)));
    public string Decrypt(string ciphertext)
    {
        var text = TextUtils.LettersOnly(ciphertext, true);
        if (text.Length % 2 != 0) throw new ArgumentException("Bản mã Playfair phải có độ dài chẵn.");
        return string.Concat(Enumerable.Range(0, text.Length / 2).Select(i => TransformPair(text.Substring(i * 2, 2), -1)));
    }
    public string EncryptPair(string pair) => TransformPair(pair, 1);
    public string DecryptPair(string pair) => TransformPair(pair, -1);

    private string TransformPair(string pair, int direction)
    {
        var value = TextUtils.LettersOnly(pair, true);
        if (value.Length != 2) throw new ArgumentException("Cặp ký tự Playfair phải gồm đúng 2 chữ cái.");
        var a = _positions[value[0]];
        var b = _positions[value[1]];
        if (a.Row == b.Row)
            return $"{_matrix[a.Row, TextUtils.Mod(a.Column + direction, 5)]}{_matrix[b.Row, TextUtils.Mod(b.Column + direction, 5)]}";
        if (a.Column == b.Column)
            return $"{_matrix[TextUtils.Mod(a.Row + direction, 5), a.Column]}{_matrix[TextUtils.Mod(b.Row + direction, 5), b.Column]}";
        return $"{_matrix[a.Row, b.Column]}{_matrix[b.Row, a.Column]}";
    }
}

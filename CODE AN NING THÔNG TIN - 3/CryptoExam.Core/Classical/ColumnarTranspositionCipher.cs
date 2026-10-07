namespace CryptoExam.Core.Classical;

public sealed class ColumnarTranspositionCipher
{
    public string Key { get; }
    public IReadOnlyList<int> ColumnOrder { get; }

    public ColumnarTranspositionCipher(string key)
    {
        Key = new string(key.ToUpperInvariant().Where(char.IsLetterOrDigit).ToArray());
        if (Key.Length < 2) throw new ArgumentException("Từ khóa phải có ít nhất 2 ký tự.");
        // ThenBy index đảm bảo ký tự khóa lặp được xếp ổn định từ trái sang phải.
        ColumnOrder = Key.Select((c, i) => (Character: c, Index: i)).OrderBy(x => x.Character).ThenBy(x => x.Index).Select(x => x.Index).ToArray();
    }

    public string Encrypt(string plaintext)
    {
        var columns = Enumerable.Range(0, Key.Length).Select(_ => new List<char>()).ToArray();
        for (var i = 0; i < plaintext.Length; i++) columns[i % Key.Length].Add(plaintext[i]);
        return string.Concat(ColumnOrder.SelectMany(column => columns[column]));
    }

    public string Decrypt(string ciphertext)
    {
        var baseLength = ciphertext.Length / Key.Length;
        var extra = ciphertext.Length % Key.Length;
        var columns = new string[Key.Length];
        var offset = 0;
        foreach (var column in ColumnOrder)
        {
            var length = baseLength + (column < extra ? 1 : 0);
            columns[column] = ciphertext.Substring(offset, length);
            offset += length;
        }
        var result = new List<char>(ciphertext.Length);
        for (var row = 0; result.Count < ciphertext.Length; row++)
            for (var column = 0; column < Key.Length; column++)
                if (row < columns[column].Length) result.Add(columns[column][row]);
        return new string(result.ToArray());
    }

    public string Matrix(string text) => string.Join(Environment.NewLine,
        Enumerable.Range(0, (text.Length + Key.Length - 1) / Key.Length)
            .Select(row => string.Join(' ', Enumerable.Range(0, Key.Length).Select(column => row * Key.Length + column < text.Length ? text[row * Key.Length + column] : '·'))));
}

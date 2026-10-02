namespace CryptoExam.Core.Classical;

public static class MatrixTranspositionCipher
{
    public static string Matrix(string text, int columns)
    {
        if (columns < 2) throw new ArgumentOutOfRangeException(nameof(columns));
        return string.Join(Environment.NewLine,
            Enumerable.Range(0, (text.Length + columns - 1) / columns)
                .Select(row => string.Join(' ', Enumerable.Range(0, columns)
                    .Select(column => row * columns + column < text.Length ? text[row * columns + column] : '·'))));
    }

    public static string Encrypt(string text, int columns)
    {
        if (columns < 2) throw new ArgumentOutOfRangeException(nameof(columns));
        var result = new List<char>(text.Length);
        for (var column = 0; column < columns; column++)
            for (var index = column; index < text.Length; index += columns) result.Add(text[index]);
        return new string(result.ToArray());
    }

    public static string Decrypt(string text, int columns)
    {
        if (columns < 2) throw new ArgumentOutOfRangeException(nameof(columns));
        var rows = (text.Length + columns - 1) / columns;
        var shortColumns = rows * columns - text.Length;
        var columnData = new string[columns];
        var offset = 0;
        for (var column = 0; column < columns; column++)
        {
            var length = rows - (column >= columns - shortColumns ? 1 : 0);
            columnData[column] = text.Substring(offset, length);
            offset += length;
        }
        var result = new List<char>(text.Length);
        for (var row = 0; row < rows; row++)
            for (var column = 0; column < columns; column++)
                if (row < columnData[column].Length) result.Add(columnData[column][row]);
        return new string(result.ToArray());
    }
}

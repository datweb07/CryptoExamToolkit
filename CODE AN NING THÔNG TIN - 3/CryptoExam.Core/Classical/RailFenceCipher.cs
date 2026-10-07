namespace CryptoExam.Core.Classical;

public static class RailFenceCipher
{
    public static string Encrypt(string text, int rails)
    {
        Validate(text, rails);
        var rows = Enumerable.Range(0, rails).Select(_ => new List<char>()).ToArray();
        foreach (var (c, rail) in text.Zip(Pattern(text.Length, rails))) rows[rail].Add(c);
        return string.Concat(rows.SelectMany(row => row));
    }

    public static string Decrypt(string cipher, int rails)
    {
        Validate(cipher, rails);
        var pattern = Pattern(cipher.Length, rails).ToArray();
        var counts = pattern.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
        var queues = new Queue<char>[rails];
        var offset = 0;
        for (var rail = 0; rail < rails; rail++)
        {
            var count = counts.GetValueOrDefault(rail);
            queues[rail] = new Queue<char>(cipher.Substring(offset, count));
            offset += count;
        }
        return string.Concat(pattern.Select(rail => queues[rail].Dequeue()));
    }

    private static IEnumerable<int> Pattern(int length, int rails)
    {
        var rail = 0; var direction = 1;
        for (var i = 0; i < length; i++)
        {
            yield return rail;
            if (rail == 0) direction = 1;
            else if (rail == rails - 1) direction = -1;
            rail += direction;
        }
    }

    private static void Validate(string text, int rails)
    {
        if (rails < 2) throw new ArgumentOutOfRangeException(nameof(rails), "Số hàng phải >= 2.");
        if (rails > text.Length && text.Length > 0) throw new ArgumentOutOfRangeException(nameof(rails), "Số hàng không được lớn hơn độ dài bản văn.");
    }
}

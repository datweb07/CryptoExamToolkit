using System.Numerics;

namespace CryptoExam.Core.Common;

/// <summary>Chuyển đổi số nguyên giữa các cơ số từ 2 đến 16.</summary>
public static class NumberBaseConverter
{
    private const string Digits = "0123456789ABCDEF";

    public static BigInteger Parse(string value, int numberBase)
    {
        ValidateBase(numberBase);
        if (string.IsNullOrWhiteSpace(value))
            throw new FormatException("Vui lòng nhập một số cần chuyển đổi.");

        value = value.Trim().ToUpperInvariant();
        var sign = 1;
        if (value[0] is '+' or '-')
        {
            if (value[0] == '-') sign = -1;
            value = value[1..];
        }

        if (value.Length == 0)
            throw new FormatException("Số nhập vào không hợp lệ.");

        var result = BigInteger.Zero;
        foreach (var character in value)
        {
            var digit = Digits.IndexOf(character);
            if (digit < 0 || digit >= numberBase)
                throw new FormatException($"Ký tự '{character}' không hợp lệ trong hệ cơ số {numberBase}.");
            result = result * numberBase + digit;
        }
        return sign < 0 ? -result : result;
    }

    public static string Format(BigInteger value, int numberBase)
    {
        ValidateBase(numberBase);
        if (value.IsZero) return "0";

        var negative = value.Sign < 0;
        value = BigInteger.Abs(value);
        var result = new List<char>();
        while (value > 0)
        {
            value = BigInteger.DivRem(value, numberBase, out var remainder);
            result.Add(Digits[(int)remainder]);
        }
        if (negative) result.Add('-');
        result.Reverse();
        return new string(result.ToArray());
    }

    private static void ValidateBase(int numberBase)
    {
        if (numberBase is < 2 or > 16)
            throw new ArgumentOutOfRangeException(nameof(numberBase), "Cơ số phải nằm trong khoảng từ 2 đến 16.");
    }
}

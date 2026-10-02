using System.Globalization;

namespace CryptoExam.Core.Common;

public static class HexUtils
{
    public static string Normalize(string input, int? exactDigits = null)
    {
        var value = new string(input.Trim().Replace("0x", "", StringComparison.OrdinalIgnoreCase)
            .Where(c => !char.IsWhiteSpace(c) && c is not '-' and not ':').ToArray()).ToUpperInvariant();
        if (value.Length == 0 || value.Any(c => !Uri.IsHexDigit(c)))
            throw new FormatException("Giá trị hex không hợp lệ.");
        if (exactDigits.HasValue && value.Length != exactDigits.Value)
            throw new FormatException($"Cần đúng {exactDigits.Value} ký tự hex.");
        return value;
    }

    public static ulong ToUInt64(string input, int digits) => ulong.Parse(Normalize(input, digits), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
    public static string ToFixedHex(ulong value, int digits) => value.ToString($"X{digits}", CultureInfo.InvariantCulture);
    public static byte[] ToBytes(string input) => Convert.FromHexString(Normalize(input));
    public static string FromBytes(ReadOnlySpan<byte> bytes) => Convert.ToHexString(bytes);
    public static string ToBinary(ulong value, int width) => Convert.ToString((long)value, 2).PadLeft(width, '0')[^width..];
    public static string HexToBinary(string hex) => string.Concat(Normalize(hex).Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
    public static string BinaryToHex(string binary)
    {
        binary = new string(binary.Where(c => !char.IsWhiteSpace(c)).ToArray());
        if (binary.Length == 0 || binary.Length % 4 != 0 || binary.Any(c => c is not '0' and not '1'))
            throw new FormatException("Chuỗi nhị phân phải chỉ chứa 0/1 và có độ dài chia hết cho 4.");
        return string.Concat(Enumerable.Range(0, binary.Length / 4)
            .Select(i => Convert.ToInt32(binary.Substring(i * 4, 4), 2).ToString("X")));
    }
}

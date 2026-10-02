using System.Text;

namespace CryptoExam.Core.DES;

public enum DesPaddingMode { None, Zero, Pkcs7 }
public sealed record DesAsciiBlock(int Number, string InputHex, string OutputHex);
public sealed record DesAsciiDecryptResult(IReadOnlyList<DesAsciiBlock> Blocks, string Text);

public static class DesAsciiService
{
    public static IReadOnlyList<DesAsciiBlock> Encrypt(string text, string keyHex, DesPaddingMode padding)
    {
        var bytes = Encoding.ASCII.GetBytes(text);
        var remainder = bytes.Length % 8;
        if (remainder != 0 || padding == DesPaddingMode.Pkcs7)
        {
            if (padding == DesPaddingMode.None) throw new ArgumentException("Text phải đủ block 8 byte hoặc chọn padding.");
            var paddingLength = remainder == 0 ? 8 : 8 - remainder;
            Array.Resize(ref bytes, bytes.Length + paddingLength);
            if (padding == DesPaddingMode.Pkcs7) Array.Fill(bytes, (byte)paddingLength, bytes.Length - paddingLength, paddingLength);
        }
        return Enumerable.Range(0, bytes.Length / 8).Select(i =>
        {
            var input = Convert.ToHexString(bytes.AsSpan(i * 8, 8));
            return new DesAsciiBlock(i + 1, input, DesCipher.Encrypt(input, keyHex));
        }).ToList();
    }

    public static DesAsciiDecryptResult Decrypt(string ciphertextHex, string keyHex, DesPaddingMode padding)
    {
        var normalized = Common.HexUtils.Normalize(ciphertextHex);
        if (normalized.Length % 16 != 0) throw new ArgumentException("Ciphertext phải gồm các block 16 ký tự hex.");
        var blocks = Enumerable.Range(0, normalized.Length / 16).Select(i =>
        {
            var input = normalized.Substring(i * 16, 16);
            return new DesAsciiBlock(i + 1, input, DesCipher.Decrypt(input, keyHex));
        }).ToList();
        var bytes = blocks.SelectMany(block => Convert.FromHexString(block.OutputHex)).ToArray();
        if (padding == DesPaddingMode.Zero) bytes = bytes.TakeWhileLastAwareZeroTrim();
        else if (padding == DesPaddingMode.Pkcs7 && bytes.Length > 0)
        {
            var count = bytes[^1];
            if (count is < 1 or > 8 || bytes.TakeLast(count).Any(value => value != count)) throw new InvalidOperationException("PKCS#7 padding không hợp lệ.");
            Array.Resize(ref bytes, bytes.Length - count);
        }
        return new(blocks, Encoding.ASCII.GetString(bytes));
    }

    private static byte[] TakeWhileLastAwareZeroTrim(this byte[] bytes)
    {
        var length = bytes.Length;
        while (length > 0 && bytes[length - 1] == 0) length--;
        return bytes[..length];
    }
}

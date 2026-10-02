using System.Security.Cryptography;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.AES;

public static class AesService
{
    public static string QuickEncrypt(string blockHex, string keyHex) => Quick(blockHex, keyHex, true);
    public static string QuickDecrypt(string blockHex, string keyHex) => Quick(blockHex, keyHex, false);

    public static AesTraceResult TraceEncrypt128(string plaintextHex, string keyHex)
    {
        var state = HexUtils.ToBytes(HexUtils.Normalize(plaintextHex, 32));
        var keys = AesKeyExpansion.Expand128(HexUtils.ToBytes(HexUtils.Normalize(keyHex, 32)));
        var traces = new List<AesRoundTrace>();
        AesTransformations.AddRoundKey(state, keys[0]);
        traces.Add(new(0, keys[0], null, null, null, (byte[])state.Clone()));
        for (var round = 1; round <= 10; round++)
        {
            AesTransformations.SubBytes(state);
            var sub = (byte[])state.Clone();
            AesTransformations.ShiftRows(state);
            var shift = (byte[])state.Clone();
            byte[]? mix = null;
            if (round != 10)
            {
                AesTransformations.MixColumns(state);
                mix = (byte[])state.Clone();
            }
            AesTransformations.AddRoundKey(state, keys[round]);
            traces.Add(new(round, keys[round], sub, shift, mix, (byte[])state.Clone()));
        }
        return new(HexUtils.Normalize(plaintextHex, 32), traces, Convert.ToHexString(state));
    }

    private static string Quick(string blockHex, string keyHex, bool encrypt)
    {
        var input = HexUtils.ToBytes(HexUtils.Normalize(blockHex, 32));
        var key = HexUtils.ToBytes(keyHex);
        if (key.Length is not (16 or 24 or 32)) throw new ArgumentException("AES key phải dài 128/192/256 bit.");
        using var aes = Aes.Create();
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.None;
        aes.Key = key;
        using var transform = encrypt ? aes.CreateEncryptor() : aes.CreateDecryptor();
        return Convert.ToHexString(transform.TransformFinalBlock(input, 0, input.Length));
    }

    public static string FormatState(byte[] state) => string.Join(Environment.NewLine,
        Enumerable.Range(0, 4).Select(row => string.Join(' ', Enumerable.Range(0, 4).Select(column => state[row + 4 * column].ToString("X2")))));
}

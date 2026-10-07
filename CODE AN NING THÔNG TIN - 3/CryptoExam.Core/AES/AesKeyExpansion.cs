namespace CryptoExam.Core.AES;

public static class AesKeyExpansion
{
    public static IReadOnlyList<byte[]> Expand128(byte[] key)
    {
        if (key.Length != 16) throw new ArgumentException("Dấu vết học thuật yêu cầu khóa AES-128 (16 byte).", nameof(key));
        var expanded = new byte[176];
        key.CopyTo(expanded, 0);
        var generated = 16;
        var rcon = 1;
        var temp = new byte[4];
        while (generated < expanded.Length)
        {
            Array.Copy(expanded, generated - 4, temp, 0, 4);
            if (generated % 16 == 0)
            {
                (temp[0], temp[1], temp[2], temp[3]) = (temp[1], temp[2], temp[3], temp[0]);
                for (var i = 0; i < 4; i++) temp[i] = AesTables.SBox[temp[i]];
                temp[0] ^= AesTables.Rcon[rcon++];
            }
            for (var i = 0; i < 4; i++)
            {
                expanded[generated] = (byte)(expanded[generated - 16] ^ temp[i]);
                generated++;
            }
        }
        return Enumerable.Range(0, 11).Select(i => expanded.Skip(i * 16).Take(16).ToArray()).ToList();
    }
}

using CryptoExam.Core.Common;

namespace CryptoExam.Core.DES;

public static class DesKeySchedule
{
    public static DesKeyScheduleResult Generate(string keyHex) => Generate(HexUtils.ToUInt64(keyHex, 16));

    public static DesKeyScheduleResult Generate(ulong key)
    {
        var pc1 = DesBitUtils.Permute(key, 64, DesTables.PC1);
        var c = (uint)(pc1 >> 28);
        var d = (uint)(pc1 & 0x0FFFFFFF);
        var c0 = c; var d0 = d;
        var rounds = new List<DesKeyRoundResult>(16);
        for (var round = 0; round < 16; round++)
        {
            c = DesBitUtils.RotateLeft28(c, DesTables.Shifts[round]);
            d = DesBitUtils.RotateLeft28(d, DesTables.Shifts[round]);
            var combined = (ulong)c << 28 | d;
            rounds.Add(new(round + 1, DesTables.Shifts[round], c, d, DesBitUtils.Permute(combined, 56, DesTables.PC2)));
        }
        return new(key, pc1, c0, d0, rounds);
    }
}

using CryptoExam.Core.Common;

namespace CryptoExam.Core.DES;

public static class DesCipher
{
    public static string Encrypt(string plaintextHex, string keyHex) => Trace(plaintextHex, keyHex, false).OutputHex;
    public static string Decrypt(string ciphertextHex, string keyHex) => Trace(ciphertextHex, keyHex, true).OutputHex;

    public static DesTraceResult Trace(string inputHex, string keyHex, bool decrypt = false) =>
        Trace(HexUtils.ToUInt64(inputHex, 16), HexUtils.ToUInt64(keyHex, 16), decrypt);

    public static DesTraceResult Trace(ulong input, ulong key, bool decrypt = false)
    {
        var schedule = DesKeySchedule.Generate(key);
        var initial = DesBitUtils.Permute(input, 64, DesTables.IP);
        var left = (uint)(initial >> 32);
        var right = (uint)initial;
        var l0 = left; var r0 = right;
        var rounds = new List<DesRoundResult>(16);
        for (var index = 0; index < 16; index++)
        {
            var scheduleIndex = decrypt ? 15 - index : index;
            var function = DesRoundFunction.Calculate(right, schedule.Rounds[scheduleIndex].SubKey);
            var nextLeft = right;
            var nextRight = left ^ function.PResult;
            rounds.Add(new(index + 1, left, right, function, nextLeft, nextRight));
            left = nextLeft;
            right = nextRight;
        }
        // DES hoán đổi hai nửa sau vòng 16: R16 || L16 trước IP^-1.
        var preOutput = (ulong)right << 32 | left;
        var output = DesBitUtils.Permute(preOutput, 64, DesTables.FinalPermutation);
        return new(decrypt, input, initial, l0, r0, schedule, rounds, preOutput, output);
    }
}

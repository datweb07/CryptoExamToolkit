using CryptoExam.Core.Common;

namespace CryptoExam.Core.DES;

public sealed record DesTraceResult(bool IsDecryption, ulong Input, ulong InitialPermutation, uint L0, uint R0,
    DesKeyScheduleResult KeySchedule, IReadOnlyList<DesRoundResult> Rounds, ulong PreOutput, ulong Output)
{
    public string InputHex => Input.ToString("X16");
    public string InputBinary => HexUtils.ToBinary(Input, 64);
    public string InitialPermutationHex => InitialPermutation.ToString("X16");
    public string InitialPermutationBinary => HexUtils.ToBinary(InitialPermutation, 64);
    public string L0Hex => L0.ToString("X8");
    public string R0Hex => R0.ToString("X8");
    public string L0Binary => HexUtils.ToBinary(L0, 32);
    public string R0Binary => HexUtils.ToBinary(R0, 32);
    public string PreOutputHex => PreOutput.ToString("X16");
    public string OutputHex => Output.ToString("X16");
}

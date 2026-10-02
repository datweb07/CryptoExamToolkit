using CryptoExam.Core.Common;

namespace CryptoExam.Core.DES;

public sealed record DesKeyRoundResult(int Round, int Shift, uint C, uint D, ulong SubKey)
{
    public string CBinary => HexUtils.ToBinary(C, 28);
    public string DBinary => HexUtils.ToBinary(D, 28);
    public string CHex => C.ToString("X7");
    public string DHex => D.ToString("X7");
    public string CDHex => ((ulong)C << 28 | D).ToString("X14");
    public string SubKeyBinary => HexUtils.ToBinary(SubKey, 48);
    public string SubKeyHex => SubKey.ToString("X12");
}

public sealed record DesKeyScheduleResult(ulong OriginalKey, ulong Pc1, uint C0, uint D0, IReadOnlyList<DesKeyRoundResult> Rounds)
{
    public string OriginalKeyHex => OriginalKey.ToString("X16");
    public string Pc1Hex => Pc1.ToString("X14");
    public string Pc1Binary => HexUtils.ToBinary(Pc1, 56);
    public string C0Hex => C0.ToString("X7");
    public string D0Hex => D0.ToString("X7");
}

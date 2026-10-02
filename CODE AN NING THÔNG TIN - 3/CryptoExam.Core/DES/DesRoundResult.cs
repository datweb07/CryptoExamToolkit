namespace CryptoExam.Core.DES;

public sealed record DesSBoxStep(int SBoxNumber, byte Input, int Row, int Column, int Value)
{
    public string InputBinary => Convert.ToString(Input, 2).PadLeft(6, '0');
    public string RowBits => $"{InputBinary[0]}{InputBinary[5]}";
    public string ColumnBits => InputBinary.Substring(1, 4);
    public string OutputBinary => Convert.ToString(Value, 2).PadLeft(4, '0');
}

public sealed record DesRoundFunctionResult(ulong ExpandedR, ulong SubKey, ulong XorResult, uint SBoxResult, uint PResult, IReadOnlyList<DesSBoxStep> SBoxes)
{
    public string ExpandedRHex => ExpandedR.ToString("X12");
    public string SubKeyHex => SubKey.ToString("X12");
    public string XorHex => XorResult.ToString("X12");
    public string SBoxHex => SBoxResult.ToString("X8");
    public string PHex => PResult.ToString("X8");
}

public sealed record DesRoundResult(int Round, uint LPrevious, uint RPrevious, DesRoundFunctionResult Function, uint L, uint R)
{
    public string LPreviousHex => LPrevious.ToString("X8");
    public string RPreviousHex => RPrevious.ToString("X8");
    public string LHex => L.ToString("X8");
    public string RHex => R.ToString("X8");
}

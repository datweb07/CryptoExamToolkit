namespace CryptoExam.Core.AES;

public sealed record AesRoundTrace(int Round, byte[] RoundKey, byte[]? AfterSubBytes, byte[]? AfterShiftRows, byte[]? AfterMixColumns, byte[] AfterAddRoundKey)
{
    public string RoundKeyHex => Convert.ToHexString(RoundKey);
    public string AddRoundKeyHex => Convert.ToHexString(AfterAddRoundKey);
    public string? SubBytesHex => AfterSubBytes is null ? null : Convert.ToHexString(AfterSubBytes);
    public string? ShiftRowsHex => AfterShiftRows is null ? null : Convert.ToHexString(AfterShiftRows);
    public string? MixColumnsHex => AfterMixColumns is null ? null : Convert.ToHexString(AfterMixColumns);
}

public sealed record AesTraceResult(string InputHex, IReadOnlyList<AesRoundTrace> Rounds, string CiphertextHex);

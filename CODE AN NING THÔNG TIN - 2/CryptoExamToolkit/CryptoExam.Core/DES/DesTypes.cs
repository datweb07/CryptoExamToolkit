// File: CryptoExam.Core/DES/DesTypes.cs
// Các kiểu dữ liệu kết quả DES - dùng record để immutable.

namespace CryptoExam.Core.DES
{
    /// <summary>
    /// Kết quả một round của key schedule.
    /// </summary>
    public record DesKeyRoundResult
    {
        public int Round { get; init; }
        public int Shift { get; init; }
        public string CBinary { get; init; } = "";
        public string DBinary { get; init; } = "";
        public string CHex { get; init; } = "";
        public string DHex { get; init; } = "";
        public string CDBinary { get; init; } = "";
        public string CDHex { get; init; } = "";
        public string SubKeyBinary { get; init; } = "";
        public string SubKeyHex { get; init; } = "";
    }

    /// <summary>
    /// Kết quả S-box của một block 6-bit.
    /// </summary>
    public record DesSBoxBlockResult
    {
        public int BlockNumber { get; init; }       // 1-8
        public string InputBits { get; init; } = "";  // 6 bits
        public int RowBit1 { get; init; }            // bit 1 (MSB)
        public int RowBit6 { get; init; }            // bit 6 (LSB)
        public int Row { get; init; }                // row = RowBit1*2 + RowBit6
        public string ColBits { get; init; } = "";   // bits 2-5
        public int Col { get; init; }                // column 0-15
        public int SBoxValue { get; init; }          // giá trị decimal
        public string OutputBits { get; init; } = ""; // 4 bits
    }

    /// <summary>
    /// Kết quả một round Feistel.
    /// </summary>
    public record DesRoundResult
    {
        public int Round { get; init; }
        public string LPrevBinary { get; init; } = "";
        public string LPrevHex { get; init; } = "";
        public string RPrevBinary { get; init; } = "";
        public string RPrevHex { get; init; } = "";
        public string SubKeyBinary { get; init; } = "";
        public string SubKeyHex { get; init; } = "";
        public string ExpandedRBinary { get; init; } = "";
        public string ExpandedRHex { get; init; } = "";
        public string XorResultBinary { get; init; } = "";
        public string XorResultHex { get; init; } = "";
        public DesSBoxBlockResult[] SBoxBlocks { get; init; } = Array.Empty<DesSBoxBlockResult>();
        public string SBoxResultBinary { get; init; } = "";
        public string SBoxResultHex { get; init; } = "";
        public string PResultBinary { get; init; } = "";
        public string PResultHex { get; init; } = "";
        public string LBinary { get; init; } = "";
        public string LHex { get; init; } = "";
        public string RBinary { get; init; } = "";
        public string RHex { get; init; } = "";
    }
}

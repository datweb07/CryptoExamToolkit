// File: CryptoExam.Core/DES/DesTraceResult.cs
using System.Collections.Generic;

namespace CryptoExam.Core.DES
{
    /// <summary>
    /// Toàn bộ trace của một lần mã hóa/giải mã DES.
    /// Chứa tất cả giá trị trung gian để truy vấn nhanh trong phòng thi.
    /// </summary>
    public class DesTraceResult
    {
        // --- Input ---
        public string InputHex { get; set; } = "";
        public string InputBinary { get; set; } = "";
        public bool IsDecrypt { get; set; }

        // --- Key Schedule ---
        public DesKeyScheduleResult KeySchedule { get; set; } = new();

        // --- Initial Permutation ---
        public string IpBinary { get; set; } = "";
        public string IpHex { get; set; } = "";
        public string L0Binary { get; set; } = "";
        public string L0Hex { get; set; } = "";
        public string R0Binary { get; set; } = "";
        public string R0Hex { get; set; } = "";

        // --- 16 Rounds ---
        public List<DesRoundResult> Rounds { get; set; } = new();

        // --- Final ---
        public string R16L16Binary { get; set; } = "";
        public string R16L16Hex { get; set; } = "";
        public string OutputBinary { get; set; } = "";
        public string OutputHex { get; set; } = "";

        // Alias cho dễ đọc
        public string CipherTextHex => OutputHex;
        public string PlainTextHex => InputHex;
    }

    /// <summary>
    /// Kết quả key schedule đầy đủ.
    /// </summary>
    public class DesKeyScheduleResult
    {
        public string OriginalKeyHex { get; set; } = "";
        public string OriginalKeyBinary { get; set; } = "";
        public string Key56BitBinary { get; set; } = ""; // sau PC1
        public string Key56BitHex { get; set; } = "";
        public string C0Binary { get; set; } = "";
        public string C0Hex { get; set; } = "";
        public string D0Binary { get; set; } = "";
        public string D0Hex { get; set; } = "";
        public List<DesKeyRoundResult> Rounds { get; set; } = new();
    }
}

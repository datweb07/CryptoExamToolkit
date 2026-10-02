// File: CryptoExam.Core/AES/AesTraceResult.cs
using System.Collections.Generic;

namespace CryptoExam.Core.AES
{
    /// <summary>
    /// Một bước trong một round AES.
    /// </summary>
    public class AesRoundTrace
    {
        public int Round { get; set; }
        public byte[,]? AfterAddRoundKey { get; set; }
        public byte[,]? AfterSubBytes { get; set; }
        public byte[,]? AfterShiftRows { get; set; }
        public byte[,]? AfterMixColumns { get; set; } // null cho round cuối
    }

    /// <summary>
    /// Toàn bộ trace AES-128.
    /// </summary>
    public class AesTraceResult
    {
        public byte[] InputBytes { get; set; } = Array.Empty<byte>();
        public byte[] KeyBytes { get; set; } = Array.Empty<byte>();
        public byte[][,] RoundKeys { get; set; } = Array.Empty<byte[,]>();
        public List<AesRoundTrace> Rounds { get; set; } = new();
        public byte[] OutputBytes { get; set; } = Array.Empty<byte>();
    }
}

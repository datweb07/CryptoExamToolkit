using System.Collections.Generic;

namespace CryptoExam.Core.DES
{
    public class DesTraceResult
    {
        public string PlainTextHex { get; set; } = "";
        public string PlainTextBinary { get; set; } = "";
        public string IpBinary { get; set; } = "";
        public string IpHex { get; set; } = "";
        public string L0Binary { get; set; } = "";
        public string L0Hex { get; set; } = "";
        public string R0Binary { get; set; } = "";
        public string R0Hex { get; set; } = "";
        
        public List<DesRoundResult> Rounds { get; set; } = new List<DesRoundResult>();
        
        public string R16L16Binary { get; set; } = "";
        public string R16L16Hex { get; set; } = "";
        
        public string CipherTextBinary { get; set; } = "";
        public string CipherTextHex { get; set; } = "";
        
        public DesKeySchedule KeySchedule { get; set; }
    }
}

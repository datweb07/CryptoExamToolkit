namespace CryptoExam.Core.DES
{
    public class DesKeyRoundResult
    {
        public int Round { get; set; }
        public int Shift { get; set; }
        public string CBinary { get; set; } = "";
        public string DBinary { get; set; } = "";
        public string CHex { get; set; } = "";
        public string DHex { get; set; } = "";
        public string CDHex { get; set; } = "";
        public string SubKeyBinary { get; set; } = "";
        public string SubKeyHex { get; set; } = "";
    }

    public class DesRoundResult
    {
        public int Round { get; set; }
        public string LPrevBinary { get; set; } = "";
        public string LPrevHex { get; set; } = "";
        public string RPrevBinary { get; set; } = "";
        public string RPrevHex { get; set; } = "";
        
        public string ExpandedRBinary { get; set; } = "";
        public string ExpandedRHex { get; set; } = "";
        
        public string SubKeyBinary { get; set; } = "";
        public string SubKeyHex { get; set; } = "";
        
        public string XorResultBinary { get; set; } = "";
        public string XorResultHex { get; set; } = "";
        
        public string SBoxResultBinary { get; set; } = "";
        public string SBoxResultHex { get; set; } = "";
        
        public string PResultBinary { get; set; } = "";
        public string PResultHex { get; set; } = "";
        
        public string LBinary { get; set; } = "";
        public string LHex { get; set; } = "";
        public string RBinary { get; set; } = "";
        public string RHex { get; set; } = "";
    }
}

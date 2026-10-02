using System;
using System.Collections.Generic;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.DES
{
    public class DesKeySchedule
    {
        public string OriginalKeyHex { get; private set; } = "";
        public string OriginalKeyBinary { get; private set; } = "";
        public string Key56BitBinary { get; private set; } = "";
        public string C0 { get; private set; } = "";
        public string D0 { get; private set; } = "";
        
        public List<DesKeyRoundResult> Rounds { get; private set; } = new List<DesKeyRoundResult>();

        public DesKeySchedule(string keyHex)
        {
            Generate(keyHex);
        }

        private void Generate(string keyHex)
        {
            OriginalKeyHex = HexUtils.CleanHex(keyHex);
            if (OriginalKeyHex.Length != 16)
                throw new ArgumentException("DES Key must be 16 hex characters (64 bits).");

            OriginalKeyBinary = HexUtils.HexToBinary(OriginalKeyHex);
            Key56BitBinary = DesBitUtils.Permute(OriginalKeyBinary, DesTables.PC1);

            C0 = Key56BitBinary.Substring(0, 28);
            D0 = Key56BitBinary.Substring(28, 28);

            string cCurrent = C0;
            string dCurrent = D0;

            for (int i = 0; i < 16; i++)
            {
                int shift = DesTables.ShiftSchedule[i];
                cCurrent = BinaryUtils.ShiftLeft(cCurrent, shift);
                dCurrent = BinaryUtils.ShiftLeft(dCurrent, shift);

                string cd = cCurrent + dCurrent;
                string subKeyBinary = DesBitUtils.Permute(cd, DesTables.PC2);

                var round = new DesKeyRoundResult
                {
                    Round = i + 1,
                    Shift = shift,
                    CBinary = cCurrent,
                    DBinary = dCurrent,
                    CHex = HexUtils.BinaryToHex(cCurrent),
                    DHex = HexUtils.BinaryToHex(dCurrent),
                    CDHex = HexUtils.BinaryToHex(cd),
                    SubKeyBinary = subKeyBinary,
                    SubKeyHex = HexUtils.BinaryToHex(subKeyBinary)
                };
                Rounds.Add(round);
            }
        }
    }
}

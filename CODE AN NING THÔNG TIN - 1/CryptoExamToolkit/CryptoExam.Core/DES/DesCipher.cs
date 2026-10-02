using System;
using System.Linq;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.DES
{
    public class DesCipher
    {
        public static DesTraceResult EncryptTrace(string plainTextHex, string keyHex)
        {
            return Process(plainTextHex, keyHex, false);
        }

        public static DesTraceResult DecryptTrace(string cipherTextHex, string keyHex)
        {
            return Process(cipherTextHex, keyHex, true);
        }

        private static DesTraceResult Process(string inputHex, string keyHex, bool isDecrypt)
        {
            inputHex = HexUtils.CleanHex(inputHex);
            if (inputHex.Length != 16)
                throw new ArgumentException("Input must be 16 hex characters (64 bits).");

            var trace = new DesTraceResult();
            trace.KeySchedule = new DesKeySchedule(keyHex);
            trace.PlainTextHex = inputHex;
            trace.PlainTextBinary = HexUtils.HexToBinary(inputHex);

            trace.IpBinary = DesBitUtils.Permute(trace.PlainTextBinary, DesTables.IP);
            trace.IpHex = HexUtils.BinaryToHex(trace.IpBinary);
            
            trace.L0Binary = trace.IpBinary.Substring(0, 32);
            trace.L0Hex = HexUtils.BinaryToHex(trace.L0Binary);
            trace.R0Binary = trace.IpBinary.Substring(32, 32);
            trace.R0Hex = HexUtils.BinaryToHex(trace.R0Binary);

            string lCurrent = trace.L0Binary;
            string rCurrent = trace.R0Binary;

            for (int i = 0; i < 16; i++)
            {
                var roundRes = new DesRoundResult();
                roundRes.Round = i + 1;
                roundRes.LPrevBinary = lCurrent;
                roundRes.LPrevHex = HexUtils.BinaryToHex(lCurrent);
                roundRes.RPrevBinary = rCurrent;
                roundRes.RPrevHex = HexUtils.BinaryToHex(rCurrent);

                int keyIndex = isDecrypt ? 15 - i : i;
                string subKeyBinary = trace.KeySchedule.Rounds[keyIndex].SubKeyBinary;
                roundRes.SubKeyBinary = subKeyBinary;
                roundRes.SubKeyHex = HexUtils.BinaryToHex(subKeyBinary);

                roundRes.ExpandedRBinary = DesBitUtils.Permute(rCurrent, DesTables.E);
                roundRes.ExpandedRHex = HexUtils.BinaryToHex(roundRes.ExpandedRBinary);
                
                roundRes.XorResultBinary = BinaryUtils.Xor(roundRes.ExpandedRBinary, subKeyBinary);
                roundRes.XorResultHex = HexUtils.BinaryToHex(roundRes.XorResultBinary);

                var fRes = DesRoundFunction.Calculate(roundRes.ExpandedRBinary, subKeyBinary);
                roundRes.SBoxResultBinary = fRes.sBoxBinary;
                roundRes.SBoxResultHex = HexUtils.BinaryToHex(fRes.sBoxBinary);
                
                roundRes.PResultBinary = fRes.pBinary;
                roundRes.PResultHex = HexUtils.BinaryToHex(fRes.pBinary);

                string lNext = rCurrent;
                string rNext = BinaryUtils.Xor(lCurrent, fRes.pBinary);

                roundRes.LBinary = lNext;
                roundRes.LHex = HexUtils.BinaryToHex(lNext);
                roundRes.RBinary = rNext;
                roundRes.RHex = HexUtils.BinaryToHex(rNext);

                trace.Rounds.Add(roundRes);

                lCurrent = lNext;
                rCurrent = rNext;
            }

            // Note: After round 16, halves are NOT swapped for final permutation.
            // We use R16 L16 (which effectively means we just concatenate R16 and L16).
            trace.R16L16Binary = rCurrent + lCurrent; 
            trace.R16L16Hex = HexUtils.BinaryToHex(trace.R16L16Binary);

            trace.CipherTextBinary = DesBitUtils.Permute(trace.R16L16Binary, DesTables.InvIP);
            trace.CipherTextHex = HexUtils.BinaryToHex(trace.CipherTextBinary);

            return trace;
        }
    }
}

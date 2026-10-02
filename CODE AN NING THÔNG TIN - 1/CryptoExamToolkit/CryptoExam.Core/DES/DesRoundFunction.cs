using System;
using System.Text;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.DES
{
    public static class DesRoundFunction
    {
        public static (string sBoxBinary, string pBinary) Calculate(string expandedR, string subKey)
        {
            string xorResult = BinaryUtils.Xor(expandedR, subKey);
            var sbSbox = new StringBuilder();

            for (int i = 0; i < 8; i++)
            {
                string block = xorResult.Substring(i * 6, 6);
                int row = Convert.ToInt32(block[0].ToString() + block[5].ToString(), 2);
                int col = Convert.ToInt32(block.Substring(1, 4), 2);
                
                int sboxVal = DesTables.SBoxes[i, row, col];
                string sboxBin = Convert.ToString(sboxVal, 2).PadLeft(4, '0');
                sbSbox.Append(sboxBin);
            }

            string sBoxBinary = sbSbox.ToString();
            string pBinary = DesBitUtils.Permute(sBoxBinary, DesTables.P);
            
            return (sBoxBinary, pBinary);
        }
    }
}

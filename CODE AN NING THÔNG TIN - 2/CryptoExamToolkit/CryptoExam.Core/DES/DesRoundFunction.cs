// File: CryptoExam.Core/DES/DesRoundFunction.cs
using System;
using System.Text;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.DES
{
    /// <summary>
    /// Hàm F của DES: E(R) XOR K -> S-boxes -> P permutation.
    /// Trả về toàn bộ chi tiết từng block S-box để debug/trace.
    /// </summary>
    public static class DesRoundFunction
    {
        /// <summary>
        /// Tính hàm F với đầy đủ trace.
        /// Input: expandedR (48 bits), subKey (48 bits).
        /// </summary>
        public static (
            string xorBinary,
            string xorHex,
            DesSBoxBlockResult[] sBoxBlocks,
            string sBoxCombinedBinary,
            string sBoxCombinedHex,
            string pBinary,
            string pHex
        ) Calculate(string expandedR, string subKey)
        {
            // E(R) XOR K
            string xorBinary = BinaryUtils.Xor(expandedR, subKey);
            string xorHex = HexUtils.BinaryToHex(xorBinary);

            // Chia thành 8 block 6-bit, đưa qua S-box
            var sBoxBlocks = new DesSBoxBlockResult[8];
            var sBoxCombined = new StringBuilder(32);

            for (int i = 0; i < 8; i++)
            {
                string block = xorBinary.Substring(i * 6, 6);

                // Row = bit đầu (bit 1) và bit cuối (bit 6) của block
                int rowBit1 = block[0] - '0';
                int rowBit6 = block[5] - '0';
                int row = rowBit1 * 2 + rowBit6;

                // Col = 4 bit giữa (bit 2-5)
                string colBits = block.Substring(1, 4);
                int col = Convert.ToInt32(colBits, 2);

                int sBoxValue = DesTables.SBoxes[i, row, col];
                string outputBits = Convert.ToString(sBoxValue, 2).PadLeft(4, '0');
                sBoxCombined.Append(outputBits);

                sBoxBlocks[i] = new DesSBoxBlockResult
                {
                    BlockNumber = i + 1,
                    InputBits = block,
                    RowBit1 = rowBit1,
                    RowBit6 = rowBit6,
                    Row = row,
                    ColBits = colBits,
                    Col = col,
                    SBoxValue = sBoxValue,
                    OutputBits = outputBits
                };
            }

            string sBoxCombinedBinary = sBoxCombined.ToString();
            string sBoxCombinedHex = HexUtils.BinaryToHex(sBoxCombinedBinary);

            // P permutation
            string pBinary = DesBitUtils.Permute(sBoxCombinedBinary, DesTables.P);
            string pHex = HexUtils.BinaryToHex(pBinary);

            return (xorBinary, xorHex, sBoxBlocks, sBoxCombinedBinary, sBoxCombinedHex, pBinary, pHex);
        }
    }
}

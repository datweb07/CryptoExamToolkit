// File: CryptoExam.Core/DES/DesCipher.cs
using System;
using System.Collections.Generic;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.DES
{
    /// <summary>
    /// DES Cipher chính: encrypt và decrypt với full trace.
    /// 
    /// Test vector bắt buộc:
    ///   Key = 133457799BBCDFF1
    ///   PT  = 0123456789ABCDEF
    ///   CT  = 85E813540F0AB405
    ///
    /// Cấu trúc DES:
    ///   1. IP (Initial Permutation)
    ///   2. 16 rounds Feistel: Li = R(i-1), Ri = L(i-1) XOR F(R(i-1), Ki)
    ///   3. Swap: R16L16 (đây là QUAN TRỌNG - hoán đổi nửa trái phải sau round 16)
    ///   4. IP^-1 -> Ciphertext
    ///
    /// Decrypt: dùng subkey theo thứ tự ngược K16, K15, ..., K1.
    /// </summary>
    public static class DesCipher
    {
        /// <summary>
        /// Mã hóa DES với full trace.
        /// </summary>
        public static DesTraceResult EncryptTrace(string plainTextHex, string keyHex)
        {
            return Process(plainTextHex, keyHex, isDecrypt: false);
        }

        /// <summary>
        /// Giải mã DES với full trace.
        /// </summary>
        public static DesTraceResult DecryptTrace(string cipherTextHex, string keyHex)
        {
            return Process(cipherTextHex, keyHex, isDecrypt: true);
        }

        private static DesTraceResult Process(string inputHex, string keyHex, bool isDecrypt)
        {
            inputHex = HexUtils.CleanHex(inputHex);
            if (inputHex.Length != 16)
                throw new ArgumentException("Input DES phải là 16 ký tự hex (64 bits).");

            var trace = new DesTraceResult { IsDecrypt = isDecrypt };

            // Key schedule
            trace.KeySchedule = DesKeySchedule.Generate(keyHex);

            // Input
            trace.InputHex = inputHex;
            trace.InputBinary = HexUtils.HexToBinary(inputHex);

            // Bước 1: IP (Initial Permutation)
            trace.IpBinary = DesBitUtils.Permute(trace.InputBinary, DesTables.IP);
            trace.IpHex = HexUtils.BinaryToHex(trace.IpBinary);
            trace.L0Binary = trace.IpBinary.Substring(0, 32);
            trace.L0Hex = HexUtils.BinaryToHex(trace.L0Binary);
            trace.R0Binary = trace.IpBinary.Substring(32, 32);
            trace.R0Hex = HexUtils.BinaryToHex(trace.R0Binary);

            string lCurrent = trace.L0Binary;
            string rCurrent = trace.R0Binary;

            // Bước 2: 16 rounds Feistel
            for (int i = 0; i < 16; i++)
            {
                // Chọn subkey theo thứ tự (decrypt dùng ngược K16..K1)
                int keyIndex = isDecrypt ? 15 - i : i;
                var keyRound = trace.KeySchedule.Rounds[keyIndex];

                // Expand R: 32 bits -> 48 bits
                string expandedR = DesBitUtils.Permute(rCurrent, DesTables.E);

                // Tính F(R, K): E(R) XOR K -> S-boxes -> P
                var (xorBin, xorHex, sBoxBlocks, sBoxCombBin, sBoxCombHex, pBin, pHex)
                    = DesRoundFunction.Calculate(expandedR, keyRound.SubKeyBinary);

                // Feistel: L_new = R_prev, R_new = L_prev XOR F
                string lNext = rCurrent;
                string rNext = BinaryUtils.Xor(lCurrent, pBin);

                var roundResult = new DesRoundResult
                {
                    Round = i + 1,
                    LPrevBinary = lCurrent,
                    LPrevHex = HexUtils.BinaryToHex(lCurrent),
                    RPrevBinary = rCurrent,
                    RPrevHex = HexUtils.BinaryToHex(rCurrent),
                    SubKeyBinary = keyRound.SubKeyBinary,
                    SubKeyHex = keyRound.SubKeyHex,
                    ExpandedRBinary = expandedR,
                    ExpandedRHex = HexUtils.BinaryToHex(expandedR),
                    XorResultBinary = xorBin,
                    XorResultHex = xorHex,
                    SBoxBlocks = sBoxBlocks,
                    SBoxResultBinary = sBoxCombBin,
                    SBoxResultHex = sBoxCombHex,
                    PResultBinary = pBin,
                    PResultHex = pHex,
                    LBinary = lNext,
                    LHex = HexUtils.BinaryToHex(lNext),
                    RBinary = rNext,
                    RHex = HexUtils.BinaryToHex(rNext)
                };

                trace.Rounds.Add(roundResult);
                lCurrent = lNext;
                rCurrent = rNext;
            }

            // Bước 3: Hoán đổi R16L16 (sau vòng 16, ghép R16 trước L16)
            trace.R16L16Binary = rCurrent + lCurrent;
            trace.R16L16Hex = HexUtils.BinaryToHex(trace.R16L16Binary);

            // Bước 4: IP^-1 -> Output
            trace.OutputBinary = DesBitUtils.Permute(trace.R16L16Binary, DesTables.InvIP);
            trace.OutputHex = HexUtils.BinaryToHex(trace.OutputBinary);

            return trace;
        }
    }
}

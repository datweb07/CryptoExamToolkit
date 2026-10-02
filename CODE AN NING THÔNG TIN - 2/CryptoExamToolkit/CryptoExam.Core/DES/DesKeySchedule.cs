// File: CryptoExam.Core/DES/DesKeySchedule.cs
using System;
using System.Collections.Generic;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.DES
{
    /// <summary>
    /// DES Key Schedule: sinh 16 subkey từ 64-bit key.
    /// Test vector:
    ///   Key = 133457799BBCDFF1
    ///   K1  = 1B02EFFC7072
    ///   K3  = 55FC8A42CF99
    ///   C3  = 0CCAAFF (28-bit, hex 7 ký tự)
    ///   D3  = 56678F5
    /// </summary>
    public static class DesKeySchedule
    {
        /// <summary>
        /// Tính toàn bộ key schedule từ 64-bit hex key.
        /// </summary>
        public static DesKeyScheduleResult Generate(string keyHex)
        {
            keyHex = HexUtils.CleanHex(keyHex);
            if (keyHex.Length != 16)
                throw new ArgumentException("DES Key phải là 16 ký tự hex (64 bits).");

            var result = new DesKeyScheduleResult();
            result.OriginalKeyHex = keyHex;
            result.OriginalKeyBinary = HexUtils.HexToBinary(keyHex); // 64 bits

            // PC-1: 64 bits -> 56 bits
            result.Key56BitBinary = DesBitUtils.Permute(result.OriginalKeyBinary, DesTables.PC1);
            result.Key56BitHex = HexUtils.BinaryToHex(result.Key56BitBinary);

            // Chia thành C0 (28 bit) và D0 (28 bit)
            result.C0Binary = result.Key56BitBinary.Substring(0, 28);
            result.D0Binary = result.Key56BitBinary.Substring(28, 28);
            result.C0Hex = HexUtils.BinaryToHex(result.C0Binary);
            result.D0Hex = HexUtils.BinaryToHex(result.D0Binary);

            string cCurrent = result.C0Binary;
            string dCurrent = result.D0Binary;

            for (int i = 0; i < 16; i++)
            {
                int shift = DesTables.ShiftSchedule[i];

                // Dịch vòng trái C và D
                cCurrent = BinaryUtils.ShiftLeft(cCurrent, shift);
                dCurrent = BinaryUtils.ShiftLeft(dCurrent, shift);

                // Ghép C_i D_i (56 bits)
                string cd = cCurrent + dCurrent;

                // PC-2: 56 bits -> 48 bits (subkey)
                string subKeyBinary = DesBitUtils.Permute(cd, DesTables.PC2);

                var roundResult = new DesKeyRoundResult
                {
                    Round = i + 1,
                    Shift = shift,
                    CBinary = cCurrent,
                    DBinary = dCurrent,
                    CHex = HexUtils.BinaryToHex(cCurrent),
                    DHex = HexUtils.BinaryToHex(dCurrent),
                    CDBinary = cd,
                    CDHex = HexUtils.BinaryToHex(cd),
                    SubKeyBinary = subKeyBinary,
                    SubKeyHex = HexUtils.BinaryToHex(subKeyBinary)
                };

                result.Rounds.Add(roundResult);
            }

            return result;
        }
    }
}

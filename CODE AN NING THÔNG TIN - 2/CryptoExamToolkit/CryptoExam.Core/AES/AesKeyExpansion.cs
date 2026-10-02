// File: CryptoExam.Core/AES/AesKeyExpansion.cs
using System;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.AES
{
    /// <summary>
    /// AES-128 Key Expansion: từ 16-byte key sinh 11 round keys (44 words).
    /// </summary>
    public static class AesKeyExpansion
    {
        /// <summary>
        /// Expand key thành 11 round keys (mỗi round key là 4x4 byte).
        /// </summary>
        public static byte[][,] ExpandKey(byte[] key)
        {
            if (key.Length != 16)
                throw new ArgumentException("AES-128 key phải là 16 bytes.");

            // Mở rộng thành 44 words (4 bytes/word)
            byte[][] w = new byte[44][];
            for (int i = 0; i < 4; i++)
            {
                w[i] = new byte[4];
                w[i][0] = key[i * 4];
                w[i][1] = key[i * 4 + 1];
                w[i][2] = key[i * 4 + 2];
                w[i][3] = key[i * 4 + 3];
            }

            for (int i = 4; i < 44; i++)
            {
                byte[] temp = (byte[])w[i - 1].Clone();
                if (i % 4 == 0)
                {
                    temp = SubWord(RotWord(temp));
                    temp[0] ^= AesTables.Rcon[i / 4];
                }
                w[i] = new byte[4];
                for (int j = 0; j < 4; j++)
                    w[i][j] = (byte)(w[i - 4][j] ^ temp[j]);
            }

            // Tổ chức thành 11 round keys, mỗi key là 4x4 state
            var roundKeys = new byte[11][,];
            for (int r = 0; r < 11; r++)
            {
                roundKeys[r] = new byte[4, 4];
                for (int c = 0; c < 4; c++)
                    for (int row = 0; row < 4; row++)
                        roundKeys[r][row, c] = w[r * 4 + c][row];
            }
            return roundKeys;
        }

        private static byte[] RotWord(byte[] word)
            => new byte[] { word[1], word[2], word[3], word[0] };

        private static byte[] SubWord(byte[] word)
            => new byte[] {
                AesTables.SBox[word[0]],
                AesTables.SBox[word[1]],
                AesTables.SBox[word[2]],
                AesTables.SBox[word[3]]
            };
    }
}

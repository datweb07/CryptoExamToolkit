// File: CryptoExam.Core/AES/AesTransformations.cs
using System;

namespace CryptoExam.Core.AES
{
    /// <summary>
    /// Các phép biến đổi AES: SubBytes, ShiftRows, MixColumns, AddRoundKey.
    /// State AES là 4x4 byte (16 bytes), đọc theo cột.
    /// </summary>
    public static class AesTransformations
    {
        // ==== SubBytes ====
        /// <summary>
        /// SubBytes: thay thế mỗi byte bằng S-box.
        /// </summary>
        public static byte[,] SubBytes(byte[,] state)
        {
            var result = new byte[4, 4];
            for (int r = 0; r < 4; r++)
                for (int c = 0; c < 4; c++)
                    result[r, c] = AesTables.SBox[state[r, c]];
            return result;
        }

        /// <summary>
        /// InvSubBytes: thay thế mỗi byte bằng Inverse S-box.
        /// </summary>
        public static byte[,] InvSubBytes(byte[,] state)
        {
            var result = new byte[4, 4];
            for (int r = 0; r < 4; r++)
                for (int c = 0; c < 4; c++)
                    result[r, c] = AesTables.InvSBox[state[r, c]];
            return result;
        }

        // ==== ShiftRows ====
        /// <summary>
        /// ShiftRows: dịch trái từng hàng. Row i dịch i vị trí.
        /// </summary>
        public static byte[,] ShiftRows(byte[,] state)
        {
            var result = new byte[4, 4];
            for (int r = 0; r < 4; r++)
                for (int c = 0; c < 4; c++)
                    result[r, c] = state[r, (c + r) % 4];
            return result;
        }

        /// <summary>
        /// InvShiftRows: dịch phải từng hàng.
        /// </summary>
        public static byte[,] InvShiftRows(byte[,] state)
        {
            var result = new byte[4, 4];
            for (int r = 0; r < 4; r++)
                for (int c = 0; c < 4; c++)
                    result[r, c] = state[r, (c - r + 4) % 4];
            return result;
        }

        // ==== MixColumns ====
        /// <summary>
        /// Nhân trong GF(2^8) với irreducible polynomial x^8+x^4+x^3+x+1 (0x11B).
        /// </summary>
        private static byte GfMul(byte a, byte b)
        {
            byte result = 0;
            for (int i = 0; i < 8; i++)
            {
                if ((b & 1) != 0) result ^= a;
                bool hiBit = (a & 0x80) != 0;
                a <<= 1;
                if (hiBit) a ^= 0x1B; // x-reduction
                b >>= 1;
            }
            return result;
        }

        /// <summary>
        /// MixColumns: nhân ma trận MDS trong GF(2^8).
        /// </summary>
        public static byte[,] MixColumns(byte[,] state)
        {
            var result = new byte[4, 4];
            for (int c = 0; c < 4; c++)
            {
                result[0, c] = (byte)(GfMul(0x02, state[0, c]) ^ GfMul(0x03, state[1, c]) ^ state[2, c] ^ state[3, c]);
                result[1, c] = (byte)(state[0, c] ^ GfMul(0x02, state[1, c]) ^ GfMul(0x03, state[2, c]) ^ state[3, c]);
                result[2, c] = (byte)(state[0, c] ^ state[1, c] ^ GfMul(0x02, state[2, c]) ^ GfMul(0x03, state[3, c]));
                result[3, c] = (byte)(GfMul(0x03, state[0, c]) ^ state[1, c] ^ state[2, c] ^ GfMul(0x02, state[3, c]));
            }
            return result;
        }

        /// <summary>
        /// InvMixColumns.
        /// </summary>
        public static byte[,] InvMixColumns(byte[,] state)
        {
            var result = new byte[4, 4];
            for (int c = 0; c < 4; c++)
            {
                result[0, c] = (byte)(GfMul(0x0E, state[0, c]) ^ GfMul(0x0B, state[1, c]) ^ GfMul(0x0D, state[2, c]) ^ GfMul(0x09, state[3, c]));
                result[1, c] = (byte)(GfMul(0x09, state[0, c]) ^ GfMul(0x0E, state[1, c]) ^ GfMul(0x0B, state[2, c]) ^ GfMul(0x0D, state[3, c]));
                result[2, c] = (byte)(GfMul(0x0D, state[0, c]) ^ GfMul(0x09, state[1, c]) ^ GfMul(0x0E, state[2, c]) ^ GfMul(0x0B, state[3, c]));
                result[3, c] = (byte)(GfMul(0x0B, state[0, c]) ^ GfMul(0x0D, state[1, c]) ^ GfMul(0x09, state[2, c]) ^ GfMul(0x0E, state[3, c]));
            }
            return result;
        }

        // ==== AddRoundKey ====
        /// <summary>
        /// AddRoundKey: XOR state với round key.
        /// </summary>
        public static byte[,] AddRoundKey(byte[,] state, byte[,] roundKey)
        {
            var result = new byte[4, 4];
            for (int r = 0; r < 4; r++)
                for (int c = 0; c < 4; c++)
                    result[r, c] = (byte)(state[r, c] ^ roundKey[r, c]);
            return result;
        }

        // ==== Helper ====
        /// <summary>
        /// Chuyển mảng bytes (16 bytes, column-major) thành state 4x4.
        /// AES state: bytes điền theo cột (column-major order).
        /// </summary>
        public static byte[,] BytesToState(byte[] bytes)
        {
            var state = new byte[4, 4];
            for (int c = 0; c < 4; c++)
                for (int r = 0; r < 4; r++)
                    state[r, c] = bytes[c * 4 + r];
            return state;
        }

        /// <summary>
        /// Chuyển state 4x4 thành mảng bytes (column-major).
        /// </summary>
        public static byte[] StateToBytes(byte[,] state)
        {
            byte[] bytes = new byte[16];
            for (int c = 0; c < 4; c++)
                for (int r = 0; r < 4; r++)
                    bytes[c * 4 + r] = state[r, c];
            return bytes;
        }

        /// <summary>
        /// In state 4x4 dạng hex.
        /// </summary>
        public static void PrintState(byte[,] state, string label = "State")
        {
            Console.WriteLine($"  {label}:");
            for (int r = 0; r < 4; r++)
            {
                Console.Write("    ");
                for (int c = 0; c < 4; c++)
                    Console.Write($"{state[r, c]:X2} ");
                Console.WriteLine();
            }
        }
    }
}

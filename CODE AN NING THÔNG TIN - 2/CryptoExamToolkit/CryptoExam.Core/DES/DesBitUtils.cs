// File: CryptoExam.Core/DES/DesBitUtils.cs
using System;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.DES
{
    /// <summary>
    /// Tiện ích bit cho DES: permutation sử dụng 1-based table.
    /// </summary>
    public static class DesBitUtils
    {
        /// <summary>
        /// Hoán vị bit theo bảng 1-based.
        /// Ví dụ: table[0]=58 nghĩa là bit đầu tiên của output = bit thứ 58 của input.
        /// </summary>
        public static string Permute(string bits, int[] table)
        {
            char[] result = new char[table.Length];
            for (int i = 0; i < table.Length; i++)
            {
                int srcIdx = table[i] - 1; // chuyển 1-based thành 0-based
                if (srcIdx < 0 || srcIdx >= bits.Length)
                    throw new IndexOutOfRangeException(
                        $"Permute: table[{i}]={table[i]}, srcIdx={srcIdx}, bits.Length={bits.Length}");
                result[i] = bits[srcIdx];
            }
            return new string(result);
        }
    }
}

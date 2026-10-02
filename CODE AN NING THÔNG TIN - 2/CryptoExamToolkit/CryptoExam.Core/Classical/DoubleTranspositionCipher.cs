// File: CryptoExam.Core/Classical/DoubleTranspositionCipher.cs
using System;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.Classical
{
    /// <summary>
    /// Double Transposition Cipher:
    ///   Encrypt: áp dụng Columnar Transposition 2 lần với key1 rồi key2.
    ///   Decrypt: áp dụng ngược lại (key2 trước, key1 sau).
    /// </summary>
    public static class DoubleTranspositionCipher
    {
        /// <summary>
        /// Mã hóa Double Transposition.
        /// </summary>
        public static (string step1, string step2) Encrypt(string plaintext, string key1, string key2, char fill = 'X')
        {
            var c1 = new ColumnarTranspositionCipher(key1);
            string step1 = c1.Encrypt(plaintext, fill);

            var c2 = new ColumnarTranspositionCipher(key2);
            string step2 = c2.Encrypt(step1, fill);

            return (step1, step2);
        }

        /// <summary>
        /// Giải mã Double Transposition (thứ tự ngược: key2 rồi key1).
        /// </summary>
        public static (string step1, string step2) Decrypt(string ciphertext, string key1, string key2, char fill = 'X')
        {
            // Giải với key2 trước
            var c2 = new ColumnarTranspositionCipher(key2);
            string step1 = c2.Decrypt(ciphertext, fill);

            // Rồi giải với key1
            var c1 = new ColumnarTranspositionCipher(key1);
            string step2 = c1.Decrypt(step1, fill);

            return (step1, step2);
        }
    }
}

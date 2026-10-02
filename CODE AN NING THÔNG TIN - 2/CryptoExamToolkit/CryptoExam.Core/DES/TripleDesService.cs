// File: CryptoExam.Core/DES/TripleDesService.cs
using System;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.DES
{
    /// <summary>
    /// Triple DES và Double DES.
    ///
    /// Double DES: C = E_K2(E_K1(P))
    ///
    /// Triple DES EDE (3 key): C = E_K1(D_K2(E_K3(P)))
    ///   hoặc dạng phổ biến nhất:
    ///   C = E_K1(D_K2(E_K1(P)))  (2 key variant)
    ///
    /// Triple DES EEE: C = E_K3(E_K2(E_K1(P)))
    ///
    /// Mỗi mode được label rõ ràng.
    /// </summary>
    public static class TripleDesService
    {
        /// <summary>
        /// Double DES: C = E_K2(E_K1(P))
        /// </summary>
        public static (string step1, string ciphertext) DoubleDesEncrypt(
            string plaintextHex, string key1Hex, string key2Hex)
        {
            var t1 = DesCipher.EncryptTrace(plaintextHex, key1Hex);
            string step1 = t1.OutputHex;

            var t2 = DesCipher.EncryptTrace(step1, key2Hex);
            string ciphertext = t2.OutputHex;

            return (step1, ciphertext);
        }

        /// <summary>
        /// Double DES Decrypt: P = D_K1(D_K2(C))
        /// </summary>
        public static (string step1, string plaintext) DoubleDesDecrypt(
            string ciphertextHex, string key1Hex, string key2Hex)
        {
            var t1 = DesCipher.DecryptTrace(ciphertextHex, key2Hex);
            string step1 = t1.OutputHex;

            var t2 = DesCipher.DecryptTrace(step1, key1Hex);
            string plaintext = t2.OutputHex;

            return (step1, plaintext);
        }

        /// <summary>
        /// Triple DES EDE (2-key): C = E_K1(D_K2(E_K1(P)))
        /// </summary>
        public static (string step1, string step2, string ciphertext) TripleDesEde2Encrypt(
            string plaintextHex, string key1Hex, string key2Hex)
        {
            var t1 = DesCipher.EncryptTrace(plaintextHex, key1Hex);
            string step1 = t1.OutputHex;

            var t2 = DesCipher.DecryptTrace(step1, key2Hex);
            string step2 = t2.OutputHex;

            var t3 = DesCipher.EncryptTrace(step2, key1Hex);
            string ciphertext = t3.OutputHex;

            return (step1, step2, ciphertext);
        }

        /// <summary>
        /// Triple DES EDE (2-key) Decrypt: P = D_K1(E_K2(D_K1(C)))
        /// </summary>
        public static (string step1, string step2, string plaintext) TripleDesEde2Decrypt(
            string ciphertextHex, string key1Hex, string key2Hex)
        {
            var t1 = DesCipher.DecryptTrace(ciphertextHex, key1Hex);
            string step1 = t1.OutputHex;

            var t2 = DesCipher.EncryptTrace(step1, key2Hex);
            string step2 = t2.OutputHex;

            var t3 = DesCipher.DecryptTrace(step2, key1Hex);
            string plaintext = t3.OutputHex;

            return (step1, step2, plaintext);
        }

        /// <summary>
        /// Triple DES EDE (3-key): C = E_K3(D_K2(E_K1(P)))
        /// Đây là chuẩn ANSI X9.52
        /// </summary>
        public static (string step1, string step2, string ciphertext) TripleDesEde3Encrypt(
            string plaintextHex, string key1Hex, string key2Hex, string key3Hex)
        {
            var t1 = DesCipher.EncryptTrace(plaintextHex, key1Hex);
            string step1 = t1.OutputHex;

            var t2 = DesCipher.DecryptTrace(step1, key2Hex);
            string step2 = t2.OutputHex;

            var t3 = DesCipher.EncryptTrace(step2, key3Hex);
            string ciphertext = t3.OutputHex;

            return (step1, step2, ciphertext);
        }

        /// <summary>
        /// Triple DES EEE (3-key): C = E_K3(E_K2(E_K1(P)))
        /// </summary>
        public static (string step1, string step2, string ciphertext) TripleDesEee3Encrypt(
            string plaintextHex, string key1Hex, string key2Hex, string key3Hex)
        {
            var t1 = DesCipher.EncryptTrace(plaintextHex, key1Hex);
            string step1 = t1.OutputHex;

            var t2 = DesCipher.EncryptTrace(step1, key2Hex);
            string step2 = t2.OutputHex;

            var t3 = DesCipher.EncryptTrace(step2, key3Hex);
            string ciphertext = t3.OutputHex;

            return (step1, step2, ciphertext);
        }
    }
}

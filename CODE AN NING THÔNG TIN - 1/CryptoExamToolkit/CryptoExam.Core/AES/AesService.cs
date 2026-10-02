using System;
using System.Security.Cryptography;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.AES
{
    public static class AesService
    {
        public static string EncryptQuick(string plainTextHex, string keyHex)
        {
            byte[] plainTextBytes = HexUtils.HexToBytes(plainTextHex);
            byte[] keyBytes = HexUtils.HexToBytes(keyHex);
            
            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.Mode = CipherMode.ECB; // Typical for basic block crypto exams
                aes.Padding = PaddingMode.None; // Expecting exactly 16 bytes (128 bits) blocks usually
                
                using (var encryptor = aes.CreateEncryptor())
                {
                    byte[] cipherText = encryptor.TransformFinalBlock(plainTextBytes, 0, plainTextBytes.Length);
                    return HexUtils.BytesToHex(cipherText);
                }
            }
        }

        public static string DecryptQuick(string cipherTextHex, string keyHex)
        {
            byte[] cipherTextBytes = HexUtils.HexToBytes(cipherTextHex);
            byte[] keyBytes = HexUtils.HexToBytes(keyHex);
            
            using (Aes aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.None;
                
                using (var decryptor = aes.CreateDecryptor())
                {
                    byte[] plainText = decryptor.TransformFinalBlock(cipherTextBytes, 0, cipherTextBytes.Length);
                    return HexUtils.BytesToHex(plainText);
                }
            }
        }
    }
}

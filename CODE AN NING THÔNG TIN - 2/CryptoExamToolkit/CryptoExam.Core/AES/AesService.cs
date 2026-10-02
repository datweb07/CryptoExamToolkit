// File: CryptoExam.Core/AES/AesService.cs
using System;
using System.Security.Cryptography;
using CryptoExam.Core.Common;

namespace CryptoExam.Core.AES
{
    /// <summary>
    /// AES Service:
    ///   1. Educational AES-128 Trace (tự implement, hiển thị từng bước)
    ///   2. Quick AES Encrypt/Decrypt (dùng System.Security.Cryptography cho AES-128/192/256)
    /// </summary>
    public static class AesService
    {
        // ==========================================
        //  EDUCATIONAL AES-128 TRACE
        // ==========================================

        /// <summary>
        /// Mã hóa AES-128 với full trace từng round.
        /// </summary>
        public static AesTraceResult EncryptTrace(byte[] plaintext, byte[] key)
        {
            if (plaintext.Length != 16) throw new ArgumentException("Plaintext phải là 16 bytes.");
            if (key.Length != 16) throw new ArgumentException("Key phải là 16 bytes (AES-128).");

            var trace = new AesTraceResult
            {
                InputBytes = (byte[])plaintext.Clone(),
                KeyBytes = (byte[])key.Clone()
            };

            // Key Expansion
            var roundKeys = AesKeyExpansion.ExpandKey(key);
            trace.RoundKeys = roundKeys;

            // Initial state
            var state = AesTransformations.BytesToState(plaintext);

            // Round 0: AddRoundKey only
            state = AesTransformations.AddRoundKey(state, roundKeys[0]);
            trace.Rounds.Add(new AesRoundTrace
            {
                Round = 0,
                AfterAddRoundKey = (byte[,])state.Clone()
            });

            // Rounds 1-9
            for (int r = 1; r <= 9; r++)
            {
                var roundTrace = new AesRoundTrace { Round = r };

                state = AesTransformations.SubBytes(state);
                roundTrace.AfterSubBytes = (byte[,])state.Clone();

                state = AesTransformations.ShiftRows(state);
                roundTrace.AfterShiftRows = (byte[,])state.Clone();

                state = AesTransformations.MixColumns(state);
                roundTrace.AfterMixColumns = (byte[,])state.Clone();

                state = AesTransformations.AddRoundKey(state, roundKeys[r]);
                roundTrace.AfterAddRoundKey = (byte[,])state.Clone();

                trace.Rounds.Add(roundTrace);
            }

            // Round 10 (final - không MixColumns)
            {
                var roundTrace = new AesRoundTrace { Round = 10 };

                state = AesTransformations.SubBytes(state);
                roundTrace.AfterSubBytes = (byte[,])state.Clone();

                state = AesTransformations.ShiftRows(state);
                roundTrace.AfterShiftRows = (byte[,])state.Clone();

                // Không MixColumns trong round cuối
                roundTrace.AfterMixColumns = null;

                state = AesTransformations.AddRoundKey(state, roundKeys[10]);
                roundTrace.AfterAddRoundKey = (byte[,])state.Clone();

                trace.Rounds.Add(roundTrace);
            }

            trace.OutputBytes = AesTransformations.StateToBytes(state);
            return trace;
        }

        /// <summary>
        /// Giải mã AES-128 với full trace.
        /// </summary>
        public static AesTraceResult DecryptTrace(byte[] ciphertext, byte[] key)
        {
            if (ciphertext.Length != 16) throw new ArgumentException("Ciphertext phải là 16 bytes.");
            if (key.Length != 16) throw new ArgumentException("Key phải là 16 bytes (AES-128).");

            var trace = new AesTraceResult
            {
                InputBytes = (byte[])ciphertext.Clone(),
                KeyBytes = (byte[])key.Clone()
            };

            var roundKeys = AesKeyExpansion.ExpandKey(key);
            trace.RoundKeys = roundKeys;

            var state = AesTransformations.BytesToState(ciphertext);

            // Initial: AddRoundKey với round key 10
            state = AesTransformations.AddRoundKey(state, roundKeys[10]);
            trace.Rounds.Add(new AesRoundTrace
            {
                Round = 0,
                AfterAddRoundKey = (byte[,])state.Clone()
            });

            // Rounds 9..1
            for (int r = 9; r >= 1; r--)
            {
                var roundTrace = new AesRoundTrace { Round = 10 - r };

                state = AesTransformations.InvShiftRows(state);
                roundTrace.AfterShiftRows = (byte[,])state.Clone();

                state = AesTransformations.InvSubBytes(state);
                roundTrace.AfterSubBytes = (byte[,])state.Clone();

                state = AesTransformations.AddRoundKey(state, roundKeys[r]);
                roundTrace.AfterAddRoundKey = (byte[,])state.Clone();

                state = AesTransformations.InvMixColumns(state);
                roundTrace.AfterMixColumns = (byte[,])state.Clone();

                trace.Rounds.Add(roundTrace);
            }

            // Round cuối (tương ứng round 10 - không InvMixColumns)
            {
                var roundTrace = new AesRoundTrace { Round = 10 };
                state = AesTransformations.InvShiftRows(state);
                roundTrace.AfterShiftRows = (byte[,])state.Clone();

                state = AesTransformations.InvSubBytes(state);
                roundTrace.AfterSubBytes = (byte[,])state.Clone();

                state = AesTransformations.AddRoundKey(state, roundKeys[0]);
                roundTrace.AfterAddRoundKey = (byte[,])state.Clone();
                trace.Rounds.Add(roundTrace);
            }

            trace.OutputBytes = AesTransformations.StateToBytes(state);
            return trace;
        }

        // ==========================================
        //  QUICK AES (System.Security.Cryptography)
        // ==========================================

        /// <summary>
        /// Quick AES Encrypt bằng .NET crypto (ECB mode, no padding).
        /// Hỗ trợ AES-128 (16), AES-192 (24), AES-256 (32) byte key.
        /// </summary>
        public static byte[] QuickEncrypt(byte[] plaintext, byte[] key)
        {
            if (plaintext.Length % 16 != 0)
                throw new ArgumentException("Plaintext phải là bội số của 16 bytes.");
            using var aes = Aes.Create();
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.None;
            aes.Key = key;
            using var enc = aes.CreateEncryptor();
            return enc.TransformFinalBlock(plaintext, 0, plaintext.Length);
        }

        /// <summary>
        /// Quick AES Decrypt bằng .NET crypto.
        /// </summary>
        public static byte[] QuickDecrypt(byte[] ciphertext, byte[] key)
        {
            if (ciphertext.Length % 16 != 0)
                throw new ArgumentException("Ciphertext phải là bội số của 16 bytes.");
            using var aes = Aes.Create();
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.None;
            aes.Key = key;
            using var dec = aes.CreateDecryptor();
            return dec.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
        }
    }
}

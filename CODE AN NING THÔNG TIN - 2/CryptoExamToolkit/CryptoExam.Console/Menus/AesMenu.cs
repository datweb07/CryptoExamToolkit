// File: CryptoExam.Console/Menus/AesMenu.cs
using System;
using CryptoExam.Core.AES;
using CryptoExam.Core.Common;
using CryptoExam.Console.Helpers;

namespace CryptoExam.Console.Menus
{
    public static class AesMenu
    {
        public static void Show()
        {
            while (true)
            {
                ConsoleOutput.PrintHeader("AES - Advanced Encryption Standard");
                System.Console.WriteLine("  --- EDUCATIONAL AES-128 (self-implemented, có trace) ---");
                System.Console.WriteLine("  1. AES-128 Encrypt (Educational Trace)");
                System.Console.WriteLine("  2. AES-128 Decrypt (Educational Trace)");
                System.Console.WriteLine("  3. Xem round key (Key Expansion)");
                System.Console.WriteLine("  4. Xem state sau từng bước của một round");
                System.Console.WriteLine("  --- QUICK AES (System.Security.Cryptography) ---");
                System.Console.WriteLine("  5. Quick AES-128 Encrypt");
                System.Console.WriteLine("  6. Quick AES-128 Decrypt");
                System.Console.WriteLine("  7. Quick AES-256 Encrypt");
                System.Console.WriteLine("  8. Quick AES-256 Decrypt");
                System.Console.WriteLine("  0. Quay lại");

                int ch = ConsoleInput.ReadMenuChoice("\n  Chọn: ", 8);
                if (ch == 0) break;

                switch (ch)
                {
                    case 1: AesEncryptTrace(); break;
                    case 2: AesDecryptTrace(); break;
                    case 3: AesKeyExpansionMenu(); break;
                    case 4: AesRoundDetailMenu(); break;
                    case 5: QuickAes(16, encrypt: true); break;
                    case 6: QuickAes(16, encrypt: false); break;
                    case 7: QuickAes(32, encrypt: true); break;
                    case 8: QuickAes(32, encrypt: false); break;
                }
            }
        }

        static void AesEncryptTrace()
        {
            ConsoleOutput.PrintHeader("AES-128 ENCRYPT (Educational Trace)");
            string ptHex = ConsoleInput.ReadHex("  Plaintext (32 hex chars = 16 bytes): ", 32);
            string keyHex = ConsoleInput.ReadHex("  Key (32 hex chars = 16 bytes): ", 32);

            try
            {
                byte[] pt = HexUtils.HexToBytes(ptHex);
                byte[] key = HexUtils.HexToBytes(keyHex);
                var trace = AesService.EncryptTrace(pt, key);

                System.Console.WriteLine();
                ConsoleOutput.PrintKV("Ciphertext", HexUtils.BytesToHex(trace.OutputBytes));
                System.Console.WriteLine();

                bool verbose = ConsoleInput.ReadYesNo("  Hiển thị tất cả các rounds?");
                if (verbose)
                {
                    foreach (var r in trace.Rounds)
                    {
                        ConsoleOutput.PrintSubHeader($"Round {r.Round}");
                        if (r.AfterSubBytes != null)  ConsoleOutput.PrintAesState(r.AfterSubBytes, "After SubBytes");
                        if (r.AfterShiftRows != null) ConsoleOutput.PrintAesState(r.AfterShiftRows, "After ShiftRows");
                        if (r.AfterMixColumns != null) ConsoleOutput.PrintAesState(r.AfterMixColumns, "After MixColumns");
                        if (r.AfterAddRoundKey != null) ConsoleOutput.PrintAesState(r.AfterAddRoundKey, "After AddRoundKey");
                    }
                }
            }
            catch (Exception ex) { ConsoleOutput.PrintError(ex.Message); }
            ConsoleInput.PressEnterToContinue();
        }

        static void AesDecryptTrace()
        {
            ConsoleOutput.PrintHeader("AES-128 DECRYPT (Educational Trace)");
            string ctHex = ConsoleInput.ReadHex("  Ciphertext (32 hex chars = 16 bytes): ", 32);
            string keyHex = ConsoleInput.ReadHex("  Key (32 hex chars = 16 bytes): ", 32);

            try
            {
                byte[] ct = HexUtils.HexToBytes(ctHex);
                byte[] key = HexUtils.HexToBytes(keyHex);
                var trace = AesService.DecryptTrace(ct, key);

                System.Console.WriteLine();
                ConsoleOutput.PrintKV("Plaintext", HexUtils.BytesToHex(trace.OutputBytes));
            }
            catch (Exception ex) { ConsoleOutput.PrintError(ex.Message); }
            ConsoleInput.PressEnterToContinue();
        }

        static void AesKeyExpansionMenu()
        {
            ConsoleOutput.PrintHeader("AES KEY EXPANSION");
            string keyHex = ConsoleInput.ReadHex("  Key (32 hex chars = 16 bytes): ", 32);

            try
            {
                byte[] key = HexUtils.HexToBytes(keyHex);
                var roundKeys = AesKeyExpansion.ExpandKey(key);
                System.Console.WriteLine();
                for (int i = 0; i <= 10; i++)
                {
                    System.Console.Write($"  RoundKey[{i,2}]: ");
                    for (int c = 0; c < 4; c++)
                        for (int r = 0; r < 4; r++)
                            System.Console.Write($"{roundKeys[i][r, c]:X2}");
                    System.Console.WriteLine();
                }
            }
            catch (Exception ex) { ConsoleOutput.PrintError(ex.Message); }
            ConsoleInput.PressEnterToContinue();
        }

        static void AesRoundDetailMenu()
        {
            ConsoleOutput.PrintHeader("AES ROUND DETAIL");
            string ptHex = ConsoleInput.ReadHex("  Plaintext (32 hex chars): ", 32);
            string keyHex = ConsoleInput.ReadHex("  Key (32 hex chars): ", 32);

            try
            {
                byte[] pt = HexUtils.HexToBytes(ptHex);
                byte[] key = HexUtils.HexToBytes(keyHex);
                var trace = AesService.EncryptTrace(pt, key);

                int round = ConsoleInput.ReadInt("  Round (0-10): ", 0, 10);
                var r = trace.Rounds[round];

                ConsoleOutput.PrintSubHeader($"Round {r.Round} detail");
                ConsoleOutput.PrintAesState(r.AfterSubBytes,   "After SubBytes");
                ConsoleOutput.PrintAesState(r.AfterShiftRows,  "After ShiftRows");
                ConsoleOutput.PrintAesState(r.AfterMixColumns, "After MixColumns (null = round cuối)");
                ConsoleOutput.PrintAesState(r.AfterAddRoundKey,"After AddRoundKey");
            }
            catch (Exception ex) { ConsoleOutput.PrintError(ex.Message); }
            ConsoleInput.PressEnterToContinue();
        }

        static void QuickAes(int keyBytes, bool encrypt)
        {
            string mode = encrypt ? "ENCRYPT" : "DECRYPT";
            int bits = keyBytes * 8;
            ConsoleOutput.PrintHeader($"QUICK AES-{bits} {mode}");

            int hexLen = 32; // 16 bytes input
            int keyHexLen = keyBytes * 2;

            string inputHex = ConsoleInput.ReadHex($"  {(encrypt ? "Plaintext" : "Ciphertext")} (32 hex chars): ", hexLen);
            string keyHex = ConsoleInput.ReadHex($"  Key ({keyHexLen} hex chars): ", keyHexLen);

            try
            {
                byte[] input = HexUtils.HexToBytes(inputHex);
                byte[] key = HexUtils.HexToBytes(keyHex);
                byte[] output = encrypt
                    ? AesService.QuickEncrypt(input, key)
                    : AesService.QuickDecrypt(input, key);

                System.Console.WriteLine();
                ConsoleOutput.PrintKV(encrypt ? "Ciphertext" : "Plaintext", HexUtils.BytesToHex(output));
            }
            catch (Exception ex) { ConsoleOutput.PrintError(ex.Message); }
            ConsoleInput.PressEnterToContinue();
        }
    }
}

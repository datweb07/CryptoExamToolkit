// File: CryptoExam.Console/Menus/ClassicalMenu.cs
using System;
using System.Collections.Generic;
using CryptoExam.Core.Classical;
using CryptoExam.Console.Helpers;

namespace CryptoExam.Console.Menus
{
    public static class ClassicalMenu
    {
        public static void Show()
        {
            while (true)
            {
                ConsoleOutput.PrintHeader("MÃ HÓA CỔ ĐIỂN");
                System.Console.WriteLine("  1. Caesar Cipher");
                System.Console.WriteLine("  2. Monoalphabetic Substitution");
                System.Console.WriteLine("  3. Vigenere Cipher");
                System.Console.WriteLine("  4. One-Time Pad (OTP)");
                System.Console.WriteLine("  5. Playfair Cipher");
                System.Console.WriteLine("  6. Rail Fence Zigzag");
                System.Console.WriteLine("  7. Columnar Transposition");
                System.Console.WriteLine("  8. Double Transposition");
                System.Console.WriteLine("  0. Quay lại Main Menu");

                int choice = ConsoleInput.ReadMenuChoice("\n  Chọn: ", 8);
                if (choice == 0) break;

                switch (choice)
                {
                    case 1: CaesarMenu(); break;
                    case 2: MonoMenu(); break;
                    case 3: VigenereMenu(); break;
                    case 4: OtpMenu(); break;
                    case 5: PlayfairMenu(); break;
                    case 6: RailFenceMenu(); break;
                    case 7: ColumnarMenu(); break;
                    case 8: DoubleTransMenu(); break;
                }
            }
        }

        // ==================== CAESAR ====================
        static void CaesarMenu()
        {
            while (true)
            {
                ConsoleOutput.PrintHeader("CAESAR CIPHER");
                System.Console.WriteLine("  1. Encrypt");
                System.Console.WriteLine("  2. Decrypt");
                System.Console.WriteLine("  3. Brute Force (thử tất cả 25 khóa)");
                System.Console.WriteLine("  0. Quay lại");
                int ch = ConsoleInput.ReadMenuChoice("\n  Chọn: ", 3);
                if (ch == 0) break;

                if (ch == 1 || ch == 2)
                {
                    string text = ConsoleInput.ReadString("  Nhập text: ");
                    int k = ConsoleInput.ReadInt("  Nhập k: ", 0, 25);
                    bool preserveSpaces = ConsoleInput.ReadYesNo("  Giữ nguyên khoảng trắng?");

                    string result = ch == 1
                        ? CaesarCipher.Encrypt(text, k, preserveSpaces)
                        : CaesarCipher.Decrypt(text, k, preserveSpaces);

                    System.Console.WriteLine();
                    ConsoleOutput.PrintKV(ch == 1 ? "Ciphertext" : "Plaintext", result);
                }
                else // Brute force
                {
                    string ct = ConsoleInput.ReadString("  Nhập ciphertext: ");
                    var results = CaesarCipher.BruteForce(ct);
                    System.Console.WriteLine();
                    foreach (var kv in results)
                        System.Console.WriteLine($"  k={kv.Key,2}: {kv.Value}");
                }
                ConsoleInput.PressEnterToContinue();
            }
        }

        // ==================== MONOALPHABETIC ====================
        static void MonoMenu()
        {
            while (true)
            {
                ConsoleOutput.PrintHeader("MONOALPHABETIC SUBSTITUTION");
                System.Console.WriteLine("  1. Encrypt");
                System.Console.WriteLine("  2. Decrypt");
                System.Console.WriteLine("  3. Show Mapping Table");
                System.Console.WriteLine("  0. Quay lại");
                int ch = ConsoleInput.ReadMenuChoice("\n  Chọn: ", 3);
                if (ch == 0) break;

                System.Console.WriteLine("  Plain alphabet  (default: ABCDEFGHIJKLMNOPQRSTUVWXYZ)");
                string plain = ConsoleInput.ReadString("  Plain alphabet : ");
                System.Console.WriteLine("  Cipher alphabet (26 ký tự A-Z, không trùng)");
                string cipher = ConsoleInput.ReadString("  Cipher alphabet: ");

                try
                {
                    var mono = new MonoalphabeticCipher(plain, cipher);
                    if (ch == 3) { mono.PrintMappingTable(); }
                    else
                    {
                        string text = ConsoleInput.ReadString("  Nhập text: ");
                        string result = ch == 1 ? mono.Encrypt(text) : mono.Decrypt(text);
                        System.Console.WriteLine();
                        ConsoleOutput.PrintKV(ch == 1 ? "Ciphertext" : "Plaintext", result);
                    }
                }
                catch (Exception ex) { ConsoleOutput.PrintError(ex.Message); }
                ConsoleInput.PressEnterToContinue();
            }
        }

        // ==================== VIGENERE ====================
        static void VigenereMenu()
        {
            while (true)
            {
                ConsoleOutput.PrintHeader("VIGENERE CIPHER");
                System.Console.WriteLine("  1. Encrypt");
                System.Console.WriteLine("  2. Decrypt");
                System.Console.WriteLine("  0. Quay lại");
                int ch = ConsoleInput.ReadMenuChoice("\n  Chọn: ", 2);
                if (ch == 0) break;

                string text = ConsoleInput.ReadString("  Nhập text: ");
                string key = ConsoleInput.ReadString("  Nhập key: ");
                bool verbose = ConsoleInput.ReadYesNo("  Hiển thị expanded key?");

                try
                {
                    string result, expKey;
                    if (ch == 1) (result, expKey) = VigenereCipher.Encrypt(text, key);
                    else (result, expKey) = VigenereCipher.Decrypt(text, key);

                    System.Console.WriteLine();
                    if (verbose) ConsoleOutput.PrintKV("Expanded Key", expKey);
                    ConsoleOutput.PrintKV(ch == 1 ? "Ciphertext" : "Plaintext", result);
                }
                catch (Exception ex) { ConsoleOutput.PrintError(ex.Message); }
                ConsoleInput.PressEnterToContinue();
            }
        }

        // ==================== OTP ====================
        static void OtpMenu()
        {
            while (true)
            {
                ConsoleOutput.PrintHeader("ONE-TIME PAD (OTP)");
                System.Console.WriteLine("  QUAN TRỌNG: Key PHẢI cùng độ dài message!");
                System.Console.WriteLine("  1. Encrypt");
                System.Console.WriteLine("  2. Decrypt");
                System.Console.WriteLine("  0. Quay lại");
                int ch = ConsoleInput.ReadMenuChoice("\n  Chọn: ", 2);
                if (ch == 0) break;

                string text = ConsoleInput.ReadString("  Nhập text: ");
                string key = ConsoleInput.ReadString("  Nhập key (cùng độ dài): ");

                try
                {
                    string result = ch == 1
                        ? OneTimePadCipher.Encrypt(text, key)
                        : OneTimePadCipher.Decrypt(text, key);
                    System.Console.WriteLine();
                    ConsoleOutput.PrintKV(ch == 1 ? "Ciphertext" : "Plaintext", result);
                }
                catch (Exception ex) { ConsoleOutput.PrintError(ex.Message); }
                ConsoleInput.PressEnterToContinue();
            }
        }

        // ==================== PLAYFAIR ====================
        static void PlayfairMenu()
        {
            while (true)
            {
                ConsoleOutput.PrintHeader("PLAYFAIR CIPHER");
                System.Console.WriteLine("  1. Encrypt");
                System.Console.WriteLine("  2. Decrypt");
                System.Console.WriteLine("  3. Show 5x5 Matrix");
                System.Console.WriteLine("  4. Show Prepared Digrams");
                System.Console.WriteLine("  5. Encrypt single pair");
                System.Console.WriteLine("  6. Decrypt single pair");
                System.Console.WriteLine("  0. Quay lại");
                int ch = ConsoleInput.ReadMenuChoice("\n  Chọn: ", 6);
                if (ch == 0) break;

                string key = ConsoleInput.ReadString("  Nhập key: ");
                try
                {
                    var pf = new PlayfairCipher(key);
                    pf.PrintMatrix();

                    switch (ch)
                    {
                        case 1:
                        {
                            string pt = ConsoleInput.ReadString("  Nhập plaintext: ");
                            var (pairs, prepared) = pf.PreparePlaintext(pt);
                            System.Console.WriteLine();
                            ConsoleOutput.PrintKV("Prepared", prepared);
                            ConsoleOutput.PrintKV("Ciphertext", pf.Encrypt(pt));
                            break;
                        }
                        case 2:
                        {
                            string ct = ConsoleInput.ReadString("  Nhập ciphertext: ");
                            ConsoleOutput.PrintKV("Plaintext", pf.Decrypt(ct));
                            break;
                        }
                        case 3: break; // matrix already printed
                        case 4:
                        {
                            string pt = ConsoleInput.ReadString("  Nhập plaintext: ");
                            var (_, prepared) = pf.PreparePlaintext(pt);
                            ConsoleOutput.PrintKV("Prepared", prepared);
                            break;
                        }
                        case 5:
                        {
                            string pair = ConsoleInput.ReadString("  Nhập cặp 2 ký tự (ví dụ: BA): ");
                            if (pair.Length < 2) { ConsoleOutput.PrintError("Cần 2 ký tự."); break; }
                            ConsoleOutput.PrintKV("Encrypted pair", pf.EncryptPair(pair[0], pair[1]));
                            break;
                        }
                        case 6:
                        {
                            string pair = ConsoleInput.ReadString("  Nhập cặp 2 ký tự (ví dụ: IB): ");
                            if (pair.Length < 2) { ConsoleOutput.PrintError("Cần 2 ký tự."); break; }
                            ConsoleOutput.PrintKV("Decrypted pair", pf.DecryptPair(pair[0], pair[1]));
                            break;
                        }
                    }
                }
                catch (Exception ex) { ConsoleOutput.PrintError(ex.Message); }
                ConsoleInput.PressEnterToContinue();
            }
        }

        // ==================== RAIL FENCE ====================
        static void RailFenceMenu()
        {
            while (true)
            {
                ConsoleOutput.PrintHeader("RAIL FENCE ZIGZAG");
                System.Console.WriteLine("  1. Encrypt");
                System.Console.WriteLine("  2. Decrypt");
                System.Console.WriteLine("  3. Show zigzag pattern");
                System.Console.WriteLine("  0. Quay lại");
                int ch = ConsoleInput.ReadMenuChoice("\n  Chọn: ", 3);
                if (ch == 0) break;

                string text = ConsoleInput.ReadString("  Nhập text: ");
                int rails = ConsoleInput.ReadInt("  Số rails: ", 2, 100);

                try
                {
                    switch (ch)
                    {
                        case 1: ConsoleOutput.PrintKV("Ciphertext", RailFenceCipher.EncryptZigZag(text, rails)); break;
                        case 2: ConsoleOutput.PrintKV("Plaintext", RailFenceCipher.DecryptZigZag(text, rails)); break;
                        case 3: RailFenceCipher.PrintZigZagPattern(text, rails); break;
                    }
                }
                catch (Exception ex) { ConsoleOutput.PrintError(ex.Message); }
                ConsoleInput.PressEnterToContinue();
            }
        }

        // ==================== COLUMNAR ====================
        static void ColumnarMenu()
        {
            while (true)
            {
                ConsoleOutput.PrintHeader("COLUMNAR TRANSPOSITION");
                System.Console.WriteLine("  1. Encrypt");
                System.Console.WriteLine("  2. Decrypt");
                System.Console.WriteLine("  3. Show column order");
                System.Console.WriteLine("  0. Quay lại");
                int ch = ConsoleInput.ReadMenuChoice("\n  Chọn: ", 3);
                if (ch == 0) break;

                string keyword = ConsoleInput.ReadString("  Nhập keyword: ");
                try
                {
                    var ct = new ColumnarTranspositionCipher(keyword);
                    System.Console.Write($"  Column order: ");
                    foreach (int idx in ct.ColumnOrder) System.Console.Write($"{idx + 1} ");
                    System.Console.WriteLine();

                    switch (ch)
                    {
                        case 1:
                        {
                            string text = ConsoleInput.ReadString("  Nhập plaintext: ");
                            ct.PrintMatrix(text);
                            ConsoleOutput.PrintKV("Ciphertext", ct.Encrypt(text));
                            break;
                        }
                        case 2:
                        {
                            string text = ConsoleInput.ReadString("  Nhập ciphertext: ");
                            ConsoleOutput.PrintKV("Plaintext", ct.Decrypt(text));
                            break;
                        }
                        case 3: break;
                    }
                }
                catch (Exception ex) { ConsoleOutput.PrintError(ex.Message); }
                ConsoleInput.PressEnterToContinue();
            }
        }

        // ==================== DOUBLE TRANSPOSITION ====================
        static void DoubleTransMenu()
        {
            while (true)
            {
                ConsoleOutput.PrintHeader("DOUBLE TRANSPOSITION");
                System.Console.WriteLine("  1. Encrypt");
                System.Console.WriteLine("  2. Decrypt");
                System.Console.WriteLine("  0. Quay lại");
                int ch = ConsoleInput.ReadMenuChoice("\n  Chọn: ", 2);
                if (ch == 0) break;

                string key1 = ConsoleInput.ReadString("  Nhập Key 1: ");
                string key2 = ConsoleInput.ReadString("  Nhập Key 2: ");
                string text = ConsoleInput.ReadString("  Nhập text: ");

                try
                {
                    if (ch == 1)
                    {
                        var (s1, s2) = DoubleTranspositionCipher.Encrypt(text, key1, key2);
                        ConsoleOutput.PrintKV("Sau Key1", s1);
                        ConsoleOutput.PrintKV("Ciphertext", s2);
                    }
                    else
                    {
                        var (s1, s2) = DoubleTranspositionCipher.Decrypt(text, key1, key2);
                        ConsoleOutput.PrintKV("Sau Key2 (ngược)", s1);
                        ConsoleOutput.PrintKV("Plaintext", s2);
                    }
                }
                catch (Exception ex) { ConsoleOutput.PrintError(ex.Message); }
                ConsoleInput.PressEnterToContinue();
            }
        }
    }
}

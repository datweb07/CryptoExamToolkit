// File: CryptoExam.Console/Menus/TripleDesMenu.cs
using System;
using CryptoExam.Core.DES;
using CryptoExam.Console.Helpers;

namespace CryptoExam.Console.Menus
{
    public static class TripleDesMenu
    {
        public static void Show()
        {
            while (true)
            {
                ConsoleOutput.PrintHeader("TRIPLE DES / DOUBLE DES");
                System.Console.WriteLine("  --- DOUBLE DES ---");
                System.Console.WriteLine("  1. Double DES Encrypt  [C = E_K2(E_K1(P))]");
                System.Console.WriteLine("  2. Double DES Decrypt  [P = D_K1(D_K2(C))]");
                System.Console.WriteLine("  --- TRIPLE DES ---");
                System.Console.WriteLine("  3. 3DES EDE 2-key Encrypt  [C = E_K1(D_K2(E_K1(P)))]");
                System.Console.WriteLine("  4. 3DES EDE 2-key Decrypt");
                System.Console.WriteLine("  5. 3DES EDE 3-key Encrypt  [C = E_K3(D_K2(E_K1(P)))]  (ANSI X9.52)");
                System.Console.WriteLine("  6. 3DES EEE 3-key Encrypt  [C = E_K3(E_K2(E_K1(P)))]");
                System.Console.WriteLine("  0. Quay lại");

                int ch = ConsoleInput.ReadMenuChoice("\n  Chọn: ", 6);
                if (ch == 0) break;

                try
                {
                    switch (ch)
                    {
                        case 1:
                        {
                            string p = ConsoleInput.ReadHex("  Plaintext (16 hex): ", 16);
                            string k1 = ConsoleInput.ReadHex("  Key1 (16 hex): ", 16);
                            string k2 = ConsoleInput.ReadHex("  Key2 (16 hex): ", 16);
                            var (s1, ct) = TripleDesService.DoubleDesEncrypt(p, k1, k2);
                            System.Console.WriteLine();
                            ConsoleOutput.PrintKV("Step1 E_K1(P)", s1);
                            ConsoleOutput.PrintKV("Ciphertext C", ct);
                            break;
                        }
                        case 2:
                        {
                            string c = ConsoleInput.ReadHex("  Ciphertext (16 hex): ", 16);
                            string k1 = ConsoleInput.ReadHex("  Key1 (16 hex): ", 16);
                            string k2 = ConsoleInput.ReadHex("  Key2 (16 hex): ", 16);
                            var (s1, pt) = TripleDesService.DoubleDesDecrypt(c, k1, k2);
                            System.Console.WriteLine();
                            ConsoleOutput.PrintKV("Step1 D_K2(C)", s1);
                            ConsoleOutput.PrintKV("Plaintext P", pt);
                            break;
                        }
                        case 3:
                        {
                            string p = ConsoleInput.ReadHex("  Plaintext (16 hex): ", 16);
                            string k1 = ConsoleInput.ReadHex("  Key1 (16 hex): ", 16);
                            string k2 = ConsoleInput.ReadHex("  Key2 (16 hex): ", 16);
                            var (s1, s2, ct) = TripleDesService.TripleDesEde2Encrypt(p, k1, k2);
                            System.Console.WriteLine();
                            ConsoleOutput.PrintKV("Step1 E_K1(P)", s1);
                            ConsoleOutput.PrintKV("Step2 D_K2(...)", s2);
                            ConsoleOutput.PrintKV("Ciphertext C", ct);
                            break;
                        }
                        case 4:
                        {
                            string c = ConsoleInput.ReadHex("  Ciphertext (16 hex): ", 16);
                            string k1 = ConsoleInput.ReadHex("  Key1 (16 hex): ", 16);
                            string k2 = ConsoleInput.ReadHex("  Key2 (16 hex): ", 16);
                            var (s1, s2, pt) = TripleDesService.TripleDesEde2Decrypt(c, k1, k2);
                            System.Console.WriteLine();
                            ConsoleOutput.PrintKV("Step1 D_K1(C)", s1);
                            ConsoleOutput.PrintKV("Step2 E_K2(...)", s2);
                            ConsoleOutput.PrintKV("Plaintext P", pt);
                            break;
                        }
                        case 5:
                        {
                            string p = ConsoleInput.ReadHex("  Plaintext (16 hex): ", 16);
                            string k1 = ConsoleInput.ReadHex("  Key1 (16 hex): ", 16);
                            string k2 = ConsoleInput.ReadHex("  Key2 (16 hex): ", 16);
                            string k3 = ConsoleInput.ReadHex("  Key3 (16 hex): ", 16);
                            var (s1, s2, ct) = TripleDesService.TripleDesEde3Encrypt(p, k1, k2, k3);
                            System.Console.WriteLine();
                            ConsoleOutput.PrintKV("Step1 E_K1(P)", s1);
                            ConsoleOutput.PrintKV("Step2 D_K2(...)", s2);
                            ConsoleOutput.PrintKV("Ciphertext C", ct);
                            break;
                        }
                        case 6:
                        {
                            string p = ConsoleInput.ReadHex("  Plaintext (16 hex): ", 16);
                            string k1 = ConsoleInput.ReadHex("  Key1 (16 hex): ", 16);
                            string k2 = ConsoleInput.ReadHex("  Key2 (16 hex): ", 16);
                            string k3 = ConsoleInput.ReadHex("  Key3 (16 hex): ", 16);
                            var (s1, s2, ct) = TripleDesService.TripleDesEee3Encrypt(p, k1, k2, k3);
                            System.Console.WriteLine();
                            ConsoleOutput.PrintKV("Step1 E_K1(P)", s1);
                            ConsoleOutput.PrintKV("Step2 E_K2(...)", s2);
                            ConsoleOutput.PrintKV("Ciphertext C", ct);
                            break;
                        }
                    }
                }
                catch (Exception ex) { ConsoleOutput.PrintError(ex.Message); }
                ConsoleInput.PressEnterToContinue();
            }
        }
    }
}

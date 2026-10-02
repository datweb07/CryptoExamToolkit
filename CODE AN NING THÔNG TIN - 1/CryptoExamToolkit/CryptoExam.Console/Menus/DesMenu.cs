using System;
using CryptoExam.Core.DES;
using CryptoExam.ConsoleApp.Helpers;

namespace CryptoExam.ConsoleApp.Menus
{
    public static class DesMenu
    {
        public static void Show()
        {
            while (true)
            {
                ConsoleOutput.PrintHeader("DES");
                Console.WriteLine("1. Quick Query Mode (Find specific values fast)");
                Console.WriteLine("2. Full Encrypt Trace");
                Console.WriteLine("3. Full Decrypt Trace");
                Console.WriteLine("--- Triple DES ---");
                Console.WriteLine("4. 3DES EDE Encrypt");
                Console.WriteLine("5. 3DES EDE Decrypt");
                Console.WriteLine("0. Back");

                int choice = ConsoleInput.ReadInt("Choice");
                if (choice == 0) return;

                switch (choice)
                {
                    case 1: QuickQueryMenu(); break;
                    case 2: FullTraceMenu(false); break;
                    case 3: FullTraceMenu(true); break;
                    case 4: TripleDesMenu(false); break;
                    case 5: TripleDesMenu(true); break;
                }
            }
        }

        private static void QuickQueryMenu()
        {
            ConsoleOutput.PrintHeader("DES QUICK QUERY");
            string keyHex = ConsoleInput.ReadString("Key (16 hex chars)");
            string msgHex = ConsoleInput.ReadString("Message (16 hex chars)");

            DesTraceResult trace;
            try
            {
                trace = DesCipher.EncryptTrace(msgHex, keyHex);
            }
            catch (Exception ex)
            {
                ConsoleOutput.PrintError(ex.Message);
                return;
            }

            while (true)
            {
                Console.WriteLine("\nWhat do you need?");
                Console.WriteLine("1. PC1(K)");
                Console.WriteLine("2. C0 / D0");
                Console.WriteLine("3. C_i / D_i / C_iD_i");
                Console.WriteLine("4. K_i (Subkey)");
                Console.WriteLine("5. IP(M) / L0 / R0");
                Console.WriteLine("6. E(R_i) (Expanded R)");
                Console.WriteLine("7. E(R_i) XOR K_i");
                Console.WriteLine("8. S-box output (combined) round i");
                Console.WriteLine("9. P output round i");
                Console.WriteLine("10. L_i / R_i");
                Console.WriteLine("11. R16L16");
                Console.WriteLine("12. Final Ciphertext");
                Console.WriteLine("13. Show everything (Full Trace)");
                Console.WriteLine("0. Exit Query");

                int c = ConsoleInput.ReadInt("Choice");
                if (c == 0) break;

                if (c == 1)
                {
                    ConsoleOutput.PrintLabel("PC1(K)", trace.KeySchedule.Key56BitBinary);
                    ConsoleOutput.PrintLabel("C0", trace.KeySchedule.C0);
                    ConsoleOutput.PrintLabel("D0", trace.KeySchedule.D0);
                }
                else if (c == 2)
                {
                    ConsoleOutput.PrintLabel("C0", trace.KeySchedule.C0);
                    ConsoleOutput.PrintLabel("D0", trace.KeySchedule.D0);
                }
                else if (c == 5)
                {
                    ConsoleOutput.PrintLabel("IP(M)", trace.IpHex);
                    ConsoleOutput.PrintLabel("L0", trace.L0Hex);
                    ConsoleOutput.PrintLabel("R0", trace.R0Hex);
                }
                else if (c == 11)
                {
                    ConsoleOutput.PrintLabel("R16L16", trace.R16L16Hex);
                }
                else if (c == 12)
                {
                    ConsoleOutput.PrintLabel("Ciphertext", trace.CipherTextHex);
                }
                else if (c == 13)
                {
                    PrintFullTrace(trace);
                }
                else
                {
                    int round = ConsoleInput.ReadInt("Round (1-16)");
                    if (round < 1 || round > 16)
                    {
                        ConsoleOutput.PrintError("Invalid round!");
                        continue;
                    }

                    var keyRound = trace.KeySchedule.Rounds[round - 1];
                    var traceRound = trace.Rounds[round - 1];

                    switch (c)
                    {
                        case 3:
                            ConsoleOutput.PrintLabel($"C{round}", keyRound.CHex);
                            ConsoleOutput.PrintLabel($"D{round}", keyRound.DHex);
                            ConsoleOutput.PrintLabel($"C{round}D{round}", keyRound.CDHex);
                            break;
                        case 4:
                            ConsoleOutput.PrintLabel($"K{round}", keyRound.SubKeyHex);
                            break;
                        case 6:
                            ConsoleOutput.PrintLabel($"E(R{round - 1})", traceRound.ExpandedRHex);
                            break;
                        case 7:
                            ConsoleOutput.PrintLabel($"E(R{round - 1}) XOR K{round}", traceRound.XorResultHex);
                            break;
                        case 8:
                            ConsoleOutput.PrintLabel($"S-Box Round {round}", traceRound.SBoxResultHex);
                            break;
                        case 9:
                            ConsoleOutput.PrintLabel($"P Round {round}", traceRound.PResultHex);
                            break;
                        case 10:
                            ConsoleOutput.PrintLabel($"L{round}", traceRound.LHex);
                            ConsoleOutput.PrintLabel($"R{round}", traceRound.RHex);
                            break;
                    }
                }
            }
        }

        private static void FullTraceMenu(bool isDecrypt)
        {
            ConsoleOutput.PrintHeader(isDecrypt ? "DES FULL DECRYPT" : "DES FULL ENCRYPT");
            string keyHex = ConsoleInput.ReadString("Key (16 hex chars)");
            string msgHex = ConsoleInput.ReadString("Message (16 hex chars)");

            try
            {
                DesTraceResult trace = isDecrypt ? DesCipher.DecryptTrace(msgHex, keyHex) : DesCipher.EncryptTrace(msgHex, keyHex);
                PrintFullTrace(trace);
            }
            catch (Exception ex)
            {
                ConsoleOutput.PrintError(ex.Message);
            }
            ConsoleOutput.WaitForKey();
        }

        private static void PrintFullTrace(DesTraceResult trace)
        {
            Console.WriteLine("\n[KEY SCHEDULE]");
            ConsoleOutput.PrintLabel("Original Key", trace.KeySchedule.OriginalKeyHex);
            ConsoleOutput.PrintLabel("C0", trace.KeySchedule.C0);
            ConsoleOutput.PrintLabel("D0", trace.KeySchedule.D0);
            for (int i = 0; i < 16; i++)
            {
                var kr = trace.KeySchedule.Rounds[i];
                Console.WriteLine($"Round {kr.Round,-2}: K = {kr.SubKeyHex}, C = {kr.CHex}, D = {kr.DHex}");
            }

            Console.WriteLine("\n[INITIAL PERMUTATION]");
            ConsoleOutput.PrintLabel("IP(M)", trace.IpHex);
            ConsoleOutput.PrintLabel("L0", trace.L0Hex);
            ConsoleOutput.PrintLabel("R0", trace.R0Hex);

            Console.WriteLine("\n[ROUNDS]");
            for (int i = 0; i < 16; i++)
            {
                var r = trace.Rounds[i];
                Console.WriteLine($"--- ROUND {r.Round} ---");
                ConsoleOutput.PrintLabel($"L{r.Round-1}", r.LPrevHex);
                ConsoleOutput.PrintLabel($"R{r.Round-1}", r.RPrevHex);
                ConsoleOutput.PrintLabel($"E(R{r.Round-1})", r.ExpandedRHex);
                ConsoleOutput.PrintLabel($"Key applied", r.SubKeyHex);
                ConsoleOutput.PrintLabel("XOR", r.XorResultHex);
                ConsoleOutput.PrintLabel("S-Box", r.SBoxResultHex);
                ConsoleOutput.PrintLabel("P", r.PResultHex);
                ConsoleOutput.PrintLabel($"L{r.Round}", r.LHex);
                ConsoleOutput.PrintLabel($"R{r.Round}", r.RHex);
                Console.WriteLine();
            }

            Console.WriteLine("[FINAL]");
            ConsoleOutput.PrintLabel("R16L16", trace.R16L16Hex);
            ConsoleOutput.PrintLabel("Ciphertext", trace.CipherTextHex);
        }

        private static void TripleDesMenu(bool isDecrypt)
        {
            ConsoleOutput.PrintHeader(isDecrypt ? "3DES EDE DECRYPT" : "3DES EDE ENCRYPT");
            string mode = ConsoleInput.ReadString("Keys to use (2 or 3)");
            
            string text = ConsoleInput.ReadString("Text");
            string k1 = ConsoleInput.ReadString("K1");
            string k2 = ConsoleInput.ReadString("K2");
            string k3 = "";
            if (mode == "3") k3 = ConsoleInput.ReadString("K3");

            try
            {
                string res = "";
                if (mode == "2")
                    res = isDecrypt ? TripleDesService.DecryptEde(text, k1, k2) : TripleDesService.EncryptEde(text, k1, k2);
                else
                    res = isDecrypt ? TripleDesService.DecryptEde3(text, k1, k2, k3) : TripleDesService.EncryptEde3(text, k1, k2, k3);
                
                ConsoleOutput.PrintLabel("Final Result", res);
            }
            catch (Exception ex)
            {
                ConsoleOutput.PrintError(ex.Message);
            }
            ConsoleOutput.WaitForKey();
        }
    }
}

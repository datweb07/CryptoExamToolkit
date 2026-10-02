// File: CryptoExam.Console/Menus/DesMenu.cs
using System;
using System.Linq;
using CryptoExam.Core.DES;
using CryptoExam.Core.Common;
using CryptoExam.Console.Helpers;

namespace CryptoExam.Console.Menus
{
    public static class DesMenu
    {
        public static void Show()
        {
            while (true)
            {
                ConsoleOutput.PrintHeader("DES - Data Encryption Standard");
                System.Console.WriteLine("  1. Quick Query (ĐỀ HỎI GÌ LẤY NGAY)  <-- DÙNG TRONG THI");
                System.Console.WriteLine("  2. Full Encrypt (với trace)");
                System.Console.WriteLine("  3. Full Decrypt (với trace)");
                System.Console.WriteLine("  4. Key Schedule (K1..K16, C_i, D_i)");
                System.Console.WriteLine("  5. Show specific Round");
                System.Console.WriteLine("  6. DES Round Function (E, XOR, S-box, P)");
                System.Console.WriteLine("  7. Triple DES / Double DES");
                System.Console.WriteLine("  0. Quay lại Main Menu");

                int choice = ConsoleInput.ReadMenuChoice("\n  Chọn: ", 7);
                if (choice == 0) break;

                switch (choice)
                {
                    case 1: QuickQueryMenu(); break;
                    case 2: FullEncryptMenu(); break;
                    case 3: FullDecryptMenu(); break;
                    case 4: KeyScheduleMenu(); break;
                    case 5: ShowRoundMenu(); break;
                    case 6: RoundFunctionMenu(); break;
                    case 7: TripleDesMenu.Show(); break;
                }
            }
        }

        // ==================== QUICK QUERY ====================
        /// <summary>
        /// Menu quan trọng nhất: nhập key + message, hỏi cần gì, lấy ngay.
        /// </summary>
        static void QuickQueryMenu()
        {
            ConsoleOutput.PrintHeader("DES QUICK QUERY - LẤY NGAY MỌI GIÁ TRỊ");
            string keyHex = ConsoleInput.ReadHex("  Key (16 hex chars): ", 16);
            string msgHex = ConsoleInput.ReadHex("  Message/Plaintext (16 hex chars): ", 16);

            DesTraceResult? trace = null;
            try
            {
                trace = DesCipher.EncryptTrace(msgHex, keyHex);
            }
            catch (Exception ex)
            {
                ConsoleOutput.PrintError(ex.Message);
                ConsoleInput.PressEnterToContinue();
                return;
            }

            while (true)
            {
                System.Console.WriteLine();
                ConsoleOutput.PrintSubHeader("Bạn cần gì?");
                System.Console.WriteLine("   1. PC1(K)             2. C0               3. D0");
                System.Console.WriteLine("   4. C_i                5. D_i              6. C_iD_i");
                System.Console.WriteLine("   7. K_i (subkey)       8. IP(M)            9. L0");
                System.Console.WriteLine("  10. R0                11. E(R_i)          12. E(R_i) XOR K_i");
                System.Console.WriteLine("  13. S-box output R_i  14. P output R_i    15. L_i");
                System.Console.WriteLine("  16. R_i               17. R16L16          18. Ciphertext");
                System.Console.WriteLine("  19. Show ALL          20. S-box detail R_i");
                System.Console.WriteLine("   0. Quay lại");

                int q = ConsoleInput.ReadMenuChoice("\n  Cần: ", 20);
                if (q == 0) break;

                var ks = trace.KeySchedule;

                switch (q)
                {
                    case 1:
                        ConsoleOutput.PrintKV("PC1(K)", ks.Key56BitBinary);
                        ConsoleOutput.PrintKV("PC1(K) hex", ks.Key56BitHex);
                        ConsoleOutput.PrintKV("C0", ks.C0Binary + $" [{ks.C0Hex}]");
                        ConsoleOutput.PrintKV("D0", ks.D0Binary + $" [{ks.D0Hex}]");
                        break;
                    case 2:
                        ConsoleOutput.PrintKV("C0 (binary)", ks.C0Binary);
                        ConsoleOutput.PrintKV("C0 (hex)", ks.C0Hex);
                        break;
                    case 3:
                        ConsoleOutput.PrintKV("D0 (binary)", ks.D0Binary);
                        ConsoleOutput.PrintKV("D0 (hex)", ks.D0Hex);
                        break;
                    case 4:
                    {
                        int i = ConsoleInput.ReadInt("  Vòng i = ", 1, 16);
                        var kr = ks.Rounds[i - 1];
                        ConsoleOutput.PrintKV($"C{i} (binary)", kr.CBinary);
                        ConsoleOutput.PrintKV($"C{i} (hex)", kr.CHex);
                        break;
                    }
                    case 5:
                    {
                        int i = ConsoleInput.ReadInt("  Vòng i = ", 1, 16);
                        var kr = ks.Rounds[i - 1];
                        ConsoleOutput.PrintKV($"D{i} (binary)", kr.DBinary);
                        ConsoleOutput.PrintKV($"D{i} (hex)", kr.DHex);
                        break;
                    }
                    case 6:
                    {
                        int i = ConsoleInput.ReadInt("  Vòng i = ", 1, 16);
                        var kr = ks.Rounds[i - 1];
                        ConsoleOutput.PrintKV($"C{i}D{i} (binary)", kr.CDBinary);
                        ConsoleOutput.PrintKV($"C{i}D{i} (hex)", kr.CDHex);
                        break;
                    }
                    case 7:
                    {
                        int i = ConsoleInput.ReadInt("  Vòng i = ", 1, 16);
                        var kr = ks.Rounds[i - 1];
                        ConsoleOutput.PrintKV($"K{i} (binary)", kr.SubKeyBinary);
                        ConsoleOutput.PrintKV($"K{i} (hex)", kr.SubKeyHex);
                        break;
                    }
                    case 8:
                        ConsoleOutput.PrintKV("IP(M) (binary)", trace.IpBinary);
                        ConsoleOutput.PrintKV("IP(M) (hex)", trace.IpHex);
                        break;
                    case 9:
                        ConsoleOutput.PrintKV("L0 (binary)", trace.L0Binary);
                        ConsoleOutput.PrintKV("L0 (hex)", trace.L0Hex);
                        break;
                    case 10:
                        ConsoleOutput.PrintKV("R0 (binary)", trace.R0Binary);
                        ConsoleOutput.PrintKV("R0 (hex)", trace.R0Hex);
                        break;
                    case 11:
                    {
                        int i = ConsoleInput.ReadInt("  Round i = ", 1, 16);
                        var r = trace.Rounds[i - 1];
                        ConsoleOutput.PrintKV($"E(R{i - 1})", r.ExpandedRHex);
                        ConsoleOutput.PrintKV($"E(R{i - 1}) binary", r.ExpandedRBinary);
                        break;
                    }
                    case 12:
                    {
                        int i = ConsoleInput.ReadInt("  Round i = ", 1, 16);
                        var r = trace.Rounds[i - 1];
                        ConsoleOutput.PrintKV($"E(R{i - 1})", r.ExpandedRHex);
                        ConsoleOutput.PrintKV($"K{i}", r.SubKeyHex);
                        ConsoleOutput.PrintKV($"XOR", r.XorResultHex);
                        break;
                    }
                    case 13:
                    {
                        int i = ConsoleInput.ReadInt("  Round i = ", 1, 16);
                        var r = trace.Rounds[i - 1];
                        ConsoleOutput.PrintKV($"S-Box R{i}", r.SBoxResultHex);
                        break;
                    }
                    case 14:
                    {
                        int i = ConsoleInput.ReadInt("  Round i = ", 1, 16);
                        var r = trace.Rounds[i - 1];
                        ConsoleOutput.PrintKV($"P R{i}", r.PResultHex);
                        break;
                    }
                    case 15:
                    {
                        int i = ConsoleInput.ReadInt("  Round i = ", 1, 16);
                        var r = trace.Rounds[i - 1];
                        ConsoleOutput.PrintKV($"L{i}", r.LHex);
                        break;
                    }
                    case 16:
                    {
                        int i = ConsoleInput.ReadInt("  Round i = ", 1, 16);
                        var r = trace.Rounds[i - 1];
                        ConsoleOutput.PrintKV($"R{i}", r.RHex);
                        break;
                    }
                    case 17:
                        ConsoleOutput.PrintKV("R16L16", trace.R16L16Hex);
                        break;
                    case 18:
                        ConsoleOutput.PrintKV("Ciphertext", trace.CipherTextHex);
                        break;
                    case 19:
                        ConsoleOutput.PrintDesTrace(trace, showSBoxDetail: false);
                        break;
                    case 20:
                    {
                        int i = ConsoleInput.ReadInt("  Round i = ", 1, 16);
                        var r = trace.Rounds[i - 1];
                        ConsoleOutput.PrintDesRound(r, showSBoxDetail: true);
                        break;
                    }
                }
            }
        }

        // ==================== FULL ENCRYPT ====================
        static void FullEncryptMenu()
        {
            ConsoleOutput.PrintHeader("DES FULL ENCRYPT");
            string key = ConsoleInput.ReadHex("  Key (16 hex): ", 16);
            string pt = ConsoleInput.ReadHex("  Plaintext (16 hex): ", 16);

            try
            {
                var trace = DesCipher.EncryptTrace(pt, key);
                bool verbose = ConsoleInput.ReadYesNo("  Hiển thị tất cả 16 rounds?");
                if (verbose)
                    ConsoleOutput.PrintDesTrace(trace);
                else
                {
                    ConsoleOutput.PrintKV("IP(M)", trace.IpHex);
                    ConsoleOutput.PrintKV("L0",    trace.L0Hex);
                    ConsoleOutput.PrintKV("R0",    trace.R0Hex);
                    System.Console.WriteLine("  ...");
                    ConsoleOutput.PrintKV("R16L16",    trace.R16L16Hex);
                    ConsoleOutput.PrintKV("Ciphertext", trace.CipherTextHex);
                }
            }
            catch (Exception ex) { ConsoleOutput.PrintError(ex.Message); }
            ConsoleInput.PressEnterToContinue();
        }

        // ==================== FULL DECRYPT ====================
        static void FullDecryptMenu()
        {
            ConsoleOutput.PrintHeader("DES FULL DECRYPT");
            string key = ConsoleInput.ReadHex("  Key (16 hex): ", 16);
            string ct = ConsoleInput.ReadHex("  Ciphertext (16 hex): ", 16);

            try
            {
                var trace = DesCipher.DecryptTrace(ct, key);
                ConsoleOutput.PrintKV("Plaintext", trace.OutputHex);
                bool verbose = ConsoleInput.ReadYesNo("  Hiển thị trace?");
                if (verbose) ConsoleOutput.PrintDesTrace(trace);
            }
            catch (Exception ex) { ConsoleOutput.PrintError(ex.Message); }
            ConsoleInput.PressEnterToContinue();
        }

        // ==================== KEY SCHEDULE ====================
        static void KeyScheduleMenu()
        {
            ConsoleOutput.PrintHeader("DES KEY SCHEDULE");
            string key = ConsoleInput.ReadHex("  Key (16 hex): ", 16);

            try
            {
                var ks = DesKeySchedule.Generate(key);

                while (true)
                {
                    System.Console.WriteLine();
                    System.Console.WriteLine("  1. Show all K1..K16");
                    System.Console.WriteLine("  2. Show one K_i");
                    System.Console.WriteLine("  3. Show C_i");
                    System.Console.WriteLine("  4. Show D_i");
                    System.Console.WriteLine("  5. Show C_iD_i");
                    System.Console.WriteLine("  6. Show PC1(K), C0, D0");
                    System.Console.WriteLine("  0. Quay lại");
                    int ch = ConsoleInput.ReadMenuChoice("\n  Chọn: ", 6);
                    if (ch == 0) break;

                    switch (ch)
                    {
                        case 1:
                            ConsoleOutput.PrintKeySchedule(ks, showAll: true);
                            break;
                        case 2:
                        {
                            int i = ConsoleInput.ReadInt("  Round = ", 1, 16);
                            var kr = ks.Rounds[i - 1];
                            ConsoleOutput.PrintKV($"K{i}", kr.SubKeyHex);
                            ConsoleOutput.PrintKV($"K{i} binary", kr.SubKeyBinary);
                            break;
                        }
                        case 3:
                        {
                            int i = ConsoleInput.ReadInt("  Round = ", 1, 16);
                            var kr = ks.Rounds[i - 1];
                            ConsoleOutput.PrintKV($"C{i} (hex)", kr.CHex);
                            ConsoleOutput.PrintKV($"C{i} (binary)", kr.CBinary);
                            break;
                        }
                        case 4:
                        {
                            int i = ConsoleInput.ReadInt("  Round = ", 1, 16);
                            var kr = ks.Rounds[i - 1];
                            ConsoleOutput.PrintKV($"D{i} (hex)", kr.DHex);
                            ConsoleOutput.PrintKV($"D{i} (binary)", kr.DBinary);
                            break;
                        }
                        case 5:
                        {
                            int i = ConsoleInput.ReadInt("  Round = ", 1, 16);
                            var kr = ks.Rounds[i - 1];
                            ConsoleOutput.PrintKV($"C{i}D{i} (hex)", kr.CDHex);
                            ConsoleOutput.PrintKV($"C{i}D{i} (binary)", kr.CDBinary);
                            break;
                        }
                        case 6:
                            ConsoleOutput.PrintKV("PC1(K) hex",    ks.Key56BitHex);
                            ConsoleOutput.PrintKV("PC1(K) binary", ks.Key56BitBinary);
                            ConsoleOutput.PrintKV("C0 (hex)",      ks.C0Hex);
                            ConsoleOutput.PrintKV("C0 (binary)",   ks.C0Binary);
                            ConsoleOutput.PrintKV("D0 (hex)",      ks.D0Hex);
                            ConsoleOutput.PrintKV("D0 (binary)",   ks.D0Binary);
                            break;
                    }
                    ConsoleInput.PressEnterToContinue();
                }
            }
            catch (Exception ex) { ConsoleOutput.PrintError(ex.Message); ConsoleInput.PressEnterToContinue(); }
        }

        // ==================== SHOW SPECIFIC ROUND ====================
        static void ShowRoundMenu()
        {
            ConsoleOutput.PrintHeader("DES - SHOW SPECIFIC ROUND");
            string key = ConsoleInput.ReadHex("  Key (16 hex): ", 16);
            string pt = ConsoleInput.ReadHex("  Plaintext (16 hex): ", 16);

            try
            {
                var trace = DesCipher.EncryptTrace(pt, key);
                int round = ConsoleInput.ReadInt("  Round (1-16): ", 1, 16);
                bool detail = ConsoleInput.ReadYesNo("  S-box chi tiết?");
                ConsoleOutput.PrintDesRound(trace.Rounds[round - 1], detail);
            }
            catch (Exception ex) { ConsoleOutput.PrintError(ex.Message); }
            ConsoleInput.PressEnterToContinue();
        }

        // ==================== ROUND FUNCTION ====================
        static void RoundFunctionMenu()
        {
            ConsoleOutput.PrintHeader("DES ROUND FUNCTION F(R, K)");
            System.Console.WriteLine("  Nhập R (32-bit) và K (48-bit) trực tiếp:");

            string rHex = ConsoleInput.ReadHex("  R (8 hex chars): ", 8);
            string kHex = ConsoleInput.ReadHex("  K (12 hex chars): ", 12);

            try
            {
                string rBin = HexUtils.HexToBinary(rHex);
                string kBin = HexUtils.HexToBinary(kHex);
                string expandedR = Core.DES.DesBitUtils.Permute(rBin, Core.DES.DesTables.E);

                var (xorBin, xorHex, sBoxBlocks, sBoxCombBin, sBoxCombHex, pBin, pHex)
                    = DesRoundFunction.Calculate(expandedR, kBin);

                ConsoleOutput.PrintKV("E(R)",    HexUtils.BinaryToHex(expandedR));
                ConsoleOutput.PrintKV("K",       kHex);
                ConsoleOutput.PrintKV("E(R)^K",  xorHex);
                System.Console.WriteLine();
                System.Console.WriteLine($"  {"Block",-6} {"Input",-8} {"Row",-5} {"Col",-5} {"S",-5} {"Out",-6}");
                System.Console.WriteLine($"  {new string('-', 38)}");
                foreach (var b in sBoxBlocks)
                    System.Console.WriteLine($"  B{b.BlockNumber,-5} {b.InputBits,-8} {b.Row,-5} {b.Col,-5} {b.SBoxValue,-5} {b.OutputBits,-6}");
                System.Console.WriteLine();
                ConsoleOutput.PrintKV("S-Box",   sBoxCombHex);
                ConsoleOutput.PrintKV("P",       pHex);
            }
            catch (Exception ex) { ConsoleOutput.PrintError(ex.Message); }
            ConsoleInput.PressEnterToContinue();
        }
    }
}

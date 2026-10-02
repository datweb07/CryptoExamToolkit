// File: CryptoExam.Console/Helpers/ConsoleOutput.cs
using System;
using CryptoExam.Core.DES;
using CryptoExam.Core.Common;

namespace CryptoExam.Console.Helpers
{
    /// <summary>
    /// Helper in output DES và các cipher khác ra Console.
    /// </summary>
    public static class ConsoleOutput
    {
        public static void PrintHeader(string title)
        {
            string line = new string('=', 52);
            System.Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine(line);
            System.Console.WriteLine($"  {title}");
            System.Console.WriteLine(line);
            System.Console.ResetColor();
        }

        public static void PrintSubHeader(string title)
        {
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine($"\n  --- {title} ---");
            System.Console.ResetColor();
        }

        public static void PrintKV(string key, string value, int pad = 16)
        {
            System.Console.Write($"  {key.PadRight(pad)} = ");
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine(value);
            System.Console.ResetColor();
        }

        public static void PrintError(string msg)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"  [ERROR] {msg}");
            System.Console.ResetColor();
        }

        public static void PrintInfo(string msg)
        {
            System.Console.ForegroundColor = ConsoleColor.DarkGray;
            System.Console.WriteLine($"  {msg}");
            System.Console.ResetColor();
        }

        /// <summary>
        /// In toàn bộ key schedule.
        /// </summary>
        public static void PrintKeySchedule(DesKeyScheduleResult ks, bool showAll = true)
        {
            PrintSubHeader("Key Schedule");
            PrintKV("Key (Hex)", ks.OriginalKeyHex);
            PrintKV("PC1(K)", ks.Key56BitBinary + $"  [{ks.Key56BitHex}]");
            PrintKV("C0", ks.C0Binary + $"  [{ks.C0Hex}]");
            PrintKV("D0", ks.D0Binary + $"  [{ks.D0Hex}]");

            if (!showAll) return;

            System.Console.WriteLine();
            System.Console.WriteLine($"  {"Round",-6} {"Shift",-6} {"C_i (Hex)",-12} {"D_i (Hex)",-12} {"Subkey Ki (Hex)",-16}");
            System.Console.WriteLine($"  {new string('-', 56)}");
            foreach (var r in ks.Rounds)
                System.Console.WriteLine($"  {r.Round,-6} {r.Shift,-6} {r.CHex,-12} {r.DHex,-12} {r.SubKeyHex,-16}");
        }

        /// <summary>
        /// In một round DES.
        /// </summary>
        public static void PrintDesRound(DesRoundResult r, bool showSBoxDetail = false)
        {
            int pad = 16;
            PrintSubHeader($"Round {r.Round}");
            PrintKV($"L{r.Round - 1}", r.LPrevHex, pad);
            PrintKV($"R{r.Round - 1}", r.RPrevHex, pad);
            PrintKV($"E(R{r.Round - 1})", r.ExpandedRHex, pad);
            PrintKV($"K{r.Round}", r.SubKeyHex, pad);
            PrintKV($"E(R) XOR K{r.Round}", r.XorResultHex, pad);

            if (showSBoxDetail && r.SBoxBlocks != null)
            {
                System.Console.WriteLine();
                System.Console.WriteLine($"  {"Block",-6} {"Input",-8} {"Row",-5} {"Col",-5} {"S-Box",-7} {"4-bit",-6}");
                System.Console.WriteLine($"  {new string('-', 40)}");
                foreach (var b in r.SBoxBlocks)
                    System.Console.WriteLine($"  B{b.BlockNumber,-5} {b.InputBits,-8} {b.Row,-5} {b.Col,-5} {b.SBoxValue,-7} {b.OutputBits,-6}");
                System.Console.WriteLine();
            }

            PrintKV("S-Box", r.SBoxResultHex, pad);
            PrintKV("P", r.PResultHex, pad);
            PrintKV($"L{r.Round}", r.LHex, pad);
            PrintKV($"R{r.Round}", r.RHex, pad);
        }

        /// <summary>
        /// In toàn bộ DES trace.
        /// </summary>
        public static void PrintDesTrace(DesTraceResult trace, bool showSBoxDetail = false)
        {
            PrintSubHeader("Initial Permutation");
            PrintKV("Input",  trace.InputHex);
            PrintKV("IP(M)",  trace.IpHex);
            PrintKV("L0",     trace.L0Hex);
            PrintKV("R0",     trace.R0Hex);

            foreach (var r in trace.Rounds)
                PrintDesRound(r, showSBoxDetail);

            PrintSubHeader("Final");
            PrintKV("R16L16",    trace.R16L16Hex);
            PrintKV("IP^-1 Output", trace.OutputHex);
        }

        /// <summary>
        /// In state AES 4x4.
        /// </summary>
        public static void PrintAesState(byte[,]? state, string label)
        {
            if (state == null) { System.Console.WriteLine($"  {label}: N/A"); return; }
            System.Console.WriteLine($"  {label}:");
            for (int r = 0; r < 4; r++)
            {
                System.Console.Write("    ");
                for (int c = 0; c < 4; c++)
                    System.Console.Write($"{state[r, c]:X2} ");
                System.Console.WriteLine();
            }
        }
    }
}

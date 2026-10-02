using CryptoExam.ConsoleApp.Helpers;
using CryptoExam.Core.Common;
using CryptoExam.Core.DES;

namespace CryptoExam.ConsoleApp.Menus;

public sealed class DesMenu
{
    public void Run()
    {
        while (true)
        {
            ConsoleOutput.Header("DES - EDUCATIONAL FIPS TRACE");
            Console.WriteLine("1. Key Schedule Query");
            Console.WriteLine("2. Initial Permutation Query");
            Console.WriteLine("3. Round Function from R and Ki");
            Console.WriteLine("4. Complete Encrypt + Trace");
            Console.WriteLine("5. Complete Decrypt + Trace");
            Console.WriteLine("6. DES QUICK QUERY (exam mode)");
            Console.WriteLine("7. ASCII Text Mode (8-byte blocks)");
            Console.WriteLine("8. Show Specific Feistel Round (full trace)");
            Console.WriteLine("0. Back");
            var choice = ConsoleInput.Integer("Chọn: ", 0, 8);
            if (choice == 0) return;
            ConsoleOutput.Guard(() => Execute(choice));
        }
    }

    private static void Execute(int choice)
    {
        switch (choice)
        {
            case 1: KeySchedule(); break;
            case 2: InitialPermutation(); break;
            case 3: ManualRoundFunction(); break;
            case 4: Complete(false); break;
            case 5: Complete(true); break;
            case 6: QuickQuery(); break;
            case 7: AsciiEncrypt(); break;
            case 8: SpecificRound(); break;
        }
    }

    private static void KeySchedule()
    {
        var schedule = DesKeySchedule.Generate(ConsoleInput.Hex("Key (16 hex): ", 16));
        Console.WriteLine("1.All K 2.One Ki 3.Ci 4.Di 5.CiDi 6.PC1(K) 7.C0/D0");
        var mode = ConsoleInput.Integer("Need: ", 1, 7);
        if (mode == 1) foreach (var round in schedule.Rounds) ConsoleOutput.Result($"K{round.Round}", round.SubKeyHex);
        else if (mode == 6) { ConsoleOutput.Result("PC1(K) hex", schedule.Pc1Hex); ConsoleOutput.Result("PC1(K) binary", schedule.Pc1Binary); }
        else if (mode == 7) { ConsoleOutput.Result("C0", schedule.C0Hex); ConsoleOutput.Result("D0", schedule.D0Hex); }
        else
        {
            var number = ConsoleInput.Integer("Round i = ", 1, 16); var round = schedule.Rounds[number - 1];
            ConsoleOutput.Result(mode switch { 2 => $"K{number}", 3 => $"C{number}", 4 => $"D{number}", _ => $"C{number}D{number}" },
                mode switch { 2 => round.SubKeyHex, 3 => round.CHex, 4 => round.DHex, _ => round.CDHex });
        }
    }

    private static void InitialPermutation()
    {
        var message = HexUtils.ToUInt64(ConsoleInput.Hex("Message (16 hex): ", 16), 16);
        var ip = DesBitUtils.Permute(message, 64, DesTables.IP); var left = (uint)(ip >> 32); var right = (uint)ip;
        ConsoleOutput.Result("M binary", HexUtils.ToBinary(message, 64)); ConsoleOutput.Result("IP(M) binary", HexUtils.ToBinary(ip, 64));
        ConsoleOutput.Result("IP(M)", ip.ToString("X16")); ConsoleOutput.Result("L0", left.ToString("X8")); ConsoleOutput.Result("R0", right.ToString("X8"));
    }

    private static void ManualRoundFunction()
    {
        var right = (uint)HexUtils.ToUInt64(ConsoleInput.Hex("R (8 hex): ", 8), 8);
        var key = HexUtils.ToUInt64(ConsoleInput.Hex("Ki (12 hex): ", 12), 12);
        PrintFunction(DesRoundFunction.Calculate(right, key));
    }

    private static void Complete(bool decrypt)
    {
        var key = ConsoleInput.Hex("Key (16 hex): ", 16);
        var input = ConsoleInput.Hex(decrypt ? "Ciphertext (16 hex): " : "Plaintext (16 hex): ", 16);
        var trace = DesCipher.Trace(input, key, decrypt);
        PrintTrace(trace, true);
    }

    private static void QuickQuery()
    {
        Console.WriteLine("HEX BLOCK MODE (64-bit)");
        var key = ConsoleInput.Hex("Key (16 hex): ", 16); var message = ConsoleInput.Hex("Message (16 hex): ", 16);
        var trace = DesCipher.Trace(message, key);
        Console.WriteLine("1.PC1(K)  2.C0  3.D0  4.C_i  5.D_i  6.C_iD_i  7.K_i");
        Console.WriteLine("8.IP(M)   9.L0 10.R0 11.E(R_i) 12.E(R_i)^K_i 13.S-box round i");
        Console.WriteLine("14.P round i 15.L_i 16.R_i 17.R16L16 18.Ciphertext 19.Show everything");
        var query = ConsoleInput.Integer("What do you need? ", 1, 19);
        int? roundNumber = query is >= 4 and <= 7 or >= 11 and <= 16 ? ConsoleInput.Integer("Round i = ", 1, 16) : null;
        var keyRound = roundNumber.HasValue ? trace.KeySchedule.Rounds[roundNumber.Value - 1] : null;
        var round = roundNumber.HasValue ? trace.Rounds[roundNumber.Value - 1] : null;
        switch (query)
        {
            case 1: ConsoleOutput.Result("PC1(K)", trace.KeySchedule.Pc1Hex); break;
            case 2: ConsoleOutput.Result("C0", trace.KeySchedule.C0Hex); break;
            case 3: ConsoleOutput.Result("D0", trace.KeySchedule.D0Hex); break;
            case 4: ConsoleOutput.Result($"C{roundNumber}", keyRound!.CHex); break;
            case 5: ConsoleOutput.Result($"D{roundNumber}", keyRound!.DHex); break;
            case 6: ConsoleOutput.Result($"C{roundNumber}D{roundNumber}", keyRound!.CDHex); break;
            case 7: ConsoleOutput.Result($"K{roundNumber}", keyRound!.SubKeyHex); break;
            case 8: ConsoleOutput.Result("IP(M)", trace.InitialPermutationHex); break;
            case 9: ConsoleOutput.Result("L0", trace.L0Hex); break;
            case 10: ConsoleOutput.Result("R0", trace.R0Hex); break;
            case 11: ConsoleOutput.Result($"E(R{roundNumber!.Value - 1})", round!.Function.ExpandedRHex); break;
            case 12: ConsoleOutput.Result($"E(R{roundNumber!.Value - 1})^K{roundNumber}", round!.Function.XorHex); break;
            case 13: ConsoleOutput.Result($"S-box round {roundNumber}", round!.Function.SBoxHex); break;
            case 14: ConsoleOutput.Result($"P round {roundNumber}", round!.Function.PHex); break;
            case 15: ConsoleOutput.Result($"L{roundNumber}", round!.LHex); break;
            case 16: ConsoleOutput.Result($"R{roundNumber}", round!.RHex); break;
            case 17: ConsoleOutput.Result("R16L16", trace.PreOutputHex); break;
            case 18: ConsoleOutput.Result("Ciphertext", trace.OutputHex); break;
            case 19: PrintTrace(trace, true); break;
        }
    }

    private static void SpecificRound()
    {
        var key = ConsoleInput.Hex("Key (16 hex): ", 16); var message = ConsoleInput.Hex("Message (16 hex): ", 16);
        var number = ConsoleInput.Integer("Round i = ", 1, 16); var round = DesCipher.Trace(message, key).Rounds[number - 1];
        ConsoleOutput.Result($"L{number - 1}", round.LPreviousHex); ConsoleOutput.Result($"R{number - 1}", round.RPreviousHex);
        PrintFunction(round.Function);
        ConsoleOutput.Result($"L{number}", round.LHex); ConsoleOutput.Result($"R{number}", round.RHex);
    }

    private static void AsciiEncrypt()
    {
        var mode = ConsoleInput.Integer("1.Encrypt text  2.Decrypt hex blocks: ", 1, 2);
        var input = ConsoleInput.Text(mode == 1 ? "ASCII text: " : "Ciphertext hex: "); var key = ConsoleInput.Hex("Key (16 hex): ", 16);
        Console.WriteLine("Padding: 1.None 2.Zero 3.PKCS#7"); var padding = (DesPaddingMode)(ConsoleInput.Integer("Chọn: ", 1, 3) - 1);
        if (mode == 1)
        {
            foreach (var block in DesAsciiService.Encrypt(input, key, padding))
                Console.WriteLine($"Block {block.Number}: {block.InputHex} -> {block.OutputHex}");
        }
        else
        {
            var result = DesAsciiService.Decrypt(input, key, padding);
            foreach (var block in result.Blocks) Console.WriteLine($"Block {block.Number}: {block.InputHex} -> {block.OutputHex}");
            ConsoleOutput.Result("ASCII plaintext", result.Text);
        }
    }

    private static void PrintTrace(DesTraceResult trace, bool includeKeys)
    {
        ConsoleOutput.Result("IP", trace.InitialPermutationHex); ConsoleOutput.Result("L0", trace.L0Hex); ConsoleOutput.Result("R0", trace.R0Hex);
        if (includeKeys)
        {
            Console.WriteLine("\nSubkeys:");
            foreach (var key in trace.KeySchedule.Rounds) ConsoleOutput.Result($"K{key.Round}", key.SubKeyHex);
        }
        Console.WriteLine("\nRounds:");
        foreach (var round in trace.Rounds) Console.WriteLine($"Round {round.Round,2}: L{round.Round}={round.LHex}  R{round.Round}={round.RHex}  P={round.Function.PHex}");
        ConsoleOutput.Result("R16L16", trace.PreOutputHex); ConsoleOutput.Result(trace.IsDecryption ? "Plaintext" : "Ciphertext", trace.OutputHex);
    }

    public static void PrintFunction(DesRoundFunctionResult result)
    {
        ConsoleOutput.Result("E(R)", result.ExpandedRHex); ConsoleOutput.Result("Ki", result.SubKeyHex); ConsoleOutput.Result("E(R) XOR Ki", result.XorHex);
        Console.WriteLine("\nS-box details:");
        foreach (var box in result.SBoxes)
            Console.WriteLine($"B{box.SBoxNumber}={box.InputBinary} row={box.RowBits}({box.Row}) col={box.ColumnBits}({box.Column}) S{box.SBoxNumber}={box.Value} -> {box.OutputBinary}");
        ConsoleOutput.Result("SBoxCombined", result.SBoxHex); ConsoleOutput.Result("P", result.PHex);
    }
}

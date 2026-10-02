using CryptoExam.ConsoleApp.Helpers;
using CryptoExam.Core.AES;

namespace CryptoExam.ConsoleApp.Menus;

public sealed class AesMenu
{
    public void Run()
    {
        while (true)
        {
            ConsoleOutput.Header("AES");
            Console.WriteLine("1. Quick Encrypt (AES-128/192/256, ECB single block)");
            Console.WriteLine("2. Quick Decrypt (AES-128/192/256, ECB single block)");
            Console.WriteLine("3. Educational AES-128 Trace - Show All");
            Console.WriteLine("4. Educational AES-128 Quick Query");
            Console.WriteLine("0. Back");
            var choice = ConsoleInput.Integer("Chọn: ", 0, 4);
            if (choice == 0) return;
            ConsoleOutput.Guard(() => Execute(choice));
        }
    }

    private static void Execute(int choice)
    {
        var block = ConsoleInput.Hex(choice == 2 ? "Ciphertext (32 hex): " : "Plaintext (32 hex): ", 32);
        var key = ConsoleInput.Hex(choice <= 2 ? "Key (32/48/64 hex): " : "AES-128 key (32 hex): ");
        if (choice == 1) ConsoleOutput.Result("Ciphertext", AesService.QuickEncrypt(block, key));
        else if (choice == 2) ConsoleOutput.Result("Plaintext", AesService.QuickDecrypt(block, key));
        else
        {
            var trace = AesService.TraceEncrypt128(block, key);
            if (choice == 3)
            {
                foreach (var round in trace.Rounds)
                {
                    Console.WriteLine($"\nROUND {round.Round}");
                    ConsoleOutput.Result("RoundKey", round.RoundKeyHex);
                    if (round.SubBytesHex is not null) ConsoleOutput.Result("After SubBytes", round.SubBytesHex);
                    if (round.ShiftRowsHex is not null) ConsoleOutput.Result("After ShiftRows", round.ShiftRowsHex);
                    if (round.MixColumnsHex is not null) ConsoleOutput.Result("After MixColumns", round.MixColumnsHex);
                    ConsoleOutput.Result("After AddRoundKey", round.AddRoundKeyHex);
                }
                ConsoleOutput.Result("Ciphertext", trace.CiphertextHex);
            }
            else
            {
                Console.WriteLine("1.RoundKey 2.After SubBytes 3.After ShiftRows 4.After MixColumns 5.After AddRoundKey 6.Final Ciphertext");
                var query = ConsoleInput.Integer("Need: ", 1, 6);
                if (query == 6) { ConsoleOutput.Result("Ciphertext", trace.CiphertextHex); return; }
                var minRound = query is 1 or 5 ? 0 : 1;
                var roundNumber = ConsoleInput.Integer("Round i = ", minRound, 10);
                var round = trace.Rounds[roundNumber];
                var value = query switch { 1 => round.RoundKeyHex, 2 => round.SubBytesHex, 3 => round.ShiftRowsHex, 4 => round.MixColumnsHex, _ => round.AddRoundKeyHex };
                ConsoleOutput.Result($"Round {roundNumber} result", value ?? "N/A (round 10 has no MixColumns)");
            }
        }
    }
}

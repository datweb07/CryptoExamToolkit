using CryptoExam.ConsoleApp.Helpers;
using CryptoExam.Core.DES;

namespace CryptoExam.ConsoleApp.Menus;

public sealed class TripleDesMenu
{
    public void Run()
    {
        while (true)
        {
            ConsoleOutput.Header("DOUBLE DES / TRIPLE DES");
            Console.WriteLine("1. Double DES Encrypt: E_K2(E_K1(P))");
            Console.WriteLine("2. Double DES Decrypt");
            Console.WriteLine("3. Triple DES EDE Encrypt (K3 optional = K1)");
            Console.WriteLine("4. Triple DES EDE Decrypt");
            Console.WriteLine("5. Triple DES EEE Encrypt (separate labeled mode)");
            Console.WriteLine("0. Back");
            var choice = ConsoleInput.Integer("Chọn: ", 0, 5);
            if (choice == 0) return;
            ConsoleOutput.Guard(() => Execute(choice));
        }
    }

    private static void Execute(int choice)
    {
        var input = ConsoleInput.Hex(choice is 2 or 4 ? "Ciphertext (16 hex): " : "Plaintext (16 hex): ", 16);
        var key1 = ConsoleInput.Hex("K1 (16 hex): ", 16); var key2 = ConsoleInput.Hex("K2 (16 hex): ", 16);
        MultiDesResult result;
        if (choice <= 2) result = choice == 1 ? TripleDesService.DoubleEncrypt(input, key1, key2) : TripleDesService.DoubleDecrypt(input, key1, key2);
        else
        {
            var key3Text = choice == 5 ? ConsoleInput.Hex("K3 (16 hex): ", 16) : ConsoleInput.Text("K3 (Enter = K1): ", true);
            var key3 = string.IsNullOrWhiteSpace(key3Text) ? key1 : CryptoExam.Core.Common.HexUtils.Normalize(key3Text, 16);
            result = choice switch { 3 => TripleDesService.EncryptEde(input, key1, key2, key3), 4 => TripleDesService.DecryptEde(input, key1, key2, key3), _ => TripleDesService.EncryptEee(input, key1, key2, key3) };
        }
        ConsoleOutput.Result("Step 1", result.Step1); ConsoleOutput.Result("Step 2", result.Step2);
        if (result.Step3 is not null) ConsoleOutput.Result("Step 3", result.Step3);
        ConsoleOutput.Result("Final", result.FinalCiphertext);
    }
}

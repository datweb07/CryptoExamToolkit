using CryptoExam.ConsoleApp.Helpers;
using CryptoExam.Core.Classical;

namespace CryptoExam.ConsoleApp.Menus;

public sealed class ClassicalMenu
{
    public void Run()
    {
        while (true)
        {
            ConsoleOutput.Header("CLASSICAL CRYPTOGRAPHY");
            Console.WriteLine("1. Caesar   2. Monoalphabetic   3. Vigenere   4. OTP");
            Console.WriteLine("5. Playfair 6. Rail Fence       7. Columnar  8. Double Transposition");
            Console.WriteLine("9. Matrix Transposition        0. Back");
            var choice = ConsoleInput.Integer("Chọn: ", 0, 9);
            if (choice == 0) return;
            ConsoleOutput.Guard(() => Execute(choice));
        }
    }

    private static void Execute(int choice)
    {
        switch (choice)
        {
            case 1: Caesar(); break;
            case 2: Mono(); break;
            case 3: Vigenere(); break;
            case 4: Otp(); break;
            case 5: Playfair(); break;
            case 6: RailFence(); break;
            case 7: Columnar(); break;
            case 8: Double(); break;
            case 9: Matrix(); break;
        }
    }

    private static void Caesar()
    {
        var mode = ConsoleInput.Integer("1.Encrypt  2.Decrypt  3.Brute Force: ", 1, 3);
        var text = ConsoleInput.Text("Text: ");
        var spaces = ConsoleInput.YesNo("Preserve spaces");
        if (mode == 3) foreach (var item in CaesarCipher.BruteForce(text, spaces)) Console.WriteLine($"k={item.Key,2}: {item.Value}");
        else
        {
            var key = ConsoleInput.Integer("k: ");
            ConsoleOutput.Result(mode == 1 ? "Ciphertext" : "Plaintext", mode == 1 ? CaesarCipher.Encrypt(text, key, spaces) : CaesarCipher.Decrypt(text, key, spaces));
        }
    }

    private static void Mono()
    {
        var cipher = new MonoalphabeticCipher(ConsoleInput.Text("Plain alphabet: "), ConsoleInput.Text("Cipher alphabet: "));
        var mode = ConsoleInput.Integer("1.Encrypt  2.Decrypt  3.Mapping: ", 1, 3);
        if (mode == 3) foreach (var (plain, encrypted) in cipher.Mapping()) Console.WriteLine($"{plain} -> {encrypted}");
        else ConsoleOutput.Result(mode == 1 ? "Ciphertext" : "Plaintext", mode == 1 ? cipher.Encrypt(ConsoleInput.Text("Text: ")) : cipher.Decrypt(ConsoleInput.Text("Text: ")));
    }

    private static void Vigenere()
    {
        var mode = ConsoleInput.Integer("1.Encrypt  2.Decrypt: ", 1, 2);
        var text = ConsoleInput.Text("Text: ");
        var key = ConsoleInput.Text("Key: ");
        if (ConsoleInput.YesNo("Show expanded key")) ConsoleOutput.Result("Expanded key", VigenereCipher.ExpandKey(text, key));
        ConsoleOutput.Result(mode == 1 ? "Ciphertext" : "Plaintext", mode == 1 ? VigenereCipher.Encrypt(text, key) : VigenereCipher.Decrypt(text, key));
    }

    private static void Otp()
    {
        var mode = ConsoleInput.Integer("1.Encrypt  2.Decrypt: ", 1, 2);
        var text = ConsoleInput.Text("Text: "); var key = ConsoleInput.Text("Key (same length): ");
        ConsoleOutput.Result(mode == 1 ? "Ciphertext" : "Plaintext", mode == 1 ? OneTimePadCipher.Encrypt(text, key) : OneTimePadCipher.Decrypt(text, key));
    }

    private static void Playfair()
    {
        var cipher = new PlayfairCipher(ConsoleInput.Text("Key: "));
        var mode = ConsoleInput.Integer("1.Matrix 2.Prepare 3.Encrypt 4.Decrypt 5.Encrypt pair 6.Decrypt pair: ", 1, 6);
        if (mode == 1) Console.WriteLine(cipher.MatrixText);
        else if (mode == 2) ConsoleOutput.Result("Prepared pairs", string.Join(' ', cipher.PreparePairs(ConsoleInput.Text("Plaintext: "))));
        else
        {
            var text = ConsoleInput.Text(mode is 3 or 5 ? "Plaintext/pair: " : "Ciphertext/pair: ");
            var output = mode switch { 3 => cipher.Encrypt(text), 4 => cipher.Decrypt(text), 5 => cipher.EncryptPair(text), _ => cipher.DecryptPair(text) };
            Console.WriteLine("Key matrix:\n" + cipher.MatrixText);
            if (mode == 3) ConsoleOutput.Result("Prepared pairs", string.Join(' ', cipher.PreparePairs(text)));
            ConsoleOutput.Result("Result", output);
        }
    }

    private static void RailFence()
    {
        var mode = ConsoleInput.Integer("1.Encrypt  2.Decrypt: ", 1, 2); var text = ConsoleInput.Text("Text: "); var rails = ConsoleInput.Integer("Rails: ", 2);
        ConsoleOutput.Result("Result", mode == 1 ? RailFenceCipher.Encrypt(text, rails) : RailFenceCipher.Decrypt(text, rails));
    }

    private static void Columnar()
    {
        var mode = ConsoleInput.Integer("1.Encrypt  2.Decrypt: ", 1, 2); var text = ConsoleInput.Text("Text: "); var cipher = new ColumnarTranspositionCipher(ConsoleInput.Text("Keyword: "));
        ConsoleOutput.Result("Column order", string.Join(' ', cipher.ColumnOrder.Select(x => x + 1)));
        if (mode == 1) Console.WriteLine("Matrix:\n" + cipher.Matrix(text));
        ConsoleOutput.Result("Result", mode == 1 ? cipher.Encrypt(text) : cipher.Decrypt(text));
    }

    private static void Double()
    {
        var mode = ConsoleInput.Integer("1.Encrypt  2.Decrypt: ", 1, 2); var text = ConsoleInput.Text("Text: "); var key1 = ConsoleInput.Text("Key 1: "); var key2 = ConsoleInput.Text("Key 2: ");
        ConsoleOutput.Result("Result", mode == 1 ? DoubleTranspositionCipher.Encrypt(text, key1, key2) : DoubleTranspositionCipher.Decrypt(text, key1, key2));
    }

    private static void Matrix()
    {
        var mode = ConsoleInput.Integer("1.Encrypt  2.Decrypt: ", 1, 2); var text = ConsoleInput.Text("Text: "); var columns = ConsoleInput.Integer("Columns: ", 2);
        var result = mode == 1 ? MatrixTranspositionCipher.Encrypt(text, columns) : MatrixTranspositionCipher.Decrypt(text, columns);
        Console.WriteLine("Matrix:\n" + MatrixTranspositionCipher.Matrix(mode == 1 ? text : result, columns));
        ConsoleOutput.Result("Result", result);
    }
}

using CryptoExam.ConsoleApp.Helpers;
using CryptoExam.Core.Classical;

namespace CryptoExam.ConsoleApp.Menus;

public sealed class ClassicalMenu
{
    public void Run()
    {
        while (true)
        {
            ConsoleOutput.Header("MẬT MÃ CỔ ĐIỂN");
            Console.WriteLine("1. Caesar              2. Thay thế đơn bảng\n3. Vigenère            4. One-Time Pad (OTP)");
            Console.WriteLine("5. Playfair            6. Rail Fence\n7. Hoán vị cột         8. Hoán vị kép");
            Console.WriteLine("9. Hoán vị ma trận     0. Quay lại");
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
        var mode = ConsoleInput.Integer("1. Mã hóa  2. Giải mã  3. Dò toàn bộ khóa: ", 1, 3);
        var text = ConsoleInput.Text("Bản văn: ");
        var spaces = ConsoleInput.YesNo("Giữ nguyên khoảng trắng");
        if (mode == 3) foreach (var item in CaesarCipher.BruteForce(text, spaces)) Console.WriteLine($"k={item.Key,2}: {item.Value}");
        else
        {
            var key = ConsoleInput.Integer("Khóa k: ");
            ConsoleOutput.Result(mode == 1 ? "Bản mã" : "Bản rõ", mode == 1 ? CaesarCipher.Encrypt(text, key, spaces) : CaesarCipher.Decrypt(text, key, spaces));
        }
    }

    private static void Mono()
    {
        var cipher = new MonoalphabeticCipher(ConsoleInput.Text("Bảng chữ cái bản rõ: "), ConsoleInput.Text("Bảng chữ cái bản mã: "));
        var mode = ConsoleInput.Integer("1. Mã hóa  2. Giải mã  3. Xem ánh xạ: ", 1, 3);
        if (mode == 3) foreach (var (plain, encrypted) in cipher.Mapping()) Console.WriteLine($"{plain} -> {encrypted}");
        else ConsoleOutput.Result(mode == 1 ? "Bản mã" : "Bản rõ", mode == 1 ? cipher.Encrypt(ConsoleInput.Text("Bản văn: ")) : cipher.Decrypt(ConsoleInput.Text("Bản văn: ")));
    }

    private static void Vigenere()
    {
        var mode = ConsoleInput.Integer("1. Mã hóa  2. Giải mã: ", 1, 2);
        var text = ConsoleInput.Text("Bản văn: ");
        var key = ConsoleInput.Text("Khóa: ");
        if (ConsoleInput.YesNo("Hiển thị khóa sau khi mở rộng")) ConsoleOutput.Result("Khóa sau khi mở rộng", VigenereCipher.ExpandKey(text, key));
        ConsoleOutput.Result(mode == 1 ? "Bản mã" : "Bản rõ", mode == 1 ? VigenereCipher.Encrypt(text, key) : VigenereCipher.Decrypt(text, key));
    }

    private static void Otp()
    {
        var mode = ConsoleInput.Integer("1. Mã hóa  2. Giải mã: ", 1, 2);
        var text = ConsoleInput.Text("Bản văn: "); var key = ConsoleInput.Text("Khóa (phải có cùng độ dài): ");
        ConsoleOutput.Result(mode == 1 ? "Bản mã" : "Bản rõ", mode == 1 ? OneTimePadCipher.Encrypt(text, key) : OneTimePadCipher.Decrypt(text, key));
    }

    private static void Playfair()
    {
        var cipher = new PlayfairCipher(ConsoleInput.Text("Khóa: "));
        var mode = ConsoleInput.Integer("1. Ma trận 2. Chuẩn bị cặp ký tự 3. Mã hóa 4. Giải mã 5. Mã hóa một cặp 6. Giải mã một cặp: ", 1, 6);
        if (mode == 1) Console.WriteLine(cipher.MatrixText);
        else if (mode == 2) ConsoleOutput.Result("Các cặp sau khi chuẩn bị", string.Join(' ', cipher.PreparePairs(ConsoleInput.Text("Bản rõ: "))));
        else
        {
            var text = ConsoleInput.Text(mode is 3 or 5 ? "Bản rõ/cặp ký tự: " : "Bản mã/cặp ký tự: ");
            var output = mode switch { 3 => cipher.Encrypt(text), 4 => cipher.Decrypt(text), 5 => cipher.EncryptPair(text), _ => cipher.DecryptPair(text) };
            Console.WriteLine("Ma trận khóa:\n" + cipher.MatrixText);
            if (mode == 3) ConsoleOutput.Result("Các cặp sau khi chuẩn bị", string.Join(' ', cipher.PreparePairs(text)));
            ConsoleOutput.Result("Kết quả", output);
        }
    }

    private static void RailFence()
    {
        var mode = ConsoleInput.Integer("1. Mã hóa  2. Giải mã: ", 1, 2); var text = ConsoleInput.Text("Bản văn: "); var rails = ConsoleInput.Integer("Số hàng: ", 2);
        ConsoleOutput.Result("Kết quả", mode == 1 ? RailFenceCipher.Encrypt(text, rails) : RailFenceCipher.Decrypt(text, rails));
    }

    private static void Columnar()
    {
        var mode = ConsoleInput.Integer("1. Mã hóa  2. Giải mã: ", 1, 2); var text = ConsoleInput.Text("Bản văn: "); var cipher = new ColumnarTranspositionCipher(ConsoleInput.Text("Từ khóa: "));
        ConsoleOutput.Result("Thứ tự cột", string.Join(' ', cipher.ColumnOrder.Select(x => x + 1)));
        if (mode == 1) Console.WriteLine("Ma trận:\n" + cipher.Matrix(text));
        ConsoleOutput.Result("Kết quả", mode == 1 ? cipher.Encrypt(text) : cipher.Decrypt(text));
    }

    private static void Double()
    {
        var mode = ConsoleInput.Integer("1. Mã hóa  2. Giải mã: ", 1, 2); var text = ConsoleInput.Text("Bản văn: "); var key1 = ConsoleInput.Text("Khóa 1: "); var key2 = ConsoleInput.Text("Khóa 2: ");
        ConsoleOutput.Result("Kết quả", mode == 1 ? DoubleTranspositionCipher.Encrypt(text, key1, key2) : DoubleTranspositionCipher.Decrypt(text, key1, key2));
    }

    private static void Matrix()
    {
        var mode = ConsoleInput.Integer("1. Mã hóa  2. Giải mã: ", 1, 2); var text = ConsoleInput.Text("Bản văn: "); var columns = ConsoleInput.Integer("Số cột: ", 2);
        var result = mode == 1 ? MatrixTranspositionCipher.Encrypt(text, columns) : MatrixTranspositionCipher.Decrypt(text, columns);
        Console.WriteLine("Ma trận:\n" + MatrixTranspositionCipher.Matrix(mode == 1 ? text : result, columns));
        ConsoleOutput.Result("Kết quả", result);
    }
}

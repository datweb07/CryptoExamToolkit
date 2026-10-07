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
            Console.WriteLine("1. Mã hóa nhanh (AES-128/192/256, ECB, một khối)");
            Console.WriteLine("2. Giải mã nhanh (AES-128/192/256, ECB, một khối)");
            Console.WriteLine("3. Dấu vết AES-128 học thuật - Hiển thị toàn bộ");
            Console.WriteLine("4. Tra cứu nhanh AES-128 học thuật");
            Console.WriteLine("0. Quay lại");
            var choice = ConsoleInput.Integer("Chọn: ", 0, 4);
            if (choice == 0) return;
            ConsoleOutput.Guard(() => Execute(choice));
        }
    }

    private static void Execute(int choice)
    {
        var block = ConsoleInput.Hex(choice == 2 ? "Bản mã (32 ký tự hex): " : "Bản rõ (32 ký tự hex): ", 32);
        var key = ConsoleInput.Hex(choice <= 2 ? "Khóa (32/48/64 ký tự hex): " : "Khóa AES-128 (32 ký tự hex): ");
        if (choice == 1) ConsoleOutput.Result("Bản mã", AesService.QuickEncrypt(block, key));
        else if (choice == 2) ConsoleOutput.Result("Bản rõ", AesService.QuickDecrypt(block, key));
        else
        {
            var trace = AesService.TraceEncrypt128(block, key);
            if (choice == 3)
            {
                foreach (var round in trace.Rounds)
                {
                    Console.WriteLine($"\nROUND {round.Round}");
                    ConsoleOutput.Result("Khóa vòng", round.RoundKeyHex);
                    if (round.SubBytesHex is not null) ConsoleOutput.Result("Sau SubBytes", round.SubBytesHex);
                    if (round.ShiftRowsHex is not null) ConsoleOutput.Result("Sau ShiftRows", round.ShiftRowsHex);
                    if (round.MixColumnsHex is not null) ConsoleOutput.Result("Sau MixColumns", round.MixColumnsHex);
                    ConsoleOutput.Result("Sau AddRoundKey", round.AddRoundKeyHex);
                }
                ConsoleOutput.Result("Bản mã", trace.CiphertextHex);
            }
            else
            {
                Console.WriteLine("1. Khóa vòng 2. Sau SubBytes 3. Sau ShiftRows 4. Sau MixColumns 5. Sau AddRoundKey 6. Bản mã cuối");
                var query = ConsoleInput.Integer("Cần lấy: ", 1, 6);
                if (query == 6) { ConsoleOutput.Result("Bản mã", trace.CiphertextHex); return; }
                var minRound = query is 1 or 5 ? 0 : 1;
                var roundNumber = ConsoleInput.Integer("Vòng i = ", minRound, 10);
                var round = trace.Rounds[roundNumber];
                var value = query switch { 1 => round.RoundKeyHex, 2 => round.SubBytesHex, 3 => round.ShiftRowsHex, 4 => round.MixColumnsHex, _ => round.AddRoundKeyHex };
                ConsoleOutput.Result($"Kết quả vòng {roundNumber}", value ?? "Không áp dụng (vòng 10 không có MixColumns)");
            }
        }
    }
}

using CryptoExam.ConsoleApp.Helpers;

namespace CryptoExam.ConsoleApp.Menus;

public sealed class MainMenu
{
    public void Run()
    {
        while (true)
        {
            ConsoleOutput.Header("BỘ CÔNG CỤ ÔN THI MẬT MÃ (.NET 8)");
            Console.WriteLine("1. Mật mã cổ điển");
            Console.WriteLine("2. DES (Học thuật + Tra cứu nhanh)");
            Console.WriteLine("3. AES");
            Console.WriteLine("4. RSA / Chữ ký số");
            Console.WriteLine("5. Lý thuyết số / Euclid / Modulo");
            Console.WriteLine("6. Double DES / Triple DES");
            Console.WriteLine("7. Chạy tự kiểm tra");
            Console.WriteLine("8. Tiện ích thi / Chuyển đổi và máy tính");
            Console.WriteLine("0. Thoát");
            switch (ConsoleInput.Integer("Chọn: ", 0, 8))
            {
                case 1: new ClassicalMenu().Run(); break;
                case 2: new DesMenu().Run(); break;
                case 3: new AesMenu().Run(); break;
                case 4: new RsaMenu().Run(); break;
                case 5: new NumberTheoryMenu().Run(); break;
                case 6: new TripleDesMenu().Run(); break;
                case 7: ConsoleOutput.Guard(SelfTestDisplay.Run); break;
                case 8: new ExamUtilitiesMenu().Run(); break;
                case 0: return;
            }
        }
    }
}

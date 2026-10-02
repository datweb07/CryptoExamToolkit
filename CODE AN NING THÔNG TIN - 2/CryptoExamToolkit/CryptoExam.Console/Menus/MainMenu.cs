// File: CryptoExam.Console/Menus/MainMenu.cs
using System;
using CryptoExam.Core;
using CryptoExam.Console.Helpers;

namespace CryptoExam.Console.Menus
{
    public static class MainMenu
    {
        public static void Show()
        {
            while (true)
            {
                try { System.Console.Clear(); } catch { /* Bỏ qua nếu console không hỗ trợ Clear */ }
                System.Console.ForegroundColor = ConsoleColor.Cyan;
                System.Console.WriteLine(@"
  ██████╗██████╗ ██╗   ██╗██████╗ ████████╗ ██████╗ 
 ██╔════╝██╔══██╗╚██╗ ██╔╝██╔══██╗╚══██╔══╝██╔═══██╗
 ██║     ██████╔╝ ╚████╔╝ ██████╔╝   ██║   ██║   ██║
 ██║     ██╔══██╗  ╚██╔╝  ██╔═══╝    ██║   ██║   ██║
 ╚██████╗██║  ██║   ██║   ██║        ██║   ╚██████╔╝
  ╚═════╝╚═╝  ╚═╝   ╚═╝   ╚═╝        ╚═╝    ╚═════╝ ");
                System.Console.ResetColor();

                System.Console.ForegroundColor = ConsoleColor.Yellow;
                System.Console.WriteLine("  ╔══════════════════════════════════════════════════╗");
                System.Console.WriteLine("  ║         CRYPTO EXAM TOOLKIT v1.0                ║");
                System.Console.WriteLine("  ║     An Toàn Thông Tin - Exam Helper              ║");
                System.Console.WriteLine("  ╚══════════════════════════════════════════════════╝");
                System.Console.ResetColor();
                System.Console.WriteLine();
                System.Console.WriteLine("  1. Mã hóa Cổ điển (Caesar, Vigenere, Playfair...)");
                System.Console.WriteLine("  2. DES  ← QUAN TRỌNG (có Quick Query)");
                System.Console.WriteLine("  3. AES");
                System.Console.WriteLine("  4. RSA / Chữ ký số (UEH + Standard mode)");
                System.Console.WriteLine("  5. Số học (GCD, Euclid, Modular Exp)");
                System.Console.WriteLine("  6. Triple DES / Double DES");
                System.Console.WriteLine("  7. Self Test (kiểm tra tất cả các thuật toán)");
                System.Console.WriteLine("  0. Thoát");
                System.Console.WriteLine();

                int choice = ConsoleInput.ReadMenuChoice("  Chọn: ", 7);

                switch (choice)
                {
                    case 0:
                        System.Console.WriteLine("\n  Tạm biệt! Chúc thi tốt!");
                        return;
                    case 1: ClassicalMenu.Show(); break;
                    case 2: DesMenu.Show(); break;
                    case 3: AesMenu.Show(); break;
                    case 4: RsaMenu.Show(); break;
                    case 5: NumberTheoryMenu.Show(); break;
                    case 6: TripleDesMenu.Show(); break;
                    case 7:
                        System.Console.Clear();
                        SelfTestRunner.RunAllTests();
                        ConsoleInput.PressEnterToContinue();
                        break;
                }
            }
        }
    }
}

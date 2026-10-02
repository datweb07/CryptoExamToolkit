// File: CryptoExam.Console/Program.cs
using CryptoExam.Console.Menus;
using CryptoExam.Core;

// Bật UTF-8 để hiển thị ký tự đặc biệt
System.Console.OutputEncoding = System.Text.Encoding.UTF8;
System.Console.InputEncoding = System.Text.Encoding.UTF8;

// Chạy self-test nếu có argument --selftest
if (args.Length > 0 && args[0].ToLower() == "--selftest")
{
    SelfTestRunner.RunAllTests();
    return;
}

// Khởi chạy main menu
MainMenu.Show();

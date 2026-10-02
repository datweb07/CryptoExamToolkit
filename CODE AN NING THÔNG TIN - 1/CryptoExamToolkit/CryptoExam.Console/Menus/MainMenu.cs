using System;
using CryptoExam.Core;
using CryptoExam.ConsoleApp.Helpers;

namespace CryptoExam.ConsoleApp.Menus
{
    public static class MainMenu
    {
        public static void Show()
        {
            while (true)
            {
                Console.Clear();
                ConsoleOutput.PrintHeader("CRYPTO EXAM TOOLKIT");
                Console.WriteLine("1. Classical Cryptography");
                Console.WriteLine("2. DES & Triple DES");
                Console.WriteLine("3. AES (Quick Mode)");
                Console.WriteLine("4. RSA / Digital Signature");
                Console.WriteLine("5. Number Theory / Euclid / Modulo");
                Console.WriteLine("6. Run Self Tests");
                Console.WriteLine("0. Exit");
                Console.WriteLine(new string('=', 40));

                int choice = ConsoleInput.ReadInt("Select an option");

                switch (choice)
                {
                    case 1: ClassicalMenu.Show(); break;
                    case 2: DesMenu.Show(); break;
                    case 3: AesMenu.Show(); break;
                    case 4: RsaMenu.Show(); break;
                    case 5: NumberTheoryMenu.Show(); break;
                    case 6: 
                        SelfTestRunner.RunAllTests();
                        ConsoleOutput.WaitForKey();
                        break;
                    case 0: return;
                }
            }
        }
    }
}

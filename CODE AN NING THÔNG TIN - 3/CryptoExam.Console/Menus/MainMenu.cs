using CryptoExam.ConsoleApp.Helpers;

namespace CryptoExam.ConsoleApp.Menus;

public sealed class MainMenu
{
    public void Run()
    {
        while (true)
        {
            ConsoleOutput.Header("CRYPTO EXAM TOOLKIT (.NET 8)");
            Console.WriteLine("1. Classical Cryptography");
            Console.WriteLine("2. DES (Educational + Quick Query)");
            Console.WriteLine("3. AES");
            Console.WriteLine("4. RSA / Digital Signature");
            Console.WriteLine("5. Number Theory / Euclid / Modulo");
            Console.WriteLine("6. Double DES / Triple DES");
            Console.WriteLine("7. Run Self-Test");
            Console.WriteLine("0. Exit");
            switch (ConsoleInput.Integer("Chọn: ", 0, 7))
            {
                case 1: new ClassicalMenu().Run(); break;
                case 2: new DesMenu().Run(); break;
                case 3: new AesMenu().Run(); break;
                case 4: new RsaMenu().Run(); break;
                case 5: new NumberTheoryMenu().Run(); break;
                case 6: new TripleDesMenu().Run(); break;
                case 7: ConsoleOutput.Guard(SelfTestDisplay.Run); break;
                case 0: return;
            }
        }
    }
}

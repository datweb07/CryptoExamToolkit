using System;
using CryptoExam.Core.AES;
using CryptoExam.ConsoleApp.Helpers;

namespace CryptoExam.ConsoleApp.Menus
{
    public static class AesMenu
    {
        public static void Show()
        {
            while (true)
            {
                ConsoleOutput.PrintHeader("AES (Quick Mode)");
                Console.WriteLine("1. Encrypt (ECB, NoPadding, 128-bit block)");
                Console.WriteLine("2. Decrypt (ECB, NoPadding, 128-bit block)");
                Console.WriteLine("0. Back");

                int choice = ConsoleInput.ReadInt("Choice");
                if (choice == 0) return;

                if (choice == 1 || choice == 2)
                {
                    string key = ConsoleInput.ReadString("Key (32 hex chars for AES-128)");
                    string text = ConsoleInput.ReadString("Text (32 hex chars)");
                    
                    try
                    {
                        string res = choice == 1 ? AesService.EncryptQuick(text, key) : AesService.DecryptQuick(text, key);
                        ConsoleOutput.PrintLabel("Result", res);
                    }
                    catch (Exception ex)
                    {
                        ConsoleOutput.PrintError(ex.Message);
                    }
                    ConsoleOutput.WaitForKey();
                }
            }
        }
    }
}

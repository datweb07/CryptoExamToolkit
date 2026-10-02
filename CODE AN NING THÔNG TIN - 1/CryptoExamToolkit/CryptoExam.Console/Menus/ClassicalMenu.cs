using System;
using CryptoExam.Core.Classical;
using CryptoExam.ConsoleApp.Helpers;

namespace CryptoExam.ConsoleApp.Menus
{
    public static class ClassicalMenu
    {
        public static void Show()
        {
            while (true)
            {
                ConsoleOutput.PrintHeader("CLASSICAL CRYPTOGRAPHY");
                Console.WriteLine("1. Caesar Cipher");
                Console.WriteLine("2. Monoalphabetic Substitution");
                Console.WriteLine("3. Vigenere Cipher");
                Console.WriteLine("4. One-Time Pad");
                Console.WriteLine("5. Playfair Cipher");
                Console.WriteLine("6. Rail Fence / Transposition");
                Console.WriteLine("0. Back");

                int choice = ConsoleInput.ReadInt("Choice");
                if (choice == 0) return;

                switch (choice)
                {
                    case 1: CaesarMenu(); break;
                    case 2: MonoMenu(); break;
                    case 3: VigenereMenu(); break;
                    case 4: OtpMenu(); break;
                    case 5: PlayfairMenu(); break;
                    case 6: TranspositionMenu(); break;
                }
            }
        }

        private static void CaesarMenu()
        {
            ConsoleOutput.PrintHeader("CAESAR CIPHER");
            Console.WriteLine("1. Encrypt");
            Console.WriteLine("2. Decrypt");
            Console.WriteLine("3. Brute Force All Keys");
            int c = ConsoleInput.ReadInt("Choice");
            
            if (c == 1 || c == 2)
            {
                string text = ConsoleInput.ReadString("Text");
                int key = ConsoleInput.ReadInt("Key (k)");
                bool keepSpaces = ConsoleInput.ReadString("Preserve spaces? (y/n)").ToLower() == "y";
                
                string res = c == 1 ? CaesarCipher.Encrypt(text, key, keepSpaces) : CaesarCipher.Decrypt(text, key, keepSpaces);
                ConsoleOutput.PrintLabel("Result", res);
            }
            else if (c == 3)
            {
                string text = ConsoleInput.ReadString("Cipher Text");
                for (int k = 1; k < 26; k++)
                {
                    Console.WriteLine($"k={k,-2}: {CaesarCipher.Decrypt(text, k, true)}");
                }
            }
            ConsoleOutput.WaitForKey();
        }

        private static void MonoMenu()
        {
            ConsoleOutput.PrintHeader("MONOALPHABETIC CIPHER");
            Console.WriteLine("1. Encrypt");
            Console.WriteLine("2. Decrypt");
            int c = ConsoleInput.ReadInt("Choice");
            if (c != 1 && c != 2) return;

            string plainAlpha = ConsoleInput.ReadString("Plain Alphabet (default A-Z)");
            if (string.IsNullOrEmpty(plainAlpha)) plainAlpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            
            string cipherAlpha = ConsoleInput.ReadString("Cipher Alphabet");
            string text = ConsoleInput.ReadString("Text");

            try
            {
                string res = c == 1 ? MonoalphabeticCipher.Encrypt(text, plainAlpha, cipherAlpha) : MonoalphabeticCipher.Decrypt(text, plainAlpha, cipherAlpha);
                ConsoleOutput.PrintLabel("Result", res);
            }
            catch (Exception ex)
            {
                ConsoleOutput.PrintError(ex.Message);
            }
            ConsoleOutput.WaitForKey();
        }

        private static void VigenereMenu()
        {
            ConsoleOutput.PrintHeader("VIGENERE CIPHER");
            Console.WriteLine("1. Encrypt");
            Console.WriteLine("2. Decrypt");
            int c = ConsoleInput.ReadInt("Choice");
            if (c != 1 && c != 2) return;
            
            string text = ConsoleInput.ReadString("Text");
            string key = ConsoleInput.ReadString("Key");
            
            string res = c == 1 ? VigenereCipher.Encrypt(text, key) : VigenereCipher.Decrypt(text, key);
            ConsoleOutput.PrintLabel("Result", res);
            ConsoleOutput.WaitForKey();
        }
        
        private static void OtpMenu()
        {
            ConsoleOutput.PrintHeader("ONE TIME PAD");
            Console.WriteLine("1. Encrypt");
            Console.WriteLine("2. Decrypt");
            int c = ConsoleInput.ReadInt("Choice");
            if (c != 1 && c != 2) return;
            
            string text = ConsoleInput.ReadString("Text");
            string key = ConsoleInput.ReadString("Key (Must be same length)");
            
            try
            {
                string res = c == 1 ? OneTimePadCipher.Encrypt(text, key) : OneTimePadCipher.Decrypt(text, key);
                ConsoleOutput.PrintLabel("Result", res);
            }
            catch (Exception ex)
            {
                ConsoleOutput.PrintError(ex.Message);
            }
            ConsoleOutput.WaitForKey();
        }

        private static void PlayfairMenu()
        {
            ConsoleOutput.PrintHeader("PLAYFAIR CIPHER");
            string key = ConsoleInput.ReadString("Key");
            var pf = new PlayfairCipher(key);
            
            Console.WriteLine("\nKey Matrix:");
            Console.WriteLine(pf.GetMatrixString());
            
            Console.WriteLine("\n1. Encrypt Text");
            Console.WriteLine("2. Decrypt Text");
            Console.WriteLine("3. Encrypt Single Pair");
            Console.WriteLine("4. Decrypt Single Pair");
            
            int c = ConsoleInput.ReadInt("Choice");
            
            if (c == 1 || c == 2)
            {
                string text = ConsoleInput.ReadString("Text");
                if (c == 1)
                {
                    Console.WriteLine("Prepared pairs: " + string.Join(" ", pf.PreparePairs(text)));
                }
                string res = c == 1 ? pf.Encrypt(text) : pf.Decrypt(text);
                ConsoleOutput.PrintLabel("Result", res);
            }
            else if (c == 3 || c == 4)
            {
                string pair = ConsoleInput.ReadString("Pair (2 chars)").ToUpperInvariant();
                if (pair.Length != 2)
                {
                    ConsoleOutput.PrintError("Must be exactly 2 chars");
                    return;
                }
                string res = c == 3 ? pf.EncryptPair(pair) : pf.DecryptPair(pair);
                ConsoleOutput.PrintLabel("Result", res);
            }
            ConsoleOutput.WaitForKey();
        }

        private static void TranspositionMenu()
        {
            ConsoleOutput.PrintHeader("TRANSPOSITION CIPHER");
            Console.WriteLine("1. Rail Fence (ZigZag)");
            Console.WriteLine("2. Columnar Transposition (Keyword)");
            Console.WriteLine("3. Double Transposition");
            int c = ConsoleInput.ReadInt("Choice");
            
            if (c == 1)
            {
                int mode = ConsoleInput.ReadInt("1: Encrypt, 2: Decrypt");
                string text = ConsoleInput.ReadString("Text");
                int rails = ConsoleInput.ReadInt("Rails");
                string res = mode == 1 ? RailFenceCipher.EncryptZigZag(text, rails) : RailFenceCipher.DecryptZigZag(text, rails);
                ConsoleOutput.PrintLabel("Result", res);
            }
            else if (c == 2)
            {
                int mode = ConsoleInput.ReadInt("1: Encrypt, 2: Decrypt");
                string text = ConsoleInput.ReadString("Text");
                string keyword = ConsoleInput.ReadString("Keyword");
                string res = mode == 1 ? ColumnarTranspositionCipher.Encrypt(text, keyword) : ColumnarTranspositionCipher.Decrypt(text, keyword);
                ConsoleOutput.PrintLabel("Result", res);
            }
            else if (c == 3)
            {
                int mode = ConsoleInput.ReadInt("1: Encrypt, 2: Decrypt");
                string text = ConsoleInput.ReadString("Text");
                string k1 = ConsoleInput.ReadString("Key 1");
                string k2 = ConsoleInput.ReadString("Key 2");
                string res = mode == 1 ? DoubleTranspositionCipher.Encrypt(text, k1, k2) : DoubleTranspositionCipher.Decrypt(text, k1, k2);
                ConsoleOutput.PrintLabel("Result", res);
            }
            ConsoleOutput.WaitForKey();
        }
    }
}

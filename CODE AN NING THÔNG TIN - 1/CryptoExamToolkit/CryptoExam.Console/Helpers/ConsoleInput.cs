using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;

namespace CryptoExam.ConsoleApp.Helpers
{
    public static class ConsoleInput
    {
        public static string ReadString(string prompt)
        {
            Console.Write($"{prompt}: ");
            return Console.ReadLine()?.Trim() ?? "";
        }

        public static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");
                string input = Console.ReadLine()?.Trim();
                if (int.TryParse(input, out int result))
                    return result;
                Console.WriteLine("Invalid input. Please enter a valid integer.");
            }
        }

        public static BigInteger ReadBigInteger(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");
                string input = Console.ReadLine()?.Trim();
                if (BigInteger.TryParse(input, out BigInteger result))
                    return result;
                Console.WriteLine("Invalid input. Please enter a valid integer.");
            }
        }

        public static List<BigInteger> ReadBigIntegerList(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt} (comma or space separated): ");
                string input = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(input)) return new List<BigInteger>();
                
                try
                {
                    string[] parts = input.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    return parts.Select(BigInteger.Parse).ToList();
                }
                catch
                {
                    Console.WriteLine("Invalid input. Please enter valid integers.");
                }
            }
        }
    }
}

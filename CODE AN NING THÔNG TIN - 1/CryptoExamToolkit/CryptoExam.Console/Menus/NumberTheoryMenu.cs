using System;
using System.Numerics;
using CryptoExam.Core.NumberTheory;
using CryptoExam.ConsoleApp.Helpers;

namespace CryptoExam.ConsoleApp.Menus
{
    public static class NumberTheoryMenu
    {
        public static void Show()
        {
            while (true)
            {
                ConsoleOutput.PrintHeader("NUMBER THEORY / MODULO");
                Console.WriteLine("1. GCD (Greatest Common Divisor)");
                Console.WriteLine("2. Extended Euclid / Modular Inverse");
                Console.WriteLine("3. Modular Exponentiation (a^b mod m)");
                Console.WriteLine("0. Back");

                int choice = ConsoleInput.ReadInt("Choice");
                if (choice == 0) return;

                switch (choice)
                {
                    case 1: GcdMenu(); break;
                    case 2: ExtendedEuclidMenu(); break;
                    case 3: ModPowMenu(); break;
                }
            }
        }

        private static void GcdMenu()
        {
            BigInteger a = ConsoleInput.ReadBigInteger("a");
            BigInteger b = ConsoleInput.ReadBigInteger("b");
            ConsoleOutput.PrintLabel("GCD", GcdService.Calculate(a, b));
            ConsoleOutput.WaitForKey();
        }

        private static void ExtendedEuclidMenu()
        {
            BigInteger a = ConsoleInput.ReadBigInteger("a (value to invert)");
            BigInteger m = ConsoleInput.ReadBigInteger("m (modulus)");
            
            var result = ExtendedEuclidService.CalculateWithSteps(a, m);
            
            Console.WriteLine("\nSteps (Euclidean Algorithm):");
            Console.WriteLine($"{"A",-10} | {"B",-10} | {"Q",-10} | {"R",-10} | {"X",-10} | {"Y",-10}");
            Console.WriteLine(new string('-', 70));
            foreach (var s in result.steps)
            {
                Console.WriteLine($"{s.A,-10} | {s.B,-10} | {s.Q,-10} | {s.R,-10} | {s.X,-10} | {s.Y,-10}");
            }
            
            Console.WriteLine();
            ConsoleOutput.PrintLabel("GCD", result.gcd);
            ConsoleOutput.PrintLabel("Bezout x", result.x);
            ConsoleOutput.PrintLabel("Bezout y", result.y);
            
            if (result.gcd == 1)
            {
                BigInteger inv = result.x % m;
                if (inv < 0) inv += m;
                Console.WriteLine($"\nMODULAR INVERSE: {a}^-1 mod {m} = {inv}");
            }
            else
            {
                Console.WriteLine($"\nNO MODULAR INVERSE because gcd({a}, {m}) = {result.gcd} != 1");
            }
            ConsoleOutput.WaitForKey();
        }

        private static void ModPowMenu()
        {
            BigInteger b = ConsoleInput.ReadBigInteger("Base");
            BigInteger e = ConsoleInput.ReadBigInteger("Exponent");
            BigInteger m = ConsoleInput.ReadBigInteger("Modulus");
            
            BigInteger res = ModularArithmetic.ModPow(b, e, m);
            ConsoleOutput.PrintLabel($"{b}^{e} mod {m}", res);
            ConsoleOutput.WaitForKey();
        }
    }
}

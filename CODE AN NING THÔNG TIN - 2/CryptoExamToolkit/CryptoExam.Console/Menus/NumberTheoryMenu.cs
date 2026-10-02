// File: CryptoExam.Console/Menus/NumberTheoryMenu.cs
using System;
using System.Numerics;
using CryptoExam.Core.NumberTheory;
using CryptoExam.Console.Helpers;

namespace CryptoExam.Console.Menus
{
    public static class NumberTheoryMenu
    {
        public static void Show()
        {
            while (true)
            {
                ConsoleOutput.PrintHeader("SỐ HỌC / EUCLID / MODULO");
                System.Console.WriteLine("  1. GCD(a, b)");
                System.Console.WriteLine("  2. Extended Euclid + Modular Inverse");
                System.Console.WriteLine("  3. Modular Exponentiation (base^exp mod m)");
                System.Console.WriteLine("  4. Kiểm tra số nguyên tố");
                System.Console.WriteLine("  0. Quay lại");

                int ch = ConsoleInput.ReadMenuChoice("\n  Chọn: ", 4);
                if (ch == 0) break;

                switch (ch)
                {
                    case 1: GcdMenu(); break;
                    case 2: EuclidMenu(); break;
                    case 3: ModPowMenu(); break;
                    case 4: PrimeMenu(); break;
                }
            }
        }

        static void GcdMenu()
        {
            ConsoleOutput.PrintHeader("GCD");
            BigInteger a = ConsoleInput.ReadBigInt("  a = ");
            BigInteger b = ConsoleInput.ReadBigInt("  b = ");
            BigInteger g = GcdService.Gcd(a, b);
            System.Console.WriteLine();
            ConsoleOutput.PrintKV($"GCD({a},{b})", g.ToString());
            ConsoleInput.PressEnterToContinue();
        }

        static void EuclidMenu()
        {
            ConsoleOutput.PrintHeader("EXTENDED EUCLID + MODULAR INVERSE");
            BigInteger a = ConsoleInput.ReadBigInt("  a = ");
            BigInteger m = ConsoleInput.ReadBigInt("  m = ");

            var result = ExtendedEuclidService.Calculate(a, m);

            System.Console.WriteLine();
            ConsoleOutput.PrintKV("gcd(a,m)", result.Gcd.ToString());
            ConsoleOutput.PrintKV("x (a*x + m*y = gcd)", result.X.ToString());
            ConsoleOutput.PrintKV("y", result.Y.ToString());
            System.Console.WriteLine($"  Kiểm tra: {a}*({result.X}) + {m}*({result.Y}) = {a * result.X + m * result.Y}");

            if (result.ModularInverse.HasValue)
            {
                System.Console.WriteLine();
                ConsoleOutput.PrintKV($"a^-1 mod m = {a}^-1 mod {m}", result.ModularInverse.Value.ToString());
            }
            else
            {
                System.Console.WriteLine();
                ConsoleOutput.PrintError($"KHÔNG TỒN TẠI NGHỊCH ĐẢO vì gcd({a},{m}) = {result.Gcd} ≠ 1");
            }

            bool showSteps = ConsoleInput.ReadYesNo("\n  Hiển thị từng bước Euclid?");
            if (showSteps)
            {
                System.Console.WriteLine();
                System.Console.WriteLine($"  {"a",-12} {"b",-12} {"q = a div b",-14} {"r = a mod b",-14}");
                System.Console.WriteLine($"  {new string('-', 55)}");
                foreach (var step in result.Steps)
                    System.Console.WriteLine($"  {step.A,-12} {step.B,-12} {step.Quotient,-14} {step.Remainder,-14}");
                System.Console.WriteLine();

                // In dạng phương trình
                System.Console.WriteLine("  Các bước phân tích:");
                foreach (var step in result.Steps)
                    System.Console.WriteLine($"  {step.A} = {step.Quotient}*{step.B} + {step.Remainder}");
            }

            ConsoleInput.PressEnterToContinue();
        }

        static void ModPowMenu()
        {
            ConsoleOutput.PrintHeader("MODULAR EXPONENTIATION");
            BigInteger baseVal = ConsoleInput.ReadBigInt("  base = ");
            BigInteger exp = ConsoleInput.ReadBigInt("  exponent = ");
            BigInteger mod = ConsoleInput.ReadBigInt("  modulus = ");

            BigInteger result = ModularArithmetic.ModPow(baseVal, exp, mod);
            System.Console.WriteLine();
            ConsoleOutput.PrintKV($"{baseVal}^{exp} mod {mod}", result.ToString());

            bool showSteps = ConsoleInput.ReadYesNo("\n  Hiển thị Square-and-Multiply steps?");
            if (showSteps)
            {
                // Chỉ dùng cho exponent vừa với long (bài thi thường nhỏ)
                if (exp <= long.MaxValue)
                {
                    var (_, steps) = ModularArithmetic.ModPowWithSteps(baseVal, exp, mod);
                    System.Console.WriteLine();
                    string expBin = Convert.ToString((long)exp, 2);
                    System.Console.WriteLine($"  Exponent {exp} = {expBin} (binary)");
                    System.Console.WriteLine($"\n  {"Bit",-6} {"Val",-6} {"Operation",-22} {"Result",-12}");
                    System.Console.WriteLine($"  {new string('-', 50)}");
                    foreach (var step in steps)
                        System.Console.WriteLine($"  {step.BitPosition,-6} {step.BitValue,-6} {step.Operation,-22} {step.Result,-12}");
                }
                else
                {
                    ConsoleOutput.PrintInfo("Exponent quá lớn, không hiển thị từng bước.");
                }
            }
            ConsoleInput.PressEnterToContinue();
        }

        static void PrimeMenu()
        {
            ConsoleOutput.PrintHeader("KIỂM TRA SỐ NGUYÊN TỐ");
            BigInteger n = ConsoleInput.ReadBigInt("  n = ");
            bool prime = PrimeUtils.IsPrime(n);
            System.Console.WriteLine();
            ConsoleOutput.PrintKV($"{n} là số nguyên tố?", prime ? "CÓ (YES)" : "KHÔNG (NO)");
            ConsoleInput.PressEnterToContinue();
        }
    }
}

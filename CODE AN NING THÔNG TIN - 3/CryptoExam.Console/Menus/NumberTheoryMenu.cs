using CryptoExam.ConsoleApp.Helpers;
using CryptoExam.Core.NumberTheory;

namespace CryptoExam.ConsoleApp.Menus;

public sealed class NumberTheoryMenu
{
    public void Run()
    {
        while (true)
        {
            ConsoleOutput.Header("NUMBER THEORY / EUCLID / MODULO");
            Console.WriteLine("1. GCD");
            Console.WriteLine("2. Extended Euclid / Modular Inverse (Fast)");
            Console.WriteLine("3. Extended Euclid / Modular Inverse (Show Steps)");
            Console.WriteLine("4. Modular Exponentiation (Fast)");
            Console.WriteLine("5. Modular Exponentiation (Square-and-Multiply Steps)");
            Console.WriteLine("6. Prime Check");
            Console.WriteLine("0. Back");
            var choice = ConsoleInput.Integer("Chọn: ", 0, 6);
            if (choice == 0) return;
            ConsoleOutput.Guard(() => Execute(choice));
        }
    }

    private static void Execute(int choice)
    {
        if (choice == 1)
        {
            var a = ConsoleInput.BigInt("a = "); var b = ConsoleInput.BigInt("b = ");
            ConsoleOutput.Result($"GCD({a},{b})", GcdService.Calculate(a, b));
        }
        else if (choice is 2 or 3)
        {
            var a = ConsoleInput.BigInt("a = "); var m = ConsoleInput.BigInt("m = ");
            var result = ExtendedEuclidService.Solve(a, m);
            if (choice == 3)
            {
                Console.WriteLine("\nEuclid steps:");
                foreach (var step in result.Steps) Console.WriteLine($"{step.Dividend} = {step.Quotient}*{step.Divisor} + {step.Remainder}");
            }
            ConsoleOutput.Result("gcd", result.Gcd); ConsoleOutput.Result("x", result.X); ConsoleOutput.Result("y", result.Y);
            Console.WriteLine($"{a}*({result.X}) + {m}*({result.Y}) = {result.Gcd}");
            if (result.Gcd == 1) ConsoleOutput.Result($"{a}^-1 mod {m}", result.Inverse(m));
            else Console.WriteLine($"NO MODULAR INVERSE because gcd({a},{m}) = {result.Gcd}");
        }
        else if (choice is 4 or 5)
        {
            var value = ConsoleInput.BigInt("Base = "); var exponent = ConsoleInput.BigInt("Exponent = "); var modulus = ConsoleInput.BigInt("Modulus = ");
            if (choice == 5)
            {
                Console.WriteLine("Step | exponent | result | base | multiply? | new result | squared base");
                foreach (var s in ModularArithmetic.TraceModPow(value, exponent, modulus))
                    Console.WriteLine($"{s.Step,4} | {s.Exponent,8} | {s.ResultBefore,6} | {s.BaseBefore,6} | {(s.Multiply ? "YES" : "NO"),9} | {s.ResultAfter,10} | {s.BaseAfter}");
            }
            ConsoleOutput.Result($"{value}^{exponent} mod {modulus}", ModularArithmetic.ModPow(value, exponent, modulus));
        }
        else
        {
            var value = ConsoleInput.BigInt("n = ");
            ConsoleOutput.Result("Is prime", PrimeUtils.IsPrime(value) ? "YES" : "NO");
        }
    }
}

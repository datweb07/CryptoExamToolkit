using CryptoExam.ConsoleApp.Helpers;
using CryptoExam.Core.NumberTheory;

namespace CryptoExam.ConsoleApp.Menus;

public sealed class NumberTheoryMenu
{
    public void Run()
    {
        while (true)
        {
            ConsoleOutput.Header("LÝ THUYẾT SỐ / EUCLID / MODULO");
            Console.WriteLine("1. Ước chung lớn nhất (GCD)");
            Console.WriteLine("2. Euclid mở rộng / Nghịch đảo modulo (Nhanh)");
            Console.WriteLine("3. Euclid mở rộng / Nghịch đảo modulo (Hiển thị bước)");
            Console.WriteLine("4. Lũy thừa modulo (Nhanh)");
            Console.WriteLine("5. Lũy thừa modulo (Hiển thị bước Bình phương và Nhân)");
            Console.WriteLine("6. Kiểm tra số nguyên tố");
            Console.WriteLine("0. Quay lại");
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
                Console.WriteLine("\nCác bước Euclid:");
                foreach (var step in result.Steps) Console.WriteLine($"{step.Dividend} = {step.Quotient}*{step.Divisor} + {step.Remainder}");
            }
            ConsoleOutput.Result("UCLN (GCD)", result.Gcd); ConsoleOutput.Result("x", result.X); ConsoleOutput.Result("y", result.Y);
            Console.WriteLine($"{a}*({result.X}) + {m}*({result.Y}) = {result.Gcd}");
            if (result.Gcd == 1) ConsoleOutput.Result($"{a}^-1 mod {m}", result.Inverse(m));
            else Console.WriteLine($"KHÔNG CÓ NGHỊCH ĐẢO MODULO vì UCLN (GCD)({a},{m}) = {result.Gcd}");
        }
        else if (choice is 4 or 5)
        {
            var value = ConsoleInput.BigInt("Cơ số = "); var exponent = ConsoleInput.BigInt("Số mũ = "); var modulus = ConsoleInput.BigInt("Modulo = ");
            if (choice == 5)
            {
                Console.WriteLine("Bước | số mũ | kết quả | cơ số | nhân? | kết quả mới | cơ số sau bình phương");
                foreach (var s in ModularArithmetic.TraceModPow(value, exponent, modulus))
                    Console.WriteLine($"{s.Step,4} | {s.Exponent,8} | {s.ResultBefore,6} | {s.BaseBefore,6} | {(s.Multiply ? "CÓ" : "KHÔNG"),9} | {s.ResultAfter,10} | {s.BaseAfter}");
            }
            ConsoleOutput.Result($"{value}^{exponent} mod {modulus}", ModularArithmetic.ModPow(value, exponent, modulus));
        }
        else
        {
            var value = ConsoleInput.BigInt("n = ");
            ConsoleOutput.Result("Là số nguyên tố", PrimeUtils.IsPrime(value) ? "CÓ" : "KHÔNG");
        }
    }
}

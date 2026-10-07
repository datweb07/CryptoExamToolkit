using System.Numerics;
using CryptoExam.ConsoleApp.Helpers;
using CryptoExam.Core.RSA;

namespace CryptoExam.ConsoleApp.Menus;

public sealed class RsaMenu
{
    public void Run()
    {
        while (true)
        {
            ConsoleOutput.Header("RSA / CHỮ KÝ SỐ");
            Console.WriteLine("1. Tính n, φ, d từ p, q, e");
            Console.WriteLine("2. Mã hóa (Chuẩn: M^e mod n)");
            Console.WriteLine("3. Giải mã (Chuẩn: C^d mod n)");
            Console.WriteLine("4. Ký - Chế độ UEH / theo slide (M^eA mod n)");
            Console.WriteLine("5. Xác thực - Chế độ UEH / theo slide (S^dA mod n)");
            Console.WriteLine("6. Ký - Chế độ chuẩn (M^d mod n)");
            Console.WriteLine("7. Xác thực - Chế độ chuẩn (S^e mod n)");
            Console.WriteLine("8. Toàn bộ quy trình RSA");
            Console.WriteLine("9. Danh sách Ký/Xác thực theo UEH");
            Console.WriteLine("0. Quay lại");
            var choice = ConsoleInput.Integer("Chọn: ", 0, 9);
            if (choice == 0) return;
            ConsoleOutput.Guard(() => Execute(choice));
        }
    }

    private static void Execute(int choice)
    {
        if (choice == 1)
        {
            PrintKeys(ReadKeys());
            return;
        }
        if (choice == 8)
        {
            var p = ConsoleInput.BigInt("p = "); var q = ConsoleInput.BigInt("q = "); var e = ConsoleInput.BigInt("e = "); var m = ConsoleInput.BigInt("Bản tin = ");
            var result = RsaService.Walkthrough(p, q, e, m);
            PrintKeys(result.Keys);
            Console.WriteLine("\nSTANDARD MODE:");
            ConsoleOutput.Result("Bản mã", result.Ciphertext); ConsoleOutput.Result("Bản rõ sau giải mã", result.Decrypted);
            ConsoleOutput.Result("Chữ ký M^d", result.StandardSignature); ConsoleOutput.Result("Giá trị xác thực S^e", result.StandardVerified);
            Console.WriteLine("\nUEH / LECTURER SLIDE MODE:");
            ConsoleOutput.Result("Chữ ký M^eA", result.UehSignature); ConsoleOutput.Result("Giá trị xác thực S^dA", result.UehVerified);
            return;
        }
        if (choice == 9)
        {
            var listMode = ConsoleInput.Integer("1. Ký bản tin + xác thực  2. Xác thực danh sách chữ ký: ", 1, 2);
            if (listMode == 1)
            {
                var list = ConsoleInput.BigIntegerList("Danh sách bản tin (phân cách bằng dấu phẩy/khoảng trắng): "); var eA = ConsoleInput.BigInt("eA (khóa bí mật theo slide) = "); var dA = ConsoleInput.BigInt("dA (khóa công khai theo slide) = "); var n = ConsoleInput.BigInt("n = ");
                foreach (var item in RsaSignatureService.SignUeh(list, eA, n))
                {
                    var verified = RsaService.VerifyUeh(item.Output, dA, n);
                    Console.WriteLine($"M{item.Index}={item.Input}  S{item.Index}={item.Output}  Verify={verified}");
                }
            }
            else
            {
                var list = ConsoleInput.BigIntegerList("Danh sách chữ ký (phân cách bằng dấu phẩy/khoảng trắng): "); var dA = ConsoleInput.BigInt("dA (khóa công khai theo slide) = "); var n = ConsoleInput.BigInt("n = ");
                foreach (var item in RsaSignatureService.VerifyUeh(list, dA, n))
                    Console.WriteLine($"S{item.Index}={item.Input}  M'{item.Index}={item.Output}");
            }
            return;
        }
        var input = ConsoleInput.BigInt(choice is 2 or 4 or 6 ? "Bản tin = " : "Bản mã/Chữ ký = ");
        var exponentLabel = choice switch { 2 or 7 => "e", 3 or 6 => "d", 4 => "eA (khóa bí mật theo slide UEH)", _ => "dA (khóa công khai theo slide UEH)" };
        var exponent = ConsoleInput.BigInt(exponentLabel + " = "); var modulus = ConsoleInput.BigInt("n = ");
        BigInteger output = choice switch
        {
            2 => RsaService.Encrypt(input, exponent, modulus), 3 => RsaService.Decrypt(input, exponent, modulus),
            4 => RsaService.SignUeh(input, exponent, modulus), 5 => RsaService.VerifyUeh(input, exponent, modulus),
            6 => RsaService.SignStandard(input, exponent, modulus), _ => RsaService.VerifyStandard(input, exponent, modulus)
        };
        ConsoleOutput.Result("Kết quả", output);
    }

    private static RsaKeyPair ReadKeys() => RsaService.CalculateKeys(ConsoleInput.BigInt("p = "), ConsoleInput.BigInt("q = "), ConsoleInput.BigInt("e = "));
    private static void PrintKeys(RsaKeyPair key)
    {
        ConsoleOutput.Result("n = p*q", key.N); ConsoleOutput.Result("phi(n)", key.Phi); ConsoleOutput.Result($"gcd({key.E},phi)", key.Gcd); ConsoleOutput.Result("d = e^-1 mod phi", key.D);
        Console.WriteLine($"Khóa công khai chuẩn = (e={key.E}, n={key.N})");
        Console.WriteLine($"Khóa bí mật chuẩn = (d={key.D}, n={key.N})");
        Console.WriteLine($"Theo slide UEH: eA={key.E} là khóa ký/bí mật, dA={key.D} là khóa xác thực/công khai.");
    }
}

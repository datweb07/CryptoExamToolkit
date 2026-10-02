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
            ConsoleOutput.Header("RSA / DIGITAL SIGNATURE");
            Console.WriteLine("1. Calculate n, phi, d from p,q,e");
            Console.WriteLine("2. Encrypt (Standard: M^e mod n)");
            Console.WriteLine("3. Decrypt (Standard: C^d mod n)");
            Console.WriteLine("4. Sign - UEH Slide Mode (M^eA mod n)");
            Console.WriteLine("5. Verify - UEH Slide Mode (S^dA mod n)");
            Console.WriteLine("6. Sign - Standard Mode (M^d mod n)");
            Console.WriteLine("7. Verify - Standard Mode (S^e mod n)");
            Console.WriteLine("8. Full RSA Walkthrough");
            Console.WriteLine("9. UEH Sign/Verify List");
            Console.WriteLine("0. Back");
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
            var p = ConsoleInput.BigInt("p = "); var q = ConsoleInput.BigInt("q = "); var e = ConsoleInput.BigInt("e = "); var m = ConsoleInput.BigInt("Message = ");
            var result = RsaService.Walkthrough(p, q, e, m);
            PrintKeys(result.Keys);
            Console.WriteLine("\nSTANDARD MODE:");
            ConsoleOutput.Result("Encrypted", result.Ciphertext); ConsoleOutput.Result("Decrypted", result.Decrypted);
            ConsoleOutput.Result("Signature M^d", result.StandardSignature); ConsoleOutput.Result("Verify S^e", result.StandardVerified);
            Console.WriteLine("\nUEH / LECTURER SLIDE MODE:");
            ConsoleOutput.Result("Signature M^eA", result.UehSignature); ConsoleOutput.Result("Verify S^dA", result.UehVerified);
            return;
        }
        if (choice == 9)
        {
            var listMode = ConsoleInput.Integer("1.Sign messages + verify  2.Verify signature list: ", 1, 2);
            if (listMode == 1)
            {
                var list = ConsoleInput.BigIntegerList("Messages (comma/space): "); var eA = ConsoleInput.BigInt("eA (private in slide) = "); var dA = ConsoleInput.BigInt("dA (public in slide) = "); var n = ConsoleInput.BigInt("n = ");
                foreach (var item in RsaSignatureService.SignUeh(list, eA, n))
                {
                    var verified = RsaService.VerifyUeh(item.Output, dA, n);
                    Console.WriteLine($"M{item.Index}={item.Input}  S{item.Index}={item.Output}  Verify={verified}");
                }
            }
            else
            {
                var list = ConsoleInput.BigIntegerList("Signatures (comma/space): "); var dA = ConsoleInput.BigInt("dA (public in slide) = "); var n = ConsoleInput.BigInt("n = ");
                foreach (var item in RsaSignatureService.VerifyUeh(list, dA, n))
                    Console.WriteLine($"S{item.Index}={item.Input}  M'{item.Index}={item.Output}");
            }
            return;
        }
        var input = ConsoleInput.BigInt(choice is 2 or 4 or 6 ? "Message = " : "Cipher/Signature = ");
        var exponentLabel = choice switch { 2 or 7 => "e", 3 or 6 => "d", 4 => "eA (PRIVATE in UEH slide)", _ => "dA (PUBLIC in UEH slide)" };
        var exponent = ConsoleInput.BigInt(exponentLabel + " = "); var modulus = ConsoleInput.BigInt("n = ");
        BigInteger output = choice switch
        {
            2 => RsaService.Encrypt(input, exponent, modulus), 3 => RsaService.Decrypt(input, exponent, modulus),
            4 => RsaService.SignUeh(input, exponent, modulus), 5 => RsaService.VerifyUeh(input, exponent, modulus),
            6 => RsaService.SignStandard(input, exponent, modulus), _ => RsaService.VerifyStandard(input, exponent, modulus)
        };
        ConsoleOutput.Result("Result", output);
    }

    private static RsaKeyPair ReadKeys() => RsaService.CalculateKeys(ConsoleInput.BigInt("p = "), ConsoleInput.BigInt("q = "), ConsoleInput.BigInt("e = "));
    private static void PrintKeys(RsaKeyPair key)
    {
        ConsoleOutput.Result("n = p*q", key.N); ConsoleOutput.Result("phi(n)", key.Phi); ConsoleOutput.Result($"gcd({key.E},phi)", key.Gcd); ConsoleOutput.Result("d = e^-1 mod phi", key.D);
        Console.WriteLine($"Standard public key  = (e={key.E}, n={key.N})");
        Console.WriteLine($"Standard private key = (d={key.D}, n={key.N})");
        Console.WriteLine($"UEH slide: eA={key.E} is signing/private, dA={key.D} is verifying/public.");
    }
}

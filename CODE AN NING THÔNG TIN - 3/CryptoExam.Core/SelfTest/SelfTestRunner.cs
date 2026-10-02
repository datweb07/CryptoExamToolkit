using CryptoExam.Core.AES;
using CryptoExam.Core.Classical;
using CryptoExam.Core.DES;
using CryptoExam.Core.NumberTheory;
using CryptoExam.Core.RSA;

namespace CryptoExam.Core.SelfTest;

public sealed record SelfTestResult(string Name, bool Passed, string Detail);

public static class SelfTestRunner
{
    public static IReadOnlyList<SelfTestResult> RunAll()
    {
        var tests = new List<SelfTestResult>();
        Check(tests, "Caesar", "KHOOR", () => CaesarCipher.Encrypt("HELLO", 3));
        Check(tests, "Vigenere", "LXFOPV", () => VigenereCipher.Encrypt("ATTACK", "LEMON"));
        Check(tests, "Vigenere decrypt", "ATTACKATDAWN", () => VigenereCipher.Decrypt("LXFOPVEFRNHR", "LEMON"));
        Check(tests, "Playfair", "IBSUPMNA", () => new PlayfairCipher("MONARCHY").Encrypt("BALLOON"));
        Check(tests, "OTP", "AMVK", () => OneTimePadCipher.Encrypt("DATA", "XMCK"));
        Check(tests, "Rail Fence", "SREUIYCT", () => RailFenceCipher.Encrypt("SECURITY", 3));
        Check(tests, "Rail Fence decrypt", "SECURITY", () => RailFenceCipher.Decrypt("SREUIYCT", 3));
        var columnar = new ColumnarTranspositionCipher("MONARCH");
        Check(tests, "Columnar round-trip", "WEAREDISCOVERED", () => columnar.Decrypt(columnar.Encrypt("WEAREDISCOVERED")));
        Check(tests, "Matrix transposition round-trip", "INFORMATIONSECURITY", () => MatrixTranspositionCipher.Decrypt(MatrixTranspositionCipher.Encrypt("INFORMATIONSECURITY", 5), 5));
        Check(tests, "Double transposition round-trip", "MEETMEATNOON", () => DoubleTranspositionCipher.Decrypt(DoubleTranspositionCipher.Encrypt("MEETMEATNOON", "MONARCH", "ZEBRAS"), "MONARCH", "ZEBRAS"));
        Check(tests, "Modular inverse 550", "355", () => ExtendedEuclidService.ModularInverse(550, 1759).ToString());
        Check(tests, "Modular inverse 7", "55", () => ExtendedEuclidService.ModularInverse(7, 192).ToString());
        Check(tests, "Modular inverse 17", "2753", () => ExtendedEuclidService.ModularInverse(17, 3120).ToString());
        Check(tests, "Modular inverse 53", "2237", () => ExtendedEuclidService.ModularInverse(53, 3120).ToString());
        Check(tests, "ModPow 64^7", "38", () => ModularArithmetic.ModPow(64, 7, 221).ToString());
        Check(tests, "ModPow 38^55", "64", () => ModularArithmetic.ModPow(38, 55, 221).ToString());
        var rsa = RsaService.CalculateKeys(13, 17, 7);
        Check(tests, "RSA dA", "55", () => rsa.D.ToString());
        Check(tests, "RSA UEH M=64", "38/64", () => { var s = RsaService.SignUeh(64, 7, 221); return $"{s}/{RsaService.VerifyUeh(s, 55, 221)}"; });
        Check(tests, "RSA UEH M=112", "5/112", () => { var s = RsaService.SignUeh(112, 7, 221); return $"{s}/{RsaService.VerifyUeh(s, 55, 221)}"; });
        Check(tests, "RSA UEH M=97", "7/97", () => { var s = RsaService.SignUeh(97, 7, 221); return $"{s}/{RsaService.VerifyUeh(s, 55, 221)}"; });
        var key = DesKeySchedule.Generate("133457799BBCDFF1");
        Check(tests, "DES K1", "1B02EFFC7072", () => key.Rounds[0].SubKeyHex);
        Check(tests, "DES K2", "79AED9DBC9E5", () => key.Rounds[1].SubKeyHex);
        Check(tests, "DES K3", "55FC8A42CF99", () => key.Rounds[2].SubKeyHex);
        Check(tests, "DES K4", "72ADD6DB351D", () => key.Rounds[3].SubKeyHex);
        Check(tests, "DES C3", "0CCAAFF", () => key.Rounds[2].CHex);
        Check(tests, "DES D3", "56678F5", () => key.Rounds[2].DHex);
        Check(tests, "DES C4D4", "332ABFC599E3D5", () => key.Rounds[3].CDHex);
        var des = DesCipher.Trace("0123456789ABCDEF", "133457799BBCDFF1");
        Check(tests, "DES IP", "CC00CCFFF0AAF0AA", () => des.InitialPermutationHex);
        Check(tests, "DES L0", "CC00CCFF", () => des.L0Hex);
        Check(tests, "DES R0", "F0AAF0AA", () => des.R0Hex);
        Check(tests, "DES E(R0)", "7A15557A1555", () => des.Rounds[0].Function.ExpandedRHex);
        Check(tests, "DES XOR", "6117BA866527", () => des.Rounds[0].Function.XorHex);
        Check(tests, "DES SBox", "5C82B597", () => des.Rounds[0].Function.SBoxHex);
        Check(tests, "DES P", "234AA9BB", () => des.Rounds[0].Function.PHex);
        Check(tests, "DES L1", "F0AAF0AA", () => des.Rounds[0].LHex);
        Check(tests, "DES R1", "EF4A6544", () => des.Rounds[0].RHex);
        Check(tests, "DES encryption", "85E813540F0AB405", () => des.OutputHex);
        Check(tests, "DES decryption", "0123456789ABCDEF", () => DesCipher.Decrypt("85E813540F0AB405", "133457799BBCDFF1"));
        Check(tests, "AES-128 FIPS", "69C4E0D86A7B0430D8CDB78070B4C55A", () => AesService.TraceEncrypt128("00112233445566778899AABBCCDDEEFF", "000102030405060708090A0B0C0D0E0F").CiphertextHex);
        Check(tests, "AES quick decrypt", "00112233445566778899AABBCCDDEEFF", () => AesService.QuickDecrypt("69C4E0D86A7B0430D8CDB78070B4C55A", "000102030405060708090A0B0C0D0E0F"));
        return tests;
    }

    private static void Check(List<SelfTestResult> results, string name, string expected, Func<string> action)
    {
        try
        {
            var actual = action();
            results.Add(new(name, actual == expected, actual == expected ? actual : $"expected {expected}, actual {actual}"));
        }
        catch (Exception ex) { results.Add(new(name, false, ex.Message)); }
    }
}

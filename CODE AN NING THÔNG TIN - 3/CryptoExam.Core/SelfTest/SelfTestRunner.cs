using CryptoExam.Core.AES;
using CryptoExam.Core.Classical;
using CryptoExam.Core.DES;
using CryptoExam.Core.NumberTheory;
using CryptoExam.Core.RSA;
using CryptoExam.Core.Common;
using System.Numerics;

namespace CryptoExam.Core.SelfTest;

public sealed record SelfTestResult(string Name, bool Passed, string Detail);

public static class SelfTestRunner
{
    public static IReadOnlyList<SelfTestResult> RunAll()
    {
        var tests = new List<SelfTestResult>();
        
        // CLASSICAL & TRANSPOSITION
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
        
        // NUMBER THEORY
        Check(tests, "Modular inverse 550", "355", () => ExtendedEuclidService.ModularInverse(550, 1759).ToString());
        Check(tests, "Modular inverse 7", "55", () => ExtendedEuclidService.ModularInverse(7, 192).ToString());
        Check(tests, "Modular inverse 17", "2753", () => ExtendedEuclidService.ModularInverse(17, 3120).ToString());
        Check(tests, "Modular inverse 53", "2237", () => ExtendedEuclidService.ModularInverse(53, 3120).ToString());
        Check(tests, "ModPow 64^7", "38", () => ModularArithmetic.ModPow(64, 7, 221).ToString());
        Check(tests, "ModPow 38^55", "64", () => ModularArithmetic.ModPow(38, 55, 221).ToString());
        
        // RSA BASIC
        var rsa = RsaService.CalculateKeys(13, 17, 7);
        Check(tests, "RSA dA", "55", () => rsa.D.ToString());
        Check(tests, "RSA UEH M=64", "38/64", () => { var s = RsaService.SignUeh(64, 7, 221); return $"{s}/{RsaService.VerifyUeh(s, 55, 221)}"; });
        Check(tests, "RSA UEH M=112", "5/112", () => { var s = RsaService.SignUeh(112, 7, 221); return $"{s}/{RsaService.VerifyUeh(s, 55, 221)}"; });
        Check(tests, "RSA UEH M=97", "7/97", () => { var s = RsaService.SignUeh(97, 7, 221); return $"{s}/{RsaService.VerifyUeh(s, 55, 221)}"; });
        
        // DES
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
        
        // AES
        Check(tests, "AES-128 FIPS", "69C4E0D86A7B0430D8CDB78070B4C55A", () => AesService.TraceEncrypt128("00112233445566778899AABBCCDDEEFF", "000102030405060708090A0B0C0D0E0F").CiphertextHex);
        Check(tests, "AES quick decrypt", "00112233445566778899AABBCCDDEEFF", () => AesService.QuickDecrypt("69C4E0D86A7B0430D8CDB78070B4C55A", "000102030405060708090A0B0C0D0E0F"));
        
        // NEW TESTS: A0Z25
        Check(tests, "A0Z25: T -> 19", "19", () => EncodingUtils.CharToA0Z25('T').ToString());
        Check(tests, "A0Z25: 19 -> T", "T", () => EncodingUtils.A0Z25ToChar(19).ToString());
        Check(tests, "A0Z25: TRUONG -> [19,17,20,14,13,6]", "19,17,20,14,13,6", () => string.Join(",", EncodingUtils.TextToA0Z25("TRUONG")));
        Check(tests, "A0Z25: [19,17,20,14,13,6] -> TRUONG", "TRUONG", () => EncodingUtils.A0Z25ToText(new[] { 19, 17, 20, 14, 13, 6 }));

        // NEW TESTS: ASCII
        Check(tests, "ASCII: T -> 84", "84", () => EncodingUtils.CharToAscii('T').ToString());
        Check(tests, "ASCII: 84 -> T", "T", () => EncodingUtils.AsciiToChar(84).ToString());
        Check(tests, "ASCII: TRUONGTHANHDAT", "84,82,85,79,78,71,84,72,65,78,72,68,65,84", () => string.Join(",", EncodingUtils.TextToAsciiList("TRUONGTHANHDAT")));

        // NEW TESTS: Digit mapping
        Check(tests, "DigitMap: 2006 -> CAAG", "CAAG", () => EncodingUtils.DigitStringToLetters("2006"));
        Check(tests, "DigitMap: CAAG -> 2006", "2006", () => EncodingUtils.LettersToDigitString("CAAG"));
        Check(tests, "DigitMap: TRUONG6 -> TRUONGG", "TRUONGG", () => EncodingUtils.NormalizeVigenereKey("TRUONG6"));
        
        // NEW TESTS: RSA Text Signature Walkthrough
        var rsaText = RsaTextSignatureService.SignText("TRUONGTHANHDAT", 11, 221, RsaTextSignatureService.TextEncoding.Ascii);
        var expectedSigs = "50,10,119,131,156,24,50,132,78,156,132,204,78,50";
        Check(tests, "RSA Text Sign Sigs", expectedSigs, () => string.Join(",", rsaText.Signatures));
        var rsaVerify = RsaTextSignatureService.VerifyText(rsaText.Signatures, 35, 221, RsaTextSignatureService.TextEncoding.Ascii, "TRUONGTHANHDAT");
        Check(tests, "RSA Text Verify Result", "VALID", () => rsaVerify.Valid ? "VALID" : "INVALID");
        Check(tests, "RSA Text Verify Recovered", "TRUONGTHANHDAT", () => rsaVerify.RecoveredText);

        // NEW TESTS: Formula tests
        Check(tests, "Formula: Caesar key", "21", () => ExamFormulaService.CaesarKeyFromDate(15, 10).K.ToString());
        Check(tests, "Formula: Rail Fence", "2", () => ExamFormulaService.RailFenceFromDay(15).Rails.ToString());

        // NEW TESTS: Monoalphabetic
        var mono = EncodingUtils.GenerateMonoAlphabet("DAT1510");
        Check(tests, "Mono Gen Normalized", "DATBFBA", () => mono.NormalizedSeed);
        Check(tests, "Mono Gen Unique", "DATBF", () => mono.UniqueSeed);
        Check(tests, "Mono Gen Final", "DATBFZYXWVUSRQPONMLKJIHGEC", () => mono.FinalAlphabet);

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

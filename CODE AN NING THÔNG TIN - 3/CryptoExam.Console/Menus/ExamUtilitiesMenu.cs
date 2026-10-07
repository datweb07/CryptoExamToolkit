using System.Numerics;
using CryptoExam.Core.Common;
using CryptoExam.Core.NumberTheory;
using CryptoExam.Core.RSA;
using CryptoExam.ConsoleApp.Helpers;

namespace CryptoExam.ConsoleApp.Menus;

public sealed class ExamUtilitiesMenu
{
    public void Run()
    {
        while (true)
        {
            ConsoleOutput.Header("EXAM UTILITIES / CONVERSION & CALCULATORS");
            Console.WriteLine("1. A=0..Z=25 Conversion");
            Console.WriteLine("2. ASCII Conversion");
            Console.WriteLine("3. Digit 0-9 <-> A-J Conversion");
            Console.WriteLine("4. Text Normalization for Classical Cipher");
            Console.WriteLine("5. RSA Digital Signature from Text");
            Console.WriteLine("6. Exam Formula Calculator");
            Console.WriteLine("7. Number / Modulo Quick Calculators");
            Console.WriteLine("8. Character Code Table (full view)");
            Console.WriteLine("9. Monoalphabetic Alphabet Generator");
            Console.WriteLine("0. Back");
            var choice = ConsoleInput.Integer("Chon: ", 0, 9);
            if (choice == 0) return;
            ConsoleOutput.Guard(() => Execute(choice));
        }
    }

    private static void Execute(int choice)
    {
        switch (choice)
        {
            case 1: A0Z25Menu(); break;
            case 2: AsciiMenu(); break;
            case 3: DigitLetterMenu(); break;
            case 4: NormMenu(); break;
            case 5: RsaTextMenu(); break;
            case 6: FormulaMenu(); break;
            case 7: QuickCalcMenu(); break;
            case 8: CharCodeTable(); break;
            case 9: MonoAlphaGen(); break;
        }
    }

    // ============================================================
    // 1. A=0..Z=25
    // ============================================================
    private static void A0Z25Menu()
    {
        ConsoleOutput.Header("A=0..Z=25 CONVERSION");
        Console.WriteLine("1. Character -> Number");
        Console.WriteLine("2. Number -> Character");
        Console.WriteLine("3. Text -> Number List");
        Console.WriteLine("4. Number List -> Text");
        var m = ConsoleInput.Integer("Chon: ", 1, 4);
        switch (m)
        {
            case 1:
                var c = ConsoleInput.Text("Character: ").ToUpperInvariant().Trim();
                if (c.Length != 1) throw new ArgumentException("Nhap dung 1 ky tu.");
                var n1 = EncodingUtils.CharToA0Z25(c[0]);
                Console.WriteLine($"\nCharacter : {c[0]}");
                Console.WriteLine($"Value     : {n1}");
                Console.WriteLine($"Formula   : '{c[0]}' - 'A' = {n1}");
                break;
            case 2:
                var n2 = ConsoleInput.Integer("Number (0-25): ", 0, 25);
                Console.WriteLine($"\n{n2} -> {EncodingUtils.A0Z25ToChar(n2)}");
                break;
            case 3:
                var text3 = ConsoleInput.Text("Text (letters only): ").ToUpperInvariant().Trim();
                var vals3 = EncodingUtils.TextToA0Z25(text3);
                Console.WriteLine($"\nText:\n{text3}\n\nA=0..Z=25:\n[{string.Join(",", vals3)}]");
                Console.WriteLine($"\n{"Char",-6} | {"Value",-5}");
                Console.WriteLine(new string('-', 16));
                for (var i = 0; i < text3.Length; i++)
                    Console.WriteLine($"{text3[i],-6} | {vals3[i],-5}");
                break;
            case 4:
                var input4 = ConsoleInput.Text("Number list (comma/space separated): ");
                var nums4 = EncodingUtils.ParseIntList(input4);
                Console.WriteLine($"\n[{string.Join(",", nums4)}] -> {EncodingUtils.A0Z25ToText(nums4)}");
                break;
        }
    }

    // ============================================================
    // 2. ASCII
    // ============================================================
    private static void AsciiMenu()
    {
        ConsoleOutput.Header("ASCII CONVERSION");
        Console.WriteLine("1. Character -> Decimal");
        Console.WriteLine("2. Decimal -> Character");
        Console.WriteLine("3. Text -> ASCII Decimal List");
        Console.WriteLine("4. ASCII Decimal List -> Text");
        Console.WriteLine("5. Decimal -> Binary (8-bit)");
        Console.WriteLine("6. Decimal -> Hex");
        Console.WriteLine("7. Character -> Full Info (Dec/Bin/Hex)");
        var m = ConsoleInput.Integer("Chon: ", 1, 7);
        switch (m)
        {
            case 1:
                var c1 = ConsoleInput.Text("Character: ").Trim();
                if (c1.Length != 1) throw new ArgumentException("Nhap dung 1 ky tu.");
                Console.WriteLine($"\n{c1[0]} -> ASCII {EncodingUtils.CharToAscii(c1[0])}");
                break;
            case 2:
                var d2 = ConsoleInput.Integer("ASCII Decimal: ", 0, 127);
                Console.WriteLine($"\n{d2} -> '{EncodingUtils.AsciiToChar(d2)}'");
                break;
            case 3:
                var t3 = ConsoleInput.Text("Text: ");
                var a3 = EncodingUtils.TextToAsciiList(t3);
                Console.WriteLine($"\n[{string.Join(", ", a3)}]");
                Console.WriteLine($"\n{"Char",-6} | {"ASCII",-5}");
                Console.WriteLine(new string('-', 16));
                for (var i = 0; i < t3.Length; i++)
                    Console.WriteLine($"{t3[i],-6} | {a3[i],-5}");
                break;
            case 4:
                var input4 = ConsoleInput.Text("ASCII list (comma/space separated): ");
                var nums4 = EncodingUtils.ParseIntList(input4);
                Console.WriteLine($"\n{EncodingUtils.AsciiListToText(nums4)}");
                break;
            case 5:
                var d5 = ConsoleInput.Integer("ASCII Decimal: ", 0, 255);
                Console.WriteLine($"\n{d5} = {EncodingUtils.AsciiToBinary(d5)}");
                break;
            case 6:
                var d6 = ConsoleInput.Integer("ASCII Decimal: ", 0, 255);
                Console.WriteLine($"\n{d6} = 0x{EncodingUtils.AsciiToHex(d6)}");
                break;
            case 7:
                var c7 = ConsoleInput.Text("Character: ").Trim();
                if (c7.Length != 1) throw new ArgumentException("Nhap dung 1 ky tu.");
                var code7 = EncodingUtils.CharToAscii(c7[0]);
                Console.WriteLine($"\nCharacter  : {c7[0]}");
                Console.WriteLine($"ASCII dec  : {code7}");
                Console.WriteLine($"ASCII bin  : {EncodingUtils.AsciiToBinary(code7)}");
                Console.WriteLine($"ASCII hex  : 0x{EncodingUtils.AsciiToHex(code7)}");
                Console.WriteLine($"A=0..Z=25  : {(c7[0] >= 'A' && c7[0] <= 'Z' ? EncodingUtils.CharToA0Z25(char.ToUpperInvariant(c7[0])).ToString() : "N/A (khong phai A-Z)")}");
                break;
        }
    }

    // ============================================================
    // 3. Digit <-> Letter
    // ============================================================
    private static void DigitLetterMenu()
    {
        ConsoleOutput.Header("DIGIT 0-9 <-> LETTER A-J");
        Console.WriteLine("Quy uoc: 0->A, 1->B, 2->C, ..., 9->J");
        Console.WriteLine("1. Digit string -> Letters (e.g. 2006 -> CAAG)");
        Console.WriteLine("2. Letters A-J -> Digit string (e.g. CAAG -> 2006)");
        Console.WriteLine("3. Single digit -> Letter");
        Console.WriteLine("4. Single Letter A-J -> Digit");
        var m = ConsoleInput.Integer("Chon: ", 1, 4);
        switch (m)
        {
            case 1:
                var digits = ConsoleInput.Text("Digit string: ").Trim();
                var letters = EncodingUtils.DigitStringToLetters(digits);
                Console.WriteLine($"\n{digits} -> {letters}");
                Console.WriteLine();
                for (var i = 0; i < digits.Length; i++)
                    Console.WriteLine($"  {digits[i]} -> {letters[i]}");
                break;
            case 2:
                var lstr = ConsoleInput.Text("Letter string (A-J only): ").ToUpperInvariant().Trim();
                var dstr = EncodingUtils.LettersToDigitString(lstr);
                Console.WriteLine($"\n{lstr} -> {dstr}");
                break;
            case 3:
                var d3 = ConsoleInput.Text("Digit (0-9): ").Trim();
                if (d3.Length != 1 || !char.IsDigit(d3[0])) throw new ArgumentException("Nhap dung 1 chu so.");
                Console.WriteLine($"\n{d3[0]} -> {EncodingUtils.DigitToLetter(d3[0])}");
                break;
            case 4:
                var l4 = ConsoleInput.Text("Letter (A-J): ").ToUpperInvariant().Trim();
                if (l4.Length != 1) throw new ArgumentException("Nhap dung 1 chu cai.");
                Console.WriteLine($"\n{l4[0]} -> {EncodingUtils.LetterToDigit(l4[0])}");
                break;
        }
    }

    // ============================================================
    // 4. Text Normalization
    // ============================================================
    private static void NormMenu()
    {
        ConsoleOutput.Header("TEXT NORMALIZATION FOR CLASSICAL CIPHER");
        var input = ConsoleInput.Text("Input text: ");
        Console.WriteLine("\n1. Letters only (bo so va ky tu dac biet)");
        Console.WriteLine("2. Preserve digits (giu A-Z va 0-9)");
        Console.WriteLine("3. Convert digits 0-9 -> A-J");
        Console.WriteLine("4. Show all modes");
        var m = ConsoleInput.Integer("Chon: ", 1, 4);
        if (m == 4)
        {
            var r1 = EncodingUtils.NormalizeText(input, EncodingUtils.NormMode.LettersOnly);
            var r2 = EncodingUtils.NormalizeText(input, EncodingUtils.NormMode.PreserveDigits);
            var r3 = EncodingUtils.NormalizeText(input, EncodingUtils.NormMode.DigitsToLetters);
            Console.WriteLine($"\nMode 1 (Letters only)       : {r1}");
            Console.WriteLine($"Mode 2 (Preserve digits)    : {r2}");
            Console.WriteLine($"Mode 3 (Digits -> A-J)      : {r3}");
        }
        else
        {
            var mode = m switch { 1 => EncodingUtils.NormMode.LettersOnly, 2 => EncodingUtils.NormMode.PreserveDigits, _ => EncodingUtils.NormMode.DigitsToLetters };
            ConsoleOutput.Result("Result", EncodingUtils.NormalizeText(input, mode));
        }
    }

    // ============================================================
    // 5. RSA Digital Signature from Text
    // ============================================================
    private static void RsaTextMenu()
    {
        ConsoleOutput.Header("RSA DIGITAL SIGNATURE FROM TEXT");
        Console.WriteLine("1. Sign Text (UEH Mode: S = M^eA mod n)");
        Console.WriteLine("2. Verify Signatures");
        Console.WriteLine("3. Full Walkthrough (Text + Keys + Sign + Verify)");
        Console.WriteLine("4. Find nearby primes (birthday helper)");
        Console.WriteLine("5. Find valid e for phi");
        var m = ConsoleInput.Integer("Chon: ", 1, 5);
        switch (m)
        {
            case 1: RsaSignText(); break;
            case 2: RsaVerifyText(); break;
            case 3: RsaFullWalkthrough(); break;
            case 4: BirthdayPrimes(); break;
            case 5: FindValidE(); break;
        }
    }

    private static (BigInteger eA, BigInteger n, RsaTextSignatureService.TextEncoding enc) ReadRsaParams()
    {
        var eA = ConsoleInput.BigInt("eA (signing/private in UEH mode) = ");
        var n = ConsoleInput.BigInt("n = ");
        Console.WriteLine("Encoding: 1.ASCII  2.A=0..Z=25");
        var encChoice = ConsoleInput.Integer("Chon: ", 1, 2);
        var enc = encChoice == 1 ? RsaTextSignatureService.TextEncoding.Ascii : RsaTextSignatureService.TextEncoding.A0Z25;
        return (eA, n, enc);
    }

    private static void RsaSignText()
    {
        var text = ConsoleInput.Text("Plaintext: ").ToUpperInvariant().Trim();
        var (eA, n, enc) = ReadRsaParams();
        var result = RsaTextSignatureService.SignText(text, eA, n, enc);
        Console.WriteLine($"\n[UEH SIGN MODE] S = M^{eA} mod {n}");
        Console.WriteLine($"Encoding: {enc}\n");
        Console.WriteLine($"{"#",-4} | {"Char",-5} | {"M",-6} | {"Formula",-20} | S");
        Console.WriteLine(new string('-', 60));
        foreach (var e in result.SignEntries)
            Console.WriteLine($"{e.Index,-4} | {e.Char,-5} | {e.MessageValue,-6} | {e.Formula,-20} | {e.Signature}");
        Console.WriteLine($"\nMessage values : [{string.Join(", ", result.MessageValues)}]");
        Console.WriteLine($"Signatures     : [{string.Join(", ", result.Signatures)}]");
    }

    private static void RsaVerifyText()
    {
        var sigInput = ConsoleInput.Text("Signatures (comma/space): ");
        var sigs = EncodingUtils.ParseIntList(sigInput).Select(x => (BigInteger)x).ToArray();
        var dA = ConsoleInput.BigInt("dA (verifying/public in UEH mode) = ");
        var n = ConsoleInput.BigInt("n = ");
        Console.WriteLine("Encoding: 1.ASCII  2.A=0..Z=25");
        var enc = ConsoleInput.Integer("Chon: ", 1, 2) == 1 ? RsaTextSignatureService.TextEncoding.Ascii : RsaTextSignatureService.TextEncoding.A0Z25;
        var origText = ConsoleInput.Text("Original text (Enter = skip): ", true).ToUpperInvariant().Trim();
        var result = RsaTextSignatureService.VerifyText(sigs, dA, n, enc, origText.Length > 0 ? origText : null);
        Console.WriteLine($"\n[UEH VERIFY MODE] M' = S^{dA} mod {n}\n");
        Console.WriteLine($"{"#",-4} | {"S",-6} | {"M'",-6} | Char");
        Console.WriteLine(new string('-', 35));
        foreach (var e in result.VerifyEntries)
            Console.WriteLine($"{e.Index,-4} | {e.Signature,-6} | {e.RecoveredValue,-6} | {e.RecoveredChar}");
        Console.WriteLine($"\nRecovered values : [{string.Join(", ", result.RecoveredValues)}]");
        Console.WriteLine($"Recovered text   : {result.RecoveredText}");
        if (origText.Length > 0) Console.WriteLine($"Verification     : {(result.Valid ? "VALID" : "INVALID")}");
    }

    private static void RsaFullWalkthrough()
    {
        Console.WriteLine("\n--- STEP 1: TEXT ---");
        var text = ConsoleInput.Text("Plaintext: ").ToUpperInvariant().Trim();
        Console.WriteLine("\n--- STEP 2: RSA PARAMETERS ---");
        var p = ConsoleInput.BigInt("p = ");
        var q = ConsoleInput.BigInt("q = ");
        var eA = ConsoleInput.BigInt("eA (signing/private) = ");
        Console.WriteLine("Encoding: 1.ASCII  2.A=0..Z=25");
        var enc = ConsoleInput.Integer("Chon: ", 1, 2) == 1 ? RsaTextSignatureService.TextEncoding.Ascii : RsaTextSignatureService.TextEncoding.A0Z25;

        var keys = RsaService.CalculateKeys(p, q, eA);
        Console.WriteLine($"\n--- STEP 3: KEY GENERATION ---");
        Console.WriteLine($"p       = {keys.P}");
        Console.WriteLine($"q       = {keys.Q}");
        Console.WriteLine($"n       = {keys.N}");
        Console.WriteLine($"phi(n)  = {keys.Phi}");
        Console.WriteLine($"eA      = {keys.E}  [UEH: signing/private exponent]");
        Console.WriteLine($"dA      = {keys.D}  [UEH: verification/public exponent]");

        Console.WriteLine($"\n--- STEP 4: ENCODE TEXT ---");
        var signResult = RsaTextSignatureService.SignText(text, keys.E, keys.N, enc);
        Console.WriteLine($"Encoding   : {enc}");
        Console.WriteLine($"Text       : {text}");
        Console.WriteLine($"M values   : [{string.Join(", ", signResult.MessageValues)}]");

        Console.WriteLine($"\n--- STEP 5: SIGN EACH CHARACTER (S = M^eA mod n) ---");
        Console.WriteLine($"{"#",-4} | {"Char",-5} | {"M",-6} | {"Formula",-20} | S");
        Console.WriteLine(new string('-', 60));
        foreach (var e in signResult.SignEntries)
            Console.WriteLine($"{e.Index,-4} | {e.Char,-5} | {e.MessageValue,-6} | {e.Formula,-20} | {e.Signature}");
        Console.WriteLine($"\nSignatures : [{string.Join(", ", signResult.Signatures)}]");

        Console.WriteLine($"\n--- STEP 6: VERIFY (M' = S^dA mod n) ---");
        var verifyResult = RsaTextSignatureService.VerifyText(signResult.Signatures, keys.D, keys.N, enc, text);
        Console.WriteLine($"{"#",-4} | {"S",-6} | {"M'",-6} | Char");
        Console.WriteLine(new string('-', 35));
        foreach (var e in verifyResult.VerifyEntries)
            Console.WriteLine($"{e.Index,-4} | {e.Signature,-6} | {e.RecoveredValue,-6} | {e.RecoveredChar}");

        Console.WriteLine($"\n--- STEP 7: RESULT ---");
        Console.WriteLine($"Original  : {text}");
        Console.WriteLine($"Recovered : {verifyResult.RecoveredText}");
        Console.WriteLine($"Valid     : {(verifyResult.Valid ? "YES - VALID" : "NO - INVALID")}");
    }

    private static void BirthdayPrimes()
    {
        var n = ConsoleInput.BigInt("Birthday number (e.g. day = 15): ");
        var (prev, _, next, nIsPrime) = RsaTextSignatureService.FindNearbyPrimes(n);
        Console.WriteLine();
        if (nIsPrime) Console.WriteLine($"{n} itself is PRIME.");
        Console.WriteLine($"Previous prime = {prev?.ToString() ?? "N/A"}");
        Console.WriteLine($"Next prime     = {next?.ToString() ?? "N/A"}");
        if (prev.HasValue && next.HasValue)
        {
            Console.WriteLine($"\nSuggested (prev < N < next):");
            Console.WriteLine($"  p = {prev}");
            Console.WriteLine($"  q = {next}");
        }
    }

    private static void FindValidE()
    {
        var phi = ConsoleInput.BigInt("phi(n) = ");
        var candidates = RsaTextSignatureService.FindValidE(phi, 20);
        Console.WriteLine($"\nValid e values for phi={phi}:");
        foreach (var e in candidates) Console.Write($"{e} ");
        Console.WriteLine();
        var checkMode = ConsoleInput.YesNo("Check a specific e?");
        if (checkMode)
        {
            var eCheck = ConsoleInput.BigInt("e = ");
            var (valid, gcd) = RsaTextSignatureService.CheckE(eCheck, phi);
            Console.WriteLine($"gcd({eCheck}, {phi}) = {gcd}  -> {(valid ? "VALID" : "INVALID")}");
        }
    }

    // ============================================================
    // 6. Exam Formula Calculator
    // ============================================================
    private static void FormulaMenu()
    {
        ConsoleOutput.Header("EXAM FORMULA CALCULATOR");
        Console.WriteLine("1. Caesar key: K = ((day * month) mod 26) + 1");
        Console.WriteLine("2. Rail Fence rails: (day mod 3) + 2");
        Console.WriteLine("3. a mod m");
        Console.WriteLine("4. (a * b) mod m");
        var m = ConsoleInput.Integer("Chon: ", 1, 4);
        switch (m)
        {
            case 1:
                var day1 = ConsoleInput.Integer("day = ");
                var month1 = ConsoleInput.Integer("month = ");
                var (k, f) = ExamFormulaService.CaesarKeyFromDate(day1, month1);
                Console.WriteLine($"\n{f}");
                break;
            case 2:
                var day2 = ConsoleInput.Integer("day = ");
                var (rails, f2) = ExamFormulaService.RailFenceFromDay(day2);
                Console.WriteLine($"\n{f2}");
                break;
            case 3:
                var a3 = ConsoleInput.BigInt("a = ");
                var mm3 = ConsoleInput.BigInt("m = ");
                var r3 = ((a3 % mm3) + mm3) % mm3;
                Console.WriteLine($"\n{a3} mod {mm3} = {r3}");
                break;
            case 4:
                var a4 = ConsoleInput.BigInt("a = ");
                var b4 = ConsoleInput.BigInt("b = ");
                var mm4 = ConsoleInput.BigInt("m = ");
                var r4 = ((a4 * b4) % mm4 + mm4) % mm4;
                Console.WriteLine($"\n({a4} * {b4}) mod {mm4} = {r4}");
                break;
        }
    }

    // ============================================================
    // 7. Quick Calculators (reuse existing services)
    // ============================================================
    private static void QuickCalcMenu()
    {
        ConsoleOutput.Header("NUMBER / MODULO QUICK CALCULATORS");
        Console.WriteLine("1. a mod m");
        Console.WriteLine("2. (a*b) mod m");
        Console.WriteLine("3. a^-1 mod m (Modular Inverse)");
        Console.WriteLine("4. a^b mod m (ModPow)");
        Console.WriteLine("5. GCD(a,b)");
        Console.WriteLine("6. Extended Euclid");
        Console.WriteLine("7. Check Prime");
        Console.WriteLine("8. Euler phi from p,q");
        Console.WriteLine("9. RSA n, phi, d from p,q,e");
        var m = ConsoleInput.Integer("Chon: ", 1, 9);
        switch (m)
        {
            case 1:
                { var a = ConsoleInput.BigInt("a = "); var mod = ConsoleInput.BigInt("m = ");
                  Console.WriteLine($"\n{a} mod {mod} = {((a % mod) + mod) % mod}"); break; }
            case 2:
                { var a = ConsoleInput.BigInt("a = "); var b = ConsoleInput.BigInt("b = "); var mod = ConsoleInput.BigInt("m = ");
                  var r = ((a * b) % mod + mod) % mod;
                  Console.WriteLine($"\n({a} * {b}) mod {mod} = {r}"); break; }
            case 3:
                { var a = ConsoleInput.BigInt("a = "); var mod = ConsoleInput.BigInt("m = ");
                  var euclid = ExtendedEuclidService.Solve(a, mod);
                  if (euclid.Gcd == 1) Console.WriteLine($"\n{a}^-1 mod {mod} = {euclid.Inverse(mod)}");
                  else Console.WriteLine($"\nNO INVERSE: gcd({a},{mod}) = {euclid.Gcd}"); break; }
            case 4:
                { var b = ConsoleInput.BigInt("Base = "); var exp = ConsoleInput.BigInt("Exponent = "); var mod = ConsoleInput.BigInt("Modulus = ");
                  Console.WriteLine($"\n{b}^{exp} mod {mod} = {ModularArithmetic.ModPow(b, exp, mod)}"); break; }
            case 5:
                { var a = ConsoleInput.BigInt("a = "); var b = ConsoleInput.BigInt("b = ");
                  Console.WriteLine($"\nGCD({a},{b}) = {BigInteger.GreatestCommonDivisor(a, b)}"); break; }
            case 6:
                { var a = ConsoleInput.BigInt("a = "); var mod = ConsoleInput.BigInt("m = ");
                  var res = ExtendedEuclidService.Solve(a, mod);
                  Console.WriteLine("\nEuclid steps:");
                  foreach (var s in res.Steps) Console.WriteLine($"  {s.Dividend} = {s.Quotient}*{s.Divisor} + {s.Remainder}");
                  Console.WriteLine($"gcd = {res.Gcd}  x = {res.X}  y = {res.Y}");
                  if (res.Gcd == 1) Console.WriteLine($"{a}^-1 mod {mod} = {res.Inverse(mod)}");
                  else Console.WriteLine($"NO INVERSE: gcd={res.Gcd}"); break; }
            case 7:
                { var n = ConsoleInput.BigInt("n = ");
                  Console.WriteLine($"\n{n} is{(PrimeUtils.IsPrime(n) ? "" : " NOT")} prime."); break; }
            case 8:
                { var p = ConsoleInput.BigInt("p = "); var q = ConsoleInput.BigInt("q = ");
                  Console.WriteLine($"\nphi({p}*{q}) = ({p}-1)*({q}-1) = {(p-1)*(q-1)}"); break; }
            case 9:
                { var p = ConsoleInput.BigInt("p = "); var q = ConsoleInput.BigInt("q = "); var e = ConsoleInput.BigInt("e = ");
                  var keys = RsaService.CalculateKeys(p, q, e);
                  Console.WriteLine($"\nn   = {keys.N}\nphi = {keys.Phi}\ngcd = {keys.Gcd}\nd   = {keys.D}"); break; }
        }
    }

    // ============================================================
    // 8. Character Code Table
    // ============================================================
    private static void CharCodeTable()
    {
        ConsoleOutput.Header("CHARACTER CODE TABLE");
        Console.WriteLine($"{"Char",-5} | {"A0Z25",-5} | {"ASCII",-5} | {"Bin",-8} | {"Hex",-4}");
        Console.WriteLine(new string('-', 40));
        for (var c = 'A'; c <= 'Z'; c++)
        {
            var a0z25 = c - 'A';
            var ascii = (int)c;
            var bin = EncodingUtils.AsciiToBinary(ascii);
            var hex = EncodingUtils.AsciiToHex(ascii);
            Console.WriteLine($"{c,-5} | {a0z25,-5} | {ascii,-5} | {bin,-8} | {hex,-4}");
        }
    }

    // ============================================================
    // 9. Monoalphabetic Alphabet Generator
    // ============================================================
    private static void MonoAlphaGen()
    {
        ConsoleOutput.Header("MONOALPHABETIC ALPHABET GENERATOR");
        Console.WriteLine("Quy tac: Uppercase -> Digit->Letter -> Remove dup -> Append Z..A\n");
        var seed = ConsoleInput.Text("Seed (e.g. DAT1510): ");
        var (normalized, unique, final) = EncodingUtils.GenerateMonoAlphabet(seed);
        Console.WriteLine($"\nOriginal seed       : {seed.ToUpperInvariant()}");
        Console.WriteLine($"After digit->letter : {normalized}");
        Console.WriteLine($"After remove dup    : {unique}");
        Console.WriteLine($"Final alphabet      : {final}");
        Console.WriteLine($"Plain alphabet      : ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        Console.WriteLine($"\nMapping:");
        for (var i = 0; i < 26; i++)
            Console.Write($"{(char)('A' + i)}->{final[i]}  ");
        Console.WriteLine();
        Console.WriteLine("\nPlayfair key normalization helper:");
        var pfKey = ConsoleInput.Text("Playfair key (Enter = skip): ", true).Trim();
        if (pfKey.Length > 0)
        {
            var pf = EncodingUtils.NormalizePlayfairKey(pfKey);
            Console.WriteLine($"Normalized Playfair key: {pf}");
        }
        Console.WriteLine("\nVigenere key normalization helper:");
        var vKey = ConsoleInput.Text("Vigenere key (Enter = skip): ", true).Trim();
        if (vKey.Length > 0)
        {
            var vn = EncodingUtils.NormalizeVigenereKey(vKey);
            Console.WriteLine($"Normalized Vigenere key: {vn}");
        }
    }
}

// File: CryptoExam.Console/Menus/RsaMenu.cs
using System;
using System.Numerics;
using CryptoExam.Core.RSA;
using CryptoExam.Core.NumberTheory;
using CryptoExam.Console.Helpers;

namespace CryptoExam.Console.Menus
{
    public static class RsaMenu
    {
        public static void Show()
        {
            while (true)
            {
                ConsoleOutput.PrintHeader("RSA / CHỮ KÝ SỐ");
                System.Console.WriteLine("  *** UEH/SLIDE CONVENTION ***");
                System.Console.WriteLine("  eA = private (dùng để KÝ), dA = eA^-1 mod phi = public (XÁC THỰC)");
                System.Console.WriteLine();
                System.Console.WriteLine("  1. Tính n, phi, d từ p, q, e");
                System.Console.WriteLine("  2. [UEH] Sign: S = M^eA mod n");
                System.Console.WriteLine("  3. [UEH] Verify: M' = S^dA mod n");
                System.Console.WriteLine("  4. [UEH] Sign + Verify danh sách M");
                System.Console.WriteLine("  5. [Standard] Encrypt: C = M^e mod n");
                System.Console.WriteLine("  6. [Standard] Decrypt: M = C^d mod n");
                System.Console.WriteLine("  7. [Standard] Sign: S = M^d mod n");
                System.Console.WriteLine("  8. [Standard] Verify: M' = S^e mod n");
                System.Console.WriteLine("  9. Full RSA Walkthrough");
                System.Console.WriteLine("  0. Quay lại");

                int ch = ConsoleInput.ReadMenuChoice("\n  Chọn: ", 9);
                if (ch == 0) break;

                switch (ch)
                {
                    case 1: CalcKeysMenu(); break;
                    case 2: UehSignMenu(); break;
                    case 3: UehVerifyMenu(); break;
                    case 4: UehBatchMenu(); break;
                    case 5: StdEncryptMenu(); break;
                    case 6: StdDecryptMenu(); break;
                    case 7: StdSignMenu(); break;
                    case 8: StdVerifyMenu(); break;
                    case 9: FullWalkthroughMenu(); break;
                }
            }
        }

        // ==================== CALC KEYS ====================
        static void CalcKeysMenu()
        {
            ConsoleOutput.PrintHeader("RSA KEY CALCULATION");
            BigInteger p = ConsoleInput.ReadBigInt("  p = ");
            BigInteger q = ConsoleInput.ReadBigInt("  q = ");
            BigInteger e = ConsoleInput.ReadBigInt("  e (UEH: eA = private signing key) = ");

            try
            {
                var kp = new RsaKeyPair(p, q, e);
                System.Console.WriteLine();
                ConsoleOutput.PrintKV("n = p*q", kp.N.ToString());
                ConsoleOutput.PrintKV("phi(n) = (p-1)(q-1)", kp.Phi.ToString());
                ConsoleOutput.PrintKV($"gcd(e,phi)", kp.GcdEPhi.ToString());
                ConsoleOutput.PrintKV("d = e^-1 mod phi", kp.D.ToString());
                System.Console.WriteLine();
                ConsoleOutput.PrintInfo("[UEH slide] eA = " + kp.E + " (private), dA = " + kp.D + " (public)");
                ConsoleOutput.PrintInfo("[Standard]  e  = " + kp.E + " (public),  d  = " + kp.D + " (private)");
            }
            catch (Exception ex) { ConsoleOutput.PrintError(ex.Message); }
            ConsoleInput.PressEnterToContinue();
        }

        // ==================== UEH SIGN ====================
        static void UehSignMenu()
        {
            ConsoleOutput.PrintHeader("[UEH] SIGN: S = M^eA mod n");
            BigInteger eA = ConsoleInput.ReadBigInt("  eA (private, từ slide) = ");
            BigInteger n = ConsoleInput.ReadBigInt("  n = ");
            BigInteger m = ConsoleInput.ReadBigInt("  M = ");

            BigInteger s = RsaSignatureService.UehSign(m, eA, n);
            System.Console.WriteLine();
            ConsoleOutput.PrintKV("M", m.ToString());
            ConsoleOutput.PrintKV("S = M^eA mod n", s.ToString());
            ConsoleInput.PressEnterToContinue();
        }

        // ==================== UEH VERIFY ====================
        static void UehVerifyMenu()
        {
            ConsoleOutput.PrintHeader("[UEH] VERIFY: M' = S^dA mod n");
            BigInteger dA = ConsoleInput.ReadBigInt("  dA (public, từ slide) = ");
            BigInteger n = ConsoleInput.ReadBigInt("  n = ");
            BigInteger s = ConsoleInput.ReadBigInt("  S (signature) = ");

            BigInteger mPrime = RsaSignatureService.UehVerify(s, dA, n);
            System.Console.WriteLine();
            ConsoleOutput.PrintKV("S", s.ToString());
            ConsoleOutput.PrintKV("M' = S^dA mod n", mPrime.ToString());
            ConsoleInput.PressEnterToContinue();
        }

        // ==================== UEH BATCH ====================
        static void UehBatchMenu()
        {
            ConsoleOutput.PrintHeader("[UEH] SIGN + VERIFY DANH SÁCH M");
            BigInteger p = ConsoleInput.ReadBigInt("  p = ");
            BigInteger q = ConsoleInput.ReadBigInt("  q = ");
            BigInteger eA = ConsoleInput.ReadBigInt("  eA = ");

            try
            {
                var kp = new RsaKeyPair(p, q, eA);
                System.Console.WriteLine();
                ConsoleOutput.PrintKV("n",   kp.N.ToString());
                ConsoleOutput.PrintKV("phi", kp.Phi.ToString());
                ConsoleOutput.PrintKV("dA",  kp.D.ToString());

                var messages = ConsoleInput.ReadBigIntList("\n  Nhập danh sách M (cách bằng dấu phẩy hoặc space): ");

                System.Console.WriteLine();
                for (int i = 0; i < messages.Length; i++)
                {
                    BigInteger m = messages[i];
                    BigInteger s = RsaSignatureService.UehSign(m, kp.E, kp.N);
                    BigInteger verified = RsaSignatureService.UehVerify(s, kp.D, kp.N);

                    System.Console.WriteLine($"  M{i + 1} = {m}");
                    System.Console.WriteLine($"  S{i + 1} = M^eA mod n = {s}");
                    System.Console.WriteLine($"  Verify{i + 1} = S^dA mod n = {verified}  [{(m == verified ? "OK" : "FAIL")}]");
                    System.Console.WriteLine();
                }
            }
            catch (Exception ex) { ConsoleOutput.PrintError(ex.Message); }
            ConsoleInput.PressEnterToContinue();
        }

        // ==================== STANDARD ENCRYPT ====================
        static void StdEncryptMenu()
        {
            ConsoleOutput.PrintHeader("[Standard] ENCRYPT: C = M^e mod n");
            BigInteger e = ConsoleInput.ReadBigInt("  e (public key) = ");
            BigInteger n = ConsoleInput.ReadBigInt("  n = ");
            BigInteger m = ConsoleInput.ReadBigInt("  M = ");
            BigInteger c = RsaService.Encrypt(m, e, n);
            System.Console.WriteLine();
            ConsoleOutput.PrintKV("C = M^e mod n", c.ToString());
            ConsoleInput.PressEnterToContinue();
        }

        // ==================== STANDARD DECRYPT ====================
        static void StdDecryptMenu()
        {
            ConsoleOutput.PrintHeader("[Standard] DECRYPT: M = C^d mod n");
            BigInteger d = ConsoleInput.ReadBigInt("  d (private key) = ");
            BigInteger n = ConsoleInput.ReadBigInt("  n = ");
            BigInteger c = ConsoleInput.ReadBigInt("  C = ");
            BigInteger m = RsaService.Decrypt(c, d, n);
            System.Console.WriteLine();
            ConsoleOutput.PrintKV("M = C^d mod n", m.ToString());
            ConsoleInput.PressEnterToContinue();
        }

        // ==================== STANDARD SIGN ====================
        static void StdSignMenu()
        {
            ConsoleOutput.PrintHeader("[Standard] SIGN: S = M^d mod n");
            BigInteger d = ConsoleInput.ReadBigInt("  d (private key) = ");
            BigInteger n = ConsoleInput.ReadBigInt("  n = ");
            BigInteger m = ConsoleInput.ReadBigInt("  M = ");
            BigInteger s = RsaService.Sign(m, d, n);
            System.Console.WriteLine();
            ConsoleOutput.PrintKV("S = M^d mod n", s.ToString());
            ConsoleInput.PressEnterToContinue();
        }

        // ==================== STANDARD VERIFY ====================
        static void StdVerifyMenu()
        {
            ConsoleOutput.PrintHeader("[Standard] VERIFY: M' = S^e mod n");
            BigInteger e = ConsoleInput.ReadBigInt("  e (public key) = ");
            BigInteger n = ConsoleInput.ReadBigInt("  n = ");
            BigInteger s = ConsoleInput.ReadBigInt("  S (signature) = ");
            BigInteger mPrime = RsaService.Verify(s, e, n);
            System.Console.WriteLine();
            ConsoleOutput.PrintKV("M' = S^e mod n", mPrime.ToString());
            ConsoleInput.PressEnterToContinue();
        }

        // ==================== FULL WALKTHROUGH ====================
        static void FullWalkthroughMenu()
        {
            ConsoleOutput.PrintHeader("FULL RSA WALKTHROUGH");
            System.Console.WriteLine("  Nhập p, q, e và message, chương trình tự tính tất cả.");

            BigInteger p = ConsoleInput.ReadBigInt("  p = ");
            BigInteger q = ConsoleInput.ReadBigInt("  q = ");
            BigInteger e = ConsoleInput.ReadBigInt("  e (UEH: eA) = ");
            BigInteger m = ConsoleInput.ReadBigInt("  Message M = ");

            try
            {
                var kp = new RsaKeyPair(p, q, e);
                BigInteger n = kp.N, phi = kp.Phi, d = kp.D;

                System.Console.WriteLine();
                ConsoleOutput.PrintSubHeader("Key Generation");
                ConsoleOutput.PrintKV("n = p*q", n.ToString());
                ConsoleOutput.PrintKV("phi(n)", phi.ToString());
                ConsoleOutput.PrintKV("gcd(e,phi)", kp.GcdEPhi.ToString());
                ConsoleOutput.PrintKV("d = e^-1 mod phi", d.ToString());

                System.Console.WriteLine();
                ConsoleOutput.PrintSubHeader("[UEH Mode] Sign & Verify");
                BigInteger sUeh = RsaSignatureService.UehSign(m, e, n);
                BigInteger vUeh = RsaSignatureService.UehVerify(sUeh, d, n);
                ConsoleOutput.PrintKV("M", m.ToString());
                ConsoleOutput.PrintKV("S = M^eA mod n", sUeh.ToString());
                ConsoleOutput.PrintKV("Verify = S^dA mod n", vUeh.ToString());
                ConsoleOutput.PrintKV("Match?", (m == vUeh ? "YES" : "NO"));

                System.Console.WriteLine();
                ConsoleOutput.PrintSubHeader("[Standard Mode] Encrypt & Decrypt");
                BigInteger cStd = RsaService.Encrypt(m, e, n);
                BigInteger mStd = RsaService.Decrypt(cStd, d, n);
                ConsoleOutput.PrintKV("C = M^e mod n (public)", cStd.ToString());
                ConsoleOutput.PrintKV("M = C^d mod n (private)", mStd.ToString());
            }
            catch (Exception ex) { ConsoleOutput.PrintError(ex.Message); }
            ConsoleInput.PressEnterToContinue();
        }
    }
}

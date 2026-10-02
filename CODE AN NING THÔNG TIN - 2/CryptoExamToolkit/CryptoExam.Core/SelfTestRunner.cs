// File: CryptoExam.Core/SelfTestRunner.cs
using System;
using System.Numerics;
using CryptoExam.Core.Classical;
using CryptoExam.Core.DES;
using CryptoExam.Core.NumberTheory;
using CryptoExam.Core.RSA;
using CryptoExam.Core.Common;

namespace CryptoExam.Core
{
    public static class SelfTestRunner
    {
        private static int _passed = 0;
        private static int _failed = 0;

        public static void RunAllTests()
        {
            _passed = 0; _failed = 0;

            Console.WriteLine("==============================================");
            Console.WriteLine("  SELF TEST - CRYPTO EXAM TOOLKIT");
            Console.WriteLine("==============================================");

            TestCaesar();
            TestVigenere();
            TestPlayfair();
            TestOtp();
            TestRailFence();
            TestDesKeySchedule();
            TestDesEncryptDecrypt();
            TestDesRound1();
            TestEuclid();
            TestModPow();
            TestRsaUeh();

            Console.WriteLine("----------------------------------------------");
            Console.WriteLine($"  Total: {_passed + _failed} | PASS: {_passed} | FAIL: {_failed}");
            if (_failed == 0)
                Console.WriteLine("  ✓ TẤT CẢ TEST PASS - Sẵn sàng thi!");
            else
                Console.WriteLine($"  ✗ CÓ {_failed} TEST FAIL - Kiểm tra lại code!");
            Console.WriteLine("==============================================");
        }

        private static void Assert(string name, string expected, string actual)
        {
            if (expected == actual)
            {
                Console.WriteLine($"  [PASS] {name}");
                _passed++;
            }
            else
            {
                Console.WriteLine($"  [FAIL] {name}");
                Console.WriteLine($"         Expected : {expected}");
                Console.WriteLine($"         Actual   : {actual}");
                _failed++;
            }
        }

        // ============== CLASSICAL ==============

        static void TestCaesar()
        {
            Console.WriteLine("\n--- Caesar Cipher ---");
            Assert("Caesar Encrypt HELLO,3", "KHOOR", CaesarCipher.Encrypt("HELLO", 3));
            Assert("Caesar Decrypt KHOOR,3", "HELLO", CaesarCipher.Decrypt("KHOOR", 3));
        }

        static void TestVigenere()
        {
            Console.WriteLine("\n--- Vigenere Cipher ---");
            var (ct, _) = VigenereCipher.Encrypt("ATTACK", "LEMON");
            Assert("Vigenere Encrypt ATTACK+LEMON", "LXFOPV", ct);

            var (pt, _) = VigenereCipher.Decrypt("LXFOPVEFRNHR", "LEMON");
            Assert("Vigenere Decrypt LXFOPVEFRNHR+LEMON", "ATTACKATDAWN", pt);
        }

        static void TestPlayfair()
        {
            Console.WriteLine("\n--- Playfair Cipher ---");
            var pf = new PlayfairCipher("MONARCHY");
            Assert("Playfair Encrypt BALLOON+MONARCHY", "IBSUPMNA", pf.Encrypt("BALLOON"));
        }

        static void TestOtp()
        {
            Console.WriteLine("\n--- One-Time Pad ---");
            Assert("OTP Encrypt DATA+XMCK", "AMVK", OneTimePadCipher.Encrypt("DATA", "XMCK"));
            Assert("OTP Decrypt AMVK+XMCK", "DATA", OneTimePadCipher.Decrypt("AMVK", "XMCK"));
        }

        static void TestRailFence()
        {
            Console.WriteLine("\n--- Rail Fence ---");
            Assert("Rail Fence Encrypt SECURITY,3", "SREUIYCT", RailFenceCipher.EncryptZigZag("SECURITY", 3));
            Assert("Rail Fence Decrypt SREUIYCT,3", "SECURITY", RailFenceCipher.DecryptZigZag("SREUIYCT", 3));
        }

        // ============== DES ==============

        static void TestDesKeySchedule()
        {
            Console.WriteLine("\n--- DES Key Schedule ---");
            var ks = DesKeySchedule.Generate("133457799BBCDFF1");

            Assert("DES K1",  "1B02EFFC7072", ks.Rounds[0].SubKeyHex);
            Assert("DES K2",  "79AED9DBC9E5", ks.Rounds[1].SubKeyHex);
            Assert("DES K3",  "55FC8A42CF99", ks.Rounds[2].SubKeyHex);
            Assert("DES K4",  "72ADD6DB351D", ks.Rounds[3].SubKeyHex);
            Assert("DES K5",  "7CEC07EB53A8", ks.Rounds[4].SubKeyHex);
            Assert("DES K6",  "63A53E507B2F", ks.Rounds[5].SubKeyHex);
            Assert("DES K9",  "E0DBEBEDE781", ks.Rounds[8].SubKeyHex);
            Assert("DES K16", "CB3D8B0E17F5", ks.Rounds[15].SubKeyHex);

            // C3, D3 (28-bit hex = 7 ký tự)
            Assert("DES C3", "0CCAAFF", ks.Rounds[2].CHex);
            Assert("DES D3", "56678F5", ks.Rounds[2].DHex);

            // C4D4 (56-bit hex = 14 ký tự)
            Assert("DES C4D4", "332ABFC599E3D5", ks.Rounds[3].CDHex);
        }

        static void TestDesEncryptDecrypt()
        {
            Console.WriteLine("\n--- DES Encrypt/Decrypt ---");
            var t1 = DesCipher.EncryptTrace("0123456789ABCDEF", "133457799BBCDFF1");
            Assert("DES IP(M)", "CC00CCFFF0AAF0AA", t1.IpHex);
            Assert("DES L0",   "CC00CCFF",          t1.L0Hex);
            Assert("DES R0",   "F0AAF0AA",           t1.R0Hex);
            Assert("DES Full Encrypt", "85E813540F0AB405", t1.OutputHex);

            var t2 = DesCipher.DecryptTrace("85E813540F0AB405", "133457799BBCDFF1");
            Assert("DES Full Decrypt", "0123456789ABCDEF", t2.OutputHex);
        }

        static void TestDesRound1()
        {
            Console.WriteLine("\n--- DES Round 1 Detail ---");
            var t = DesCipher.EncryptTrace("0123456789ABCDEF", "133457799BBCDFF1");
            var r1 = t.Rounds[0];
            Assert("DES E(R0)",       "7A15557A1555", r1.ExpandedRHex);
            Assert("DES E(R0) XOR K1","6117BA866527", r1.XorResultHex);
            Assert("DES S-Box R1",    "5C82B597",     r1.SBoxResultHex);
            Assert("DES P R1",        "234AA9BB",     r1.PResultHex);
            Assert("DES L1",          "F0AAF0AA",     r1.LHex);
            Assert("DES R1",          "EF4A6544",     r1.RHex);
        }

        // ============== NUMBER THEORY ==============

        static void TestEuclid()
        {
            Console.WriteLine("\n--- Extended Euclid ---");
            Assert("inverse(550,1759)", "355",  ExtendedEuclidService.GetInverse(550, 1759).ToString());
            Assert("inverse(7,192)",    "55",   ExtendedEuclidService.GetInverse(7, 192).ToString());
            Assert("inverse(17,3120)",  "2753", ExtendedEuclidService.GetInverse(17, 3120).ToString());
            Assert("inverse(53,3120)",  "2237", ExtendedEuclidService.GetInverse(53, 3120).ToString());
        }

        static void TestModPow()
        {
            Console.WriteLine("\n--- Modular Exponentiation ---");
            Assert("64^7 mod 221",  "38",  ModularArithmetic.ModPow(64, 7, 221).ToString());
            Assert("38^55 mod 221", "64",  ModularArithmetic.ModPow(38, 55, 221).ToString());
            Assert("112^7 mod 221", "5",   ModularArithmetic.ModPow(112, 7, 221).ToString());
            Assert("5^55 mod 221",  "112", ModularArithmetic.ModPow(5, 55, 221).ToString());
            Assert("97^7 mod 221",  "7",   ModularArithmetic.ModPow(97, 7, 221).ToString());
            Assert("7^55 mod 221",  "97",  ModularArithmetic.ModPow(7, 55, 221).ToString());
        }

        static void TestRsaUeh()
        {
            Console.WriteLine("\n--- RSA UEH Slide Mode ---");
            var kp = new RsaKeyPair(13, 17, 7);
            Assert("RSA n",   "221", kp.N.ToString());
            Assert("RSA phi", "192", kp.Phi.ToString());
            Assert("RSA d",   "55",  kp.D.ToString()); // dA = eA^-1 mod phi

            // Sign M^eA mod n, Verify S^dA mod n
            BigInteger n = kp.N, eA = kp.E, dA = kp.D;
            Assert("UEH Sign M=64,   S=38",  "38",  RsaSignatureService.UehSign(64,  eA, n).ToString());
            Assert("UEH Verify S=38, M=64",  "64",  RsaSignatureService.UehVerify(38, dA, n).ToString());
            Assert("UEH Sign M=112,  S=5",   "5",   RsaSignatureService.UehSign(112, eA, n).ToString());
            Assert("UEH Verify S=5,  M=112", "112", RsaSignatureService.UehVerify(5,  dA, n).ToString());
            Assert("UEH Sign M=97,   S=7",   "7",   RsaSignatureService.UehSign(97,  eA, n).ToString());
            Assert("UEH Verify S=7,  M=97",  "97",  RsaSignatureService.UehVerify(7,  dA, n).ToString());
        }
    }
}

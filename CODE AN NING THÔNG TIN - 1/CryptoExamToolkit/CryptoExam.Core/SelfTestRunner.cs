using System;
using System.Numerics;
using CryptoExam.Core.Classical;
using CryptoExam.Core.DES;
using CryptoExam.Core.NumberTheory;
using CryptoExam.Core.RSA;

namespace CryptoExam.Core
{
    public static class SelfTestRunner
    {
        public static void RunAllTests()
        {
            Console.WriteLine("=====================================");
            Console.WriteLine(" SELF TEST");
            Console.WriteLine("=====================================");

            int passed = 0, total = 0;

            void AssertEqual(string name, object expected, object actual)
            {
                total++;
                if (expected.ToString() == actual.ToString())
                {
                    Console.WriteLine($"[PASS] {name}");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] {name}. Expected: {expected}, Actual: {actual}");
                }
            }

            // Caesar
            AssertEqual("Caesar Encrypt", "KHOOR", CaesarCipher.Encrypt("HELLO", 3));
            AssertEqual("Caesar Decrypt", "HELLO", CaesarCipher.Decrypt("KHOOR", 3));

            // Vigenere
            AssertEqual("Vigenere Encrypt", "LXFOPV", VigenereCipher.Encrypt("ATTACK", "LEMON"));
            
            // Playfair
            var pf = new PlayfairCipher("MONARCHY");
            AssertEqual("Playfair Encrypt", "IBSUPMNA", pf.Encrypt("BALLOON"));

            // OTP
            AssertEqual("OTP Encrypt", "AMVK", OneTimePadCipher.Encrypt("DATA", "XMCK"));

            // Rail Fence
            AssertEqual("Rail Fence Encrypt", "SREUIYCT", RailFenceCipher.EncryptZigZag("SECURITY", 3));

            // DES
            string desKey = "133457799BBCDFF1";
            string desPt = "0123456789ABCDEF";
            string expectedDesCt = "85E813540F0AB405";
            
            var t1 = DesCipher.EncryptTrace(desPt, desKey);
            AssertEqual("DES Full Encrypt", expectedDesCt, t1.CipherTextHex);
            
            var t2 = DesCipher.DecryptTrace(expectedDesCt, desKey);
            AssertEqual("DES Full Decrypt", desPt, t2.PlainTextHex);

            AssertEqual("DES K1", "1B02EFFC7072", t1.KeySchedule.Rounds[0].SubKeyHex);
            AssertEqual("DES K2", "79AED9DBC9E5", t1.KeySchedule.Rounds[1].SubKeyHex);
            AssertEqual("DES K3", "55FC8A42CF99", t1.KeySchedule.Rounds[2].SubKeyHex);
            AssertEqual("DES K4", "72ADD6DB351D", t1.KeySchedule.Rounds[3].SubKeyHex);
            
            AssertEqual("DES IP(M)", "CC00CCFFF0AAF0AA", t1.IpHex);
            AssertEqual("DES L0", "CC00CCFF", t1.L0Hex);
            AssertEqual("DES R0", "F0AAF0AA", t1.R0Hex);
            
            var r1 = t1.Rounds[0];
            AssertEqual("DES E(R0)", "7A15557A1555", r1.ExpandedRHex);
            AssertEqual("DES E(R0) XOR K1", "6117BA866527", r1.XorResultHex);
            AssertEqual("DES S-Box 1", "5C82B597", r1.SBoxResultHex);
            AssertEqual("DES P(S-Box 1)", "234AA9BB", r1.PResultHex);
            AssertEqual("DES L1", "F0AAF0AA", r1.LHex);
            AssertEqual("DES R1", "EF4A6544", r1.RHex);

            // Euclid
            AssertEqual("Euclid Inverse (550, 1759)", "355", ExtendedEuclidService.GetInverse(550, 1759));
            AssertEqual("Euclid Inverse (7, 192)", "55", ExtendedEuclidService.GetInverse(7, 192));

            // RSA UEH
            AssertEqual("RSA ModPow 1", "38", ModularArithmetic.ModPow(64, 7, 221));
            AssertEqual("RSA ModPow 2", "64", ModularArithmetic.ModPow(38, 55, 221));
            AssertEqual("RSA ModPow 3", "5", ModularArithmetic.ModPow(112, 7, 221));
            AssertEqual("RSA ModPow 4", "112", ModularArithmetic.ModPow(5, 55, 221));
            AssertEqual("RSA ModPow 5", "7", ModularArithmetic.ModPow(97, 7, 221));
            AssertEqual("RSA ModPow 6", "97", ModularArithmetic.ModPow(7, 55, 221));

            Console.WriteLine("-------------------------------------");
            Console.WriteLine($"Total: {total}, Passed: {passed}, Failed: {total - passed}");
            Console.WriteLine("=====================================");
            Console.WriteLine();
        }
    }
}

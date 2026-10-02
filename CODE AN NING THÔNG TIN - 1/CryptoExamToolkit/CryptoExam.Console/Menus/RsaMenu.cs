using System;
using System.Numerics;
using System.Collections.Generic;
using CryptoExam.Core.RSA;
using CryptoExam.ConsoleApp.Helpers;

namespace CryptoExam.ConsoleApp.Menus
{
    public static class RsaMenu
    {
        public static void Show()
        {
            while (true)
            {
                ConsoleOutput.PrintHeader("RSA & DIGITAL SIGNATURE");
                Console.WriteLine("1. Calculate Keys (n, phi, d) from p, q, e");
                Console.WriteLine("2. Full RSA Walkthrough");
                Console.WriteLine("--- UEH LECTURER SLIDE MODE ---");
                Console.WriteLine("3. Sign (UEH Mode)");
                Console.WriteLine("4. Verify (UEH Mode)");
                Console.WriteLine("--- STANDARD MODE ---");
                Console.WriteLine("5. Encrypt (Standard)");
                Console.WriteLine("6. Decrypt (Standard)");
                Console.WriteLine("7. Sign (Standard)");
                Console.WriteLine("8. Verify (Standard)");
                Console.WriteLine("0. Back");

                int choice = ConsoleInput.ReadInt("Choice");
                if (choice == 0) return;

                switch (choice)
                {
                    case 1: CalcKeysMenu(); break;
                    case 2: WalkthroughMenu(); break;
                    case 3: UehSignMenu(); break;
                    case 4: UehVerifyMenu(); break;
                    case 5: StdEncryptMenu(); break;
                    case 6: StdDecryptMenu(); break;
                    case 7: StdSignMenu(); break;
                    case 8: StdVerifyMenu(); break;
                }
            }
        }

        private static void CalcKeysMenu()
        {
            BigInteger p = ConsoleInput.ReadBigInteger("p");
            BigInteger q = ConsoleInput.ReadBigInteger("q");
            BigInteger e = ConsoleInput.ReadBigInteger("e");
            
            var keys = RsaService.GenerateKeys(p, q, e);
            Console.WriteLine();
            ConsoleOutput.PrintLabel("n (p*q)", keys.N);
            ConsoleOutput.PrintLabel("phi(n)", keys.Phi);
            ConsoleOutput.PrintLabel("gcd(e, phi)", keys.Gcd);
            ConsoleOutput.PrintLabel("d (e^-1 mod phi)", keys.D);
            ConsoleOutput.WaitForKey();
        }

        private static void WalkthroughMenu()
        {
            BigInteger p = ConsoleInput.ReadBigInteger("p");
            BigInteger q = ConsoleInput.ReadBigInteger("q");
            BigInteger e = ConsoleInput.ReadBigInteger("e");
            BigInteger m = ConsoleInput.ReadBigInteger("Message (M)");
            
            var keys = RsaService.GenerateKeys(p, q, e);
            Console.WriteLine("\n[KEYS]");
            ConsoleOutput.PrintLabel("n", keys.N);
            ConsoleOutput.PrintLabel("phi", keys.Phi);
            ConsoleOutput.PrintLabel("d", keys.D);
            
            Console.WriteLine("\n[UEH SLIDE MODE: e=Private, d=Public]");
            BigInteger uehSig = RsaService.SignUeh(m, keys.E, keys.N);
            ConsoleOutput.PrintLabel($"Sign M={m}", $"{m}^{keys.E} mod {keys.N} = {uehSig}");
            BigInteger uehVer = RsaService.VerifyUeh(uehSig, keys.D, keys.N);
            ConsoleOutput.PrintLabel($"Verify S={uehSig}", $"{uehSig}^{keys.D} mod {keys.N} = {uehVer}");
            
            Console.WriteLine("\n[STANDARD MODE: e=Public, d=Private]");
            BigInteger stdEnc = RsaService.EncryptStandard(m, keys.E, keys.N);
            ConsoleOutput.PrintLabel($"Encrypt M={m}", $"{m}^{keys.E} mod {keys.N} = {stdEnc}");
            BigInteger stdDec = RsaService.DecryptStandard(stdEnc, keys.D, keys.N);
            ConsoleOutput.PrintLabel($"Decrypt C={stdEnc}", $"{stdEnc}^{keys.D} mod {keys.N} = {stdDec}");
            
            BigInteger stdSig = RsaService.SignStandard(m, keys.D, keys.N);
            ConsoleOutput.PrintLabel($"Sign M={m}", $"{m}^{keys.D} mod {keys.N} = {stdSig}");
            BigInteger stdVer = RsaService.VerifyStandard(stdSig, keys.E, keys.N);
            ConsoleOutput.PrintLabel($"Verify S={stdSig}", $"{stdSig}^{keys.E} mod {keys.N} = {stdVer}");

            ConsoleOutput.WaitForKey();
        }

        private static void UehSignMenu()
        {
            Console.WriteLine("\nUEH Sign: s = m^eA mod n");
            BigInteger eA = ConsoleInput.ReadBigInteger("eA (Private Key)");
            BigInteger n = ConsoleInput.ReadBigInteger("n");
            List<BigInteger> ms = ConsoleInput.ReadBigIntegerList("Message(s)");
            
            for (int i = 0; i < ms.Count; i++)
            {
                BigInteger m = ms[i];
                BigInteger s = RsaService.SignUeh(m, eA, n);
                Console.WriteLine($"\nM{i+1}={m}");
                Console.WriteLine($"S{i+1}={s}");
                BigInteger v = RsaService.VerifyUeh(s, eA, n); // just to show if eA is used again? Wait, verify uses dA!
                // We can't auto-verify if we don't have dA.
            }
            ConsoleOutput.WaitForKey();
        }

        private static void UehVerifyMenu()
        {
            Console.WriteLine("\nUEH Verify: m' = s^dA mod n");
            BigInteger dA = ConsoleInput.ReadBigInteger("dA (Public Key)");
            BigInteger n = ConsoleInput.ReadBigInteger("n");
            List<BigInteger> ss = ConsoleInput.ReadBigIntegerList("Signature(s)");
            
            for (int i = 0; i < ss.Count; i++)
            {
                BigInteger s = ss[i];
                BigInteger m = RsaService.VerifyUeh(s, dA, n);
                Console.WriteLine($"\nS{i+1}={s}");
                Console.WriteLine($"Verify={m}");
            }
            ConsoleOutput.WaitForKey();
        }

        private static void StdEncryptMenu()
        {
            BigInteger e = ConsoleInput.ReadBigInteger("e (Public Key)");
            BigInteger n = ConsoleInput.ReadBigInteger("n");
            BigInteger m = ConsoleInput.ReadBigInteger("M");
            ConsoleOutput.PrintLabel("C", RsaService.EncryptStandard(m, e, n));
            ConsoleOutput.WaitForKey();
        }

        private static void StdDecryptMenu()
        {
            BigInteger d = ConsoleInput.ReadBigInteger("d (Private Key)");
            BigInteger n = ConsoleInput.ReadBigInteger("n");
            BigInteger c = ConsoleInput.ReadBigInteger("C");
            ConsoleOutput.PrintLabel("M", RsaService.DecryptStandard(c, d, n));
            ConsoleOutput.WaitForKey();
        }
        
        private static void StdSignMenu()
        {
            BigInteger d = ConsoleInput.ReadBigInteger("d (Private Key)");
            BigInteger n = ConsoleInput.ReadBigInteger("n");
            BigInteger m = ConsoleInput.ReadBigInteger("M");
            ConsoleOutput.PrintLabel("S", RsaService.SignStandard(m, d, n));
            ConsoleOutput.WaitForKey();
        }

        private static void StdVerifyMenu()
        {
            BigInteger e = ConsoleInput.ReadBigInteger("e (Public Key)");
            BigInteger n = ConsoleInput.ReadBigInteger("n");
            BigInteger s = ConsoleInput.ReadBigInteger("S");
            ConsoleOutput.PrintLabel("M'", RsaService.VerifyStandard(s, e, n));
            ConsoleOutput.WaitForKey();
        }
    }
}

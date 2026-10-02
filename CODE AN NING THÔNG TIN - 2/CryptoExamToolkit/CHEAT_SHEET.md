# CHEAT SHEET - CRYPTO EXAM TOOLKIT

## CACH SU DUNG TRONG PHONG THI

```
1. Mo Visual Studio
2. File > Open > Solution > chon CryptoExamToolkit.slnx
3. Set startup project = CryptoExam.Console
4. Nhan Ctrl+F5 (Run without debug)
5. Dung menu so de chon nhanh
```

---

## QUICK LOOKUP - DE HOI GI -> CHON MENU NAO

| De hoi | Menu | Sub-menu |
|--------|------|----------|
| DES: K3, K4, Ki | 2. DES | 1. Quick Query -> 7. K_i -> nhap i |
| DES: C3, D3, C_i, D_i | 2. DES | 1. Quick Query -> 4. C_i hoac 5. D_i |
| DES: C_iD_i | 2. DES | 1. Quick Query -> 6. C_iD_i |
| DES: PC1(K), C0, D0 | 2. DES | 1. Quick Query -> 1. PC1(K) |
| DES: IP(M) | 2. DES | 1. Quick Query -> 8. IP(M) |
| DES: L0, R0 | 2. DES | 1. Quick Query -> 9. L0 / 10. R0 |
| DES: E(R_i) | 2. DES | 1. Quick Query -> 11. E(R_i) -> nhap round |
| DES: E(R_i) XOR K_i | 2. DES | 1. Quick Query -> 12. XOR |
| DES: S-box output round i | 2. DES | 1. Quick Query -> 13. S-box |
| DES: P output round i | 2. DES | 1. Quick Query -> 14. P |
| DES: L_i, R_i | 2. DES | 1. Quick Query -> 15. L_i / 16. R_i |
| DES: R16L16 | 2. DES | 1. Quick Query -> 17. R16L16 |
| DES: Ciphertext | 2. DES | 1. Quick Query -> 18. Ciphertext |
| DES: Xem tat ca | 2. DES | 1. Quick Query -> 19. Show ALL |
| DES encrypt | 2. DES | 2. Full Encrypt |
| DES decrypt | 2. DES | 3. Full Decrypt |
| DES: tat ca subkey | 2. DES | 4. Key Schedule -> 1. Show all K1..K16 |
| Caesar encrypt/decrypt | 1. Classical | 1. Caesar |
| Caesar brute force | 1. Classical | 1. Caesar -> 3. Brute Force |
| Vigenere encrypt/decrypt | 1. Classical | 3. Vigenere |
| Playfair encrypt/decrypt | 1. Classical | 5. Playfair |
| Playfair 5x5 matrix | 1. Classical | 5. Playfair -> 3. Show Matrix |
| OTP encrypt/decrypt | 1. Classical | 4. One-Time Pad |
| Rail Fence | 1. Classical | 6. Rail Fence Zigzag |
| Columnar Transposition | 1. Classical | 7. Columnar Transposition |
| Double Transposition | 1. Classical | 8. Double Transposition |
| GCD(a,b) | 5. So hoc | 1. GCD |
| a^-1 mod m | 5. So hoc | 2. Extended Euclid |
| Euclid tung buoc | 5. So hoc | 2. Extended Euclid -> Y xem steps |
| base^exp mod m | 5. So hoc | 3. Modular Exponentiation |
| Square-and-Multiply | 5. So hoc | 3. Modular Exp -> Y xem steps |
| RSA: tinh n, phi, d | 4. RSA | 1. Tinh n, phi, d |
| RSA UEH: ky M=64 | 4. RSA | 2. [UEH] Sign |
| RSA UEH: xac thuc S | 4. RSA | 3. [UEH] Verify |
| RSA UEH: ky danh sach M | 4. RSA | 4. [UEH] Batch Sign+Verify |
| RSA Standard encrypt | 4. RSA | 5. [Standard] Encrypt |
| RSA Standard decrypt | 4. RSA | 6. [Standard] Decrypt |
| RSA Walkthrough | 4. RSA | 9. Full Walkthrough |
| AES encrypt | 3. AES | 1. AES-128 Encrypt Trace |
| AES round detail | 3. AES | 4. Xem round detail |
| Double DES | 6. Triple DES | 1. Double DES Encrypt |
| Triple DES EDE | 6. Triple DES | 3. 3DES EDE 2-key |
| Self test | 7. Self Test | (auto run) |

---

## KEY CONVENTIONS

### RSA - UEH Slide vs Standard

| | UEH Slide | Standard |
|-|-----------|----------|
| Private (ky/giai ma) | eA | d |
| Public (xac thuc/ma hoa) | dA = eA^-1 mod phi | e |
| Sign | S = M^eA mod n | S = M^d mod n |
| Verify | M' = S^dA mod n | M' = S^e mod n |

### DES - Decrypt dung subkey nguoc
- Encrypt: K1, K2, ..., K16
- Decrypt: K16, K15, ..., K1 (chuong trinh tu xu ly)

### OTP - Key phai cung do dai
- Chuong trinh bao loi neu key ngan hon message

---

## INPUT FORMAT DUOC CHAP NHAN

| Loai | Vi du |
|------|-------|
| Hex key | 133457799BBCDFF1 |
| Hex co khoang | 1334 5779 9BBC DFF1 |
| Hex co 0x | 0x133457799BBCDFF1 |
| Hex hoa/thuong | 133457799bbcdff1 |
| Danh sach so | 64 112 97 hoac 64,112,97 |

---

## TEST VECTORS DA VERIFIED (50/50 PASS)

Caesar:   HELLO + k=3 -> KHOOR
Vigenere: ATTACK + LEMON -> LXFOPV
Playfair: BALLOON + MONARCHY -> IBSUPMNA
OTP:      DATA + XMCK -> AMVK
Rail Fence: SECURITY, 3 rails -> SREUIYCT

DES Key:  133457799BBCDFF1
DES PT:   0123456789ABCDEF
DES CT:   85E813540F0AB405

K1=1B02EFFC7072  K2=79AED9DBC9E5
K3=55FC8A42CF99  K4=72ADD6DB351D
K9=E0DBEBEDE781  K16=CB3D8B0E17F5

C3=0CCAAFF  D3=56678F5  C4D4=332ABFC599E3D5

IP(M)=CC00CCFFF0AAF0AA  L0=CC00CCFF  R0=F0AAF0AA
E(R0)=7A15557A1555  XOR=6117BA866527
S-Box=5C82B597  P=234AA9BB  L1=F0AAF0AA  R1=EF4A6544

550^-1 mod 1759 = 355    7^-1 mod 192 = 55
17^-1 mod 3120 = 2753    53^-1 mod 3120 = 2237

RSA: p=13, q=17, eA=7 -> n=221, phi=192, dA=55
M=64  -> S=38  -> Verify=64
M=112 -> S=5   -> Verify=112
M=97  -> S=7   -> Verify=97

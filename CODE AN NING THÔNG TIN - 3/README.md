# Crypto Exam Toolkit

Bộ công cụ Console .NET 8 dành cho bài tập giữa kỳ An toàn thông tin. Mọi dữ liệu được nhập từ Console; không cần sửa source code theo từng đề.

## Mở và chạy trong Visual Studio

1. Mở `CryptoExamToolkit.sln` bằng Visual Studio 2022.
2. Trong Solution Explorer, nhấp phải `CryptoExam.Console` → **Set as Startup Project**.
3. Nhấn **Ctrl+F5** để chạy không debug.
4. Tại Main Menu, nhập số của nhóm bài. Sau mỗi phép tính, nhấn Enter để quay lại menu.
5. Trước khi thi, chọn mục **7. Run Self-Test** ở Main Menu. Chỉ sử dụng khi tất cả đều `[PASS]`.

Chạy bằng terminal (tùy chọn):

```powershell
dotnet run --project CryptoExam.Console
dotnet run --project CryptoExam.SelfTest
```

## Quy ước input nhanh

- Hex chấp nhận chữ hoa/thường, tiền tố `0x` và khoảng trắng: `0x1334 5779 9bbc dff1`.
- Danh sách RSA chấp nhận cả `64,112,97` và `64 112 97`.
- Classical cipher tự đổi chữ thường thành chữ hoa.
- OTP không lặp khóa; độ dài chữ cái của key phải đúng bằng message.
- DES Hex Block luôn cần đúng 16 ký tự hex; Ki cần 12, R cần 8.

## Cheat sheet menu

| Đề hỏi | Đường dẫn menu |
|---|---|
| Find DES K4 | `2 DES → 6 Quick Query → 7 K_i → round 4` |
| Find DES C3 / D3 / C3D3 | `2 → 6 → mục 4 / 5 / 6 → round 3` |
| Find IP(M), L0, R0 | `2 → 6 → mục 8 / 9 / 10` |
| Find E(R0), XOR, S-box, P round 1 | `2 → 6 → mục 11 / 12 / 13 / 14 → round 1` |
| Find DES R7 | `2 → 6 → 16 R_i → round 7` |
| Xem toàn bộ trace vòng DES 7 | `2 → 8 Show Specific Feistel Round → round 7` |
| DES encrypt/decrypt đầy đủ | `2 → 4 / 5` |
| Xem chi tiết từng S-box từ R, Ki | `2 → 3 Round Function` |
| Caesar brute force | `1 Classical → 1 Caesar → 3` |
| Vigenere decrypt | `1 → 3 Vigenere → 2` |
| Playfair matrix / pair | `1 → 5 Playfair → 1 / 5 / 6` |
| Rail Fence zigzag | `1 → 6 Rail Fence` |
| Columnar / Double Transposition | `1 → 7 / 8` |
| Find 550^-1 mod 1759 | `5 Number Theory → 2 Fast` |
| Xem bước Euclid | `5 → 3 Show Steps` |
| Tính a^b mod n | `5 → 4 Fast` hoặc `5 Show Steps` |
| RSA tính n, phi, d | `4 RSA → 1` |
| RSA ký M=64 theo slide UEH | `4 → 4 Sign UEH` |
| RSA ký/verify danh sách | `4 → 9` |
| RSA chuẩn encrypt/decrypt/sign/verify | `4 → 2 / 3 / 6 / 7` |
| RSA giải toàn bộ | `4 → 8 Walkthrough` |
| AES-128 state trung gian | `3 AES → 4 Quick Query` |
| AES-192/256 nhanh | `3 → 1 / 2` |
| Double DES / Triple DES EDE / EEE | `6 → chọn mode có label tương ứng` |

## Lưu ý ký hiệu RSA

- **UEH / Lecturer Slide Mode:** `eA` là khóa bí mật dùng ký; `dA` là khóa công khai dùng xác thực. `S=M^eA mod n`, `M'=S^dA mod n`.
- **Standard Mode:** public `(e,n)`, private `(d,n)`. Encrypt `M^e`, decrypt `C^d`, sign `M^d`, verify `S^e`.

Hai mode được ghi nhãn riêng trong menu để tránh nhầm.

## Cấu trúc

```text
CryptoExamToolkit.sln
global.json
README.md
CryptoExam.Core/
├── AES/
│   ├── AesKeyExpansion.cs
│   ├── AesService.cs
│   ├── AesTables.cs
│   ├── AesTraceResult.cs
│   └── AesTransformations.cs
├── Classical/
│   ├── CaesarCipher.cs
│   ├── ColumnarTranspositionCipher.cs
│   ├── DoubleTranspositionCipher.cs
│   ├── MatrixTranspositionCipher.cs
│   ├── MonoalphabeticCipher.cs
│   ├── OneTimePadCipher.cs
│   ├── PlayfairCipher.cs
│   ├── RailFenceCipher.cs
│   └── VigenereCipher.cs
├── Common/
│   ├── HexUtils.cs
│   └── TextUtils.cs
├── DES/
│   ├── DesAsciiService.cs
│   ├── DesBitUtils.cs
│   ├── DesCipher.cs
│   ├── DesKeyRoundResult.cs
│   ├── DesKeySchedule.cs
│   ├── DesRoundFunction.cs
│   ├── DesRoundResult.cs
│   ├── DesTables.cs
│   ├── DesTraceResult.cs
│   └── TripleDesService.cs
├── NumberTheory/
│   ├── ExtendedEuclidService.cs
│   ├── GcdService.cs
│   ├── ModularArithmetic.cs
│   └── PrimeUtils.cs
├── RSA/
│   ├── RsaKeyPair.cs
│   ├── RsaService.cs
│   └── RsaSignatureService.cs
└── SelfTest/
    └── SelfTestRunner.cs
CryptoExam.Console/
├── Helpers/
│   ├── ConsoleInput.cs
│   └── ConsoleOutput.cs
├── Menus/
│   ├── AesMenu.cs
│   ├── ClassicalMenu.cs
│   ├── DesMenu.cs
│   ├── MainMenu.cs
│   ├── NumberTheoryMenu.cs
│   ├── RsaMenu.cs
│   ├── SelfTestDisplay.cs
│   └── TripleDesMenu.cs
└── Program.cs
CryptoExam.SelfTest/
└── Program.cs
```

## Vector kiểm tra tiêu biểu

- DES `K=133457799BBCDFF1`, `P=0123456789ABCDEF` → `85E813540F0AB405`.
- AES-128 FIPS `P=00112233445566778899AABBCCDDEEFF`, `K=000102030405060708090A0B0C0D0E0F` → `69C4E0D86A7B0430D8CDB78070B4C55A`.
- Playfair `BALLOON`, key `MONARCHY` → `IBSUPMNA`.
- RSA UEH `p=13`, `q=17`, `eA=7`, `dA=55`: `64 → 38 → 64`.

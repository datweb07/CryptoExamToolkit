# Bộ công cụ ôn thi mật mã

Ứng dụng Console trên .NET 8 hỗ trợ ôn tập và tra cứu nhanh các chủ đề An toàn thông tin. Dữ liệu được nhập trực tiếp trong chương trình.

## Chạy chương trình

1. Mở `CryptoExamToolkit.sln` bằng Visual Studio 2022.
2. Chọn `CryptoExam.Console` làm dự án khởi động.
3. Nhấn **Ctrl+F5** để chạy.
4. Chọn mục **7. Chạy tự kiểm tra** trước khi sử dụng.

Chạy từ terminal:

```powershell
dotnet run --project CryptoExam.Console
dotnet run --project CryptoExam.SelfTest
```

## Quy ước nhập liệu

- Hex chấp nhận chữ hoa/thường, tiền tố `0x` và khoảng trắng, ví dụ `0x1334 5779 9bbc dff1`.
- Danh sách số RSA chấp nhận dấu phẩy hoặc khoảng trắng, ví dụ `64,112,97` hoặc `64 112 97`.
- Mật mã cổ điển tự chuyển chữ thường thành chữ hoa.
- One-Time Pad không lặp khóa; khóa phải có cùng số chữ cái với bản tin.
- DES xử lý khối hex cần 16 ký tự; khóa vòng K_i cần 12 ký tự và R cần 8 ký tự.

## Tra cứu nhanh

| Nội dung | Đường dẫn menu |
|---|---|
| Khóa vòng DES K4 | `2 → 6 → 7. K_i → vòng 4` |
| C3, D3, C3D3 của DES | `2 → 6 → mục 4 / 5 / 6 → vòng 3` |
| IP(M), L0, R0 | `2 → 6 → mục 8 / 9 / 10` |
| E(R0), XOR, S-box, P vòng 1 | `2 → 6 → mục 11 / 12 / 13 / 14 → vòng 1` |
| Dấu vết đầy đủ vòng DES 7 | `2 → 8 → vòng 7` |
| Mã hóa/giải mã DES đầy đủ | `2 → 4 / 5` |
| Chi tiết S-box từ R và K_i | `2 → 3` |
| Dò toàn bộ khóa Caesar | `1 → 1 → 3` |
| Giải mã Vigenère | `1 → 3 → 2` |
| Ma trận và cặp ký tự Playfair | `1 → 5 → 1 / 5 / 6` |
| Hoán vị Rail Fence | `1 → 6` |
| Hoán vị cột / hoán vị kép | `1 → 7 / 8` |
| Nghịch đảo 550 modulo 1759 | `5 → 2` |
| Các bước Euclid | `5 → 3` |
| Tính a^b mod n | `5 → 4 / 5` |
| Tính n, φ, d trong RSA | `4 → 1` |
| Ký RSA theo slide UEH | `4 → 4` |
| Ký/xác thực danh sách RSA | `4 → 9` |
| Mã hóa/giải mã/ký/xác thực RSA chuẩn | `4 → 2 / 3 / 6 / 7` |
| Quy trình RSA đầy đủ | `4 → 8` |
| Trạng thái trung gian AES-128 | `3 → 4` |
| Mã hóa/giải mã AES-192/256 nhanh | `3 → 1 / 2` |
| Double DES / Triple DES EDE / EEE | `6 → chọn chế độ tương ứng` |
| Chuyển đổi, máy tính, tiện ích thi | `8` |

## Quy ước chữ ký RSA

- **UEH / theo slide:** `eA` là khóa bí mật dùng để ký; `dA` là khóa công khai dùng để xác thực. Công thức: `S=M^eA mod n`, `M'=S^dA mod n`.
- **Chế độ chuẩn:** khóa công khai `(e,n)`, khóa bí mật `(d,n)`. Mã hóa `M^e`, giải mã `C^d`, ký `M^d`, xác thực `S^e`.

## Vectơ kiểm tra

- DES `K=133457799BBCDFF1`, `P=0123456789ABCDEF` → `85E813540F0AB405`.
- AES-128 FIPS `P=00112233445566778899AABBCCDDEEFF`, `K=000102030405060708090A0B0C0D0E0F` → `69C4E0D86A7B0430D8CDB78070B4C55A`.
- Playfair `BALLOON`, khóa `MONARCHY` → `IBSUPMNA`.
- RSA UEH `p=13`, `q=17`, `eA=7`, `dA=55`: `64 → 38 → 64`.

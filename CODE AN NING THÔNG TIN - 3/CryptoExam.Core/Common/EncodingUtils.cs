using System.Text;

namespace CryptoExam.Core.Common;

/// <summary>
/// Chuyen doi giua cac quy uoc ky hieu dung trong bai thi:
///   A=0..Z=25  (quy uoc alphabet pho bien trong mat ma co dien)
///   ASCII      (decimal cua ky tu, dung trong RSA tu van ban)
///   Digit 0-9 -> A-J (quy uoc ma so thanh chu cai)
///
/// BA QUY UOC NAY HOAN TOAN DOC LAP, KHONG DUOC TRON LAN.
/// </summary>
public static class EncodingUtils
{
    // =========================================================
    // A=0 .. Z=25
    // =========================================================

    /// <summary>Chu cai -> so (A=0, B=1, ..., Z=25). Chi chap nhan A-Z.</summary>
    public static int CharToA0Z25(char c)
    {
        c = char.ToUpperInvariant(c);
        if (c < 'A' || c > 'Z') throw new ArgumentOutOfRangeException(nameof(c), $"'{c}' khong phai A-Z.");
        return c - 'A';
    }

    /// <summary>So -> chu cai (0=A, 1=B, ..., 25=Z). Chi chap nhan 0-25.</summary>
    public static char A0Z25ToChar(int n)
    {
        if (n < 0 || n > 25) throw new ArgumentOutOfRangeException(nameof(n), $"{n} nam ngoai pham vi 0..25.");
        return (char)('A' + n);
    }

    /// <summary>Chuoi chu cai -> danh sach so A0Z25.</summary>
    public static int[] TextToA0Z25(string text)
    {
        text = text.ToUpperInvariant();
        var result = new List<int>();
        foreach (var c in text)
        {
            if (c < 'A' || c > 'Z') throw new ArgumentException($"Ky tu '{c}' khong phai A-Z.");
            result.Add(c - 'A');
        }
        return result.ToArray();
    }

    /// <summary>Danh sach so A0Z25 -> chuoi chu cai.</summary>
    public static string A0Z25ToText(IEnumerable<int> values)
    {
        var sb = new StringBuilder();
        foreach (var n in values) sb.Append(A0Z25ToChar(n));
        return sb.ToString();
    }

    // =========================================================
    // ASCII (decimal)
    // =========================================================

    /// <summary>Chu cai -> gia tri ASCII decimal.</summary>
    public static int CharToAscii(char c) => (int)c;

    /// <summary>ASCII decimal -> chu cai. Ho tro 0-127 (standard ASCII).</summary>
    public static char AsciiToChar(int code)
    {
        if (code < 0 || code > 127) throw new ArgumentOutOfRangeException(nameof(code), $"{code} nam ngoai pham vi ASCII 0..127.");
        return (char)code;
    }

    /// <summary>Chuoi -> danh sach ASCII decimal.</summary>
    public static int[] TextToAsciiList(string text) => text.Select(c => (int)c).ToArray();

    /// <summary>Danh sach ASCII decimal -> chuoi.</summary>
    public static string AsciiListToText(IEnumerable<int> codes)
    {
        var sb = new StringBuilder();
        foreach (var code in codes) sb.Append(AsciiToChar(code));
        return sb.ToString();
    }

    /// <summary>ASCII decimal -> chuoi binary 8 bit.</summary>
    public static string AsciiToBinary(int code)
    {
        if (code < 0 || code > 255) throw new ArgumentOutOfRangeException(nameof(code));
        return Convert.ToString(code, 2).PadLeft(8, '0');
    }

    /// <summary>ASCII decimal -> hex 2 ky tu.</summary>
    public static string AsciiToHex(int code)
    {
        if (code < 0 || code > 255) throw new ArgumentOutOfRangeException(nameof(code));
        return code.ToString("X2");
    }

    // =========================================================
    // Digit 0-9 -> A-J  (quy uoc rieng trong bai tap)
    // =========================================================

    /// <summary>Chu so '0'-'9' -> chu cai 'A'-'J'.</summary>
    public static char DigitToLetter(char digit)
    {
        if (digit < '0' || digit > '9') throw new ArgumentException($"'{digit}' khong phai chu so 0-9.");
        return (char)('A' + (digit - '0'));
    }

    /// <summary>Chu cai 'A'-'J' -> chu so '0'-'9'.</summary>
    public static char LetterToDigit(char letter)
    {
        letter = char.ToUpperInvariant(letter);
        if (letter < 'A' || letter > 'J') throw new ArgumentException($"'{letter}' khong phai A-J (chi dung A-J trong quy uoc nay).");
        return (char)('0' + (letter - 'A'));
    }

    /// <summary>Chuoi chi gom chu so -> chuoi chu cai theo quy uoc 0->A...9->J.</summary>
    public static string DigitStringToLetters(string digits)
    {
        var sb = new StringBuilder();
        foreach (var c in digits)
        {
            if (!char.IsDigit(c)) throw new ArgumentException($"Ky tu '{c}' khong phai chu so.");
            sb.Append(DigitToLetter(c));
        }
        return sb.ToString();
    }

    /// <summary>Chuoi chu cai A-J -> chuoi chu so theo quy uoc A->0...J->9.</summary>
    public static string LettersToDigitString(string letters)
    {
        letters = letters.ToUpperInvariant();
        var sb = new StringBuilder();
        foreach (var c in letters) sb.Append(LetterToDigit(c));
        return sb.ToString();
    }

    // =========================================================
    // Text Normalization (cho Classical Cipher)
    // =========================================================

    public enum NormMode
    {
        LettersOnly,        // Chi giu A-Z, bo so va ky tu dac biet
        PreserveDigits,     // Giu A-Z va 0-9 nhu nguyen
        DigitsToLetters,    // Chuyen 0-9 -> A-J, giu A-Z
        LettersToDigits     // Chuyen A-J -> 0-9, giu cac chu khac
    }

    public static string NormalizeText(string input, NormMode mode)
    {
        input = input.ToUpperInvariant();
        var sb = new StringBuilder();
        foreach (var c in input)
        {
            if (c >= 'A' && c <= 'Z') sb.Append(c);
            else if (c >= '0' && c <= '9')
            {
                switch (mode)
                {
                    case NormMode.PreserveDigits: sb.Append(c); break;
                    case NormMode.DigitsToLetters: sb.Append((char)('A' + (c - '0'))); break;
                }
            }
        }
        return sb.ToString();
    }

    // =========================================================
    // Monoalphabetic Key Generator
    // =========================================================

    /// <summary>
    /// Sinh bang chu cai thay the tu seed theo quy tac:
    /// 1. Uppercase
    /// 2. Chuyen digit 0-9 -> A-J
    /// 3. Loai duplicate (giu lan xuat hien dau tien)
    /// 4. Them cac chu chua xuat hien theo thu tu Z -> A
    /// </summary>
    public static (string NormalizedSeed, string UniqueSeed, string FinalAlphabet) GenerateMonoAlphabet(string seed)
    {
        seed = seed.ToUpperInvariant();
        var normalizedSb = new StringBuilder();
        foreach (var c in seed)
        {
            if (c >= 'A' && c <= 'Z') normalizedSb.Append(c);
            else if (c >= '0' && c <= '9') normalizedSb.Append((char)('A' + (c - '0')));
        }
        var normalized = normalizedSb.ToString();
        var seen = new HashSet<char>();
        var uniqueSb = new StringBuilder();
        foreach (var c in normalized)
        {
            if (seen.Add(c)) uniqueSb.Append(c);
        }
        var unique = uniqueSb.ToString();
        var remaining = new StringBuilder();
        for (var c = 'Z'; c >= 'A'; c--)
        {
            if (!seen.Contains(c)) remaining.Append(c);
        }
        var final = unique + remaining.ToString();
        if (final.Length != 26) throw new Exception("Loi logic: alphabet khong du 26 ky tu.");
        return (normalized, unique, final);
    }

    // =========================================================
    // Playfair Key Normalization Helper
    // =========================================================

    public static string NormalizePlayfairKey(string key)
    {
        key = key.ToUpperInvariant();
        var sb = new StringBuilder();
        foreach (var c in key)
        {
            if (c >= 'A' && c <= 'Z') sb.Append(c == 'J' ? 'I' : c);
            else if (c >= '0' && c <= '9') sb.Append((char)('A' + (c - '0')));
        }
        var seen = new HashSet<char>();
        var result = new StringBuilder();
        foreach (var c in sb.ToString())
        {
            if (seen.Add(c)) result.Append(c);
        }
        return result.ToString();
    }

    // =========================================================
    // Vigenere Key Normalization (digit -> letter)
    // =========================================================

    public static string NormalizeVigenereKey(string key)
    {
        key = key.ToUpperInvariant();
        var sb = new StringBuilder();
        foreach (var c in key)
        {
            if (c >= 'A' && c <= 'Z') sb.Append(c);
            else if (c >= '0' && c <= '9') sb.Append((char)('A' + (c - '0')));
        }
        return sb.ToString();
    }

    // =========================================================
    // Parse integer list from string (comma/space/semicolon)
    // =========================================================

    public static int[] ParseIntList(string input)
    {
        var parts = input.Split(new[] { ',', ';', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        return parts.Select(p =>
        {
            if (!int.TryParse(p, out var v)) throw new ArgumentException($"'{p}' khong phai so nguyen.");
            return v;
        }).ToArray();
    }
}

using System.Numerics;
using CryptoExam.Core.Common;

namespace CryptoExam.ConsoleApp.Helpers;

public static class ConsoleInput
{
    public static string Text(string label, bool allowEmpty = false)
    {
        while (true)
        {
            Console.Write(label);
            var value = (Console.ReadLine() ?? "").Trim();
            if (allowEmpty || value.Length > 0) return value;
            Error("Không được để trống.");
        }
    }

    public static int Integer(string label, int? min = null, int? max = null)
    {
        while (true)
        {
            if (int.TryParse(Text(label), out var value) && (!min.HasValue || value >= min) && (!max.HasValue || value <= max)) return value;
            Error($"Vui lòng nhập số nguyên{(min.HasValue ? $" >= {min}" : "")}{(max.HasValue ? $" <= {max}" : "")}.");
        }
    }

    public static BigInteger BigInt(string label)
    {
        while (true)
        {
            if (BigInteger.TryParse(Text(label), out var value)) return value;
            Error("Vui lòng nhập số nguyên hợp lệ.");
        }
    }

    public static string Hex(string label, int? digits = null)
    {
        while (true)
        {
            try { return HexUtils.Normalize(Text(label), digits); }
            catch (Exception ex) { Error(ex.Message); }
        }
    }

    public static bool YesNo(string label)
    {
        while (true)
        {
            var value = Text(label + " (Y/N): ").ToUpperInvariant();
            if (value is "Y" or "YES" or "C" or "CO") return true;
            if (value is "N" or "NO" or "K" or "KHONG") return false;
            Error("Nhập Y hoặc N.");
        }
    }

    public static IReadOnlyList<BigInteger> BigIntegerList(string label)
    {
        while (true)
        {
            var parts = Text(label).Split([',', ';', ' ', '\t'], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (parts.Length > 0 && parts.All(x => BigInteger.TryParse(x, out _))) return parts.Select(BigInteger.Parse).ToList();
            Error("Nhập danh sách số nguyên cách nhau bằng dấu phẩy hoặc khoảng trắng.");
        }
    }

    private static void Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("[INPUT ERROR] " + message);
        Console.ResetColor();
    }
}

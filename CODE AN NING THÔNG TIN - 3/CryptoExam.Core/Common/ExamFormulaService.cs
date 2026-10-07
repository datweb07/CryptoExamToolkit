namespace CryptoExam.Core.Common;

/// <summary>
/// Cong thuc tinh khoa nhanh theo ngay sinh dung trong de thi.
/// </summary>
public static class ExamFormulaService
{
    /// <summary>Cong thuc Caesar key: K = ((day * month) mod 26) + 1</summary>
    public static (int K, string Formula) CaesarKeyFromDate(int day, int month)
    {
        var product = day * month;
        var mod = product % 26;
        var k = mod + 1;
        var formula = $"K = (({day} x {month}) mod 26) + 1 = ({product} mod 26) + 1 = {mod} + 1 = {k}";
        return (k, formula);
    }

    /// <summary>Cong thuc Rail Fence rails: rails = (day mod 3) + 2</summary>
    public static (int Rails, string Formula) RailFenceFromDay(int day)
    {
        var mod = day % 3;
        var rails = mod + 2;
        var formula = $"rails = ({day} mod 3) + 2 = {mod} + 2 = {rails}";
        return (rails, formula);
    }

    /// <summary>Tinh a mod m</summary>
    public static long SimpleMod(long a, long m)
    {
        if (m <= 0) throw new ArgumentException("m phải > 0");
        return ((a % m) + m) % m;
    }

    /// <summary>Tinh (a * b) mod m</summary>
    public static long MulMod(long a, long b, long m)
    {
        if (m <= 0) throw new ArgumentException("m phải > 0");
        return (long)(((long)a % m * ((long)b % m)) % m + m) % m;
    }
}

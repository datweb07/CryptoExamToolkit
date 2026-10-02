using CryptoExam.Core.SelfTest;

namespace CryptoExam.ConsoleApp.Menus;

public static class SelfTestDisplay
{
    public static void Run()
    {
        Console.WriteLine("SELF TEST");
        var results = SelfTestRunner.RunAll();
        foreach (var test in results)
        {
            Console.ForegroundColor = test.Passed ? ConsoleColor.Green : ConsoleColor.Red;
            Console.WriteLine($"[{(test.Passed ? "PASS" : "FAIL")}] {test.Name}: {test.Detail}");
        }
        Console.ResetColor();
        Console.WriteLine($"\n{results.Count(x => x.Passed)}/{results.Count} tests passed.");
        if (results.Any(x => !x.Passed)) throw new InvalidOperationException("Self-test có lỗi; không nên dùng tool cho đến khi sửa.");
    }
}

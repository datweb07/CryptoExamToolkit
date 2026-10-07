using CryptoExam.Core.SelfTest;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("TỰ KIỂM TRA");
var results = SelfTestRunner.RunAll();
foreach (var test in results) Console.WriteLine($"[{(test.Passed ? "ĐẠT" : "KHÔNG ĐẠT")}] {test.Name}: {test.Detail}");
Console.WriteLine($"{results.Count(x => x.Passed)}/{results.Count} phép kiểm tra đã đạt.");
return results.All(x => x.Passed) ? 0 : 1;

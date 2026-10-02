using CryptoExam.Core.SelfTest;

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("SELF TEST");
var results = SelfTestRunner.RunAll();
foreach (var test in results) Console.WriteLine($"[{(test.Passed ? "PASS" : "FAIL")}] {test.Name}: {test.Detail}");
Console.WriteLine($"{results.Count(x => x.Passed)}/{results.Count} tests passed.");
return results.All(x => x.Passed) ? 0 : 1;

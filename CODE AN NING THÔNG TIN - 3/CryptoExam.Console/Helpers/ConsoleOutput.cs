namespace CryptoExam.ConsoleApp.Helpers;

public static class ConsoleOutput
{
    public static void Header(string title)
    {
        if (!Console.IsOutputRedirected)
        {
            try { Console.Clear(); }
            catch (IOException) { /* Một số terminal/runner không hỗ trợ Clear. */ }
        }
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(new string('=', 56));
        Console.WriteLine(" " + title);
        Console.WriteLine(new string('=', 56));
        Console.ResetColor();
    }

    public static void Result(string label, object? value) => Console.WriteLine($"{label,-18} = {value}");
    public static void Pause() { Console.WriteLine(); Console.Write("Nhấn Enter để quay lại menu..."); Console.ReadLine(); }

    public static void Guard(Action action)
    {
        try { action(); }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"LỖI: {ex.Message}");
            Console.ResetColor();
        }
        Pause();
    }
}

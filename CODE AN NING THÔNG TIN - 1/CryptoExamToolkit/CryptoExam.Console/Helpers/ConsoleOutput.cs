using System;

namespace CryptoExam.ConsoleApp.Helpers
{
    public static class ConsoleOutput
    {
        public static void PrintHeader(string title)
        {
            Console.WriteLine();
            Console.WriteLine(new string('=', 40));
            Console.WriteLine($" {title.ToUpper()}");
            Console.WriteLine(new string('=', 40));
        }

        public static void PrintLabel(string label, object value)
        {
            Console.WriteLine($"{label,-15} = {value}");
        }

        public static void PrintSuccess(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        
        public static void WaitForKey()
        {
            Console.WriteLine("\nPress Enter to return to the menu...");
            Console.ReadLine();
        }
    }
}

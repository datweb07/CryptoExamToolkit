using System;
using CryptoExam.ConsoleApp.Menus;

namespace CryptoExam.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Crypto Exam Toolkit - Antigravity";
            try
            {
                MainMenu.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred:");
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}

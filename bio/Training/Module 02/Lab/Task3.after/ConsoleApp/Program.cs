using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StringProcessor;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (string argument in args)
            {
                Console.WriteLine(argument);
            }

            Run();
        }

        private static void Run()
        {
            IStringProcessor processor = new ReverseProcessor();
            ConsoleColor originalColor = Console.ForegroundColor;
            try
            {
                while (true) 
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Enter Some Text: ");
                    Console.ForegroundColor = originalColor;

                    string input = Console.ReadLine();
                    if (string.IsNullOrEmpty(input))
                        return;

                    string result = processor.Process(input);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(result);
                }
            }
            finally
            {
                Console.ForegroundColor = originalColor;
            }
        }
    }
}

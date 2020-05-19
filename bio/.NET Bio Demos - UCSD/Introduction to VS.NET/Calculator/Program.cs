using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedContracts;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    RunCalculator(new ConsoleInputReader(), 
                        new MessageBoxOutputWriter());
                }
                catch (Exception ex)
                {


                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Failed, try again.");
                }
            }
        }

        private static void RunCalculator(IReader reader, IWriter writer)
        {
            //IReader reader = new ConsoleInputReader();
            //IWriter writer = new ConsoleOutputWriter();

            writer.WriteLine("Enter two numbers and an operation");

            int number1 = Int32.Parse(reader.ReadLine());
            int number2 = Int32.Parse(reader.ReadLine());
            string operation = reader.ReadLine();

            int result = 0;
            Calculator calc = new Calculator();

            switch (operation.ToUpper())
            {
                case "ADD":
                    result = calc.Add(number1, number2);
                    break;
                case "SUB":
                    result = calc.Subtract(number1, number2);
                    break;
                case "MUL":
                    result = calc.Multiply(number1, number2);
                    break;
                case "DIV":
                    result = calc.Divide(number1, number2);
                    break;
                default:
                    break;
            }

            writer.WriteLine(
                string.Format("{0} {1} {2} = {3}",
                              number1, operation, number2, result));
        }
    }
}

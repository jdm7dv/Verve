using System;
using SharedContracts;

namespace Calculator
{
    public class ConsoleOutputWriter : IWriter
    {
        public void WriteLine(string line)
        {
            Console.WriteLine(line);
        }
    }
}
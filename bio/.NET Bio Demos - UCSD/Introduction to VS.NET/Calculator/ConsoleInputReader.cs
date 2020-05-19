using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharedContracts;

namespace Calculator
{
    public class ConsoleInputReader : IReader
    {
        public string ReadLine()
        {
            return Console.ReadLine();
        }
    }
}

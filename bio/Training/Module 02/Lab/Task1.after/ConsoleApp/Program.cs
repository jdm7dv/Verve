using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        }
    }
}

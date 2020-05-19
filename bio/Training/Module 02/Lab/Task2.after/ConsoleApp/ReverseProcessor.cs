using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StringProcessor;

namespace ConsoleApp
{
    class ReverseProcessor : IStringProcessor
    {
        #region IStringProcessor Members

        public string Process(string input)
        {
            char[] charInput = input.ToCharArray();
            Array.Reverse(charInput);
            return new string(charInput);
        }

        #endregion
    }
}

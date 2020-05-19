using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StringProcessor;
using ConsoleApp;

namespace GuiApp
{
    class ReverseWords : IStringProcessor
    {
        #region IStringProcessor Members

        public string Process(string input)
        {
            // We will use the original reverse processor
            ReverseProcessor reverser = new ReverseProcessor();

            // Split the string into words using space as the delimiter
            string[] words = input.Split(' ');
            
            // Go through the words and run each throught the processor.
            // We use a for() loop because foreach() cannot modify the elements.
            for (int i = 0; i < words.Length; i++)
                words[i] = reverser.Process(words[i]);

            // Turn it back into a single string.
            return String.Join(" ", words);
        }

        #endregion

        public override string ToString()
        {
            return "Reverse the Words";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using StringProcessor;

namespace GuiApp
{
    class RandomizeString : IStringProcessor
    {
        #region IStringProcessor Members

        public string Process(string input)
        {
            Random rng = new Random();
            List<char> charInput = input.ToList();
            List<char> charOutput = new List<char>();

            // Go through character input, remove a random
            // character and add it to the output.
            while (charInput.Count > 0)
            {
                int pos = rng.Next(charInput.Count - 1);
                charOutput.Add(charInput[pos]);
                charInput.RemoveAt(pos);
            }

            // Generate a string from the character collection.
            return new string(charOutput.ToArray());
        }

        #endregion

        public override string ToString()
        {
            return "Scramble the String";
        }
    }
}

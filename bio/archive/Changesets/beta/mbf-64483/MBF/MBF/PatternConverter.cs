// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MBF.Properties;

namespace MBF
{
    /// <summary>
    /// Implements the IPatternConverter.
    /// Converts the prosite pattern into protein/dna/rna sequence, applying the ruleset from 
    /// http://www.hpa-bioinfotools.org.uk/ps_scan/PS_SCAN_PATTERN_SYNTAX.html and
    /// http://www.ncbi.nlm.nih.gov/blast/html/PHIsyntax.html
    /// </summary>
    public class PatternConverter : IPatternConverter
    {
        /// <summary>
        /// Seperator character.
        /// </summary>
        private const char Seperator = '-';

        /// <summary>
        /// Seperator character.
        /// </summary>
        private const char RangeSeperator = ',';

        /// <summary>
        /// Left square bracket
        /// </summary>
        private const char LeftSquareBracket = '[';

        /// <summary>
        /// Right square bracket
        /// </summary>
        private const char RightSquareBracket = ']';

        /// <summary>
        /// Left curly bracket
        /// </summary>
        private const char LeftCurlyBracket = '{';

        /// <summary>
        /// Right curly bracket
        /// </summary>
        private const char RightCurlyBracket = '}';

        /// <summary>
        /// Left parenthesis
        /// </summary>
        private const char LeftParenthesis = '(';

        /// <summary>
        /// Right parenthesis
        /// </summary>
        private const char RightParenthesis = ')';

        /// <summary>
        /// Left angle bracket
        /// </summary>
        private const char LeftAngle = '<';

        /// <summary>
        /// Right angle bracket
        /// </summary>
        private const char RightAngle = '>';

        /// <summary>
        /// Repeater character
        /// </summary>
        private const char Repeater = '*';

        /// <summary>
        /// Multiton instance of PatternConverter
        /// </summary>
        private static IDictionary<IAlphabet, IPatternConverter> _patternConverter =
            new Dictionary<IAlphabet, IPatternConverter>();

        /// <summary>
        /// List of unique and ambiguous Alphabets
        /// </summary>
        private HashSet<char> _alphabets = null;

        /// <summary>
        /// Alphabet set supported by this pattern converter
        /// </summary>
        private IAlphabet _alphabetSet = null;

        /// <summary>
        /// Multiton class.
        /// </summary>
        private PatternConverter(IAlphabet alphabetSet)
        {
            _alphabetSet = alphabetSet;

            // Generate unique & ambiguous Alphabets list
            _alphabets = new HashSet<char>();
            _alphabetSet.LookupAll(true, false, false, false).All(amp => _alphabets.Add(amp.Symbol));
        }

        /// <summary>
        /// Returns an instance of PatternConverter
        /// </summary>
        public static IPatternConverter GetInstanace(IAlphabet alphabetSet)
        {
            IPatternConverter patternConverter = null;

            if (!_patternConverter.TryGetValue(alphabetSet, out patternConverter))
            {
                lock (_patternConverter)
                {
                    if (!_patternConverter.TryGetValue(alphabetSet, out patternConverter))
                    {
                        patternConverter = new PatternConverter(alphabetSet);
                        _patternConverter.Add(alphabetSet, patternConverter);
                    }
                }
            }

            return patternConverter;
        }

        #region IPatternConverter Members
        /// <summary>
        /// Convert the given list of patterns into locally understood patterns.
        /// </summary>
        /// <param name="patterns">List of patterns to be converted.</param>
        /// <returns>Converted list of patterns.</returns>
        public IDictionary<string, IList<string>> Convert(IList<string> patterns)
        {
            // Create tasks
            IList<Task<KeyValuePair<string, IList<string>>>> tasks = patterns.Select(
                    pattern => Task<KeyValuePair<string, IList<string>>>.Factory.StartNew(
                            t => new KeyValuePair<string, IList<string>>(pattern, Convert(pattern)),
                            TaskCreationOptions.None)).ToList();

            // Wait for all the task
            Task.WaitAll(tasks.ToArray());

            IDictionary<string, IList<string>> results = new Dictionary<string, IList<string>>();
            foreach (Task<KeyValuePair<string, IList<string>>> task in tasks)
            {
                results.Add(task.Result.Key, task.Result.Value);
            }

            return results;
        }

        /// <summary>
        /// Convert the given pattern into locally understood patterns.
        /// </summary>
        /// <param name="pattern">Pattern to be converted.</param>
        /// <returns>Converted list of patterns.</returns>
        public IList<string> Convert(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                throw new ArgumentNullException("pattern");
            }

            IList<string> result = new List<string>();
            int index = 0;
            // Note that '<' & '>' are special cases and can occur only once in pattern
            bool isLeftAngleSet = false, isRightAngleSet = false;
            IList<StringBuilder> patterns = new List<StringBuilder>();

            while (index < pattern.Length)
            {
                if (index + 1 < pattern.Length
                        && pattern[index + 1] == LeftParenthesis)
                {
                    index++;
                    continue;
                }

                switch (pattern[index])
                {
                    case Seperator:
                        index++;
                        continue;

                    case LeftSquareBracket:
                        // Resolve any square bracket
                        index = ResolveSquareBracket(
                                pattern,
                                index,
                                patterns,
                                ref isLeftAngleSet,
                                ref isRightAngleSet);

                        break;

                    case RightSquareBracket:
                        // Error
                        throw new FormatException(
                            string.Format(CultureInfo.InvariantCulture, Resource.InvalidPatternFormat, pattern[index], index));

                    case LeftCurlyBracket:
                        // Resolve any curly bracket
                        index = ResolveCurlyBracket(pattern, index, patterns);
                        break;

                    case RightCurlyBracket:
                        // Error
                        throw new FormatException(
                            string.Format(CultureInfo.InvariantCulture, Resource.InvalidPatternFormat, pattern[index], index));

                    case LeftParenthesis:
                        // Resolve repeating character (n)
                        index = ResolveParenthesis(pattern, index, patterns);
                        break;

                    case RightParenthesis:
                        // Error
                        throw new FormatException(
                            string.Format(CultureInfo.InvariantCulture, Resource.InvalidPatternFormat, pattern[index], index));

                    case LeftAngle:
                        // Denotes start of the pattern
                        if (!(index == 0)
                            || isLeftAngleSet)
                        {
                            throw new FormatException(
                                string.Format(CultureInfo.InvariantCulture, Resource.InvalidPatternFormat, pattern[index], index));
                        }

                        isLeftAngleSet = true;
                        ExtendPattern(patterns, MapSymbol(pattern[index]));
                        break;

                    case RightAngle:
                        // Denotes end of the pattern
                        if (!(index == pattern.Length - 1)
                            || isRightAngleSet)
                        {
                            throw new FormatException(
                                string.Format(CultureInfo.InvariantCulture, Resource.InvalidPatternFormat, pattern[index], index));
                        }

                        isRightAngleSet = true;
                        ExtendPattern(patterns, MapSymbol(pattern[index]));
                        break;

                    default:
                        // Add the symbol to result
                        ResolveSymbol(pattern, index, patterns);
                        break;
                }

                index++;
            }

            foreach (StringBuilder sb in patterns)
            {
                result.Add(sb.ToString());
            }

            return result;
        }
        #endregion

        /// <summary>
        /// Create a dictionary which has the mapping for ambiguous/unambiguous characters
        /// </summary>
        /// <summary>
        /// Find the list of characters that maps to the inputs symbol and applies the pattern rule.
        /// </summary>
        /// <param name="symbol">Character which has to be mapped.</param>
        private char[] MapSymbol(char symbol)
        {
            if (_alphabets.Contains(symbol))
            {
                return new char[] { symbol };
            }

            switch (symbol)
            {
                case LeftAngle:
                case RightAngle:
                case Repeater:
                    return new char[] { symbol };

                default:
                    break;
            }

            return _alphabetSet.GetBasicSymbols(_alphabetSet.LookupBySymbol(symbol)).Select(c => c.Symbol).ToArray();
        }

        /// <summary>
        /// Resolve the pattern for Square bracket
        /// </summary>
        /// <param name="pattern">Pattern string.</param>
        /// <param name="index">Current index in pattern.</param>
        /// <param name="patterns">List of result patterns.</param>
        /// <param name="isLeftAngleSet">Is left angle bracket set.</param>
        /// <param name="isRightAngleSet">Is right angle bracket set.</param>
        /// <returns>Current index in pattern</returns>
        private int ResolveSquareBracket(
                string pattern,
                int index,
                IList<StringBuilder> patterns,
                ref bool isLeftAngleSet,
                ref bool isRightAngleSet)
        {
            HashSet<char> symbols = new HashSet<char>();
            index++;

            // Validate pattern
            if (pattern[index] == RightSquareBracket
                || pattern.IndexOf(RightSquareBracket, index) == -1)
            {
                throw new FormatException(
                    string.Format(CultureInfo.InvariantCulture, Resource.InvalidPatternFormat, pattern[index], index));
            }

            while (pattern[index] != RightSquareBracket)
            {
                if (pattern[index] == Seperator)
                {
                    index++;
                    continue;
                }

                // Validate pattern
                if (pattern[index] == LeftSquareBracket
                    || pattern[index] == LeftCurlyBracket
                    || pattern[index] == RightCurlyBracket
                    || pattern[index] == LeftParenthesis
                    || pattern[index] == RightParenthesis)
                {
                    throw new FormatException(
                        string.Format(CultureInfo.InvariantCulture, Resource.InvalidPatternFormat, pattern[index], index));
                }

                if (pattern[index] == LeftAngle)
                {
                    if (!(pattern[0] == LeftSquareBracket && index == 1)
                        || isLeftAngleSet)
                    {
                        throw new FormatException(
                            string.Format(CultureInfo.InvariantCulture, Resource.InvalidPatternFormat, pattern[index], index));
                    }

                    isLeftAngleSet = true;
                }

                if (pattern[index] == RightAngle)
                {
                    if ((pattern[pattern.Length - 1] == RightSquareBracket && index == pattern.Length - 2)
                        || isRightAngleSet)
                    {
                        throw new FormatException(
                            string.Format(CultureInfo.InvariantCulture, Resource.InvalidPatternFormat, pattern[index], index));
                    }

                    isRightAngleSet = true;
                }

                char[] symbolMap = MapSymbol(pattern[index]);

                if (symbolMap == null
                    || symbolMap.Length == 0)
                {
                    throw new FormatException(
                        string.Format(CultureInfo.InvariantCulture, Resource.InvalidPatternFormat, pattern[index], index));
                }

                if (char.IsLower(pattern[index]))
                {
                    for (int i = 0; i < symbolMap.Length; i++)
                    {
                        symbolMap[i] = char.ToLower(symbolMap[i], CultureInfo.InvariantCulture);
                    }
                }

                foreach (char symbol in symbolMap)
                {
                    if (!symbols.Contains(symbol))
                    {
                        symbols.Add(symbol);
                    }
                }

                index++;
            }

            ExtendPattern(patterns, symbols.ToArray());
            return index;
        }

        /// <summary>
        /// Resolve the pattern for Curly bracket
        /// </summary>
        /// <param name="pattern">Pattern string.</param>
        /// <param name="index">Current index in pattern.</param>
        /// <param name="patterns">List of result patterns.</param>
        /// <returns>Current index in pattern.</returns>
        private int ResolveCurlyBracket(
                string pattern,
                int index,
                IList<StringBuilder> patterns)
        {
            IList<char> symbols = new List<char>();

            index++;

            // Validate pattern
            if (pattern[index] == RightCurlyBracket
                || pattern.IndexOf(RightCurlyBracket, index) == -1)
            {
                throw new FormatException(
                    string.Format(CultureInfo.InvariantCulture, Resource.InvalidPatternFormat, pattern[index], index));
            }

            while (pattern[index] != RightCurlyBracket)
            {
                if (pattern[index] == Seperator)
                {
                    index++;
                    continue;
                }

                // Validate pattern
                if (pattern[index] == LeftSquareBracket
                    || pattern[index] == RightSquareBracket
                    || pattern[index] == LeftCurlyBracket
                    || pattern[index] == LeftParenthesis
                    || pattern[index] == RightParenthesis
                    || pattern[index] == LeftAngle
                    || pattern[index] == RightAngle
                    || !_alphabets.Contains(pattern[index])
                    || symbols.Contains(pattern[index]))
                {
                    throw new FormatException(
                        string.Format(CultureInfo.InvariantCulture, Resource.InvalidPatternFormat, pattern[index], index));
                }

                symbols.Add(pattern[index]);
                index++;
            }

            symbols = this._alphabets.Where(s => !symbols.Contains(s)).ToList();
            ExtendPattern(patterns, symbols.ToArray());
            return index;
        }

        /// <summary>
        /// Resolve the pattern for Parenthesis
        /// </summary>
        /// <param name="pattern">Pattern string.</param>
        /// <param name="index">Current index in pattern.</param>
        /// <param name="patterns">List of result patterns.</param>
        /// <returns>Current index in pattern.</returns>
        private static int ResolveParenthesis(
                string pattern,
                int index,
                IList<StringBuilder> patterns)
        {
            index++;
            int rightLimit = pattern.IndexOf(RightParenthesis, index);

            // Either a single digit or command seperated
            string repeatString = pattern.Substring(index, rightLimit - index);
            string[] repeatLimit = repeatString.Split(
                new char[] { RangeSeperator }, StringSplitOptions.RemoveEmptyEntries);
            if (index == 1
                || repeatLimit.Length == 0
                || repeatLimit.Length > 2)
            {
                throw new FormatException(
                    string.Format(CultureInfo.InvariantCulture, Resource.InvalidPatternFormat, pattern[index], index));
            }

            char repeatCharacter = pattern[index - 2];
            int lowerLimit, upperLimit;

            if (int.TryParse(repeatLimit[0], out lowerLimit))
            {
                upperLimit = lowerLimit;
            }
            else
            {
                throw new FormatException(
                    string.Format(CultureInfo.InvariantCulture, Resource.InvalidPatternFormat, pattern[index], index));
            }
            if (repeatLimit.Length == 2)
            {
                if (!int.TryParse(repeatLimit[1], out upperLimit))
                {
                    throw new FormatException(
                        string.Format(CultureInfo.InvariantCulture, Resource.InvalidPatternFormat, pattern[index], index));
                }
            }

            if (upperLimit < lowerLimit)
            {
                throw new FormatException(
                    string.Format(CultureInfo.InvariantCulture, Resource.InvalidPatternFormat, pattern[index], index));
            }

            ExtendPattern(patterns, repeatCharacter, lowerLimit, upperLimit);

            index = rightLimit;
            return index;
        }

        /// <summary>
        /// Resolve the symbol and extend the result patterns list
        /// </summary>
        /// <param name="pattern">Pattern string.</param>
        /// <param name="index">Current index in pattern.</param>
        /// <param name="patterns">List of result patterns.</param>
        private void ResolveSymbol(string pattern, int index, IList<StringBuilder> patterns)
        {
            char[] symbolMap = MapSymbol(pattern[index]);

            if (symbolMap == null
                || symbolMap.Length == 0)
            {
                throw new FormatException(
                    string.Format(CultureInfo.InvariantCulture, Resource.InvalidPatternFormat, pattern[index], index));
            }

            if (char.IsLower(pattern[index]))
            {
                for (int i = 0; i < symbolMap.Length; i++)
                {
                    symbolMap[i] = char.ToLower(symbolMap[i], CultureInfo.InvariantCulture);
                }
            }

            ExtendPattern(patterns, symbolMap);
        }

        /// <summary>
        /// Extend the list of pattern.
        /// Resolve the symbol and extend the pattern.
        /// </summary>
        /// <param name="patterns">Current list of pattern.</param>
        /// <param name="symbolMap">List of characters to be appended.</param>
        private static void ExtendPattern(IList<StringBuilder> patterns, char[] symbolMap)
        {
            if (patterns.Count == 0)
            {
                foreach (char symbol in symbolMap)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(symbol);
                    patterns.Add(sb);
                }
            }
            else
            {
                foreach (StringBuilder sb in patterns)
                {
                    sb.Append(symbolMap[0]);
                }

                if (symbolMap.Length > 1)
                {
                    int patternCount = patterns.Count;
                    for (int patternIndex = 0; patternIndex < patternCount; patternIndex++)
                    {
                        string currentPattern = patterns[patternIndex].ToString();
                        currentPattern = currentPattern.Substring(0, currentPattern.Length - 1);

                        for (int symbolIndex = 1; symbolIndex < symbolMap.Length; symbolIndex++)
                        {
                            StringBuilder sb = new StringBuilder(currentPattern);
                            sb.Append(symbolMap[symbolIndex]);
                            patterns.Add(sb);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Extend the list of pattern.
        /// Resolve the symbol and extend the pattern.
        /// </summary>
        /// <param name="patterns">Current list of pattern.</param>
        /// <param name="character">Character to be appended.</param>
        /// <param name="startCount">Lower limit of repeat count.</param>
        /// <param name="endCount">Upper limit of repeat count.</param>
        private static void ExtendPattern(IList<StringBuilder> patterns, char character, int startCount, int endCount)
        {
            if (patterns.Count == 0)
            {
                for (int repeatCount = startCount; repeatCount <= endCount; repeatCount++)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(character, repeatCount);
                    patterns.Add(sb);
                }
            }
            else
            {
                int patternCount = patterns.Count;
                for (int patternIndex = 0; patternIndex < patternCount; patternIndex++)
                {
                    string currentPattern = patterns[patternIndex].ToString();

                    for (int repeatCount = startCount; repeatCount <= endCount; repeatCount++)
                    {
                        if (repeatCount == startCount)
                        {
                            patterns[patternIndex].Append(character, repeatCount);
                        }
                        else
                        {
                            StringBuilder sb = new StringBuilder(currentPattern);
                            sb.Append(character, repeatCount);
                            patterns.Add(sb);
                        }
                    }
                }
            }
        }
    }
}
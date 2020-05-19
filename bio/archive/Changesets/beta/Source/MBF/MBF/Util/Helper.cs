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
using System.Text.RegularExpressions;
using MBF.IO.GenBank;
using MBF.Properties;

namespace MBF.Util
{
    // all methods in this class should be static

    /// <summary>
    /// Generally useful static methods.
    /// </summary>
    /// <remarks>
    /// The util class contains utility methods used throughout the MBF library. These
    /// are general rather than biological in nature. In another language, they might be implemented
    /// as global functions. They are generally stateless, so they should be public and static.
    /// </remarks>
    public static class Helper
    {
        /// <summary>
        /// Key to get GenBankMetadata object from Metadata of a sequence which is parsed from GenBankParser.
        /// </summary>
        public const string GenBankMetadataKey = "GenBank";

        /// <summary>
        /// Key to get SAMAlignmentHeader object from Metadata of a sequence alignment which is parsed from SAMParser.
        /// </summary>
        public const string SAMAlignmentHeaderKey = "SAMAlignmentHeader";

        /// <summary>
        /// Key to get SAMAlignedSequenceHeader object from Metadata of a aligned sequence which is parsed from SAMParser.
        /// </summary>
        public const string SAMAlignedSequenceHeaderKey = "SAMAlignedSequenceHeader";

        private const string SingleStrand = "ss-";
        private const string DoubleStrand = "ds-";
        private const string MixedStrand = "ms-";
        private const string LinearStrandTopology = "linear";
        private const string CircularStrandTopology = "circular";
        private const string ProjectDBLink = "Project";
        private const string TraceAssemblyArchiveDBLink = "Trace Assembly Archive";
        private const string Colon = ":";
        private const string Comma = ",";
        private const string Space = " ";
        private const string SegmentDelim = " of ";

        private static Random random = new Random();

        /// <summary>
        /// Remove both single and double quotation mark delimiters from a string.
        /// </summary>
        /// <remarks>
        /// This does not ignore escaped quotes.
        /// </remarks>
        /// <param name="str">The input string.</param>
        /// <returns>The string minus delimiting quotes.</returns>
        public static string Dequote(string str)
        {
            int len = str.Length;
            if (len >= 2 && (str[0] == '\"' || str[0] == '\''))
            {
                if (str[0] == str[len - 1])
                {
                    return str.Substring(1, len - 2);
                }
            }
            return str;
        }

        // note: could be an extension method to string

        /// <summary>
        /// String Multiply. Build a string by concatenating copies of the input string.
        /// </summary>
        /// <param name="str">The string to multiply.</param>
        /// <param name="count">The number of copies wanted.</param>
        /// <returns>The multiplied string.</returns>
        public static string StringMultiply(string str, int count)
        {
            StringBuilder sb = new StringBuilder(count);
            for (int i = 0; i < count; ++i)
            {
                sb.Append(str);
            }
            return sb.ToString();
        }

        /// <summary>
        /// see if string test, starting at startPos, matches string match, up to 
        /// the length of match. if so, set pos to the position just after the match.
        /// safely returns false if test is too short to match.
        /// </summary>
        /// <param name="test">The string to test.</param>
        /// <param name="startPos">Where to start testing.</param>
        /// <param name="match">The match string to test for.</param>
        /// <param name="pos">on success, the position just after the match.</param>
        /// <returns>true if the strings match.</returns>
        public static bool StringMatches(string test, int startPos, string match, out int pos)
        {
            int iTest = startPos, maxTest = test.Length - 1, iMatch = 0, maxMatch = match.Length - 1;
            for (; iTest <= maxTest; ++iTest, ++iMatch)
            {
                if (test[iTest] != match[iMatch] || iMatch > maxMatch)
                {
                    pos = -1;
                    return false;
                }
            }
            pos = iTest;
            return true;
        }

        /// <summary>
        /// Overload that doesn't return the position as an out parameter.
        /// </summary>
        /// <param name="test">The string to test.</param>
        /// <param name="startPos">Where to start testing.</param>
        /// <param name="match">The match string to test for.</param>
        /// <returns>true if the strings match.</returns>
        public static bool StringMatches(string test, int startPos, string match)
        {
            int dontcare;
            return StringMatches(test, startPos, match, out dontcare);
        }

        /// <summary>
        /// Overload that starts at position 0.
        /// </summary>
        /// <param name="test">The string to test.</param>
        /// <param name="match">The match string to test for.</param>
        /// <returns>true if the strings match.</returns>
        public static bool StringMatches(string test, string match)
        {
            return StringMatches(test, 0, match);
        }

        /// <summary>
        /// See if test string is identical to any of the passed list of strings.
        /// </summary>
        /// <param name="test">The string to test.</param>
        /// <param name="args">Variable number of strings to test against.</param>
        /// <returns>True if test matches one of the subsequent arguments.</returns>
        public static bool StringHasMatch(string test, params string[] args)
        {
            foreach (string s in args)
            {
                if (test == s)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// see if test string contains any of the passed list of strings.
        /// </summary>
        /// <param name="test">The string to test.</param>
        /// <param name="args">Variable number of strings to test against.</param>
        /// <returns>true if test contains one of the subsequent arguments.</returns>
        public static bool StringContains(string test, params string[] args)
        {
            foreach (string s in args)
            {
                if (test.Contains(s))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Remove all whitespace from the provided string
        /// </summary>
        /// <param name="input">The string to modify</param>
        /// <returns>A copy of 'input' with all whitespace characters removed</returns>
        public static string StringRemoveWhitespace(string input)
        {
            var b = new StringBuilder(input.Length);
            foreach (var ch in input)
            {
                if (!Char.IsWhiteSpace(ch))
                {
                    b.Append(ch);
                }
            }
            return b.ToString();
        }

        /// <summary>
        /// Character version of StrIn; see if passed char matches one of the later arguments.
        /// </summary>
        /// <param name="test">The char to test.</param>
        /// <param name="args">Variable list of chars to test against.</param>
        /// <returns>True if test equals one of the subsequent arguments.</returns>
        public static bool CharIn(char test, params char[] args)
        {
            foreach (char c in args)
            {
                if (test == c)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Count up the occurrences of char ch in string str.
        /// </summary>
        /// <param name="str">string being examined.</param>
        /// <param name="ch">character to look for and count.</param>
        /// <returns>number of occurrences of ch in str.</returns>
        public static int CountChar(string str, char ch)
        {
            int ret = 0;
            for (int i = 0, len = str.Length; i < len; ++i)
            {
                if (str[i] == ch)
                {
                    ++ret;
                }
            }
            return ret;
        }

        /// <summary>
        /// Count up occurrences, in the string str, of any of the subsequent char arguments.
        /// </summary>
        /// <param name="str">string being examined.</param>
        /// <param name="args">Variable list of chars to look for and count.</param>
        /// <returns>number of occurrences.</returns>
        public static int CountChars(string str, params char[] args)
        {
            int ret = 0;
            for (int i = 0, len = str.Length; i < len; ++i)
            {
                foreach (char c in args)
                {
                    if (str[i] == c)
                    {
                        ++ret;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Test whether a string consists entirely of a specified set of characters.
        /// </summary>
        /// <param name="str">The string to test.</param>
        /// <param name="args">Variable list of chars comprising the set.</param>
        /// <returns>true if str contains only characters in the set.</returns>
        public static bool ContainsOnly(string str, params char[] args)
        {
            for (int i = 0, len = str.Length; i < len; ++i)
            {
                bool found = false;
                foreach (char c in args)
                {
                    if (str[i] == c)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Test a string to see if it's made up only of digits (like regex \d+, but faster).
        /// </summary>
        /// <param name="str">The test string.</param>
        /// <returns>true if the string contains only digits.</returns>
        public static bool IsDigits(string str)
        {
            foreach (char ch in str)
            {
                if (ch < '0' || ch > '9')
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Identifies if a file extension is a
        /// valid extension for SAM formats.
        /// </summary>
        /// <returns>
        /// true  : if it is a valid SAM file extension.
        /// false : if it is a in-valid SAM file extension.
        /// </returns>
        public static bool IsSAM(string fileName)
        {
            bool isSAM = false;
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return isSAM;
            }

            int extensionDelimiter = fileName.LastIndexOf('.');
            if (-1 < extensionDelimiter)
            {
                string fileExtension = fileName.Substring(extensionDelimiter);
                string samExtensions = Properties.Resource.SAM_FILEEXTENSION;
                string[] extensions = samExtensions.Split(Comma.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string extension in extensions)
                {
                    if (fileExtension.Equals(extension, StringComparison.CurrentCultureIgnoreCase))
                    {
                        isSAM = true;
                        break;
                    }
                }
            }

            return isSAM;
        }

        /// <summary>
        /// Identifies if a file extension is a
        /// valid extension for BAM formats.
        /// </summary>
        /// <returns>
        /// true  : if it is a valid BAM file extension.
        /// false : if it is a in-valid BAM file extension.
        /// </returns>
        public static bool IsBAM(string fileName)
        {
            bool isBAM = false;
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return isBAM;
            }

            int extensionDelimiter = fileName.LastIndexOf('.');
            if (-1 < extensionDelimiter)
            {
                string fileExtension = fileName.Substring(extensionDelimiter);
                string bamExtensions = Properties.Resource.BAM_FILEEXTENSION;
                string[] extensions = bamExtensions.Split(Comma.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string extension in extensions)
                {
                    if (fileExtension.Equals(extension, StringComparison.CurrentCultureIgnoreCase))
                    {
                        isBAM = true;
                        break;
                    }
                }
            }

            return isBAM;
        }

        /// <summary>
        /// Identifies if a file extension is a
        /// valid extension for GenBank formats.
        /// </summary>
        /// <returns>
        /// true  : if it is a valid fasta file extension.
        /// false : if it is a in-valid fasta file extension.
        /// </returns>
        public static bool IsGenBank(string fileName)
        {
            bool isGenBank = false;
            int extensionDelimiter = fileName.LastIndexOf('.');
            if (-1 < extensionDelimiter)
            {
                string fileExtension = fileName.Substring(extensionDelimiter);
                string genBankExtensions = Properties.Resource.GENBANK_FILEEXTENSION;
                string[] extensions = genBankExtensions.Split(',');
                foreach (string extension in extensions)
                {
                    if (fileExtension.Equals(extension, StringComparison.CurrentCultureIgnoreCase))
                    {
                        isGenBank = true;
                        break;
                    }
                }
            }

            return isGenBank;
        }

        /// <summary>
        /// Identifies if a file extension is a
        /// valid extension for FastQ formats.
        /// </summary>
        /// <returns>
        /// true  : if it is a valid fastq file extension.
        /// false : if it is a in-valid fastq file extension.
        /// </returns>
        public static bool IsFastQ(string fileName)
        {
            bool isFastQ = false;
            int extensionDelimiter = fileName.LastIndexOf('.');
            if (-1 < extensionDelimiter)
            {
                string fileExtension = fileName.Substring(extensionDelimiter);
                string fastQExtensions = Properties.Resource.FASTQ_FILEEXTENSION;
                string[] extensions = fastQExtensions.Split(',');
                foreach (string extension in extensions)
                {
                    if (fileExtension.Equals(extension, StringComparison.CurrentCultureIgnoreCase))
                    {
                        isFastQ = true;
                        break;
                    }
                }
            }

            return isFastQ;
        }

        /// <summary>
        /// Identifies if a file extension is a
        /// valid extension for FASTA formats.
        /// </summary>
        /// <returns>
        /// true  : if it is a valid fasta file extension.
        /// false : if it is a in-valid fasta file extension.
        /// </returns>
        public static bool IsFasta(string fileName)
        {
            bool isFasta = false;
            int extensionDelimiter = fileName.LastIndexOf('.');
            if (-1 < extensionDelimiter)
            {
                string fileExtension = fileName.Substring(extensionDelimiter);
                string fastaExtensions = Properties.Resource.FASTA_FILEEXTENSION;
                string[] extensions = fastaExtensions.Split(',');
                foreach (string extension in extensions)
                {
                    if (fileExtension.Equals(extension, StringComparison.CurrentCultureIgnoreCase))
                    {
                        isFasta = true;
                        break;
                    }
                }
            }

            return isFasta;
        }

        /// <summary>
        /// Returns random numbers according to an approximate normal distribution
        /// with an average and standard deviation set by the caller.
        /// </summary>
        /// <param name="average">Average result returned from calling the method</param>
        /// <param name="standardDeviation">Standard deviation applied to the normal curve</param>
        /// <returns>A random value</returns>
        public static double GetNormalRandom(double average, double standardDeviation)
        {
            return GetNormalRandom(average, standardDeviation, 8);
        }

        /// <summary>
        /// Returns random numbers according to an approximate normal distribution
        /// with an average and standard deviation set by the caller. This is done iteratively
        /// using the central limit theorem.
        /// </summary>
        /// <param name="average">Average result returned from calling the method</param>
        /// <param name="standardDeviation">Standard deviation applied to the normal curve</param>
        /// <param name="steps">
        /// The number of iterative steps to take in generating each number. The higher this number
        /// is, the closer to a true normal distribution the results will be, but the higher the
        /// computation cost. A value between 4 and 8 should be sufficient for most uses.
        /// </param>
        /// <returns>A random value</returns>
        public static double GetNormalRandom(double average, double standardDeviation, int steps)
        {
            if (steps < 0)
                throw new ArgumentException("Step count can not be negative");

            double sum = 0.0;
            for (int i = 0; i < steps; i++)
                sum += random.NextDouble();

            // RandomDouble gives us a uniformly distributed number between 0 and 1.
            // This value has average=0.5 and var=1/12. For the sum, this is
            // average=steps*0.5 and var=steps/12.

            sum -= ((double)steps / 2.0);						// Go to N(0, 1/12n)
            sum *= (standardDeviation / (Math.Sqrt((double)steps / 12.0)));	// Go to N(0, var)
            sum += average;										// Go to N(mu, var)

            return sum;
        }

        /// <summary>
        /// Returns true if all the characters in the specified string belongs to the 
        /// specified alphabet, else returns false.
        /// </summary>
        /// <param name="alphabet">Alphabet against which the characters in the specified sequence data has to be validated.</param>
        /// <param name="sequence">Sequence data to be validated.</param>
        /// <param name="invalidCharacter">First character found to be invalid.</param>
        /// <returns>Returns true if all character are valid, else returns false.</returns>
        public static bool IsValidSequence(IAlphabet alphabet, string sequence, out char invalidCharacter)
        {
            bool isvalid = true;
            invalidCharacter = char.MinValue;

            if (string.IsNullOrEmpty(sequence))
            {
                isvalid = false;
            }
            else
            {
                foreach(char ch in sequence.Distinct())
                {
                    if (alphabet.LookupBySymbol(ch) == null)
                    {
                        isvalid = false;
                        invalidCharacter = ch;
                        break;
                    }
                }
            }

            return isvalid;
        }

        /// <summary>
        /// Returns the sequence items for the specified string.
        /// If any character in the string is unknown by the specified alphabet
        /// an exception will occur.
        /// </summary>
        /// <param name="alphabet">
        /// The alphabet the sequence uses (eg. Alphabets.DNA, Alphabets.RNA or Alphabets.Protein)
        /// </param>
        /// <param name="sequence">A description of the sequence data.</param>
        /// <returns>Returns list of sequence items.</returns>
        public static List<ISequenceItem> GetSequenceItems(IAlphabet alphabet, string sequence)
        {
            if (alphabet == null)
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameAlphabet);
            }

            if (string.IsNullOrEmpty(sequence))
            {
                throw new ArgumentNullException(Properties.Resource.ParameterNameSequence);
            }

            List<ISequenceItem> seqItems = new List<ISequenceItem>();
            foreach (char ch in sequence)
            {
                ISequenceItem seqItem = alphabet.LookupBySymbol(ch);
                if (seqItem == null)
                {
                    throw new ArgumentException(
                        string.Format(
                        CultureInfo.CurrentCulture,
                        Properties.Resource.InvalidSymbol,
                        ch));
                }

                seqItems.Add(seqItem);
            }

            return seqItems;
        }

        /// <summary>
        /// Returns a SequenceStrandType corresponds to the specified string.
        /// </summary>
        /// <param name="strand">Strand type.</param>
        /// <returns>Returns SequenceStrandType.</returns>
        public static SequenceStrandType GetStrandType(string strand)
        {
            if (string.IsNullOrEmpty(strand))
            {
                return SequenceStrandType.None;
            }

            strand = strand.ToLower(CultureInfo.InvariantCulture);

            if (strand.Equals(SingleStrand))
            {
                return SequenceStrandType.Single;
            }

            if (strand.Equals(DoubleStrand))
            {
                return SequenceStrandType.Double;
            }

            if (strand.Equals(MixedStrand))
            {
                return SequenceStrandType.Mixed;
            }

            return SequenceStrandType.None;
        }

        /// <summary>
        /// Returns a string which represents specified SequenceStrandType.
        /// </summary>
        /// <param name="strand">Strand type.</param>
        /// <returns>Returns string.</returns>
        public static string GetStrandType(SequenceStrandType strand)
        {
            switch (strand)
            {
                case SequenceStrandType.Single:
                    return SingleStrand;

                case SequenceStrandType.Double:
                    return DoubleStrand;

                case SequenceStrandType.Mixed:
                    return MixedStrand;
            }

            return string.Empty;
        }

        /// <summary>
        /// Returns a SequenceStrandTopology corresponds to the specified string.
        /// </summary>
        /// <param name="strandTopology">Strand topology.</param>
        /// <returns>Returns SequenceStrandTopology.</returns>
        public static SequenceStrandTopology GetStrandTopology(string strandTopology)
        {
            if (string.IsNullOrEmpty(strandTopology))
            {
                return SequenceStrandTopology.None;
            }

            strandTopology = strandTopology.ToLower(CultureInfo.InvariantCulture);
            if (strandTopology.Equals(LinearStrandTopology))
            {
                return SequenceStrandTopology.Linear;
            }

            if (strandTopology.Equals(CircularStrandTopology))
            {
                return SequenceStrandTopology.Circular;
            }

            return SequenceStrandTopology.None;
        }

        /// <summary>
        /// Returns a string which represents specified SequenceStrandTopology.
        /// </summary>
        /// <param name="strandTopology">Strand topology.</param>
        /// <returns>Returns string.</returns>
        public static string GetStrandTopology(SequenceStrandTopology strandTopology)
        {
            switch (strandTopology)
            {
                case SequenceStrandTopology.Linear:
                    return LinearStrandTopology;
                case SequenceStrandTopology.Circular:
                    return CircularStrandTopology;
            }

            return string.Empty;
        }

        /// <summary>
        /// Returns a string which represents specified CrossReferenceLink.
        /// </summary>
        /// <param name="crossReferenceLink">CrossReferenceLink.</param>
        /// <returns>Returns string.</returns>
        public static string GetCrossReferenceLink(CrossReferenceLink crossReferenceLink)
        {
            StringBuilder strBuilder = new StringBuilder();
            string referenceType = string.Empty;
            if (crossReferenceLink.Type == CrossReferenceType.Project)
            {
                referenceType = ProjectDBLink;
            }
            else if (crossReferenceLink.Type == CrossReferenceType.TraceAssemblyArchive)
            {
                referenceType = TraceAssemblyArchiveDBLink;
            }

            strBuilder.Append(referenceType);
            strBuilder.Append(Colon);

            for (int i = 0; i < crossReferenceLink.Numbers.Count; i++)
            {
                strBuilder.Append(crossReferenceLink.Numbers[i]);
                if (i != crossReferenceLink.Numbers.Count - 1)
                {
                    strBuilder.Append(Comma);
                }
            }

            return strBuilder.ToString();
        }

        /// <summary>
        /// Returns a string which represents specified GenBankAccession.
        /// </summary>
        /// <param name="accession">GenBankAccession instance.</param>
        /// <returns>Returns string.</returns>
        public static string GetGenBankAccession(GenBankAccession accession)
        {
            StringBuilder strBuilder = new StringBuilder();
            if (accession.Primary != null)
            {
                strBuilder.Append(accession.Primary);
            }

            foreach (string str in accession.Secondary)
            {
                strBuilder.Append(Space);
                strBuilder.Append(str);
            }

            return strBuilder.ToString();
        }

        /// <summary>
        /// Returns a string which represents specified ProjectIdentifier.
        /// </summary>
        /// <param name="projectIdentifier">ProjectIdentifier instance.</param>
        /// <returns>Returns string.</returns>
        public static string GetProjectIdentifier(ProjectIdentifier projectIdentifier)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append(projectIdentifier.Name);
            strBuilder.Append(Colon);

            for (int i = 0; i < projectIdentifier.Numbers.Count; i++)
            {
                strBuilder.Append(projectIdentifier.Numbers[i]);
                if (i != projectIdentifier.Numbers.Count - 1)
                {
                    strBuilder.Append(Comma);
                }
            }

            return strBuilder.ToString();
        }

        /// <summary>
        /// Returns a string which represents specified SequenceSegment.
        /// </summary>
        /// <param name="segment">SequenceSegment instance.</param>
        /// <returns>Returns string.</returns>
        public static string GetSequenceSegment(SequenceSegment segment)
        {
            return segment.Current.ToString(CultureInfo.InvariantCulture) + SegmentDelim + segment.Count.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns Dna alphabet if all the symbols in distinctSymbols are 
        /// known by Dna alphabet else it continue to verify with Rna alpabet by 
        /// calling StartCheckFromRna method.
        /// </summary>
        /// <param name="distinctSymbols">Distinct symbols of sequence</param>
        /// <returns>If success then returns an instance of IAlphabet else returns null.</returns>
        public static IAlphabet StartCheckFromDna(IEnumerable<char> distinctSymbols)
        {
            if (!IsDnaAlphabet(distinctSymbols))
            {
                return StartCheckFromRna(distinctSymbols);
            }

            return Alphabets.DNA;
        }

        /// <summary>
        /// Returns Rna alphabet if all the symbols in distinctSymbols are 
        /// known by Rna alphabet else it continue to verify with Protein alpabet by 
        /// calling StartCheckFromProtein method.
        /// </summary>
        /// <param name="distinctSymbols">Distinct symbols of sequence</param>
        /// <returns>If success then returns an instance of IAlphabet else returns null.</returns>
        public static IAlphabet StartCheckFromRna(IEnumerable<char> distinctSymbols)
        {
            if (!IsRnaAlphabet(distinctSymbols))
            {
                return StartCheckFromProtein(distinctSymbols);
            }

            return Alphabets.RNA;
        }

        /// <summary>
        /// Returns Protein alphabet if all the symbols in distinctSymbols are 
        /// known by protein alphabet else returns null.
        /// </summary>
        /// <param name="distinctSymbols">Distinct symbols of sequence</param>
        /// <returns>If all symbols in distinctSymbols are known by protein alphabet 
        /// then returns protein Alphabet else returns null.</returns>
        public static IAlphabet StartCheckFromProtein(IEnumerable<char> distinctSymbols)
        {
            if (!IsProteinAlphabet(distinctSymbols))
            {
                return null;
            }

            return Alphabets.Protein;
        }

        /// <summary>
        /// Returns true if all symbols in the specified list are known by Dna.
        /// </summary>
        /// <param name="characters">List of symbols.</param>
        /// <returns>True if all symbols are known else returns false.</returns>
        private static bool IsDnaAlphabet(IEnumerable<char> characters)
        {
            foreach (char ch in characters)
            {
                if (Alphabets.DNA.LookupBySymbol(ch) == null)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns true if all symbols in the specified list are known by Rna.
        /// </summary>
        /// <param name="characters">List of symbols.</param>
        /// <returns>True if all symbols are known else returns false.</returns>
        private static bool IsRnaAlphabet(IEnumerable<char> characters)
        {
            foreach (char ch in characters)
            {
                if (Alphabets.RNA.LookupBySymbol(ch) == null)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns true if all symbols in the specified list are known by Protein.
        /// </summary>
        /// <param name="characters">List of symbols.</param>
        /// <returns>True if all symbols are known else returns false.</returns>
        private static bool IsProteinAlphabet(IEnumerable<char> characters)
        {
            foreach (char ch in characters)
            {
                if (Alphabets.Protein.LookupBySymbol(ch) == null)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Validates specified value with the specified regular expression. 
        /// </summary>
        /// <param name="pattern">Regular expression.</param>
        /// <param name="value">Value to validate.</param>
        /// <returns>Returns true if value completely match with the specified 
        /// regular expression; otherwise false.</returns>
        public static bool IsValidRegexValue(string pattern, string value)
        {
            Regex regx = new Regex(pattern);
            Match match = regx.Match(value);
            if (!match.Success || match.Value.Length != value.Length)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates specified value with the specified regular expression. 
        /// </summary>
        /// <param name="regx">Regular expression object.</param>
        /// <param name="value">Value to validate.</param>
        /// <returns>Returns true if value completely match with the specified 
        /// regular expression; otherwise false.</returns>
        public static bool IsValidRegexValue(Regex regx, string value)
        {
            if (regx == null)
            {
                throw new ArgumentNullException("regx");
            }

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            Match match = regx.Match(value);
            if (!match.Success || match.Value.Length != value.Length)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates specified value with the specified regular expression pattern.
        /// </summary>
        /// <param name="name">Name of the field.</param>
        /// <param name="value">Value to validate.</param>
        /// <param name="pattern">Regular exression pattern.</param>
        /// <returns>Returns empty string if valid; otherwise error message.</returns>
        public static string IsValidPatternValue(string name, string value, string pattern)
        {
            if (string.IsNullOrEmpty(value) || !IsValidRegexValue(pattern, value))
            {
                string message = string.Format(CultureInfo.CurrentCulture,
                                Resource.InvalidPatternMessage,
                                name,
                                value,
                                pattern);

                return message;
            }

            return string.Empty;
        }

        /// <summary>
        /// Validates specified value with the specified regular expression.
        /// </summary>
        /// <param name="name">Name of the field.</param>
        /// <param name="value">Value to validate.</param>
        /// <param name="regx">Regular exression object.</param>
        /// <returns>Returns empty string if valid; otherwise error message.</returns>
        public static string IsValidPatternValue(string name, string value, Regex regx)
        {
            if (regx == null)
            {
                throw new ArgumentNullException("regx");
            }

            if (string.IsNullOrEmpty(value) || !IsValidRegexValue(regx, value))
            {
                string message = string.Format(CultureInfo.CurrentCulture,
                                Resource.InvalidPatternMessage,
                                name,
                                value,
                                regx.ToString());

                return message;
            }

            return string.Empty;
        }

        /// <summary>
        /// Validates int value.
        /// </summary>
        /// <param name="name">Name of the field.</param>
        /// <param name="value">Value to validate.</param>
        /// <param name="minValue">Minimum value.</param>
        /// <param name="maxValue">Maximum value.</param>
        /// <returns>Returns empty string if valid; otherwise error message.</returns>
        public static string IsValidRange(string name, int value, int minValue, int maxValue)
        {
            if (value < minValue || value > maxValue)
            {
                return string.Format(CultureInfo.CurrentCulture, Resource.InvalidRangeMessage, name, value, minValue, maxValue);
            }

            return string.Empty;
        }

        /// <summary>
        /// Get reverse complement of sequence string.
        /// Handles only unambiguous DNA sequence strings.
        /// Note: This method is a light-weight implementation of sequence.ReverseComplement.
        /// This only works for unambiguous DNA sequences, which is characteristic of the input for de-novo.
        /// </summary>
        /// <param name="sequence">Sequence string</param>
        /// <param name="reverseComplementBuilder">String builder for building reverse complement</param>
        /// <returns>Reverse Complement sequence string</returns>
        public static string GetReverseComplement(this string sequence, char[] reverseComplementBuilder)
        {
            if (sequence == null)
            {
                throw new ArgumentNullException("sequence");
            }

            if (reverseComplementBuilder == null)
            {
                throw new ArgumentNullException("reverseComplementBuilder");
            }

            if (sequence.Length != reverseComplementBuilder.Length)
            {
                throw new ArgumentException(Properties.Resource.BuilderIncorrectLength);
            }

            for (int i = sequence.Length - 1; i >= 0; i--)
            {
                char rc;
                switch (sequence[i])
                {
                    case 'A':
                        rc = 'T';
                        break;
                    case 'T':
                        rc = 'A';
                        break;
                    case 'G':
                        rc = 'C';
                        break;
                    case 'C':
                        rc = 'G';
                        break;
                    default:
                        throw new ArgumentException(string.Format(
                            CultureInfo.CurrentCulture, Properties.Resource.InvalidSymbol, sequence[i]));
                }

                reverseComplementBuilder[sequence.Length - 1 - i] = (rc);
            }

            return new string(reverseComplementBuilder);
        }

        #region BAM Related Methods
        /// <summary>
        /// Gets a byte array which represents value of 16 bit singed integer in LittleEndian format.
        /// </summary>
        /// <param name="value">16 bit singed integer value.</param>
        public static byte[] GetLittleEndianByteArray(Int16 value)
        {
            byte[] array = new byte[2];

            array[0] = (byte)(value & 0x00FF);
            array[1] = (byte)((value & 0xFF00) >> 8);
            return array;
        }

        /// <summary>
        /// Gets a byte array which represents value of 16 bit unsinged integer in LittleEndian format.
        /// </summary>
        /// <param name="value">16 bit unsinged integer value.</param>
        public static byte[] GetLittleEndianByteArray(UInt16 value)
        {
            byte[] array = new byte[2];

            array[0] = (byte)(value & 0x00FF);
            array[1] = (byte)((value & 0xFF00) >> 8);
            return array;
        }

        /// <summary>
        /// Gets a byte array which represents value of 32 bit singed integer in LittleEndian format.
        /// </summary>
        /// <param name="value">32 bit singed integer value.</param>
        public static byte[] GetLittleEndianByteArray(int value)
        {
            byte[] array = new byte[4];

            array[0] = (byte)(value & 0x000000FF);
            array[1] = (byte)((value & 0x0000FF00) >> 8);
            array[2] = (byte)((value & 0x00FF0000) >> 16);
            array[3] = (byte)((value & 0xFF000000) >> 24);
            return array;
        }

        /// <summary>
        /// Gets a byte array which represents value of 32 bit unsinged integer in LittleEndian format.
        /// </summary>
        /// <param name="value">32 bit unsinged integer value.</param>
        public static byte[] GetLittleEndianByteArray(uint value)
        {
            byte[] array = new byte[4];

            array[0] = (byte)(value & 0x000000FF);
            array[1] = (byte)((value & 0x0000FF00) >> 8);
            array[2] = (byte)((value & 0x00FF0000) >> 16);
            array[3] = (byte)((value & 0xFF000000) >> 24);
            return array;
        }

        /// <summary>
        /// Gets byte array which represents value of float in LittleEndian format.
        /// </summary>
        /// <param name="value">Float value.</param>
        public static byte[] GetLittleEndianByteArray(float value)
        {
            return BitConverter.GetBytes(value);
        }

        /// <summary>
        /// Gets the integer value from the bytes stored as little-endian.
        /// </summary>
        /// <param name="byteArray">byte array.</param>
        /// <param name="startIndex">Start index of the byte array.</param>
        /// <param name="count">number of bytes to be considered to create integer value.</param>
        public static int GetValue(byte[] byteArray, int startIndex, int count)
        {
            if (byteArray == null)
            {
                throw new ArgumentNullException("byteArray");
            }

            if (startIndex < 0 || startIndex >= byteArray.Length)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }

            if (startIndex + count >= byteArray.Length)
            {
                throw new ArgumentOutOfRangeException("count");
            }

            int value = 0;
            for (int i = count - 1; i >= 0; i--)
            {
                value = (value << 8) + byteArray[startIndex + i];
            }

            return value;
        }

        /// <summary>
        /// Returns 16 bit signed integer from the byte array stored as little-endian.
        /// </summary>
        /// <param name="byteArray">byte array.</param>
        /// <param name="startIndex">Start index of the byte array.</param>
        public static Int16 GetInt16(byte[] byteArray, int startIndex)
        {
            if (byteArray == null)
            {
                throw new ArgumentNullException("byteArray");
            }

            if (startIndex < 0 || startIndex >= byteArray.Length)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }

            Int16 value = 0;
            value = (Int16)(byteArray[startIndex + 1] << 8);
            value += (Int16)byteArray[startIndex];
            return value;
        }

        /// <summary>
        /// Returns 16 bit unsigned integer from the byte array stored as little-endian.
        /// </summary>
        /// <param name="byteArray">byte array.</param>
        /// <param name="startIndex">Start index of the byte array.</param>
        public static UInt16 GetUInt16(byte[] byteArray, int startIndex)
        {
            if (byteArray == null)
            {
                throw new ArgumentNullException("byteArray");
            }

            if (startIndex < 0 || startIndex >= byteArray.Length)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }

            UInt16 value = 0;
            value = (UInt16)(byteArray[startIndex + 1] << 8);
            value += (UInt16)byteArray[startIndex];
            return value;
        }

        /// <summary>
        /// Returns 32 bit signed integer from the byte array stored as little-endian.
        /// </summary>
        /// <param name="byteArray">byte array.</param>
        /// <param name="startIndex">Start index of the byte array.</param>
        public static Int32 GetInt32(byte[] byteArray, int startIndex)
        {
            if (byteArray == null)
            {
                throw new ArgumentNullException("byteArray");
            }

            if (startIndex < 0 || startIndex >= byteArray.Length)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }

            return (byteArray[startIndex + 3] << 24) + (byteArray[startIndex + 2] << 16) + (byteArray[startIndex + 1] << 8) + byteArray[startIndex];
        }

        /// <summary>
        /// Returns 32 bit unsigned integer from the byte array stored as little-endian.
        /// </summary>
        /// <param name="byteArray">byte array.</param>
        /// <param name="startIndex">Start index of the byte array.</param>
        public static UInt32 GetUInt32(byte[] byteArray, int startIndex)
        {
            if (byteArray == null)
            {
                throw new ArgumentNullException("byteArray");
            }

            if (startIndex < 0 || startIndex >= byteArray.Length)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }

            return (UInt32)(byteArray[startIndex + 3] << 24) + (UInt32)(byteArray[startIndex + 2] << 16) + (UInt32)(byteArray[startIndex + 1] << 8) + (UInt32)byteArray[startIndex];
        }

        /// <summary>
        /// Returns 64 bit unsigned integer from the byte array stored as little-endian.
        /// </summary>
        /// <param name="byteArray">byte array.</param>
        /// <param name="startIndex">Start index of the byte array.</param>
        public static UInt64 GetUInt64(byte[] byteArray, int startIndex)
        {
            if (byteArray == null)
            {
                throw new ArgumentNullException("byteArray");
            }

            if (startIndex < 0 || startIndex >= byteArray.Length)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }

            UInt64 value = byteArray[startIndex + 7];
            value = (value << 8) + byteArray[startIndex + 6];
            value = (value << 8) + byteArray[startIndex + 5];
            value = (value << 8) + byteArray[startIndex + 4];
            value = (value << 8) + byteArray[startIndex + 3];
            value = (value << 8) + byteArray[startIndex + 2];
            value = (value << 8) + byteArray[startIndex + 1];
            value = (value << 8) + byteArray[startIndex];
            return value;
        }

        /// <summary>
        /// Returns float from the byte array.
        /// </summary>
        /// <param name="byteArray">byte array.</param>
        /// <param name="startIndex">Start index of the byte array.</param>
        public static float GetSingle(byte[] byteArray, int startIndex)
        {
            if (byteArray == null)
            {
                throw new ArgumentNullException("byteArray");
            }

            if (startIndex < 0 || startIndex >= byteArray.Length)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }

            return BitConverter.ToSingle(byteArray, startIndex);
        }

        /// <summary>
        /// Gets the HexString from the specified byte array.
        /// </summary>
        /// <param name="byteArray">Byte array.</param>
        /// <param name="startIndex">Start index of array from which HexString is stored.</param>
        /// <param name="length">Length of HexString to read.</param>
        public static string GetHexString(byte[] byteArray, int startIndex, int length)
        {
            if (byteArray == null)
            {
                throw new ArgumentNullException("byteArray");
            }

            if (startIndex < 0 || startIndex >= byteArray.Length)
            {
                throw new ArgumentOutOfRangeException("startIndex");
            }

            if (startIndex + length >= byteArray.Length)
            {
                throw new ArgumentOutOfRangeException("length");
            }

            StringBuilder Result = new StringBuilder();
            string HexAlphabet = "0123456789ABCDEF";
            for (int i = startIndex; i < startIndex + length; i++)
            {
                Result.Append(HexAlphabet[(byteArray[i] >> 4)]);
                Result.Append(HexAlphabet[(byteArray[i] & 0xF)]);
            }

            return Result.ToString();
        }
        #endregion

        /// <summary>
        /// Confirms that a condition is true. Raise an exception if it is not.
        /// </summary>
        /// <param name="condition">The condition to check</param>
        public static void CheckCondition(bool condition)
        {
            CheckCondition(condition, Properties.Resource.ErrorCheckConditionFailed);
        }

        /// <summary>
        /// Confirms that a condition is true. Raise an exception if it is not.
        /// </summary>
        /// <remarks>
        /// Warning: The message with be evaluated even if the condition is true, so don't make it's calculation slow.
        ///           Avoid this with the "messageFunction" version.
        /// </remarks>
        /// <param name="condition">The condition to check</param>
        /// <param name="message">A message for the exception</param>
        public static void CheckCondition(bool condition, string message)
        {
            if (!condition)
            {
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Confirms that a condition is true. Raise an exception if it is not.
        /// </summary>
        /// <remarks>
        /// Warning: The message with be evaluated even if the condition is true, so don't make it's calculation slow.
        ///           Avoid this with the "messageFunction" version.
        /// </remarks>
        /// <param name="condition">The condition to check</param>
        /// <param name="messageToFormat">A message for the exception</param>
        /// <param name="formatValues">Values for the exception's message.</param>
        public static void CheckCondition(bool condition, string messageToFormat, params object[] formatValues)
        {
            if (!condition)
            {
                throw new Exception(string.Format(messageToFormat, formatValues));
            }
        }

        /// <summary>
        /// Confirms that a condition is true. Raise an exception if it is not.
        /// </summary>
        /// <remarks>
        /// messageFunction will only be evaluated of condition is false. Use this version for messages that are costly to compute.
        /// </remarks>
        /// <param name="condition">The condition to check</param>
        /// <param name="messageFunction">Function that will generate the message if the condition is false.</param>
        public static void CheckCondition(bool condition, Func<string> messageFunction)
        {
            if (!condition)
            {
                string message = messageFunction();
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Confirms that a condition is true. Raise an exception of type T if it is not.
        /// </summary>
        /// <param name="condition">The condition to check</param>
        /// <typeparam name="T">The type of exception that will be raised.</typeparam>
        public static void CheckCondition<T>(bool condition) where T : Exception
        {
            CheckCondition<T>(condition, Properties.Resource.ErrorCheckConditionFailed);
        }

        /// <summary>
        /// Confirms that a condition is true. Raise an exception of type T if it is not.
        /// </summary>
        /// <remarks>
        /// Warning: The message with be evaluated even if the condition is true, so don't make it's calculation slow.
        ///           Avoid this with the "messageFunction" version.
        /// </remarks>
        /// <param name="condition">The condition to check</param>
        /// <param name="message">A message for the exception</param>
        /// <typeparam name="T">The type of exception that will be raised.</typeparam>
        public static void CheckCondition<T>(bool condition, string message) where T : Exception
        {
            if (!condition)
            {
                Type t = typeof(T);
                System.Reflection.ConstructorInfo constructor = t.GetConstructor(new Type[] { typeof(string) });
                T exception = (T)constructor.Invoke(new object[] { message });
                throw exception;
            }
        }

        /// <summary>
        /// Confirms that a condition is true. Raise an exception if it is not.
        /// </summary>
        /// <remarks>
        /// Warning: The message with be evaluated even if the condition is true, so don't make it's calculation slow.
        ///           Avoid this with the "messageFunction" version.
        /// </remarks>
        /// <param name="condition">The condition to check</param>
        /// <param name="messageToFormat">A message for the exception</param>
        /// <param name="formatValues">Values for the exception's message.</param>
        /// <typeparam name="T">The type of exception that will be raised.</typeparam>
        public static void CheckCondition<T>(bool condition, string messageToFormat, params object[] formatValues) where T : Exception
        {
            if (!condition)
            {
                CheckCondition<T>(condition, string.Format(messageToFormat, formatValues));
            }
        }

        /// <summary>
        /// Confirms that a condition is true. Raise an exception if it is not.
        /// </summary>
        /// <remarks>
        /// messageFunction will only be evaluated of condition is false. Use this version for messages that are costly to compute.
        /// </remarks>
        /// <param name="condition">The condition to check</param>
        /// <param name="messageFunction">Function that will generate the message if the condition is false.</param>
        public static void CheckCondition<T>(bool condition, Func<string> messageFunction) where T : Exception
        {
            if (!condition)
            {
                string message = messageFunction();
                CheckCondition<T>(condition, message);
            }
        }

        /// <summary>
        /// Creates a tab-delimited string containing the object's string values.
        /// </summary>
        /// <param name="objectCollection">The objects to put in the string</param>
        /// <returns>A tab-delimited string</returns>
        public static string CreateTabString(params object[] objectCollection)
        {
            return objectCollection.StringJoin("\t");
        }

        /// <summary>
        /// Creates a delimited string containing the object's string values.
        /// </summary>
        /// <param name="delimiter">The string that will delimit the objects</param>
        /// <param name="objectCollection">The objects to put in the string</param>
        /// <returns>A delimiter-delimited string</returns>
        public static string CreateDelimitedString(string delimiter, params object[] objectCollection)
        {
            return objectCollection.StringJoin(delimiter);
        }


        /// <summary>
        /// Returns the first item in sequence that is one item long. (Raises an
        /// exception of the sequence is more than one item long).
        /// </summary>
        /// <typeparam name="T">The type of elements of the sequence</typeparam>
        /// <param name="sequence">The one-item long sequence</param>
        /// <returns>The first item in the sequence.</returns>
        public static T FirstAndOnly<T>(IEnumerable<T> sequence)
        {
            IEnumerator<T> enumor = sequence.GetEnumerator();
            CheckCondition(enumor.MoveNext(), Properties.Resource.ErrorCheckConditionFirstAndOnlyTooFew);
            T t = enumor.Current;
            CheckCondition(!enumor.MoveNext(), Properties.Resource.ErrorCheckConditionFirstAndOnlyTooMany);
            return t;
        }


        /// <summary>
        /// Efficently (log n) test if two dictionaries have the same key set.
        /// </summary>
        /// <typeparam name="TKey">The key type of the dictionaries</typeparam>
        /// <typeparam name="TValue1">The value type of dictionary 1</typeparam>
        /// <typeparam name="TValue2">The value type of dictionary 2</typeparam>
        /// <param name="dictionary1">The first dictionary</param>
        /// <param name="dictionary2">The second dictonary</param>
        /// <returns>True if the two key sets are "set equal"; false, otherwise.</returns>
        public static bool KeysEqual<TKey, TValue1, TValue2>(IDictionary<TKey, TValue1> dictionary1, IDictionary<TKey, TValue2> dictionary2)
        {
            if (dictionary1.Count != dictionary2.Count)
            {
                return false;
            }

            foreach (TKey key in dictionary1.Keys)
            {
                //Debug.Assert(set1[key]); // real assert - all values must be "true"
                if (!dictionary2.ContainsKey(key))
                {
                    return false;
                }
                else
                {
                    // Debug.Assert(set2[key]); // real assert - all values must be "true"
                }
            }
            return true;
        }


        /// <summary>
        /// Shifts the bits of an int around in a wrapped way. It is useful for creating hashcodes of collections.
        /// </summary>
        /// <param name="someInt">the int to shift</param>
        /// <param name="count">The number of bits to shift the int</param>
        /// <returns>The shifted int.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow", MessageId = "32-count")]
        public static int WrapAroundLeftShift(int someInt, int count)
        {
            //Tip: Use "?Convert.ToString(WrapAroundLeftShift(someInt,count),2)" to see this work
            int result = (someInt << count) | ((~(-1 << count)) & (someInt >> (8 * sizeof(int) - count)));
            return result;
        }
    }
}
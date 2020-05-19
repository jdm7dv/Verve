// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

/****************************************************************************
 * Utility.cs
 * 
 *   This file contains the all the common functions in the automation test cases.
 * 
***************************************************************************/

using System;
using System.Globalization;
using System.IO;
using System.Linq;

namespace MBF.TestAutomation.Util
{
    /// <summary>
    /// This class contains the all the common functions/variables used by all the automation test cases.
    /// </summary>
    internal class Utility
    {
        internal XmlUtility _xmlUtil;

        /// <summary>
        /// Constructor which sets the filepath
        /// </summary>
        /// <param name="filePath"></param>
        internal Utility(string filePath)
        {
            _xmlUtil = new XmlUtility(filePath);
        }

        /// <summary>
        /// Gets the IAlphabet for the alphabet string passed.
        /// </summary>
        /// <param name="alphabet">Protein/Dna/Rna</param>
        /// <returns>IAphabet equivalent.</returns>
        internal static IAlphabet GetAlphabet(string alphabet)
        {
            IAlphabet alp = null;
            switch (alphabet.ToLower(CultureInfo.CurrentCulture))
            {
                case "protein":
                    alp = Alphabets.Protein;
                    break;
                case "rna":
                    alp = Alphabets.RNA;
                    break;
                case "dna":
                    alp = Alphabets.DNA;
                    break;
                default:
                    break;
            }

            return alp;
        }

        /// <summary>
        /// Gets the MoleculeType for the molecule type string passed.
        /// </summary>
        /// <param name="molType">Protein/Dna/Rna</param>
        /// <returns>MoleculeType equivalent.</returns>
        internal static MoleculeType GetMoleculeType(string molType)
        {
            MoleculeType mol = MoleculeType.Invalid;

            switch (molType.ToLower(CultureInfo.CurrentCulture))
            {
                case "protein":
                    mol = MoleculeType.Protein;
                    break;
                case "rna":
                    mol = MoleculeType.RNA;
                    break;
                case "dna":
                    mol = MoleculeType.DNA;
                    break;
                default:
                    break;
            }

            return mol;
        }

        /// <summary>
        /// Gets the FastQFormatType for the format passed.
        /// </summary>
        /// <param name="formatType">Illumina/Sanger/Solexa</param>
        /// <returns>FastQFormat</returns>
        internal static FastQFormatType GetFastQFormatType(string formatType)
        {
            FastQFormatType format = FastQFormatType.Illumina;

            switch (formatType)
            {
                case "Illumina":
                    format = FastQFormatType.Illumina;
                    break;
                case "Sanger":
                    format = FastQFormatType.Sanger;
                    break;
                case "Solexa":
                    format = FastQFormatType.Solexa;
                    break;
                default:
                    break;
            }

            return format;
        }

        /// <summary>
        /// Generate random number array inside supplied max range
        /// </summary>
        /// <param name="maxRange">Max value of random number</param>
        /// <param name="count">Return array size.</param>
        /// <returns>Array of Random numbers</returns>
        internal static int[] RandomNumberGenerator(int maxRange, int count)
        {
            int[] randomNumbers = new int[count];

            int index = 0;
            while (index < randomNumbers.Length)
            {
                Random rndNumberGenerator = new Random();
                int rndNumber = rndNumberGenerator.Next(maxRange);

                // Add the unique number to the list
                if (!randomNumbers.Contains(rndNumber))
                {
                    randomNumbers[index] = rndNumber;
                    index++;
                }
            }
            return randomNumbers;
        }

        /// <summary>
        /// Gets the file content for the file path passed. 
        /// If the file doesnt exist throw the exception.
        /// </summary>
        /// <param name="filePath">File path, the content of which to be read.</param>
        /// <returns>Content of the Text file.</returns>
        internal static string GetFileContent(string filePath)
        {
            string fileContent = string.Empty;

            // Check if the File path exists, if not throw exception.
            if (File.Exists(filePath))
            {
                using (StreamReader textFile = new StreamReader(filePath))
                {
                    fileContent = textFile.ReadToEnd();
                }
            }
            else
            {
                throw new FileNotFoundException(string.Format((IFormatProvider)null, "File '{0}' not found.", filePath));
            }

            return fileContent;
        }
    }
}

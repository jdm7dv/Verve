// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;

namespace MBF
{
    /// <summary>
    /// Interface that defines a converter for the given list of patterns into locally 
    /// understood patterns. i.e., converting a prosite pattern to simple 
    /// Protein/Dna/Rna sequence/string.
    /// </summary>
    public interface IPatternConverter
    {
        /// <summary>
        /// Convert the given list of patterns into locally understood patterns.
        /// To simple Protein/Dna/Rna
        /// </summary>
        /// <param name="patterns">List of patterns to be converted.</param>
        /// <returns>Converted list of patterns.</returns>
        IDictionary<string, IList<string>> Convert(IList<string> patterns);

        /// <summary>
        /// Convert the given pattern into locally understood patterns.
        /// To simple Protein/Dna/Rna
        /// </summary>
        /// <param name="pattern">Pattern to be converted.</param>
        /// <returns>Converted list of patterns.</returns>
        IList<string> Convert(string pattern);
    }
}
// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;
using System.IO;

namespace MBF.Web.Blast
{
    /// <summary>
    /// This interface defines the contract that has to be implemented by a class
    /// that parse an output from blast service.
    /// Blast service can be in different format e.g., text / xml
    /// </summary>
    public interface IBlastParser
    {
        /// <summary>
        /// Read BLAST data from the reader, and build one or more BlastRecordGroup 
        /// objects (each containing one or more BlastSearchRecord results).
        /// </summary>
        /// <param name="reader">Blast data source</param>
        /// <returns>A list of BLAST iteration objects</returns>
        IList<BlastResult> Parse(TextReader reader);
    }
}
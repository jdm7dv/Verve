// *********************************************************
// 
//     Copyright (c) Microsoft. All rights reserved.
//     This code is licensed under the Apache License, Version 2.0.
//     THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//     ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//     IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//     PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// 
// *********************************************************
namespace SequenceAssembler
{
    #region -- Using Directive --

    using System.Collections.Generic;
    using Bio.Web.Blast;

    #endregion -- Using Directive --

    /// <summary>
    /// Interface which will be implemented by the component
    /// which will handle web services required by Sequence Assemblers
    /// </summary>
    public interface IWebServicePresenter
    {
        /// <summary>
        /// This method will display BLAST webservice output
        /// on the UI.
        /// </summary>
        /// <param name="results">Blast results.</param>
        /// <param name="blastService">Service handler for the blast</param>
        /// <param name="databaseName">Database name used for the blast</param>
        void DisplayWebServiceOutput(IList<BlastResult> results, IBlastServiceHandler blastService, string databaseName);
    }
}

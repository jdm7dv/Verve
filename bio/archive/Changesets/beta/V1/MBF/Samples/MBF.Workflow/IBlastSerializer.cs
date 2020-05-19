// -------------------------------------------------------------------------------------
// <copyright file="IBlastSerializer.cs" company="Microsoft">
//  Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// <summary>
// IBlastSerializer interface defines member which takes 
// the output of BLAST and serializes the output, and returns 
// the serialized stream. 
// </summary>
// -------------------------------------------------------------------------------------
namespace MBF.Workflow
{
    #region -- Using Directive --

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using MBF;
    using MBF.Algorithms.Assembly;
    using MBF.Web.Blast;

    #endregion -- Using Directive --

    /// <summary>
    /// IBlastSerializer interface defines member which takes 
    /// the output of BLAST and serializes the output, and returns 
    /// the serialized stream. 
    /// </summary>
    public interface IBlastSerializer
    {
        /// <summary>
        /// Gets the serializer type used for serialization
        /// </summary>
        string SerializerType { get; }

        /// <summary>
        /// This method would serialize and return the serialized stream
        /// </summary>
        /// <param name="result">Collection of blast hit results</param>
        /// <returns>Serialized stream</returns>
        Stream SerializeBlastOutput(IList<BlastResultCollator> result);
    }
}

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
using System.Collections.Generic;

namespace Bio.Algorithms.Alignment
{
    /// <summary>
    /// Aligners will implements this interface to list the attributes supported
    /// or required.
    /// </summary>
    public interface IAlignmentAttributes
    {
        /// <summary>
        /// Gets list of attributes.
        /// </summary>
        Dictionary<string, AlignmentInfo> Attributes { get; }
    }
}
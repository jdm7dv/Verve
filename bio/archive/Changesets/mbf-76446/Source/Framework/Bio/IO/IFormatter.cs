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
namespace Bio.IO
{
    /// <summary>
    /// Interface that defines the common properties for a formatter.
    /// All other formatters must extend this Interface.
    /// </summary>
    public interface IFormatter
    {
        /// <summary>
        /// Gets the name of the formatter being implemented.
        /// This is intended to give the developer name of the formatter.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the description of the formatter being implemented.
        /// This is intended to give the developer some 
        /// information of the formatter.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the file extensions that the formatter will support.
        /// If multiple extensions are supported then this property 
        /// will return a string containing all extensions with a ',' delimited.
        /// </summary>
        string SupportedFileTypes { get; }
    }
}

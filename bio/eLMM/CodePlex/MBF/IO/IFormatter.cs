//*********************************************************
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//
//
//
//
//
//*********************************************************



using Bio.Encoding;
using Bio.Properties;

namespace Bio.IO
{
    /// <summary>
    /// Interface that defines the common properties for a formatter.
    /// All other formatters must extend this Interface
    /// </summary>
    public interface IFormatter
    {
        /// <summary>
        /// Gets the name of the sequence alignment formatter being
        /// implemented. This is intended to give the developer some
        /// information of the formatter type.
        /// </summary>
        string Name {get;}

        /// <summary>
        /// Gets the description of the sequence alignment formatter being
        /// implemented. This is intended to give the developer some 
        /// information of the formatter.
        /// </summary>
        string Description {get;}

        /// <summary>
        /// Gets the file extensions that the formatter implementation
        /// will support.
        /// </summary>
        string FileTypes { get; }
    }
}

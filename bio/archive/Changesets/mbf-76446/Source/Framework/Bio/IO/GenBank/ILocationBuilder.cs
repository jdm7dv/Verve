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
namespace Bio.IO.GenBank
{
    /// <summary>
    /// Interface to build the location from location string and from location object to location string.
    /// </summary>
    public interface ILocationBuilder
    {
        /// <summary>
        /// Returns the location object for the specified location string.
        /// </summary>
        /// <param name="location">Location string.</param>
        ILocation GetLocation(string location);

        /// <summary>
        /// Returns the location string for the specified location.
        /// </summary>
        /// <param name="location">Location instance.</param>
        string GetLocationString(ILocation location);
    }
}

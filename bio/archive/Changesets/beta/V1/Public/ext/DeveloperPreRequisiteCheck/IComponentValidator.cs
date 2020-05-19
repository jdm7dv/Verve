// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;

namespace DeveloperPreRequisiteCheck
{
    /// <summary>
    /// This interface has to be implemented for each component that has to be validated. 
    /// Defines method and properties required to implement a validator
    /// </summary>
    public interface IComponentValidator
    {
        /// <summary>
        /// Gets the name of component.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the short description of component.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets the minimum supported version of component.
        /// </summary>
        string Version { get; }

        /// <summary>
        /// Gets the parameter required by Validator component.
        /// </summary>
        Dictionary<string, string> Parameters { get; }

        /// <summary>
        /// Validate if the component is installed.
        ///  1. If not, provide a message to install the component.
        ///  2.	If yes, provide a message directing user to copy the folders/assemblies to required target folder.
        /// </summary>
        /// <returns>Validation result.</returns>
        ValidationResult Validate();
    }
}
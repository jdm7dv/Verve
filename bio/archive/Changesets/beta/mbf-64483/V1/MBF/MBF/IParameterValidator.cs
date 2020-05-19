// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

namespace MBF
{
    /// <summary>
    /// A simple interface to an object that can check a value
    /// for conformance to any required validation rules.
    /// </summary>
    public interface IParameterValidator
    {
        /// <summary>
        /// Given a value as an object, return true if the value is allowed.
        /// </summary>
        /// <param name="parameterValue">The value.</param>
        /// <returns>True if the value is valid.</returns>
        bool IsValid(object parameterValue);

        /// <summary>
        /// Given a value in string form, return true if the value is allowed.
        /// </summary>
        /// <param name="parameterValue">The value.</param>
        /// <returns>True if the value is valid.</returns>
        bool IsValid(string parameterValue);
    }
}

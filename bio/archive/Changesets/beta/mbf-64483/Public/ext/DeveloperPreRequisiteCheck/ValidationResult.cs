// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

namespace DeveloperPreRequisiteCheck
{
    /// <summary>
    /// This class contains the result of validation.
    /// </summary>
    public class ValidationResult
    {
        /// <summary>
        /// validation Message
        /// </summary>
        private string _message;

        /// <summary>
        /// validation Result
        /// </summary>
        private bool _result;

        /// <summary>
        /// Constructor: Sets the values of validation result.
        /// </summary>
        /// <param name="result">validation Result.</param>
        /// <param name="message">validation Message.</param>
        public ValidationResult(bool result, string message)
        {
            _result = result;
            _message = message;
        }

        /// <summary>
        /// Gets the message returned by validation.
        /// </summary>
        public string Message { get { return _message; } }

        /// <summary>
        /// Gets a value indicating whether validation is successful.
        /// </summary>
        public bool Result { get { return _result; } }
    }
}
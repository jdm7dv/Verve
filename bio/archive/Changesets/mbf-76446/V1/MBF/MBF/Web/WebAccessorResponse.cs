// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

namespace MBF.Web
{
    /// <summary>
    /// Represent an object containing the result of web request.
    /// </summary>
    public class WebAccessorResponse
    {
        /// <summary>
        /// Gets or sets HTTP status string returned from the operation
        /// </summary>
        public string StatusDescription { get; set; }

        /// <summary>
        /// Gets or sets response as a single string
        /// </summary>
        public string ResponseString { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the resquest was successful
        /// </summary>
        public bool IsSuccessful { get; set; }
    }
}
// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

namespace MBF.Web.Blast
{
    /// <summary>
    /// A data structure returned from validation of a set of parameters, allowing
    /// the caller to either diagnose validation errors, or proceed with a service call.
    /// </summary>
    public class ParameterValidationResult
    {
        #region Properties

        /// <summary>
        /// True means that the parameters are valid for the service of interest.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// If a service requires a parameters object of a certain class, the validation
        /// routine can populate and set this object for the caller's use (on success).
        /// </summary>
        public object ParametersObject { get; set; }

        /// <summary>
        /// A human-readable summary of errors found during an unsuccessful validation.
        /// </summary>
        public string ValidationErrors { get; set; }

        #endregion
    }
}

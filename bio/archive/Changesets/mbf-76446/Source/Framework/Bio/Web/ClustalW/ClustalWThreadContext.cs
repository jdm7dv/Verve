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
namespace Bio.Web.ClustalW
{
    /// <summary>
    /// This class has the list of properties that needs to be passed on to
    /// the BackGroundWorker thread to execute the ClustalW request.
    /// </summary>
    public class ClustalWThreadContext
    {
        /// <summary>
        /// ClustalW Service parameters object
        /// </summary>
        private ServiceParameters parameters;

        /// <summary>
        /// Initializes a new instance of the ClustalWThreadContext class. 
        /// </summary>
        /// <param name="parameters">ClustalW Service</param>
        public ClustalWThreadContext(
                ServiceParameters parameters)
        {
            this.parameters = parameters;
        }

        /// <summary>
        /// Gets the ClustalW Service parameters
        /// </summary>
        public ServiceParameters Parameters
        {
            get { return parameters; }
        }
    }
}
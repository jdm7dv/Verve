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

namespace Bio.Web.ClustalW
{
    /// <summary>
    /// Contains the list of parameter obtained as result of initiating an service request
    /// with web service.
    /// </summary>
    public class ServiceParameters
    {
        /// <summary>
        /// Job Identifier Key
        /// </summary>
        private const string JobIdKey = "JOBID";

        /// <summary>
        /// Contains key value pair of
        /// </summary>
        private Dictionary<string, object> parameters;

        /// <summary>
        /// Initializes a new instance of the ServiceParameters class.
        /// Constructor: Initialize class fields
        /// </summary>
        public ServiceParameters()
        {
            parameters = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or sets the Job Identifer of Service Request
        /// </summary>
        public string JobId
        {
            get
            {
                if (parameters.ContainsKey(JobIdKey))
                {
                    return parameters[JobIdKey].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }

            set 
            { 
                parameters[JobIdKey] = value; 
            }
        }

        /// <summary>
        /// Gets the key value dictionary of service parameters
        /// </summary>
        public Dictionary<string, object> Parameters
        {
            get { return parameters; }
        }
    }
}
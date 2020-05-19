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



using System;
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
        private Dictionary<string, object> _parameters;

        /// <summary>
        /// Initializes a new instance of the ServiceParameters class.
        /// Constructor: Initialize class fields
        /// </summary>
        public ServiceParameters()
        {
            _parameters = new Dictionary<string, object>();
        }

        /// <summary>
        /// Gets or sets the Job Identifer of Service Request
        /// </summary>
        public string JobId
        {
            get
            {
                if (_parameters.ContainsKey(JobIdKey))
                {
                    return _parameters[JobIdKey].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }

            set 
            { 
                _parameters[JobIdKey] = value; 
            }
        }

        /// <summary>
        /// Gets the key value dictionary of service parameters
        /// </summary>
        public Dictionary<string, object> Parameters
        {
            get { return _parameters; }
        }
    }
}

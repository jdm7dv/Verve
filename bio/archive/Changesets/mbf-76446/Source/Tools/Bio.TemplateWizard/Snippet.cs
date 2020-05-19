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

namespace Bio.TemplateWizard
{
    /// <summary>
    /// Class which will hold information about a snippet
    /// </summary>
    public class Snippet
    {
        /// <summary>
        /// Tag of the snippet
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Namespaces to be included for this snippet to compile
        /// </summary>
        public IList<string> Namespaces { get; set; }

        /// <summary>
        /// Assembly references required for this snippet
        /// </summary>
        public string Assembly { get; set; }

        /// <summary>
        /// Code Snippet
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Initializes a new instance of the Snippet class.
        /// </summary>
        public Snippet()
        {
            Namespaces = new List<string>();
        }
    }
}

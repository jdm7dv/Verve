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
using Bio.Util;

namespace Bio.IO.SAM
{
    /// <summary>
    /// This class holds SAM optional field.
    /// </summary>
    [Serializable]
    public class SAMOptionalField
    {
        /// <summary>
        /// Tag of the option field.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Type of the value present in the "Value" property.
        /// </summary>
        public string VType { get; set; }

        /// <summary>
        /// Value of the optional field.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Validates this optional field.
        /// </summary>
        /// <returns>Returns empty string if valid; otherwise error message.</returns>
        public string IsValid()
        {
            string message = string.Empty;
            message = Helper.IsValidPatternValue("Tag", Tag, "[A-Z][A-Z0-9]");
            if (!string.IsNullOrEmpty(message))
            {
                return message;
            }

            message = Helper.IsValidPatternValue("VType", VType, "[AifZH]");
            if (!string.IsNullOrEmpty(message))
            {
                return message;
            }

            message = Helper.IsValidPatternValue("Value", Value, "[^\t\n\r]+");
            if (!string.IsNullOrEmpty(message))
            {
                return message;
            }

            return string.Empty;
        }
    }
}

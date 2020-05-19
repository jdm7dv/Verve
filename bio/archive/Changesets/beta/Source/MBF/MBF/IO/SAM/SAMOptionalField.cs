// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using MBF.Util;
using System.Text.RegularExpressions;

namespace MBF.IO.SAM
{
    /// <summary>
    /// This class holds SAM optional field.
    /// </summary>
    [Serializable]
    public class SAMOptionalField
    {
        /// <summary>
        /// Holds regular expression pattern of Tag.
        /// </summary>
        private const string TagRegexExprPattern = "[A-Za-z][A-Za-z0-9]";

        /// <summary>
        /// Holds regular expression pattern of Vtype.
        /// </summary>
        private const string VTypeRegexExprPattern = "[AifZH]";

        /// <summary>
        /// Holds regular expression pattern of value.
        /// </summary>
        private const string ValueRegexExprPattern = "[^\t\n\r]+";

        /// <summary>
        /// Holds regular expression for Tag.
        /// </summary>
        private static Regex TagRegexExpr = new Regex(TagRegexExprPattern);

        /// <summary>
        /// Holds regular expression for Vtype.
        /// </summary>
        private static Regex VTypeRegexExpr = new Regex(VTypeRegexExprPattern);

        /// <summary>
        /// Holds regular expression for Value.
        /// </summary>
        private static Regex ValueRegexExpr = new Regex(ValueRegexExprPattern);

        /// <summary>
        /// Holds tag value of the option field.
        /// </summary>
        private string _tag;

        /// <summary>
        /// Holds type of the value present in the "Value" property.
        /// </summary>
        private string _vtype;

        /// <summary>
        /// Holds value of the optional field.
        /// </summary>
        private string _value;

        /// <summary>
        /// Tag of the option field.
        /// </summary>
        public string Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                string message = IsValidTag(value);
                if (!string.IsNullOrEmpty(message))
                {
                    throw new ArgumentException(message);
                }

                _tag = value;
            }
        }

        /// <summary>
        /// Type of the value present in the "Value" property.
        /// </summary>
        public string VType
        {
            get
            {
                return _vtype;
            }
            set
            {
                string message = IsValidVType(value);
                if (!string.IsNullOrEmpty(message))
                {
                    throw new ArgumentException(message);
                }

                _vtype = value;
            }
        }

        /// <summary>
        /// Value of the optional field.
        /// </summary>
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                string message = IsValidValue(value);
                if (!string.IsNullOrEmpty(message))
                {
                    throw new ArgumentException(message);
                }

                _value = value;
            }
        }

        /// <summary>
        /// Validates Tag.
        /// </summary>
        /// <param name="tag">Tag value to validate.</param>
        private static string IsValidTag(string tag)
        {
            return Helper.IsValidPatternValue("Tag", tag, TagRegexExpr);
        }

        /// <summary>
        /// Validates VType.
        /// </summary>
        /// <param name="vtype">VType value to validate.</param>
        private static string IsValidVType(string vtype)
        {
            return Helper.IsValidPatternValue("VType", vtype, VTypeRegexExpr);
        }

        /// <summary>
        /// Validates Value.
        /// </summary>
        /// <param name="value">Value to validate.</param>
        private static string IsValidValue(string value)
        {
            return Helper.IsValidPatternValue("Value", value, ValueRegexExpr);
        }
    }
}

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
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using LISUtil.Utils;

namespace LISUtil.Utils
{
    /// <summary>
    /// A delegate used in error reporting.
    /// </summary>
    public delegate void ErrorReporter(string message);

    /// <summary>
    /// Used to control parsing of command line arguments.
    /// </summary>
    [Flags]
    public enum ArgumentType
    {
        /// <summary>
        /// Indicates that this field is required. An error will be displayed
        /// if it is not present when parsing arguments.
        /// </summary>
        Required = 0x01,

        /// <summary>
        /// Only valid in conjunction with Multiple.
        /// Duplicate values will result in an error.
        /// </summary>
        Unique = 0x02,

        /// <summary>
        /// Indicates that the argument may be specified more than once.
        /// Only valid if the argument is a collection.
        /// </summary>
        Multiple = 0x04,

        /// <summary>
        /// The default type for non-collection arguments.
        /// The argument is not required, but an error will be reported if it is specified more than once.
        /// </summary>
        AtMostOnce = 0x00,

        /// <summary>
        /// For non-collection arguments, when the argument is specified more than
        /// once no error is reported and the value of the argument is the last
        /// value which occurs in the argument list.
        /// </summary>
        LastOccurenceWins = Multiple,

        /// <summary>
        /// The default type for collection arguments.
        /// The argument is permitted to occur multiple times, but duplicate 
        /// values will cause an error to be reported.
        /// </summary>
        MultipleUnique = Multiple | Unique,
    }

    /// <summary>
    /// Allows control of command line parsing.
    /// Attach this attribute to instance fields of types used
    /// as the destination of command line argument parsing.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ArgumentAttribute : Attribute
    {
        private string shortName;
        private string longName;
        private string helpText;
        private object defaultValue;
        private ArgumentType type;

        /// <summary>
        /// Initializes a new instance of the ArgumentAttribute class.
        /// </summary>
        /// <param name="type"> Specifies the error checking to be done on the argument. </param>
        public ArgumentAttribute(ArgumentType type)
        {
            this.type = type;
        }

        /// <summary>
        /// Gets the error checking to be done on the argument.
        /// </summary>
        public ArgumentType Type
        {
            get { return this.type; }
        }

        /// <summary>
        /// Gets a value indicating whether the argument has short name or not. Returns true if the argument did not have an explicit short name specified.
        /// </summary>
        public bool DefaultShortName 
        { 
            get 
            { 
                return null == this.shortName; 
            } 
        }

        /// <summary>
        /// Gets or sets the short name of the argument.
        /// Set to null means use the default short name if it does not
        /// conflict with any other parameter name.
        /// Set to String.Empty for no short name.
        /// This property should not be set for DefaultArgumentAttributes.
        /// </summary>
        public string ShortName
        {
            get 
            { 
                return this.shortName; 
            }

            set 
            { 
                Debug.Assert(value == null || !(this is DefaultArgumentAttribute), "the short name of the argument."); 
                this.shortName = value; 
            }
        }

        /// <summary>
        /// Gets a value indicating whether argument did not have an explicit long name specified.
        /// </summary>
        public bool DefaultLongName 
        { 
            get 
            { 
                return null == this.longName; 
            } 
        }

        /// <summary>
        /// Gets or sets the long name of the argument.
        /// Set to null means use the default long name.
        /// The long name for every argument must be unique.
        /// It is an error to specify a long name of String.Empty.
        /// </summary>
        public string LongName
        {
            get 
            { 
                Debug.Assert(!this.DefaultLongName, "the long name of the argument."); 
                return this.longName; 
            }

            set 
            { 
                Debug.Assert(!string.IsNullOrEmpty(value), "null means use the default long name.");
                this.longName = value; 
            }
        }

        /// <summary>
        /// Gets or sets the default value of the argument.
        /// </summary>
        public object DefaultValue
        {
            get { return this.defaultValue; }
            set { this.defaultValue = value; }
        }

        /// <summary>
        /// Gets a value indicating whether the argument has a default value.
        /// </summary>
        public bool HasDefaultValue 
        { 
            get 
            { 
                return null != this.defaultValue; 
            } 
        }

        /// <summary>
        /// Gets a value indicating whether the argument has help text specified.
        /// </summary>
        public bool HasHelpText 
        { 
            get 
            { 
                return null != this.helpText; 
            } 
        }

        /// <summary>
        /// Gets or sets the help text for the argument.
        /// </summary>
        public string HelpText
        {
            get 
            { 
                return this.helpText; 
            }

            set 
            { 
                this.helpText = value; 
            }
        }
    }
}

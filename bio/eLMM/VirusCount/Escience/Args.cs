//*********************************************************
//
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Apache License, Version 2.0.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************

using System;
using System.Reflection;
using System.Text;
using MBT.Escience.Parse;
using Bio.Util;

namespace MBT.Escience
{
    /// <summary>
    /// This is the base class for self-parsing args classes. 
    /// </summary>
    /// <remarks>
    /// DIRECTIONS:
    /// To create a self-parsing args class, derive from Args, then specify the optional args/flags, followed by the required args. 
    /// For optional args, the names will be what the user must specify on the commandline. Note that commandline parsing is NOT case sensitive.
    /// The default values for optional args will be taken from the code. In the example below, default values are false, "EM", and 0.05, respectively.
    /// If the type of an optional arg is a bool, then the uses specifies it "flag" style. In the example below, the user sets cleanup to true using -cleanup
    /// with no argument. 
    /// 
    /// All required arguments must follow the optional arguments, and must be specified in the order in which the user must enter them. A public field
    /// named END_OPTIONAL (of any type) must separate the optional and required args. The system uses this field as a marker, such that all fields
    /// before the marker are considered optional and all after are considered required.
    /// 
    /// *** IMPORTANT: The END_OPTIONAL field must lie between the optional and required args. An exception will be thrown if this field doesn't exist. 
    /// *** IMPORTANT: All optional arguments and flags must preceed all required arguments in the file.
    /// *** IMPORTANT: The required arguments will be parsed in the order in which they are defined.
    /// *** IMPORTANT: Only public fields will be automatically parsed. Protected, private, and static fields, will not be parsed, nor will Properties.
    /// 
    /// EXAMPLE:
    ///  // OPTIONAL arguments must be before required arguments. Set default values.
    ///  public bool cleanup = false;
    ///  public string Optimizer = "EM";
    ///  public double MaxPValue = 0.05;
    /// 
    ///  public bool END_OPTIONAL; // specifies that all preceding fields are optional, everything from here on is required. note that the type doesn't matter.
    /// 
    ///  // REQUIRED arguments must follow in the order in which they should be parsed
    ///  public string PhyloTreeFileName;
    ///  public string PredictorSparseFileName;
    /// 
    /// 
    ///  // NOT COMMANDLINE EXPOSED. Use private/protected fields for things you don't want parsed, or which will be computed automatically from parsed fields.
    ///  // to expose them, use public Properties, which do not trigger automatic parsing.
    ///  private RangeCollection _skipRowIndexRange = null;
    ///  private string _baseFileName = null;
    ///  private string _outputFileName = null;
    /// </remarks>
    [Obsolete("This has been replaced by ArgumentCollection.Construct<T>()")]
    public abstract class Args
    {
        protected Args() { }
        protected Args(string args) : this(new CommandArguments(args)) { }
        protected Args(string[] args) : this(new CommandArguments(args)) { }

        protected Args(ArgumentCollection args)
        {
            LoadArguments(args);
        }

        private int _numOptional = -1;
        protected int NumOptionalArgs
        {
            get
            {
                if (_numOptional < 0)
                {
                    FieldInfo[] fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
                    for (_numOptional = 0; _numOptional < fields.Length; _numOptional++)
                        if (IsOptionsBoundary(fields[_numOptional]))
                            break;
                    Helper.CheckCondition(_numOptional < fields.Length, "The list of optional arguments must end with a public field named END_OPTIONAL.");
                }
                return _numOptional;
            }
        }

        private bool IsOptionsBoundary(FieldInfo field)
        {
            return field.Name.Equals("END_OPTIONAL");
        }

        /// <summary>
        /// Checks that all constraints are met. Throws exception on first failure.
        /// </summary>
        public virtual void CheckConstraints() { }

        public virtual void LoadArguments(ArgumentCollection argCollection)
        {
            LoadOptionsOnly(argCollection);

            FieldInfo[] fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            argCollection.CheckNoMoreOptions(fields.Length - NumOptionalArgs - 1); // -1 accounts for END_OPTIONAL

            for (int i = NumOptionalArgs + 1; i < fields.Length; i++)
            {
                try
                {
                    LoadRequiredParameter(fields[i], argCollection);
                }
                catch (Exception e)
                {
                    throw new ArgumentException("Error parsing required argument." + e.InnerException.Message);
                }
            }

            argCollection.CheckThatEmpty();
            CheckConstraints();
        }

        public virtual void LoadOptionsOnly(ArgumentCollection argCollection)
        {
            FieldInfo[] fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            for (int i = 0; i < NumOptionalArgs; i++)
            {
                FieldInfo field = fields[i];
                string flag = field.Name;
                object value = field.GetValue(this);
                bool isFlag = value is bool;

                //try
                {
                    if (isFlag)
                    {
                        LoadFlag(field, argCollection);
                    }
                    else
                    {
                        LoadOption(field, argCollection);
                    }
                }
                //catch (Exception e)
                //{
                //    //throw new ArgumentException("Error parsing optional flag " + field.Name + ". " + e.InnerException.Message);
                //}
            }
        }

        public ArgumentCollection ToArgCollection()
        {
            return new CommandArguments(ToString());
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(30);

            int fieldNumber = 0;
            foreach (System.Reflection.FieldInfo field in this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                Helper.CheckCondition(!field.IsStatic, "shouldn't be possible.");
                if (IsOptionsBoundary(field))
                    continue;

                // the first field members will be the flags and options.
                if (fieldNumber++ < NumOptionalArgs)
                {
                    string flag = field.Name;
                    object value = field.GetValue(this);
                    bool isFlag = value is bool;

                    if (value != null)
                    {
                        if (isFlag && (bool)value)
                        {
                            sb.Append(" -" + flag);
                        }
                        else if (!isFlag && value != null)
                        {
                            string sValue = SpecialFunctions.ResolveValueToString(value);

                            sb.Append(" -" + flag + " " + sValue);
                        }
                    }
                }
                else // these are required
                {
                    sb.Append(" " + field.GetValue(this));
                }
            }

            return sb.ToString();
        }



        public override bool Equals(object obj)
        {
            return obj.ToString().Equals(this.ToString(), StringComparison.CurrentCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return this.ToString().ToLower().GetHashCode();
        }



        private void LoadRequiredParameter(System.Reflection.FieldInfo field, ArgumentCollection argCollection)
        {
            object fieldValue = field.GetValue(this);
            MethodInfo argCollectionExtractOption = argCollection.GetType().GetMethod("ExtractNext");
            MethodInfo genericExtractOption = argCollectionExtractOption.MakeGenericMethod(field.FieldType);

            object[] args = new object[] { field.Name };

            object value = genericExtractOption.Invoke(argCollection, args);
            field.SetValue(this, value);
        }

        private void LoadOption(System.Reflection.FieldInfo field, ArgumentCollection argCollection)
        {
            object fieldValue = field.GetValue(this);
            MethodInfo argCollectionExtractOption = argCollection.GetType().GetMethod("ExtractOptional");
            MethodInfo genericExtractOption = argCollectionExtractOption.MakeGenericMethod(field.FieldType);

            object[] args = new object[] { field.Name, fieldValue };

            object newValue = genericExtractOption.Invoke(argCollection, args);
            field.SetValue(this, newValue);
        }

        private void LoadFlag(System.Reflection.FieldInfo field, ArgumentCollection argCollection)
        {
            bool value = (bool)argCollection.ExtractOptionalFlag(field.Name);
            field.SetValue(this, value);
        }
    }
}

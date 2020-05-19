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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Bio.Util;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Collections.Concurrent;

namespace MBT.Escience.Parse
{
    /// <summary>
    /// Supports declarative and strongly typed parsing. Use Construct() to convert an argument collection to an instance of an object
    /// </summary>
    public abstract class ArgumentCollection : ICloneable, IEnumerable<string>
    {
        private const string NO_DOCUMENTATION_STRING = "[No documentation]";
        private List<string> _argList;
        public string SubtypeName { get; set; }


        public int Count
        {
            get
            {
                return _argList.Count;
            }
        }

        /// <summary>
        /// Enumerates all flag-value pairs. In case in which there are back to back flags, the first flag is enumerated with null as the value.
        /// </summary>
        public IEnumerable<KeyValuePair<string, string>> FlagValuePairs
        {
            get
            {
                for (int i = 0; i < _argList.Count - 1; i++)
                {
                    if (IsFlag(_argList[i]))
                    {
                        if (IsFlag(_argList[i + 1]))
                            yield return new KeyValuePair<string, string>(_argList[i], null);
                        else
                            yield return new KeyValuePair<string, string>(_argList[i++], _argList[i]);
                    }
                }
            }
        }

        protected ArgumentCollection(IEnumerable<string> argList)
        {
            _argList = argList.ToList();
        }

        protected ArgumentCollection(string lineToParse)
        {
            ParseString(lineToParse);
        }

        private void ParseString(string lineToParse)
        {
            SubtypeName = ExtractSubtypeName(ref lineToParse);
            _argList = CreateArgList(lineToParse).ToList();
        }

        abstract protected string CreateUsageString(IEnumerable<MemberInfo> requireds, MemberInfo requiredParamsOrNull, Type constructingType);

        abstract protected string ExtractSubtypeName(ref string lineToParse);

        abstract protected IEnumerable<string> CreateArgList(string lineToParse);

        abstract protected bool ExtractOptionalFlagInternal(string flag, bool removeFlag);

        abstract public bool MatchesFlag(string query, string flagBase);

        abstract public bool IsFlag(string query);

        abstract protected string CreateFlagString(string flagBase);

        abstract public void AddOptionalFlag(string argumentName);


        public bool ExtractOptionalFlag(string flag)
        {
            return ExtractOptionalFlagInternal(flag, true);
        }

        public bool PeekOptionalFlag(string flag)
        {
            return ExtractOptionalFlagInternal(flag, false);
        }

        public T ExtractOptional<T>(string flag, T defaultValue)
        {
            return ExtractOptionalInternal<T>(flag, defaultValue, true, null);
        }

        public T PeekOptional<T>(string flag, T defaultValue)
        {
            return ExtractOptionalInternal<T>(flag, defaultValue, false, null);
        }

        protected T ExtractOptionalInternal<T>(string flag, T defaultValue, bool removeFlagAndValue, string defaultParseArgsOrNull)
        {
            int argIndex = FindFlag(flag);

            if (argIndex == -1)
            {
                return defaultValue;
            }

            Helper.CheckCondition<ParseException>(argIndex < _argList.Count - 1, @"Expect a value after ""{0}""", flag);

            if (removeFlagAndValue)
                RemoveAt(argIndex);
            else
                argIndex++;

            return ExtractAtInternal<T>(flag, argIndex, defaultParseArgsOrNull, removeFlagAndValue);

            //T t;
            //CheckForHelp<T>(_argList[argIndex + 1]);

            //if (!string.IsNullOrWhiteSpace(defaultParseArgsOrNull))
            //    AddArgsToConstructorArgsAt(defaultParseArgsOrNull, argIndex + 1);

            //if (!Parser.TryParse(_argList[argIndex + 1], out t))
            //    throw new ParseException(@"Expect value after ""{0}"" to be an {1}. Read {2}", flag, typeof(T), _argList[argIndex + 1]);

            //if (removeFlagAndValue)
            //{
            //    RemoveAt(argIndex);
            //    RemoveAt(argIndex);
            //}

            //return t;

        }

        public T ExtractAt<T>(string argumentName, int argPosition)
        {
            return ExtractAtInternal<T>(argumentName, argPosition, null);
        }

        protected T ExtractAtInternal<T>(string argumentName, int argPosition, string defaultParseArgsOrNull, bool removeValue = true)
        {
            Helper.CheckCondition<ParseException>(_argList.Count > argPosition, "Expect {0} at position {1}. Only {2} arguments remain to be parsed.", argumentName, argPosition, Count);
            T t;
            CheckForHelp<T>(_argList[argPosition]);

            if (!string.IsNullOrWhiteSpace(defaultParseArgsOrNull)) // we know we're parsing via ConstructorArguments. 
            {
                if (!defaultParseArgsOrNull.StartsWith("(")) defaultParseArgsOrNull = "(" + defaultParseArgsOrNull + ")";
                ConstructorArguments defaultArgs = new ConstructorArguments(defaultParseArgsOrNull);
                ConstructorArguments baseArgs = new ConstructorArguments(_argList[argPosition]);
                t = baseArgs.Construct<T>(defaultArgsOrNull: defaultArgs, checkComplete: true);
            }
            else
            {
                Parser.TryParse(_argList[argPosition], out t).Enforce<ParseException>(@"Expect value for ""{0}"" to be a {1}. Read {2}", argumentName, typeof(T).ToTypeString(), _argList[argPosition]);
            }

            if (removeValue)
                RemoveAt(argPosition);

            return t;
        }



        private void AddArgsToConstructorArgsAt(string constructorStyleArgs, int argPosition)
        {
            string oldArg = _argList[argPosition];
            bool oldArgIsEmpty = oldArg.Length == 0;
            int insertionPoint = 0;

            // if this is of the form (args) or SubtypeName(args), then we need to add write inside the parens.
            if (oldArg.EndsWith(")"))
            {
                insertionPoint = oldArg.IndexOf("(") + 1;
                oldArgIsEmpty = oldArg.Length - insertionPoint - 1 == 0;
            }
            else // if more than one argument, need to have parens so that Constructor args can parse it.
            {
                oldArg = "(" + oldArg + ")";
                insertionPoint = 1;
            }

            string commaOrEmpty = oldArgIsEmpty ? "" : ",";

            string newArg = oldArg.Substring(0, insertionPoint) + constructorStyleArgs + commaOrEmpty + oldArg.Substring(insertionPoint);

            _argList[argPosition] = newArg;
        }

        public void CheckThatEmpty()
        {
            if (_argList.Count != 0)
                throw new ParseException(@"Unknown arguments found. Check the spelling of flags. {0}", _argList.StringJoin(" "));
        }

        public void CheckNoMoreOptions(int? numberOfRequiredArgumentsOrNull, string parseObjectTypeOrNull = null)
        {
            //!!! hack. Want to find flags, but -1-9 isn't a flag, it's a range. So for now, look only for flags that don't start
            //with a number.
            //Regex regex = new Regex(@"^-[,\D]");
            foreach (string arg in _argList)
            {
                if (IsFlag(arg)) throw new ParseException("Unknown option found: {0}", arg);
                //SpecialFunctions.Helper.CheckCondition<ParseException>(!arg.StartsWith("-"), string.Format(@"Unknown option found, {0}", arg));
                //SpecialFunctions.Helper.CheckCondition<ParseException>(!arg.StartsWith("/"), @"Unknown option found, {0}", arg);
                //SpecialFunctions.Helper.CheckCondition<ParseException>(!regex.IsMatch(arg), @"Unknown option found, {0}", arg);
            }

            if (null != numberOfRequiredArgumentsOrNull)
            {
                if (_argList.Count != (int)numberOfRequiredArgumentsOrNull)
                    throw new ParseException(string.Format("{3}Expected {0} required arguments after parsing named arguments (which may include required), but there are {1}:\n{2}",
                        numberOfRequiredArgumentsOrNull, _argList.Count, ToString(), string.IsNullOrEmpty(parseObjectTypeOrNull) ? "" : string.Format("Error parsing {0}: ", parseObjectTypeOrNull)));
            }
        }



        public void ForceOptionalFlag(string optionalFlag)
        {
            this.ExtractOptionalFlag(optionalFlag);
            this.AddOptionalFlag(optionalFlag);
        }

        public void ForceOptional<T>(string argumentName, T argumentValue)
        {
            this.ExtractOptional<T>(argumentName, argumentValue);
            this.AddOptional(argumentName, argumentValue);
        }

        public T ExtractNext<T>(string argumentName)
        {
            Helper.CheckCondition<ParseException>(_argList.Count > 0, @"Expect ""{0}"" value", argumentName);


            return ExtractAt<T>(argumentName, 0);
        }



        public bool ContainsOptionalFlag(string flag)
        {
            return FindFlag(flag) > -1;
        }


        public int FindFlag(string flag)
        {

            for (int i = 0; i < _argList.Count; i++)
            {
                if (MatchesFlag(_argList[i], flag))
                    return i;
            }
            return -1;
        }



        public string[] GetUnderlyingArray()
        {
            return _argList.ToArray();
        }



        public void Insert(int idx, string argumentName)
        {
            _argList.Insert(idx, argumentName);
            //List<string> tmpArgList = new List<string>(_argList);
            //tmpArgList.Add("placeHolderGarbage");
            //for (int j = tmpArgList.Count - 1; j >= n + 1; j--)
            //{
            //    tmpArgList[j] = tmpArgList[j - 1];
            //}
            //tmpArgList[n] = argumentName;
            //_argList = tmpArgList;
        }

        public void Add(object argument)
        {
            _argList.Add((argument ?? "null").ToString());
        }

        public void AddOptional(string argumentName, object argumentValue)
        {
            _argList.Add(CreateFlagString(argumentName));
            _argList.Add((argumentValue ?? "null").ToString());
        }

        public override bool Equals(object obj)
        {
            return obj.GetType().Equals(this.GetType()) && _argList.SequenceEqual(((ArgumentCollection)obj)._argList);
        }

        public override int GetHashCode()
        {
            return _argList.StringJoin(";").GetHashCode();
        }

        #region ICloneable Members

        public object Clone()
        {
            ArgumentCollection result = (ArgumentCollection)MemberwiseClone();
            result._argList = new List<string>(_argList);
            return result;
        }

        #endregion

        #region IEnumerable<string> Members

        public IEnumerator<string> GetEnumerator()
        {
            return _argList.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        protected void RemoveAt(int idx)
        {
            _argList.RemoveAt(idx);
        }

        #region Type construction

        /// <summary>
        /// Constructs T, then runs it. Catches Help and Parse exceptions and write's their messages to Console.Error. 
        /// All other exceptions are allowed to pass on through. Note that if a help exception is caught, the ExitCode
        /// will be set to 10022 (Operation canceled by user); if a parse exception is caught, the ExitCode is set
        /// to 1223 (Invalid argument). In either case, there is no apparent affect on the console, but the cluster
        /// will mark the task as failed.
        /// </summary>
        /// <typeparam name="T">A parsable type that implements IRunnable.</typeparam>
        public void ConstructAndRun<T>() where T : IRunnable
        {
            try
            {
                try
                {
                    T runnable = Construct<T>();
                    runnable.Run();
                }
                catch (Exception e)
                {
                    throw (e.InnerException is ParseException || e.InnerException is HelpException) ? e.InnerException : e;
                }
            }
            catch (HelpException help)
            {
                Console.Error.WriteLine(help.Message);
                Environment.ExitCode = 10022; // "The operation was canceled by the user"
            }
            catch (ParseException parse)
            {
                Console.Error.WriteLine("Parse Error: " + parse.Message);
                Environment.ExitCode = 1223; // "An invalid argument was supplied"
            }
           
        }



        /// <summary>
        /// Constructs an instance of type T from this ArgumentCollection. If SubtypeName is not null, will construct an instance 
        /// of the type SubtypeName, which is constructed by looking in all referenced assemblies for a type with the corresponding name.
        /// If SubtypeName is not a subtype of T, then an exception will be thrown. Whatever type is constructed must have a parameterless 
        /// constructor. Also, the type must have the //[Parsable] attribute. By default all of the type's public fields will be optional parameters. 
        /// Non-public fields can be marked as parsable, as can properties. Public fields can be hidden from parsing. Any field or property can 
        /// be marked as required. Mark fields as properties using the Parse attribute. The three Parse attributers are:
        /// [Parse(ParseAction.Optional)]  (mark as optional. public fields default to this)
        /// [Parse(ParseAction.Required)]  (mark as requried.)
        /// [Parse(ParseAction.Ignore)]    (ignore this public field. has no effect on non-public members or non-fields)
        /// </summary>
        /// <exception cref="HelpException">HelpException is thrown if a help request is encountered. The message will contain the help string.</exception>
        /// <exception cref="ParseException">ParseException is thrown if a type is not able to be parsed.</exception>
        /// <typeparam name="T">The type to construct. If SubtypeName is not null, will attempt to construct an instance of SubtypeName. If that is not 
        /// and instance of T, an exception will be thrown.</typeparam>
        /// <param name="checkComplete">If true, will make sure ArgumentCollection is empty when done parsing and will throw an exception if it is not.
        /// Set to false if you want to construct a class from ArgumentCollection and you expect arguments to be left over.</param>
        /// <returns></returns>
        public T Construct<T>(bool checkComplete = true, ArgumentCollection defaultArgsOrNull = null)
        {
            Type tType = typeof(T);

            object result = CreateInstance<T>();
            if (result == null)
                return default(T);  // which is null, but compiler won't let us return null

            Helper.CheckCondition<ParseException>(result is T, "Constructed an instance of {0}, which is not an instance of {1}", tType.ToTypeString(), typeof(T).ToTypeString());

            ParseInto((T)result, checkComplete, defaultArgsOrNull);

            return (T)result;
        }

        public void ParseInto<T>(T parseResult, bool checkComplete = true, ArgumentCollection defaultArgsOrNull = null)
        {
            object result = parseResult;
            Type tType = result.GetType();   // update type in case we constructed a derived type
            tType.IsConstructable().Enforce("Type {0} does not have a public default constructor and so cannot be parsed.", tType);

            if (HelpIsRequested())
            {
                HelpException helpMsg = CreateHelpMessage(result);
                throw helpMsg;
            }

            AddDefaultArgsIfMissing(defaultArgsOrNull);

            List<MemberInfo> optionals, requireds, constructingStrings;
            MemberInfo requiredParams;
            GetParsableMembers(tType, out optionals, out requireds, out constructingStrings, out requiredParams);

            // if the user wants to know the exact string used to construct this object, set these fields.
            string constString = this.ToString();
            constructingStrings.ForEach(member => SetFieldOrPropertyValue(ref result, member, constString));

            LoadOptionalArguments(ref result, optionals);
            LoadRequiredArguments(ref result, ref requiredParams, requireds, checkComplete && requiredParams == null);
            if (requiredParams != null)
                LoadRequiredParams(ref result, requiredParams);

            if (checkComplete) CheckThatEmpty();

            if (result is IParsable)
                ((IParsable)result).FinalizeParse();

            //return (T)result;
        }

        /// <summary>
        /// Looks at all the flag-value pairs in defaultArgsOrNull and adds any to the current collection that are not already there.
        /// </summary>
        /// <param name="defaultArgsOrNull"></param>
        private void AddDefaultArgsIfMissing(ArgumentCollection defaultArgsOrNull)
        {
            if (defaultArgsOrNull == null)
                return;

            foreach (var flagAndValue in defaultArgsOrNull.FlagValuePairs)
            {
                if (!this.ContainsOptionalFlag(flagAndValue.Key))
                {
                    if (flagAndValue.Value == null) // is a flag from a CommandArguments
                        AddOptionalFlag(flagAndValue.Key);
                    else
                        AddOptional(flagAndValue.Key, flagAndValue.Value);
                }
            }
        }


        private bool HelpIsRequested()
        {
            return ExtractOptionalFlag("help") || _argList.Any(arg => arg.Equals("help!", StringComparison.CurrentCultureIgnoreCase));
        }



        //private bool IsParsable(Type type)
        //{
        //    return Attribute.IsDefined(type, typeof(ParsableAttribute));
        //}

        private T CreateInstance<T>()
        {
            CheckForHelp<T>(SubtypeName);
            Type t = typeof(T);

            // first, see if SubtypeName is simply refering to T
            if (SubtypeName != null && SubtypeName.Equals(t.ToTypeString(), StringComparison.CurrentCultureIgnoreCase))
                SubtypeName = null;

            // now check to see if the users wants a null reference
            if ("null".Equals(SubtypeName, StringComparison.CurrentCultureIgnoreCase))
            {
                Helper.CheckCondition<ParseException>(t.IsClass, "Cannot construct a null instance of a non-ref type. {0} is not a reference.", t.ToTypeString());
                return default(T);
            }

            // now try creating an instance of type T out of the SubtypeName
            Type subtype;
            if (SubtypeName != null && TypeFactory.TryGetType(SubtypeName, t, out subtype) && subtype.HasPublicDefaultConstructor())
                return (T)Activator.CreateInstance(subtype);


            // If there were no arguments, then it's a good chance SubtypeName was supposed to be a single required argument
            if (SubtypeName != null)
            {
                Helper.CheckCondition<ParseException>(_argList.Count == 0, "Cannot construct an instance of type {0} from the string {1}", t.ToTypeString(), SubtypeName);
                _argList.Add(SubtypeName);
                SubtypeName = null;
            }

            Helper.CheckCondition<ParseException>(!typeof(T).IsAbstract && !typeof(T).IsInterface, "Can't create an instance of an abstract type or interface. Please specify a valid subtype name, or use help for options. Input string: {0}", this);
            return Activator.CreateInstance<T>();
        }

        //private T CreateInstanceOld<T>()
        //{
        //    // Special case: if there is only one required argument, it will look like a Subtype when we
        //    // create the ConstructorArguments. But if T is not abstract or interface, then we know that didn't
        //    // mean a subtype, but rather the required argument.
        //    // !!! really? What if we want to allow a default base type that is not abstract, but still allow user to specify a derived type??
        //    // !!! this will still work in most cases (more than one argument). But some cases it won't. Not sure what to do about that...
        //    if (SubtypeName != null && _argList.Count == 0 && !(typeof(T).IsAbstract || typeof(T).IsInterface))
        //    {
        //        _argList.Add(SubtypeName);
        //        SubtypeName = null;
        //    }
        //    if (SubtypeName != null && SubtypeName.Equals(TypeFactory.ToTypeString(typeof(T)), StringComparison.CurrentCultureIgnoreCase))
        //        SubtypeName = null;

        //    CheckForHelp<T>(SubtypeName);

        //    if (SubtypeName == null)
        //        return Activator.CreateInstance<T>();
        //    else if (SubtypeName.Equals("null", StringComparison.CurrentCultureIgnoreCase))
        //    {
        //        Helper.CheckCondition<ParseException>(typeof(T).IsClass, "Cannot construct a null instance of a non-ref type. {0} is not a reference.", typeof(T));
        //        return default(T);
        //    }
        //    else
        //    {
        //        Type subtype;
        //        if (!TypeFactory.TryGetType(SubtypeName, typeof(T), out subtype))
        //            throw new ParseException(string.Format("Cannot construct an instance of {0} type out of the string {1}", typeof(T).ToTypeString(), SubtypeName));
        //        return (T)Activator.CreateInstance(subtype);
        //    }
        //}

        // ref so that this works with structs
        private void LoadRequiredParams(ref object result, MemberInfo requiredParamsArg)
        {
            //Helper.CheckCondition<ParseException>(_argList.Count > 0, "Expected at least one remaining argument for the params argument {0}", requiredParamsArg.Name);
            if (_argList.Count == 1 || _argList.Count == 2 && IsFlag(_argList[0]))
            {
                this.LoadArgument(ref result, requiredParamsArg, isOptional: _argList.Count == 2);
            }
            else
            {
                string remainingArgsAsList = string.Format("({0})", _argList.StringJoin(ConstructorArguments.ArgumentDelimiter.ToString()));
                _argList.Clear();
                _argList.Add(remainingArgsAsList);
                this.LoadArgument(ref result, requiredParamsArg, isOptional: false);
            }
        }


        // ref so that this works with structs
        private void LoadRequiredArguments(ref object result, ref MemberInfo requiredParams, IEnumerable<MemberInfo> requireds, bool checkComplete)
        {
            List<MemberInfo> unparsedRequireds = new List<MemberInfo>();
            foreach (var member in requireds)
            {
                if (!LoadArgument(ref result, member, true))    // try to load it as an optional arg first.
                    unparsedRequireds.Add(member);
            }

            if (requiredParams != null)
                if (LoadArgument(ref result, requiredParams, true))
                    requiredParams = null;

            if (checkComplete) CheckNoMoreOptions(unparsedRequireds.Count, result.GetType().ToTypeString());
            else if (unparsedRequireds.Count > 0) CheckNoMoreOptions(null, result.GetType().ToTypeString()); // you have to at least check that there are no more options if there are unnamed required arguments to parse.

            foreach (var member in unparsedRequireds)
                LoadArgument(ref result, member, false);
        }

        // ref so that this works with structs
        private void LoadOptionalArguments(ref object result, IEnumerable<MemberInfo> optionals)
        {
            foreach (MemberInfo member in optionals)
            {
                string flag = member.Name;
                if (TreatOptionAsFlag(result, member))  // load as a flag if and only if it's a boolean and the default is false.
                {
                    LoadFlag(ref result, member);
                }
                else
                {
                    LoadArgument(ref result, member, true);
                }
            }
        }

        private static bool TreatOptionAsFlag(object defaultObject, MemberInfo member)
        {
            Type memberType = GetActualParsingFieldOrPropertyType(member);
            return memberType.Equals(typeof(bool)) && !(bool)GetFieldOrPropertyValue(defaultObject, member);
        }

        // ref so that this works with structs
        private void LoadFlag(ref object result, MemberInfo member)
        {
            bool value = ExtractOptionalFlag(member.Name);

            SetFieldOrPropertyValue(ref result, member, value);
        }

        private static void SetFieldOrPropertyValue(ref object obj, MemberInfo member, object value)
        {
            Type declaredType = GetFieldOrPropertyType(member);
            value = ImplicitlyCastValueToType(value, declaredType);

            //Type parseType = member.GetParseAttribute().ParseTypeOrNull;
            //if (parseType != null && !parseType.IsSubclassOf(FieldOrPropertyTypeOfMemberInfo(member))) //Convert
            //{
            //    var method = parseType.GetMethod("op_Implicit", new Type[] { parseType });
            //    Helper.CheckCondition<ParseException>(null != method, "The class {0} must define an implicit operator for converting to {1}. See the C# keyword 'implicit'.",
            //        parseType, TypeFactory.ToTypeString(FieldOrPropertyTypeOfMemberInfo(member)));
            //    value = method.Invoke(null, new object[] { value });
            //}

            try
            {
                FieldInfo field = member as FieldInfo;
                if (field != null)
                {
                    field.SetValue(obj, value);
                }
                else
                {
                    PropertyInfo property = member as PropertyInfo;
                    Helper.CheckCondition<ParseException>(property != null, "Invalid member type {0}", member.MemberType);
                    property.SetValue(obj, value, null);
                }
            }
            catch (TargetInvocationException e)
            {
                if (e.InnerException is ParseException || e.InnerException is HelpException)
                {
                    throw e.InnerException;
                }
            }
        }

        //private static object ImplicitlyCastValueToType(MemberInfo member, object value, Type destinationTypeOrNull)
        //{
        //    if (destinationTypeOrNull != null && !destinationTypeOrNull.IsSubclassOf(FieldOrPropertyTypeOfMemberInfo(member))) //Convert
        //    {
        //        var method = destinationTypeOrNull.GetMethod("op_Implicit", new Type[] { destinationTypeOrNull });
        //        Helper.CheckCondition<ParseException>(null != method, "The class {0} must define an implicit operator for converting to {1}. See the C# keyword 'implicit'.",
        //            destinationTypeOrNull.ToTypeString(), TypeFactory.ToTypeString(FieldOrPropertyTypeOfMemberInfo(member)));
        //        value = method.Invoke(null, new object[] { value });
        //    }
        //    return value;
        //}

        public static object ImplicitlyCastValueToType(object value, Type destinationTypeOrNull)
        {
            if (value == null || destinationTypeOrNull == null)
                return value;

            Type sourceType = value.GetType();

            if (sourceType.Equals(destinationTypeOrNull) || sourceType.IsSubclassOfOrImplements(destinationTypeOrNull))
                return value;


            // either type can define the implicit cast...
            var method = destinationTypeOrNull.GetMethod("op_Implicit", new Type[] { sourceType }) ??
                            sourceType.GetMethod("op_Implicit", new Type[] { sourceType });

            object result;
            if (null != method)
            {
                result = method.Invoke(null, new object[] { value });
            }
            else if (value is ICollection && destinationTypeOrNull.Implements(typeof(ICollection)))
            {
                Type nestedDestinationType = destinationTypeOrNull.GetGenericArguments()[0];
                result = Activator.CreateInstance(destinationTypeOrNull);
                var addMethod = result.GetType().GetMethod("Add", new Type[] { nestedDestinationType });
                foreach (object obj in ((ICollection)value))
                {
                    object destinationObject = ImplicitlyCastValueToType(obj, nestedDestinationType);
                    addMethod.Invoke(result, new object[] { destinationObject });
                }
            }
            else
            {
                throw new ParseException("The class {0} must define an implicit operator for converting from {1}. See the C# keyword 'implicit'.",
                                destinationTypeOrNull.ToTypeString(), sourceType.ToTypeString());
            }
            return result;
        }


        private static object GetFieldOrPropertyValue(object obj, MemberInfo member)
        {
            FieldInfo field = member as FieldInfo;
            object value;
            if (field != null)
            {
                value = field.GetValue(obj);
            }
            else
            {
                PropertyInfo property = member as PropertyInfo;
                Helper.CheckCondition<ParseException>(property != null, "Invalid member type {0}", member.MemberType);
                value = property.GetValue(obj, null);
            }

            return value;

        }

        private static Type GetActualParsingFieldOrPropertyType(MemberInfo member)
        {
            return member.GetParseTypeOrNull() ?? GetFieldOrPropertyType(member);
        }

        private static Type GetFieldOrPropertyType(MemberInfo memberInfo)
        {
            FieldInfo field = memberInfo as FieldInfo;
            if (field != null)
            {
                return field.FieldType;
            }
            else
            {
                PropertyInfo property = memberInfo as PropertyInfo;
                Helper.CheckCondition<ParseException>(property != null, "Invalid member type {0}", memberInfo.MemberType);
                return property.PropertyType;
            }
        }

        // ref so that this works with structs
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="member"></param>
        /// <param name="isOptional"></param>
        /// <returns>Returns true if the value was loaded from the ArgumentCollection. False otherwise (only can be false if isOption is true)</returns>
        private bool LoadArgument(ref object result, MemberInfo member, bool isOptional)
        {
            bool isField = member.MemberType == MemberTypes.Field;
            object defaultValue = GetFieldOrPropertyValue(result, member);
            Type parseTypeOrNull = member.GetParseTypeOrNull();

            defaultValue = ImplicitlyCastValueToType(defaultValue, parseTypeOrNull);

            //if (parseTypeOrNull != null && !parseTypeOrNull.IsSubclassOf(FieldOrPropertyTypeOfMemberInfo(member)))
            //{
            //    var method = parseTypeOrNull.GetMethod("op_Implicit", new Type[] { FieldOrPropertyTypeOfMemberInfo(member) });
            //    Helper.CheckCondition<ParseException>(null != method, "The class {0} must define an implicit operator for converting from {1}. See the C# keyword 'implicit'.",
            //        parseTypeOrNull, FieldOrPropertyTypeOfMemberInfo(member));
            //    defaultValue = method.Invoke(null, new object[] { defaultValue });
            //}

            MethodInfo argCollectionExtractOption = this.GetType().GetMethod(isOptional ? "ExtractOptionalInternal" : "ExtractAtInternal", BindingFlags.NonPublic | BindingFlags.Instance);
            MethodInfo genericExtractOption = argCollectionExtractOption.MakeGenericMethod(GetActualParsingFieldOrPropertyType(member));

            object[] args = isOptional ?
                new object[] { member.Name, defaultValue, true /*remove flag and value*/, member.GetDefaultParametersOrNull() } :
                new object[] { member.Name, 0 /* remove the next item*/, member.GetDefaultParametersOrNull(), true /* remove value */ };

            bool flagIsPresent = FindFlag(member.Name) >= 0;

            object newValue = null;
            try
            {
                newValue = genericExtractOption.Invoke(this, args);
            }
            catch (TargetInvocationException e)
            {
                Exception eToThrow = e;
                do
                {
                    eToThrow = eToThrow.InnerException;
                } while (eToThrow is TargetInvocationException && eToThrow.InnerException != null);

                throw eToThrow; //Should be a HelpException.
            }

            if (newValue != defaultValue)    // no point unless it's different. also, if null, could cause problems in some cases.
                SetFieldOrPropertyValue(ref result, member, newValue);

            return flagIsPresent || !isOptional;
        }








        private static void GetParsableMembers(Type tType, out List<MemberInfo> optionals, out List<MemberInfo> requireds, out List<MemberInfo> constructingStrings, out MemberInfo requiredParams)
        {
            optionals = new List<MemberInfo>();
            requireds = new List<MemberInfo>();
            constructingStrings = new List<MemberInfo>();
            requiredParams = null;
            Type[] typeInheritanceHierarchy = tType.GetInheritanceHierarchy();

            foreach (MemberInfo memInfo in tType.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance))
            {
                //ParseAction parseAction = memInfo.MemberType == MemberTypes.Field && ((FieldInfo)memInfo).IsPublic ? ParseAction.Optional : ParseAction.Ignore;
                //Type parseTypeOrNull = null;
                //ParseAttribute parseAttribute = (ParseAttribute)Attribute.GetCustomAttribute(memInfo, typeof(ParseAttribute));
                //if (parseAttribute != null)
                //{
                //    parseAction = parseAttribute.Action;
                //    parseTypeOrNull = parseAttribute.ParseTypeOrNull;
                //}

                //MemberAndParseType memberAndParseType = new MemberAndParseType { MemberInfo = memInfo, ParseTypeOrNull = parseTypeOrNull };
                ParseAttribute parseAttribute = memInfo.GetParseAttribute(typeInheritanceHierarchy);


                switch (parseAttribute.Action)
                {
                    case ParseAction.Optional:
                        optionals.Add(memInfo); break;
                    case ParseAction.Required:
                        requireds.Add(memInfo); break;
                    case ParseAction.ArgumentString:
                        constructingStrings.Add(memInfo);
                        Helper.CheckCondition<ParseException>(GetActualParsingFieldOrPropertyType(memInfo).Equals(typeof(string)), "Attribute [Parse({0})] must be set on a field or property of type string.", parseAttribute.Action);
                        break;
                    case ParseAction.Ignore:
                        break;
                    case ParseAction.Params:
                        Helper.CheckCondition<ParseException>(requiredParams == null, "Can only have one parameter of labeled as RequiredParams.");
                        requiredParams = memInfo;
                        break;
                    default:
                        throw new NotImplementedException("Forgot to implement action for " + parseAttribute.Action);

                }
            }
        }
        #endregion

        protected void PopulateFromParsableObject(object obj, bool suppressDefaults)
        {
            Type type = obj.GetType();
            type.IsConstructable().Enforce("object of type {0} is not parsable. Missing public default constructor.", type);

            List<MemberInfo> optionals, requireds, constructingStrings;
            MemberInfo requiredParams;
            GetParsableMembers(type, out optionals, out requireds, out constructingStrings, out requiredParams);
            //this.ExtractOptionalFlag("help");   //make sure this isn't in there.

            string constructingStringOrNull;
            if (constructingStrings.Count() > 0 && null != (constructingStringOrNull = (string)GetFieldOrPropertyValue(obj, constructingStrings.First())))
            {
                constructingStrings.All(member => constructingStringOrNull.Equals(GetFieldOrPropertyValue(obj, member))).Enforce("For some reason this object has multiple constructing strings and they disagree with each other.");
                ParseString(constructingStringOrNull);
            }
            else
            {

                object defaultObj = null;
                int paramCount = optionals.Count + requireds.Count;
                foreach (var member in optionals)
                {
                    AddMemberToCollection(ref defaultObj, obj, member, isOptional: true, suppressDefaults: suppressDefaults, labelRequireds: paramCount > 1);
                }
                foreach (var member in requireds)
                {
                    AddMemberToCollection(ref defaultObj, obj, member, isOptional: false, suppressDefaults: suppressDefaults, labelRequireds: paramCount > 1);
                }
                if (requiredParams != null)
                {
                    AddRequiredParamsToCollection(obj, requiredParams, suppressDefaults);
                }
            }
        }




        //private bool TryValueAsCollectionString(ref object value, Type memberType, Type parseType)
        private bool TryValueAsCollectionString(ref object value, MemberInfo member, bool suppressDefaults)
        {
            return TryValueAsCollectionString(ref value, GetFieldOrPropertyType(member), member.GetParseTypeOrNull(), suppressDefaults);
        }

        private bool TryValueAsCollectionString(ref object value, Type baseType, Type parseTypeOrNull, bool suppressDefaults)
        {
            if (value != null)
            {
                Type valueType = value.GetType();
                if ((parseTypeOrNull ?? baseType).ParseAsCollection())// || (parseTypeOrNull == null && valueType.FindInterfaces(Module.FilterTypeNameIgnoreCase, "ICollection*").Length > 0))
                {
                    List<string> memberStrings = new List<string>();
                    foreach (object o in (IEnumerable)value)
                    {
                        string s = o.GetType().IsConstructable() ?  ConstructorArguments.ToString(o, suppressDefaults) : o.ToString();
                        memberStrings.Add(s);
                    }
                    value = string.Format("{0}({1})", baseType.Equals(valueType) ? "" : value.GetType().ToTypeString(), memberStrings.StringJoin(","));
                    return true;
                }
            }
            return false;
        }

        private void AddRequiredParamsToCollection(object obj, MemberInfo requiredParams, bool suppressDefaults)
        {
            object paramList = GetFieldOrPropertyValue(obj, requiredParams);
            if (paramList == null)  // e.g. constructing Help
                return;
            Type listType = paramList.GetType();
            Helper.CheckCondition<ParseException>(listType.ToTypeString().StartsWith("List<"), "The required params attribute must be placed on a member of type List<T>", listType.ToTypeString());
            Type genericType = listType.GetGenericArguments().Single();

            foreach (object item in (IEnumerable)paramList)
            {
                object valueToAdd = item;
                if (!genericType.HasParseMethod() && !TryValueAsCollectionString(ref valueToAdd, genericType, null, suppressDefaults) && genericType.IsConstructable())
                {
                    object valueAsParseType = ImplicitlyCastValueToType(valueToAdd, genericType);
                    ConstructorArguments constructor = ConstructorArguments.FromParsable(valueAsParseType, parseTypeOrNull: genericType, suppressDefaults: suppressDefaults);
                    valueToAdd = constructor.ToString();
                }
                Add(valueToAdd);
            }
        }

        /// <summary>
        /// Note that everything is named. The isOptional only makes a difference for boolean fields that would be marked as a flag if optional.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="member"></param>
        /// <param name="isOptional"></param>
        private void AddMemberToCollection(ref object defaultObjOrNull, object obj, MemberInfo member, bool isOptional, bool suppressDefaults, bool labelRequireds)
        {
            //object value = memberAndParseType.MemberInfo.MemberType == MemberTypes.Field ? ((FieldInfo)memberAndParseType.MemberInfo).GetValue(obj) : ((PropertyInfo)memberAndParseType.MemberInfo).GetValue(obj, null);
            //Type memberType = memberAndParseType.MemberInfo.MemberType == MemberTypes.Field ? ((FieldInfo)memberAndParseType.MemberInfo).FieldType : ((PropertyInfo)memberAndParseType.MemberInfo).PropertyType;

            string name = member.Name;
            object value = GetFieldOrPropertyValue(obj, member);
            Type parseType = GetActualParsingFieldOrPropertyType(member);
            //Type parseType = member.GetParseTypeOrNull() ?? (value == null ? GetActualParsingFieldOrPropertyType(member) : value.GetType());

            if (!parseType.HasParseMethod() && !TryValueAsCollectionString(ref value, member, suppressDefaults) && value != null && parseType.IsConstructable() && value.GetType().IsConstructable())
            {
                object valueAsParseType = ImplicitlyCastValueToType(value, member.GetParseTypeOrNull());
                ConstructorArguments constructor = ConstructorArguments.FromParsable(valueAsParseType, parseTypeOrNull: parseType, suppressDefaults: suppressDefaults);
                value = constructor.ToString();
            }

            if (defaultObjOrNull == null && isOptional && (value is bool || suppressDefaults))
                defaultObjOrNull = Activator.CreateInstance(obj.GetType());

            object defaultValue = defaultObjOrNull == null ? null : GetFieldOrPropertyValue(defaultObjOrNull, member);
            bool valueIsDefault = value == defaultValue || value != null && value.Equals(defaultValue) || defaultValue != null && defaultValue.Equals(value);
            TryValueAsCollectionString(ref defaultValue, member, suppressDefaults);
            if (!suppressDefaults || !isOptional || !valueIsDefault)
            {
                if (isOptional && value is bool && TreatOptionAsFlag(defaultObjOrNull, member))
                {
                    if ((bool)value)
                        AddOptionalFlag(name);
                }
                else if (!isOptional && !labelRequireds)
                {
                    Add(value);
                }
                else
                {
                    AddOptional(name, value);
                }
            }
        }

        public static HelpException CreateHelpMessage(Type t)
        {
            object defaultInstance = Activator.CreateInstance(t);
            ConstructorArguments args = new ConstructorArguments();
            HelpException help = args.CreateHelpMessage(defaultInstance);
            return help;
        }

        public HelpException CreateHelpMessage<T>()
        {
            T result = CreateInstance<T>();
            return CreateHelpMessage(result);
        }

        private HelpException CreateHelpMessage(object defaultInstance)
        {
            _argList.Clear();
            PopulateFromParsableObject(defaultInstance, false);

            Type type = defaultInstance.GetType();

            List<MemberInfo> optionals, requireds, constructingStrings;
            MemberInfo requiredParams;
            GetParsableMembers(type, out optionals, out requireds, out constructingStrings, out requiredParams);
            XDocument docFile = LoadXmlCodeDocumentationFile(type);

            StringBuilder helpMsg = new StringBuilder("Help for parsing type " + defaultInstance.GetType().ToTypeString());
            helpMsg.AppendFormat("<br><br>USAGE: " + CreateUsageString(requireds, requiredParams, type));
            helpMsg.Append("<br>Use help as the value for complex options for more info. Required arguments can be named like optionals.");
            helpMsg.Append("<br><br>" + GetXmlDocumentation(type, docFile));


            helpMsg.Append("<br><br>REQUIRED:");
            helpMsg.Append("<indent>");
            foreach (MemberInfo requirement in requireds)
            {
                helpMsg.Append("<br>" + CreateHelpMessage(defaultInstance, requirement, false));
                helpMsg.Append("<br><indent>" + GetXmlDocumentation(requirement, docFile) + "</indent><br>");
            }
            if (requiredParams != null)
            {
                helpMsg.Append("<br>" + CreateHelpMessage(defaultInstance, requiredParams, false));
                helpMsg.Append("<br><indent> PARAMS: Can specify arguments as a single list wrapped in () or as consecutive single arguments. Either way, these must be the last arguments. If there are more than one, none can be named.");
                helpMsg.Append("<br><indent>" + GetXmlDocumentation(requiredParams, docFile) + "</indent><br>");
            }
            helpMsg.Append("</indent>");
            helpMsg.Append("<br><br>OPTIONS:");
            helpMsg.Append("<indent>");
            foreach (MemberInfo option in optionals)
            {
                helpMsg.Append("<br>" + CreateHelpMessage(defaultInstance, option, true));
                helpMsg.Append("<br><indent>" + GetXmlDocumentation(option, docFile) + "</indent><br>");
            }
            helpMsg.Append("</indent>");

            if (defaultInstance is IParsable)
            {
                string finalizeMessage = GetXmlDocumentationForFinalizeParse(type, docFile);
                if (!string.IsNullOrEmpty(finalizeMessage))
                    helpMsg.AppendLine("<br>Post-parsing actions: " + finalizeMessage);
            }

            return new HelpException(helpMsg.ToString());
        }

        private static string GetXmlDocumentationForFinalizeParse(Type type, XDocument xmlDoc)
        {
            string xmlTagName = "M:" + type.FullName + ".FinalizeParse";
            return GetXmlDocumentation(xmlTagName, xmlDoc);
        }

        private static string GetXmlDocumentation(Type type, XDocument xmlDoc)
        {
            string xmlTagName = "T:" + type.FullName;
            return GetXmlDocumentation(xmlTagName, xmlDoc);
        }

        private static string GetXmlDocumentation(MemberInfo member, XDocument xmlDoc)
        {
            FieldInfo field = member as FieldInfo;
            PropertyInfo property = member as PropertyInfo;

            string xmlTagName = field != null ?
                "F:" + field.DeclaringType.FullName + "." + field.Name :
                "P:" + property.DeclaringType.FullName + "." + property.Name;

            return GetXmlDocumentation(xmlTagName, xmlDoc);
        }

        static Regex _doubleNewLineRegEx = new Regex(@"\n[\s]*\n"
#if !SILVERLIGHT
            , RegexOptions.Compiled
#endif
            );

        private static string GetXmlDocumentation(string xmlTagName, XDocument xmlDoc)
        {
            if (null == xmlDoc)
            {
                return NO_DOCUMENTATION_STRING;
            }
//            var xmlElements = xmlDoc.GetElementsByTagName("member").Cast<XmlNode>().Where(node => node.Attributes["name"].Value == xmlTagName).ToList();
            var xmlElements = xmlDoc.Elements("doc").Elements("members").Elements("member").Where(node => node.Attribute("name").Value == xmlTagName).ToList();
            
            if (xmlElements.Count > 0)
            {
                Helper.CheckCondition<ParseException>(xmlElements.Count == 1, "Problem with xml documentation file: there are {0} entries for type {1}", xmlElements.Count, xmlTagName);

                //var summaryElement = xmlElements[0].ChildNodes.Cast<XmlNode>().Where(node => node.Name == "summary").ToList();
                //var summaryElement = xmlElements[0].DescendantNodes().Cast<XElement>().Where(node => node.Name == "summary").ToList();
                XElement summaryElement = xmlElements[0].Element("summary");

                if (null != summaryElement)
                {
                    //string docText = summaryElement[0].InnerText.Trim();
                    string docText = summaryElement.Value.Trim();

                    docText = _doubleNewLineRegEx.Replace(docText, "<br><br>");
                    return docText;
                }
            }
            return NO_DOCUMENTATION_STRING;
        }

        private static ConcurrentDictionary<Type, XDocument> xmlDocumentCache = new ConcurrentDictionary<Type, XDocument>();

        private static XDocument LoadXmlCodeDocumentationFile(Type type)
        {
            XDocument xmlDoc = xmlDocumentCache.GetOrAdd(type, (t) =>
            {
                string xmlFile = Path.ChangeExtension(t.Assembly.Location, "xml");
                if (!File.Exists(xmlFile))
                    return null;

                //XDocument xmlDoc = new XDocument();
                //xmlDoc.Load(xmlFile);
                return XDocument.Load(xmlFile);
            });
            return xmlDoc;
        }



        private string CreateHelpMessage(object defaultInstance, MemberInfo member, bool isOption)
        {
            //var member = memberAndParseType.MemberInfo;
            string flag = CreateFlagString(member.Name);//isOption ? CreateFlagString(member.Name) : member.Name;
            object value = GetFieldOrPropertyValue(defaultInstance, member);
            Type memberType = GetActualParsingFieldOrPropertyType(member);
            string memberTypeString = memberType.ToTypeString();


            if (!TryValueAsCollectionString(ref value, member, suppressDefaults: true) && value != null)
                value = value.ToParseString(memberType, suppressDefaults:true);

            string helpMsg = isOption ?
                (this is CommandArguments && value is bool && !(bool)value ?    // is this a boolean flag for CommandArguments?
                    string.Format("{0} <BooleanFlag> [false if absent]", flag) :
                    string.Format("{0} <{1}> default: {2}", flag, memberTypeString, value == null ? "null" : value.ToString())) :
                string.Format("{0} <{1}>", flag, memberTypeString);

            return helpMsg;
        }

        private void CheckForHelp<T>(string value)
        {
            if (value != null && (value.Equals("help", StringComparison.CurrentCultureIgnoreCase) || value.Equals("help!", StringComparison.CurrentCultureIgnoreCase)))
            {
                Type type = typeof(T);

                HelpException help = GetHelpOnKnownSubtypes(type);
                throw help;
            }
        }

        public static HelpException GetHelpOnKnownSubtypes(Type type)
        {
            StringBuilder sb = new StringBuilder();

            XDocument xmlDoc = LoadXmlCodeDocumentationFile(type);
            string typeDocumentation = GetXmlDocumentation(type, xmlDoc).Replace(NO_DOCUMENTATION_STRING, "");
            if (type.IsEnum)
            {
                sb.AppendFormat("Enum type {0}: {1}", type.ToTypeString(), typeDocumentation);
                sb.Append("<br>OPTIONS:<br><indent>");
                foreach (var member in type.GetFields().Where(f => f.IsStatic))
                {
                    string docstring = GetXmlDocumentation(member, xmlDoc);
                    sb.Append(member.Name + "<br>");
                    if (docstring != NO_DOCUMENTATION_STRING)
                        sb.AppendFormat("<indent>{0}</indent><br>", docstring);
                }
                sb.Append("</indent>");
            }
            else if (type.Implements(typeof(ICollection)))
            {
                sb.Append("Help for type " + type.ToTypeString());
                sb.Append("<br><br>Collections can be specified using a comma delimited list, wrapped in parentheses.<br>");
                sb.Append("To get help on the nested type, using help! in the list. For example:");
                sb.Append("<br><indent>(help)");
                sb.Append("<br>(item1,item2,help)</indent>");

            }
            else
            {
                IEnumerable<Type> implementingTypes = null;
                if (type.IsInterface)
                {
                    implementingTypes = type.GetImplementingTypes().Where(t => !t.IsAbstract && !t.IsInterface && t.IsConstructable());
                    sb.AppendFormat("Interface {0}: {1}<br>", type.ToTypeString(), typeDocumentation);
                }
                else// if (type.IsAbstract)
                {
                    implementingTypes = type.GetDerivedTypes().Where(t => !t.IsAbstract && !t.IsInterface && t.IsConstructable());

                    if (type.IsAbstract)
                        sb.AppendFormat("Abstract class  {0}: {1}<br>", type.ToTypeString(), typeDocumentation);
                    else
                        implementingTypes = type.AsSingletonEnumerable().Concat(implementingTypes);
                }
                //else
                //{
                //    throw new HelpException("Type {0} does not have any derived types.<br>" +
                //        "To get help for the members of this type, use {0}(help). To get help for the context in which this type occurred, use help!", type.ToTypeString());
                //}

                sb.AppendFormat("Types that implement {0}:<br>", type.ToTypeString());
                sb.Append("<indent>");
                foreach (var implementingType in implementingTypes)
                {
                    string docstring = GetXmlDocumentation(implementingType, LoadXmlCodeDocumentationFile(implementingType));
                    sb.Append(implementingType.ToTypeString() + "<br>");
                    if (docstring != NO_DOCUMENTATION_STRING)
                        sb.AppendFormat("<indent>{0}</indent><br>", docstring);
                }
                sb.Append("</indent>");
            }
            return new HelpException(sb.ToString());
        }
    }
}

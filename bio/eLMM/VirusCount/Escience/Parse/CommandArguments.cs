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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Bio.Util;
using System.Text;

namespace MBT.Escience.Parse
{


    /// <summary>
    /// Command line arguments take the form:
    /// <code>
    /// -flag 1 ... -option1 value -option2 value ... required1 required 2
    /// </code>
    /// Note that the presence of the flag indicates that the optional value is true.
    /// Note also that required arguments can be named, in which case they can be in any order.
    /// </summary>
    public class CommandArguments : ArgumentCollection
    {
        //!!! This one doesn't seem good with .NET 4 "\u00AD"
        private static string[] FLAG_PREFIXES = new string[] { "/", /*a windows dash*/ "\xFB", /*every unicode hyphen*/ "\u002D", "\u2010", "\u2011", "\u2012", "\u2013", "\u2014", "\u2015", "\u2212" };



        public CommandArguments()
            : base(new string[0])
        { }

        public CommandArguments(string args)
            : base(args)
        {
            if (Count == 0) AddOptionalFlag("help");
        }

        public CommandArguments(IEnumerable<string> args)
            : base(args)
        {
            if (Count == 0) AddOptionalFlag("help");
        }

        /// <summary>
        /// Constructs and instance of T, then runs it. This convenience method creates an instance of CommandArguments, then
        /// call ConstructAndRun on that result.
        /// </summary>
        /// <typeparam name="T">A //[Parsable] type that implements IExecutable.</typeparam>
        /// <param name="commandArgs">Command line arguments.</param>
        public static void ConstructAndRun<T>(string[] commandArgs) where T : IRunnable
        {
            CommandArguments command = new CommandArguments(commandArgs);
            command.ConstructAndRun<T>();
        }



        /// <summary>
        /// Simple wrapper that constructs an instance of type T from the command line array. 
        /// See ArgumentCollection.Construct() for documentation.
        /// </summary>
        /// <typeparam name="T">The Parsable type to be constructed</typeparam>
        /// <param name="commandArgs">The string array from which to construct</param>
        /// <returns>The fully instantiated object</returns>
        public static T Construct<T>(string[] commandArgs)
        {
            CommandArguments command = new CommandArguments(commandArgs);
            return command.Construct<T>();
        }

        /// <summary>
        /// Simple wrapper that constructs an instance of type T from the command line string. 
        /// See ArgumentCollection.Construct() for documentation.
        /// </summary>
        /// <typeparam name="T">The Parsable type to be constructed</typeparam>
        /// <param name="commandString">The string from which to construct</param>
        /// <returns>The fully instantiated object</returns>
        public static T Construct<T>(string commandString)
        {
            CommandArguments command = new CommandArguments(commandString);
            return command.Construct<T>();
        }

        /// <summary>
        /// Constructs and instance of CommandArguments from a parsable object. This is the inverse of Construct().
        /// Will include all default arguments.
        /// </summary>
        /// <param name="obj">The object from which to construct the ConstructorArguments</param>
        /// <returns>The result</returns>
        public static CommandArguments FromParsable(object obj)
        {
            return FromParsable(obj, false);
        }

        /// <summary>
        /// Constructs and instance of CommandArguments from a parsable object. This is the inverse of Construct().
        /// </summary>
        /// <param name="obj">The object from which to construct the ConstructorArguments</param>
        /// <param name="suppressDefaults">Specifies whether values that are equal to the defaults should be included in the resulting ArgumentCollection</param>
        /// <returns>The result</returns>
        public static CommandArguments FromParsable(object obj, bool suppressDefaults)
        {
            CommandArguments cmd = new CommandArguments();
            cmd.PopulateFromParsableObject(obj, suppressDefaults);
            return cmd;
        }



        /// <summary>
        /// Shortcut for CommandArguments.FromParsable(obj).ToString().  Note that Construct(ToString(obj)) == obj.
        /// </summary>
        /// <param name="parsableObject">An obejct with the //[Parsable] attribute.</param>
        /// <param name="suppressDefaults">Specifies whether values that are equal to the defaults should be included in the resulting ArgumentCollection</param>
        /// <returns>A Command string that could be used to reconstruct parsableObject.</returns>
        public static string ToString(object parsableObject, bool suppressDefaults = false, bool protect = false)
        {
            return FromParsable(parsableObject, suppressDefaults).ToString(protect);
        }

        protected override string CreateUsageString(IEnumerable<System.Reflection.MemberInfo> requireds, System.Reflection.MemberInfo requiredParamsOrNull, Type constructingType)
        {
            string exeName = Path.GetFileName(Assembly.
#if !SILVERLIGHT
                GetEntryAssembly().Location);
#else
                GetExecutingAssembly().Location);
#endif
            string baseString = string.Format("{0} [OPTIONS] {1}", exeName, requireds.Select(member => member.Name).StringJoin(" "));
            if (null != requiredParamsOrNull)
            {
                string opName = requiredParamsOrNull.Name.EndsWith("s", StringComparison.CurrentCultureIgnoreCase) ?
                    requiredParamsOrNull.Name.Substring(0, requiredParamsOrNull.Name.Length - 1) :
                    requiredParamsOrNull.Name;

                baseString += string.Format(" {0}_1[ {0}_2 ...]", opName);
            }
            return baseString;
        }

        protected override string ExtractSubtypeName(ref string lineToParse)
        {
            return null;
        }

        protected override IEnumerable<string> CreateArgList(string lineToParse)
        {
            return lineToParse.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }

        protected override bool ExtractOptionalFlagInternal(string flag, bool removeFlag)
        {
            int argIndex = FindFlag(flag);

            if (argIndex == -1)
            {
                return false;
            }

            if (removeFlag)
                RemoveAt(argIndex);

            return true;
        }

        public override bool MatchesFlag(string query, string flagBase)
        {
            foreach (var pre in FLAG_PREFIXES)
                if (query.Equals(pre + flagBase, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        public override bool IsFlag(string query)
        {
            if (query.Length < 2)
                return false;

            double dummy;
            if (string.IsNullOrWhiteSpace(query) || double.TryParse(query, out dummy))
                return false;

            foreach (var pre in FLAG_PREFIXES)
                if (query.StartsWith(pre, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        protected override string CreateFlagString(string flagBase)
        {
            return IsFlag(flagBase) ? flagBase : "-" + flagBase;
        }

        public override void AddOptionalFlag(string argumentName)
        {
            //AddOptional(argumentName, "");
            Add(CreateFlagString(argumentName));
        }

        public override string ToString()
        {
            return ToString(protectWithQuotes: false);
        }

        public string ToString(bool protectWithQuotes)
        {
            if (protectWithQuotes)
                return GetUnderlyingArray().Select(s => "\"" + s + "\"").StringJoin(" ");
            else
                return GetUnderlyingArray().StringJoin(" ");
        }
    }
}

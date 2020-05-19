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
using System.Linq;
using System.Reflection;
using Bio.Util;

namespace MBT.Escience.Parse
{
    public static class Parser
    {

        /// <summary>
        /// Checks if this type has a Parse or TryParse static method that takes a string as the argument. 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool HasParseMethod(this Type type)
        {
            MethodInfo tryParse = type.GetMethod("TryParse", new Type[] { typeof(string), type.MakeByRefType() });
            if (tryParse != null && tryParse.IsStatic)
                return true;

            MethodInfo parse = type.GetMethod("Parse", new Type[] { typeof(string) });
            if (parse != null && parse.IsStatic)
                return true;

            return false;
        }


        public static bool TryParseAll<T>(IEnumerable<string> values, out IList<T> result)
        {
            result = new List<T>();

            foreach (string s in values)
            {
                T value;
                if (TryParse<T>(s, out value))
                {
                    result.Add(value);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public static IEnumerable<T> ParseAll<T>(IEnumerable<string> values)
        {
            foreach (string s in values)
            {
                yield return Parse<T>(s);
            }
        }

        /// <summary>
        /// This method should be updated to use the rest of the methods in this class.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object Parse(string field, Type type)
        {
            var potentialMethods = typeof(Parser).GetMethods().Where(m => m.Name == "Parse" && m.IsGenericMethod);
            MethodInfo parseInfo = potentialMethods.First().MakeGenericMethod(type);

            object[] parameters = new object[] { field };
            return parseInfo.Invoke(null, parameters);

            //if (type.Equals(typeof(string)))
            //{
            //    return field;
            //}
            //if (type.Equals(typeof(int)))
            //{
            //    return int.Parse(field);
            //}
            //if (type.Equals(typeof(double)))
            //{
            //    return double.Parse(field);
            //}
            //if (type.Equals(typeof(bool)))
            //{
            //    return bool.Parse(field);
            //}
            //if (type.Equals(typeof(char)))
            //{
            //    return char.Parse(field);
            //}
            //if (type.Equals(typeof(DateTime)))
            //{
            //    return DateTime.Parse(field);
            //}
            //Helper.CheckCondition<ParseException>(false, "Don't know how to parse type " + type.Name);
            //return null;
        }

        /// <summary>
        /// Will parse s into T, provided T has a Parse(string) or TryParse(string s, out T t) method defined, or is one of the magical
        /// special cases we've implemented (including ICollection (comma delimited), Nullable and Enums).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static T Parse<T>(string s)
        {
            s = s.Trim();
            T t;
            if (TryParse(s, out t))
            {
                return t;
            }
            else
            {
                throw new ArgumentException(string.Format("Could not parse \"{0}\" into an instance of type {1}", s, typeof(T)));
            }
        }

        /// <summary>
        /// Will parse s into T, provided T has a Parse(string) or TryParse(string s, out T t) method defined, or is one of the magical
        /// special cases we've implemented (including ICollection (comma delimited), Nullable and Enums).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool TryParse<T>(string s, out T t)
        {
            s = s.Trim();
            Type type = typeof(T);
            if (s.Equals("help", StringComparison.CurrentCultureIgnoreCase))
            {
                throw ArgumentCollection.GetHelpOnKnownSubtypes(type);
            }
            else if (s.Equals("help!", StringComparison.CurrentCultureIgnoreCase))
            {
                throw ArgumentCollection.CreateHelpMessage(type);
            }
            else if (s.Equals("null", StringComparison.CurrentCultureIgnoreCase) && type.IsClass)
            {
                t = default(T);  // return null
                return true;
            }
            else if (s is T)
            {
                return StringTryParse(s, out t);
            }
            else if (type.IsEnum)
            {
                return EnumTryParse(s, out t);
            }
            else if (type.IsGenericType)
            {
                //if (type.FindInterfaces(Module.FilterTypeNameIgnoreCase, "ICollection*").Length > 0)
                if (type.ParseAsCollection())
                {
                    return CollectionsTryParse(s, out t);
                }
                else if (type.Name.StartsWith("Nullable"))
                {
                    return NullableTryParse(s, out t);
                }
            }

            return GenericTryParse(s, out t);
        }

        private static bool NullableTryParse<T>(string s, out T t)
        {
            t = default(T);
            if (string.IsNullOrEmpty(s) || s.Equals("null", StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }

            Type type = typeof(T);
            Type underlyingType = type.GetGenericArguments()[0];
            //underlyingType.TypeInitializer
            MethodInfo tryParse = typeof(Parser).GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static);
            MethodInfo genericTryParse = tryParse.MakeGenericMethod(underlyingType);

            object[] args = new object[] { s, Activator.CreateInstance(underlyingType) };

            bool success = (bool)genericTryParse.Invoke(null, args);
            if (success)
            {
                t = (T)args[1];
            }
            return success;
        }

        private static bool StringTryParse<T>(string s, out T t)
        {
            t = (T)(object)s;
            return true;
        }

        private static bool CollectionsTryParse<T>(string s, out T t)
        {
            Type type = typeof(T);
            Type genericType = type.GetGenericArguments()[0];

            MethodInfo collectionTryParse = typeof(Parser).GetMethod("GenericCollectionsTryParse", BindingFlags.NonPublic | BindingFlags.Static);
            MethodInfo genericCollectionTryParse = collectionTryParse.MakeGenericMethod(type, genericType);
            t = default(T);
            object[] args = new object[] { s, t };

            bool success = (bool)genericCollectionTryParse.Invoke(null, args);
            if (success)
            {
                t = (T)args[1];
            }
            return success;
        }

        private static bool GenericCollectionsTryParse<T, S>(string s, out T t) where T : ICollection<S>, new()
        {
            t = new T();

            // remove wrapping parens if present
            if (s.StartsWith("(") && s.EndsWith(")"))
                s = s.Substring(1, s.Length - 2);

            //If the string is empty, then the list will be empty
            if (s == "")
            {
                return true;
            }

            foreach (string itemAsString in s.ProtectedSplit('(', ')', false, ','))
            {
                S item;
                if (TryParse<S>(itemAsString, out item))
                {
                    t.Add(item);
                }
                else
                {
                    t = default(T);
                    return false;
                }
            }
            return true;
        }

        private static bool EnumTryParse<T>(string s, out T t)
        {
            int i;
            if (int.TryParse(s, out i))
            {
                t = (T)(object)i;
                return true;
            }

            try
            {
                t = (T)Enum.Parse(typeof(T), s, true);
                return true;
            }
            catch (ArgumentException)
            {
            }
            t = default(T);
            return false;
        }

        //private static bool NullableTryParse<T>(string s, out T t) where T:System.Nullable
        //{
        //	if (string.IsNullOrEmpty(s) || s.Equals("null", StringComparison.CurrentCultureIgnoreCase))
        //	{
        //		return null;
        //	}


        //}

        private static bool GenericTryParse<T>(string s, out T t)
        {
            return GenericParser<T>.TryParse(s, out t);
            //// now the general one.
            //bool success = false;
            //t = default(T);
            //Type type = typeof(T);

            //MethodInfo tryParse = type.GetMethod("TryParse", new Type[] { typeof(string), type.MakeByRefType() });

            //if (tryParse != null && tryParse.IsStatic)
            //{
            //    object[] args = new object[] { s, t };

            //    success = (bool)tryParse.Invoke(null, args);

            //    if (success)
            //    {
            //        t = (T)args[1];
            //    }
            //}
            //else
            //{
            //    MethodInfo parse = type.GetMethod("Parse", new Type[] { typeof(string) });
            //    if (parse != null && parse.IsStatic)
            //    {
            //        Helper.CheckCondition<ParseException>(parse != null, "Cannot parse type {0}. It does not have a TryParse or Parse method defined", typeof(T).ToTypeString());

            //        try
            //        {
            //            object[] args = new object[] { s };
            //            t = (T)parse.Invoke(null, args);
            //            success = true;
            //        }
            //        catch (TargetInvocationException e)
            //        {
            //            if (e.InnerException is HelpException || e.InnerException is ParseException)
            //                throw e.InnerException;
            //        }
            //    }
            //    else //if (type.IsParsable() || type.IsInterface || type.IsAbstract)
            //    {
            //        ConstructorArguments constLine = new ConstructorArguments(s);
            //        try
            //        {
            //            t = constLine.Construct<T>();
            //            success = true;
            //        }
            //        catch (HelpException)
            //        {
            //            throw;
            //        }
            //        catch (ParseException)
            //        {
            //            throw;
            //        }
            //    }
            //    //else
            //    //{
            //    //    throw new ParseException("Cannot parse type {0}. It does not have a TryParse or Parse method defined, nor does it have a public default constructor, nor is it an interface or abstract type.", typeof(T).ToTypeString());
            //    //}
            //}

            //return success;
        }

        private static class GenericParser<T>
        {
            private static MethodInfo _tryParse, _parse;

            static GenericParser()
            {
                Type type = typeof(T);
                _tryParse = type.GetMethod("TryParse", new Type[] { typeof(string), type.MakeByRefType() });
                if (_tryParse != null && !_tryParse.IsStatic)
                    _tryParse = null;

                if (_tryParse == null)
                {
                    _parse = type.GetMethod("Parse", new Type[] { typeof(string) });
                    if (_parse != null && !_parse.IsStatic)
                        _parse = null;
                }
            }

            public static bool IsParsable()
            {
                return _tryParse != null || _parse != null;
            }

            public static bool TryParse(string s, out T t)
            {
                //Helper.CheckCondition(IsParsable(), "Cannot parse type {0}. It does not have a TryParse or Parse method defined", typeof(T));
                // now the general one.
                bool success = false;
                t = default(T);

                if (_tryParse != null)
                {
                    object[] args = new object[] { s, t };

                    success = (bool)_tryParse.Invoke(null, args);

                    if (success)
                    {
                        t = (T)args[1];
                    }
                }
                else if (_parse != null)
                {
                    try
                    {
                        object[] args = new object[] { s };
                        t = (T)_parse.Invoke(null, args);
                        success = true;
                    }
                    catch { }
                }
                else
                {
                    ConstructorArguments constLine = new ConstructorArguments(s);
                    try
                    {
                        t = constLine.Construct<T>();
                        success = true;
                    }
                    catch (HelpException)
                    {
                        throw;
                    }
                    catch (ParseException)
                    {
                        throw;
                    }
                }

                return success;
            }
        }
    }

}

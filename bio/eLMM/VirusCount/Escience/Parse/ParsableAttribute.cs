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
using Bio.Util;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Collections.Concurrent;

namespace MBT.Escience.Parse
{
    //[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    //public class ParsableAttribute : Attribute { }

    public enum ParseAction
    {
        /// <summary>
        /// Specifies that a field is required when parsing.
        /// </summary>
        Required,
        /// <summary>
        /// Specifies that an element is optional. Note that all public fields are optional by default. This allows you to mark private or protected fields as parsable
        /// </summary>
        Optional,
        /// <summary>
        /// Specifies that a field should not be parsed. This only is useful for public fields that would otherwise be automatically parsed.
        /// </summary>
        Ignore,
        /// <summary>
        /// Specifies that the string used to construct this argument should be stored here. Note that this MUST be of type string.
        /// </summary>
        ArgumentString,
        /// <summary>
        /// Behaves like the params keyword for methods: sucks up all the final arguments and constructs a list out of them. They must all be the same type, as
        /// specified by the type of the list that this attribute is attached to. This can only be placed on a member of type List. This is considered an optional
        /// argument, in the sense that if there are no arguments left, an empty list will be returned. It's up to the parsable type to decide if it wants to check
        /// that the list is non-empty in its FinalizeParse method.
        /// </summary>
        Params
    };

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ParseAttribute : Attribute
    {
        public ParseAttribute(ParseAction action) : this(action, null) { }

        public ParseAttribute(ParseAction action, Type parseType)
        {
            Action = action;
            ParseTypeOrNull = parseType;
        }


        public ParseAction Action { get; private set; }
        public Type ParseTypeOrNull { get; private set; }

        /// <summary>
        /// Use ConstructorArguments syntax to hard code settings.
        /// </summary>
        public string DefaultParameters { get; set; }
    }

    /// <summary>
    /// Labels a Collection type as not being parsed as a collection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ParseAsNonCollectionAttribute : Attribute { }

    /// <summary>
    /// Marks a class so that only fields and properties that have explicit Parse attributes SET IN THE CURRENT CLASS OR A DERIVED CLASS will be parsed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class DoNotParseInheritedAttribute : Attribute { }

    public static class ParseExtensions
    {
        private static ParseAttribute DefaultOptionalAttribute = new ParseAttribute(ParseAction.Optional);
        private static ParseAttribute DefaultIgnoreAttribute = new ParseAttribute(ParseAction.Ignore);

        /// <summary>
        /// Determines if the ParseExplicit attribute has been set.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool DoNotParseInherited(this Type type)
        {
            return Attribute.IsDefined(type, typeof(DoNotParseInheritedAttribute));
        }

        public static Type GetParseTypeOrNull(this MemberInfo member)
        {
            ParseAttribute parseAttribute = (ParseAttribute)Attribute.GetCustomAttribute(member, typeof(ParseAttribute));
            return parseAttribute == null ? null : parseAttribute.ParseTypeOrNull;
        }

        public static string GetDefaultParametersOrNull(this MemberInfo member)
        {
            ParseAttribute parseAttribute = (ParseAttribute)Attribute.GetCustomAttribute(member, typeof(ParseAttribute));
            return parseAttribute == null ? null : parseAttribute.DefaultParameters;
        }

        //static ThreadLocal<Cache<MemberInfo, ParseAttribute>> _parseAttributeCache =
        //    new ThreadLocal<Cache<MemberInfo, ParseAttribute>>(
        //        () => new Cache<MemberInfo, ParseAttribute>(maxSize: 10000, recoverySize: 100));
        static ConcurrentDictionary<MemberInfo, ParseAttribute> _parseAttributeCache = new ConcurrentDictionary<MemberInfo, ParseAttribute>();

        public static ParseAttribute GetParseAttribute(this MemberInfo member, Type[] actualTypeInheritanceHierarchy)
        {
            ParseAttribute pa = _parseAttributeCache.GetOrAdd(member, (m) => GetParseAttributeInternal(m, actualTypeInheritanceHierarchy));
            //ParseAttribute pa;
            //if (!_parseAttributeCache.Value.TryGetValue(member, out pa))
            //{
            //    pa = GetParseAttributeInternal(member, actualTypeInheritanceHierarchy);
            //    _parseAttributeCache.Value.Add(member, pa);
            //}
            return pa;
        }

        /// <summary>
        /// Gets the default or declared parse attribute for the specified member.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        private static ParseAttribute GetParseAttributeInternal(this MemberInfo member, Type[] actualTypeInheritanceHierarchy)
        {
            // march up the stack, starting at the actual type's base. If we find the member before we find a ParseExplicit, we're ok to parse this.
            // If we find a parseExplicity first, then we know we can't parse this member.  If a member is declared in the first class that we see ParseExplicit, 
            // keep it, because we define ParseExplicit as "keep for this and all derived types"
            for (int i = actualTypeInheritanceHierarchy.Length - 1; i >= 0; i--)
            {
                if (member.DeclaringType.Equals(actualTypeInheritanceHierarchy[i]))
                    break;
                if (actualTypeInheritanceHierarchy[i].DoNotParseInherited())
                    return DefaultIgnoreAttribute;
            }

            ParseAttribute parseAttribute = (ParseAttribute)Attribute.GetCustomAttribute(member, typeof(ParseAttribute));
            if (IsIndexer(member))
            {
                Helper.CheckCondition<ParseException>(parseAttribute == null || parseAttribute.Action == ParseAction.Ignore, "Can't parse an Indexer.");
                return DefaultIgnoreAttribute;
            }

            PropertyInfo property = member as PropertyInfo;
            if (parseAttribute == null)
            {
                FieldInfo field = member as FieldInfo;
                if (field != null)
                {
                    return field.IsPublic ? DefaultOptionalAttribute : DefaultIgnoreAttribute;
                }
                else if (property != null)
                {
                    parseAttribute = property.GetGetMethod() != null && property.GetSetMethod() != null && property.GetGetMethod().IsPublic && property.GetSetMethod().IsPublic ? // either will be null if don't exist or non-public
                        DefaultOptionalAttribute : DefaultIgnoreAttribute;
                }
                else
                {
                    parseAttribute = DefaultIgnoreAttribute;
                }
            }
            return parseAttribute;
        }

        private static bool IsIndexer(MemberInfo member)
        {
            return member is PropertyInfo && ((PropertyInfo)member).GetIndexParameters().Length > 0;
        }

        /// <summary>
        /// Returns true if and only if the type has a public default constuctor, or is an interface or abstract class, in which case a derived type may be parsed.
        /// </summary>
        public static bool IsConstructable(this Type t)
        {
            if ((t.IsInterface || t.IsAbstract))
                return true;

            return t.HasPublicDefaultConstructor();
            //return Attribute.IsDefined(t, typeof(ParsableAttribute));
        }

        public static bool HasPublicDefaultConstructor(this Type t)
        {
            var constructor = t.GetConstructor(Type.EmptyTypes);
            return constructor != null && constructor.IsPublic;
        }

        public static bool ParseAsCollection(this Type parseType)
        {
            bool result = !Attribute.IsDefined(parseType, typeof(ParseAsNonCollectionAttribute)) &&
#if !SILVERLIGHT
                parseType.FindInterfaces(Module.FilterTypeNameIgnoreCase, "ICollection*").Length > 0
#else
                parseType.GetInterfaces().Any(interface1 => interface1.ToString().StartsWith("ICollection"))
#endif
                && !parseType.HasParseMethod();
            return result;
        }

        public static Type[] GetInheritanceHierarchy(this Type type)
        {
            var result = new Stack<Type>();
            while (type != null)
            {
                result.Push(type);
                type = type.BaseType;
            }
            return result.ToArray();
        }

        public static string ToParseString(this object o, Type parseTypeOrNull = null, bool suppressDefaults = false)
        {
            Type t = o.GetType();
            if (t.HasParseMethod() || !t.IsConstructable())
                return o.ToString();
            else if (parseTypeOrNull == null)
                return ConstructorArguments.ToString(o);    // can only get here if t is constructable.
            else
            {
                object valueAsParseType = ArgumentCollection.ImplicitlyCastValueToType(o, parseTypeOrNull);
                return ConstructorArguments.ToString(valueAsParseType, suppressDefaults);
            }
        }
    }
}

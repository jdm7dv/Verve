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
using System.Text;
using Bio.Util;
using System.Reflection;


namespace MBT.Escience.Parse
{
    public static class TypeExtensions
    {

        public static string ToTypeString(this Type type)
        {
            if (type.Name == "Int32") return "int";
            if (type.Name == "Int64") return "long";
            if (type.Name == "Boolean") return "bool";

            StringBuilder typeString = new StringBuilder(type.Name);
            if (type.IsGenericType)
            {
                typeString.Remove(typeString.Length - 2, 2);
                typeString.Append('<');
                typeString.Append(type.GetGenericArguments().Select(genericType => ToTypeString(genericType)).StringJoin(","));
                typeString.Append('>');
            }
            return typeString.ToString();
        }

        public static string[] GetEnumNames(this Type type)
        {
            Helper.CheckCondition(type.IsEnum, "{0} is not an enum type.", type);
#if !SILVERLIGHT
            return Enum.GetNames(type);
#else
            return type.GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => x.ToString()).ToArray();
#endif
        }

        public static IEnumerable<Type> GetImplementingTypes(this Type interfaceType)
        {
            if (!interfaceType.IsInterface) throw new ParseException("type {0} is not an interface", interfaceType);
            string interfaceName = interfaceType.Name;
            foreach (Type t in TypeFactory.GetReferencedTypes())
            {
                if (t.IsPublic && t.GetInterface(interfaceName,ignoreCase:true) != null)
                    yield return t;
            }
        }

        public static IEnumerable<Type> GetDerivedTypes(this Type classType)
        {
            foreach (Type t in TypeFactory.GetReferencedTypes())
            {
                if (t.IsPublic && t.IsSubclassOf(classType))
                    yield return t;
            }
        }

        public static bool Implements(this Type type, Type interfaceType)
        {
            return type.GetInterface(interfaceType.Name, ignoreCase: false) != null;
        }

        public static bool IsSubclassOfOrImplements(this Type type, Type baseType)
        {
            return baseType.IsInterface ? type.Implements(baseType) : type.IsSubclassOf(baseType);
        }

        public static IEnumerable<PropertyInfo> GetPropertiesOfType(this Type type, Type propertyType)
        {
            var result = from p in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                         where p.PropertyType == propertyType
                         select p;

            return result;
        }

        public static IEnumerable<FieldInfo> GetFieldsOfType(this Type type, Type fieldType)
        {
            var result = from p in type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                         where p.FieldType == fieldType
                         select p;

            return result;
        }

        public static IEnumerable<MemberInfo> GetFieldsAndPropertiesOfType(this Type type, Type memberType)
        {
            var result = type.GetFieldsOfType(memberType).Cast<MemberInfo>().Concat(type.GetPropertiesOfType(memberType).Cast<MemberInfo>());

            return result;
        }
    }
}

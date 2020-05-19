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
using System.Text;
using Bio.Util;

namespace MBT.Escience.Parse
{
    //!! This code is largely taken from MatrixFactory
    public static class TypeFactory
    {
        private static IEnumerable<Assembly> _allReferencedAssemblies;
        private static IEnumerable<Assembly> AllReferencedAssemblies
        {
            get
            {
                if (_allReferencedAssemblies == null)
                {
                    var userAssemblies = EnumerateAllUserAssemblyCodeBases().ToHashSet();
                    var systemAssemblies = EnumerateReferencedSystemAssemblies(userAssemblies).ToHashSet();
                    _allReferencedAssemblies = userAssemblies.Union(systemAssemblies);
                }
                return _allReferencedAssemblies;
            }
        }



        /// <summary>
        /// Returns the first type in any of the referenced assemblies that matches the type name. If typeName includes the namespace, 
        /// then matches on the fully qualified name. Else, looks for the first type in any of namespaces
        /// that matches typeName AND is a subtype of baseType (use typeof(object) as a default).
        /// </summary>
        /// <param name="typeName">The type name we're searching for. May either be fully qualified or contain only the class name</param>
        /// <param name="baseType">Will only return a type that is a subtype of baseType</param>
        /// <param name="returnType">The type matching typeName, if found, or null.</param>
        /// <returns>true iff the typeName could be resolved into a type.</returns>
        public static bool TryGetType(string typeName, Type baseType, out Type returnType)
        {
            // rename the built-int shortcuts.
            switch (typeName.ToLower())
            {
                case "int":
                    typeName = "Int32"; break;
                case "long":
                    typeName = "Int64"; break;
                case "bool":
                    typeName = "Boolean"; break;
                default:
                    break;
            }

            returnType = null;
            Type[] genericTypes;

            if (!TryGetGenericParameters(baseType, ref typeName, out genericTypes))
                return false;

            foreach (Assembly assembly in AllReferencedAssemblies)
            {
                returnType = GetType(assembly, baseType, typeName);

                if (returnType != null)
                {
                    if (genericTypes != null)
                    {
                        returnType = returnType.MakeGenericType(genericTypes);
                    }
                    return true;
                }
            }



            returnType = null;
            return false;
        }

        /// <summary>
        /// Returns the first type in assembly that matches the type name. If typeName includes the namespace, 
        /// then matches on the fully qualified name. Else, looks for the first type in any of Assembly's namespaces
        /// that matches typeName
        /// </summary>
        /// <param name="assembly">The assembly in which to search</param>
        /// <param name="baseType">Will only return a type that is a subtype of baseType</param>
        /// <param name="typeName">The type name we're searching for. May either be fully qualified or contain only the class name</param>
        /// <returns>The type, if found, or null.</returns>
        private static Type GetType(Assembly assembly, Type baseType, string typeName)
        {
            //SpecialFunctions.CheckDate(2010, 4, 13);
            //if (assembly.FullName.StartsWith("ShoViz"))
            //{
            //    return null;
            //}
            try
            {
                // if it's a fully qualified name (with namespace), then use the built in search
                if (typeName.Contains('.'))
                {
                    return assembly.GetType(typeName, throwOnError: false
#if !SILVERLIGHT
                    , ignoreCase: true);
#else
                        );
#endif
                }

                foreach (Type type in assembly.GetTypes())
                {
                    if (type.Name.Equals(typeName, StringComparison.CurrentCultureIgnoreCase) && type.IsSubclassOfOrImplements(baseType))
                        return type;
                }
            }
            catch (Exception) { }
            return null;
        }

        public static IEnumerable<Type> GetReferencedTypes()
        {
            foreach (Assembly assembly in AllReferencedAssemblies)
                foreach (Type type in GetAssemblyTypes(assembly))
                    yield return type;

        }

        private static IEnumerable<Type> GetAssemblyTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException)
            {
                // a problem with an assembly. just ignore
            }
            return new Type[0];
        }

        /// <summary>
        /// Checks to see if the type name is generic. If so, modifies the typeName and tries to construct the generic type arguments. 
        /// </summary>
        /// <param name="typeName">The type name. Will be modified if generic.</param>
        /// <param name="genericTypes">List of generic types. Null if this type is not generic.</param>
        /// <returns>True if there was no problem parsing. False is there is a problem.</returns>
        private static bool TryGetGenericParameters(Type baseType, ref string typeName, out Type[] genericTypes)
        {
            genericTypes = null;

            int firstIdx = typeName.IndexOf('<');
            if (firstIdx < 0)
                return true;

            int lastIdx = typeName.LastIndexOf('>');
            Helper.CheckCondition(lastIdx == typeName.Length - 1, "Unbalanced <>");

            string typeListString = typeName.Substring(firstIdx + 1, lastIdx - firstIdx - 1);
            typeName = typeName.Substring(0, firstIdx);
            List<Type> genericTypesAsList = new List<Type>();

            IEnumerable<string> typeArgs;
            try { typeArgs = typeListString.ProtectedSplit('<', '>', false, ','); }
            catch { return false; }

            foreach (string typeArgument in typeArgs)
            {
                Type genericArgumentType;
                if (!TryGetType(typeArgument, typeof(object), out genericArgumentType))
                    return false;
                genericTypesAsList.Add(genericArgumentType);
            }
            typeName += "`" + genericTypesAsList.Count;
            genericTypes = genericTypesAsList.ToArray();

            return true;
        }

        private static IEnumerable<Assembly> EnumerateAllUserAssemblyCodeBases()
        {
            Assembly entryAssembly = SpecialFunctions.GetEntryOrCallingAssembly();

            yield return entryAssembly;

            string exePath = Path.GetDirectoryName(entryAssembly.Location);
            Assembly assembly;
            foreach (string dllName in Directory.EnumerateFiles(exePath, "*.dll").Union(Directory.EnumerateFiles(exePath, "*.exe")))
            {
                assembly = null;
                try
                {
                    assembly = Assembly.LoadFrom(dllName);
                }
                catch
                {
                }
                if (assembly != null)
                {
                    yield return assembly;
                }
            }
        }

        private static IEnumerable<Assembly> EnumerateReferencedSystemAssemblies(IEnumerable<Assembly> userAssemblies)
        {
#if SILVERLIGHT
            yield break;
#else
            HashSet<string> alreadySeen = new HashSet<string>();
            foreach (Assembly userAssembly in userAssemblies)
            {
                if (!alreadySeen.Contains(userAssembly.FullName))
                {
                    alreadySeen.Add(userAssembly.FullName);
                    foreach (AssemblyName assemblyName in userAssembly.GetReferencedAssemblies())
                    {
                        // SpecialFunctions.CheckDate(2010, 4, 13);
                        if (assemblyName.FullName.StartsWith("System.Windows.Forms.DataVisualization"))
                        {
                            continue; //not break;
                        }

                        if (assemblyName.FullName.StartsWith("System") || assemblyName.FullName.StartsWith("mscorlib"))
                        {
                            Assembly systemAssembly = null;
                            try
                            {
                                systemAssembly = Assembly.Load(assemblyName);
                            }
                            catch { }
                            if (systemAssembly != null)
                                yield return systemAssembly;

                        }
                    }
                }
            }
#endif
        }
    }
}

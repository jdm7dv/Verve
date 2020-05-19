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
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using Microsoft.Win32;

namespace Bio.Registration
{
    /// <summary>
    /// Internal class gets the instance of defined (RegistrableAttribute) 
    /// attribute in Bio namespace
    /// </summary>
    internal class AssemblyResolver
    {
        #region -- Public Methods --
        /// <summary>
        /// Gets the MBF installed path from current assembly location
        /// </summary>
        /// <returns></returns>
        public static string BioInstallationPath
        {
            get
            {
                //typical path is
                //\Program Files\Microsoft Biology Initiative\Microsoft Biology Framework
                // it needs to get it from installer.
                try
                {
                    // for any exe under MBF
                    return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                }
                catch
                {
                    string codeBase = Assembly.GetCallingAssembly().CodeBase.ToString();
                    Uri uri = new Uri(codeBase);

                    // just for excel specific
                    if (codeBase.Contains("exce..vsto"))
                    {
                        //look into [HKEY_CURRENT_USER\Software\Microsoft\Office\Excel\Addins\ExcelWorkbench]
                        RegistryKey regKeyAppRoot = Registry.CurrentUser.OpenSubKey
                            (@"Software\Microsoft\Office\Excel\Addins\ExcelWorkbench");
                        uri = new Uri(regKeyAppRoot.GetValue("Manifest").ToString());
                    }
                    return Uri.UnescapeDataString(Path.GetDirectoryName(uri.AbsolutePath));
                }
            }
        }
                
        /// <summary>
        /// Resolves the local/loaded assembly with the registed attribute
        /// </summary>
        /// <returns>List of objects</returns>
        public static IList<object> Resolve()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            return Resolve(assembly);
        }

        /// <summary>
        /// Resolves the specified assembly with the registed attribute
        /// </summary>
        /// <param name="assemblyName">assembly name</param>
        /// <returns>List of objects</returns>
        public static IList<object> Resolve(string assemblyName)
        {
            Assembly assembly;
            string excutingAssemblyPath = Path.GetFileName(
                Assembly.GetExecutingAssembly().GetName().CodeBase);
            if (assemblyName.IndexOf(
                excutingAssemblyPath, StringComparison.OrdinalIgnoreCase) > 0)
            {
                assembly = Assembly.GetExecutingAssembly();
            }
            else
            {
                assembly = Assembly.LoadFrom(assemblyName);
            }
            return Resolve(assembly);
        }

        #endregion -- Public Methods --

        #region -- Private Methods --
        /// <summary>
        /// Creates the instance of specified type
        /// </summary>
        /// <param name="assembly">assembly reference</param>
        /// <returns>List of objects</returns>
        private static IList<object> Resolve(Assembly assembly)         
        {
            RegistrableAttribute registrableAttribute;
            List<object> resolvedTypes = new List<object>();

            Type[] availableTypes = assembly.GetExportedTypes();

            foreach (Type availableType in availableTypes)
            {
                registrableAttribute = (RegistrableAttribute)Attribute.GetCustomAttribute(
                    availableType, typeof(RegistrableAttribute));
                if (registrableAttribute != null)
                {
                    if (registrableAttribute.IsRegistrable)
                    {
                        try
                        {
                            //most of the time, MissingMethodException
                            object obj = assembly.CreateInstance(availableType.FullName);
                            resolvedTypes.Add(obj);
                        }
                        catch(ArgumentException)
                        {
                            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
                                Properties.Resource.REGISTRATION_LOADING_ERROR, assembly.GetName().CodeBase, 
                                availableType.FullName));
                        }
                    }
                }
            }

            return resolvedTypes;
        }
        #endregion -- Private Methods --
    }
}
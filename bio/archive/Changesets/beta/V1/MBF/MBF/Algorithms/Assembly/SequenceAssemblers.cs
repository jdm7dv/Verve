// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using MBF.Registration;

namespace MBF.Algorithms.Assembly
{
    /// <summary>
    /// SequenceAssemblers class is an abstraction class which provides instances
    /// and lists of all Assemblers currently supported by MBF. 	
    /// </summary>
    public static class SequenceAssemblers
    {
        /// <summary>
        /// A singleton instance of SimpleSequenceAssembler which implements
        /// a simple greedy assembly algorithm for DNA.
        /// </summary>
        private static OverlapDeNovoAssembler simple = new OverlapDeNovoAssembler();

        /// <summary>
        /// List of sequence assemblers.
        /// </summary>
        private static List<IDeNovoAssembler> all = new List<IDeNovoAssembler>() { SequenceAssemblers.simple };

        /// <summary>
        /// Gets an instance of SimpleSequenceAssembler which implements
        /// a simple greedy assembly algorithm for DNA.
        /// </summary>
        public static OverlapDeNovoAssembler Simple
        {
            get
            {
                return simple;
            }
        }

        /// <summary>
        /// Gets the list of all assemblers which is supported by the framework.
        /// </summary>
        public static IList<IDeNovoAssembler> All
        {
            get
            {
                return all.AsReadOnly();
            }
        }

        /// <summary>
        /// Static constructor
        /// </summary>
        static SequenceAssemblers()
        {
            //get the registered assemblers
            IList<IDeNovoAssembler> registeredAssemblers = GetAssemblers(true);

            if (null != registeredAssemblers && registeredAssemblers.Count > 0)
            {
                foreach (IDeNovoAssembler assembler in registeredAssemblers)
                {
                    if (assembler != null && all.FirstOrDefault(IA => string.Compare(IA.Name, assembler.Name,
                        StringComparison.InvariantCultureIgnoreCase) == 0) == null)
                    {
                        all.Add(assembler);
                    }
                }
                registeredAssemblers.Clear();
            }
        }

        /// <summary>
        /// Get all registered assemblers in core folder and addins (optional) folders
        /// </summary>
        /// <param name="includeAddinFolder">include add-ins folder or not</param>
        /// <returns>List of registered assemblers</returns>
        private static IList<IDeNovoAssembler> GetAssemblers(bool includeAddinFolder)
        {
            IList<IDeNovoAssembler> registeredAssemblers = new List<IDeNovoAssembler>();

            if (includeAddinFolder)
            {
                IList<IDeNovoAssembler> addInAssemblers;
                if (null != RegisteredAddIn.AddinFolderPath)
                {
                    addInAssemblers = RegisteredAddIn.GetInstancesFromAssemblyPath<IDeNovoAssembler>(RegisteredAddIn.AddinFolderPath, RegisteredAddIn.DLLFilter);
                    if (null != addInAssemblers && addInAssemblers.Count > 0)
                    {
                        foreach (IDeNovoAssembler assembler in addInAssemblers)
                        {
                            if (assembler != null &&
                                registeredAssemblers.FirstOrDefault(IA => string.Compare(IA.Name, assembler.Name,
                                    StringComparison.OrdinalIgnoreCase) == 0) == null)
                            {
                                registeredAssemblers.Add(assembler);
                            }
                        }
                    }
                }
            }
            return registeredAssemblers;
        }
    }
}

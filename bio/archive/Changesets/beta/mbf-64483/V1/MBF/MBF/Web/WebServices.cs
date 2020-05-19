// *****************************************************************
//    Copyright (c) Microsoft. All rights reserved.
//    This code is licensed under the Microsoft Public License.
//    THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
//    ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
//    IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
//    PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
// *****************************************************************

using System.Collections.Generic;
using MBF.Properties;
using MBF.Registration;
using MBF.Web.Blast;
using System.IO;

namespace MBF.Web
{
    /// <summary>
    /// WebServices class is an abstraction class which provides instances
    /// and lists of all Webservices currently supported by MBF. 
    /// </summary>
    public static class WebServices
    {
        /// <summary>
        /// List of supported Webservices by the MBF.
        /// </summary>
        private static List<IBlastServiceHandler> all = (List<IBlastServiceHandler>)
            RegisteredAddIn.GetInstancesFromAssembly<IBlastServiceHandler>(
                Path.Combine(AssemblyResolver.MBFInstallationPath, Resource.SERVICE_HANDLER_ASSEMBLY));

        /// <summary>
        /// Gets an instance of NcbiQBlast class which implements the client side 
        /// functionality required to perform Blast Search Requests against the 
        /// the NCBI QBlast system using their Blast URL APIs. 
        /// </summary>
        public static IBlastServiceHandler NcbiBlast
        {
            get
            {
                foreach (IBlastServiceHandler serviceHandler in All)
                {
                    if (serviceHandler.Name.Equals(Resource.NCBIQBLAST_NAME))
                    {
                        return serviceHandler;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets an instance of EBI WUBlast class which will implement the 
        /// client side functionality required to perform Blast Search Requests 
        /// against the EBI WUBlast web-service using their published interface proxy.
        /// </summary>
        public static IBlastServiceHandler EbiBlast
        {
            get
            {
                foreach (IBlastServiceHandler serviceHandler in All)
                {
                    if (serviceHandler.Name.Equals(Resource.EBIWUBLAST_NAME))
                    {
                        return serviceHandler;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets an instance of AzureBlast class which will implement the 
        /// client side functionality required to perform Blast Search Requests 
        /// against the Azure Blast web-service using their published interface proxy.
        /// </summary>
        public static IBlastServiceHandler AzureBlast
        {
            get
            {
                foreach (IBlastServiceHandler serviceHandler in All)
                {
                    if (serviceHandler.Name.Equals(Resource.AZURE_BLAST_NAME))
                    {
                        return serviceHandler;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets an instance of BioHPC Blast class which will implement the 
        /// client side functionality required to perform Blast Search Requests 
        /// against the Azure Blast web-service using their published interface proxy.
        /// </summary>
        public static IBlastServiceHandler BioHPCBlast
        {
            get
            {
                foreach (IBlastServiceHandler serviceHandler in All)
                {
                    if (serviceHandler.Name.Equals(Resource.BIOHPC_BLAST_NAME))
                    {
                        return serviceHandler;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets the list of all Webservices supported by the MBF.
        /// </summary>
        public static IList<IBlastServiceHandler> All
        {
            get
            {
                return all.AsReadOnly();
            }
        }
    }
}

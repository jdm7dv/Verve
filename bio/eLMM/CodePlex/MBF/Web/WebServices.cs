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



using System.Collections.Generic;
using Bio.Properties;
using Bio.Registration;
using Bio.Web.Blast;
using System.IO;

namespace Bio.Web
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
                Path.Combine(AssemblyResolver.BioInstallationPath, Resource.SERVICE_HANDLER_ASSEMBLY));

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
        /// Gets an instance of BIWUBlast class which will implement the 
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

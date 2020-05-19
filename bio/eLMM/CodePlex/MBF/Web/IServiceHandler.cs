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



namespace Bio.Web
{
    /// <summary>
    /// A simple summary of the status of a remote service request.
    /// </summary>
    public enum ServiceRequestStatus
    {
        /// <summary>
        /// The request is queued at the server.
        /// </summary>
        Queued,
        /// <summary>
        /// The request is being processed by the server.
        /// </summary>
        Waiting,
        /// <summary>
        /// The request has been processed and results are available.
        /// </summary>
        Ready,
        /// <summary>
        /// An error has occurred while processing the request.
        /// </summary>
        Error,
        /// <summary>
        /// The request has been cancelled at server.
        /// </summary>
        Canceled,
    }

    /// <summary>
    /// A return value for service requests, giving the status 
    /// as well as additional information from the server.
    /// </summary>
    public struct ServiceRequestInformation
    {
        /// <summary>
        /// The status summary.
        /// </summary>
        public ServiceRequestStatus Status { get; set; }

        /// <summary>
        /// Additional information from the server.
        /// </summary>
        public string StatusInformation { get; set; }

        /// <summary>
        /// Override the Equals implementation
        /// </summary>
        /// <param name="obj">The System.Object to compare with the current instance.</param>
        /// <returns>Returns true is given object is equal to current instance</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ServiceRequestInformation))
            {
                return false;
            }

            return Equals((ServiceRequestInformation)obj);
        }

        /// <summary>
        /// generate a number (hash code) that corresponds to the value of an object
        /// </summary>
        /// <returns>Hash of the object</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified ServiceRequestInformation is equal to the current instance.
        /// </summary>
        /// <param name="obj">The ServiceRequestInformation to compare with the current instance.</param>
        /// <returns>Returns true is given ServiceRequestInformation is equal to current instance</returns>
        public bool Equals(ServiceRequestInformation obj)
        {
            if (Status != obj.Status || StatusInformation != obj.StatusInformation)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Overloading ==
        /// </summary>
        /// <param name="object1">Object1 to be compared</param>
        /// <param name="object2">Object@ to be compared</param>
        /// <returns></returns>
        public static bool operator ==(ServiceRequestInformation object1, ServiceRequestInformation object2)
        {
            return object1.Equals(object2);
        }

        /// <summary>
        /// Overloading !=
        /// </summary>
        /// <param name="object1">Object1 to be compared</param>
        /// <param name="object2">Object2 to be compared</param>
        /// <returns></returns>
        public static bool operator !=(ServiceRequestInformation object1, ServiceRequestInformation object2)
        {
            return !object1.Equals(object2);
        }
    }

    /// <summary>
    /// Interface that must be extended by all the service providers of Microsoft 
    /// Biology Foundation. This interface contains properties that are common to any 
    /// type of service provided by MBF.
    /// </summary>
    public interface IServiceHandler
    {
        /// <summary>
        /// Gets user-friendly implementation description
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Gets user-friendly implementation name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets or sets settings for web access, such as user-agent string and 
        /// proxy configuration
        /// </summary>
        ConfigParameters Configuration { get; set; }
    }
}

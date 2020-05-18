using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace CompName
{
    /// <summary>
    /// This enumeration contains computer name values that are used 
    /// for the GetComputerNameEx() method call.
    /// </summary>
    public enum COMPUTER_NAME_FORMAT
    {
        ComputerNameNetBIOS,
        ComputerNameDnsHostname,
        ComputerNameDnsDomain,
        ComputerNameDnsFullyQualified,
        ComputerNamePhysicalNetBIOS,
        ComputerNamePhysicalDnsHostname,
        ComputerNamePhysicalDnsDomain,
        ComputerNamePhysicalDnsFullyQualified,
        ComputerNameMax
    };

    /// <summary>
    /// An interface used to access the ComputerName functions.
    /// </summary>
    [Guid("AD65E427-A809-4ca0-B408-D364D25D619A"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
    public interface IComputerName
    {
        string GetSingleName(COMPUTER_NAME_FORMAT NameType);
        string GetAllNames();
    }

    /// <summary>
    /// This class obtains computer name information from
    /// the remote server.
    /// </summary>
    [Guid("EE24578A-29CC-4554-95F6-C8F4D4F73284"),
    ClassInterface(ClassInterfaceType.None)]
    public class ComputerName : Component, IComputerName
    {
        public ComputerName()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// Obtains one of many names held by a computer. The precise name 
        /// returned is determined by the NameType argument.
        /// </summary>
        [DllImport("Kernel32.dll", 
             CharSet=CharSet.Auto, 
             SetLastError=true)]
        public static extern Boolean GetComputerNameEx(
            COMPUTER_NAME_FORMAT NameType, 
            StringBuilder lpBuffer, 
            ref Int32 lpnSize);

        public string GetSingleName(COMPUTER_NAME_FORMAT NameType)
        {
            StringBuilder  Buffer;     // Buffer used to hold name data.
            Int32          BufferSize; // Size of the data buffer on 
            // return.

            // Initialize the buffer.
            Buffer = new StringBuilder(80);
            BufferSize = 80;

            // Obtain the requested name string.
            if (GetComputerNameEx(NameType, Buffer, ref BufferSize))
                return Buffer.ToString();
            else
                return "No Name Available";
        }

        public string GetAllNames()
        {
            StringBuilder  Buffer;  // Buffer used to hold entire string.

            // Initialize the buffer.
            Buffer = new StringBuilder();

            // Call each name string in turn. Add the name of each string
            // before making the call.
            Buffer.Append("ComputerNameDnsDomain = ");
            Buffer.Append(
                GetSingleName(COMPUTER_NAME_FORMAT.ComputerNameDnsDomain));
            Buffer.Append("\r\nComputerNameDnsFullyQualified = ");
            Buffer.Append(
                GetSingleName
                (COMPUTER_NAME_FORMAT.ComputerNameDnsFullyQualified));
            Buffer.Append("\r\nComputerNameDnsHostname = ");
            Buffer.Append(GetSingleName
                (COMPUTER_NAME_FORMAT.ComputerNameDnsHostname));
            Buffer.Append("\r\nComputerNameMax = ");
            Buffer.Append(
                GetSingleName(COMPUTER_NAME_FORMAT.ComputerNameMax));
            Buffer.Append("\r\nComputerNameNetBIOS = ");
            Buffer.Append(
                GetSingleName
                (COMPUTER_NAME_FORMAT.ComputerNameNetBIOS));
            Buffer.Append("\r\nComputerNamePhysicalDnsDomain = ");
            Buffer.Append(
                GetSingleName
                (COMPUTER_NAME_FORMAT.ComputerNamePhysicalDnsDomain));
            Buffer.Append("\r\nComputerNamePhysicalDnsFullyQualified = ");
            Buffer.Append(
                GetSingleName
                (COMPUTER_NAME_FORMAT.ComputerNamePhysicalDnsFullyQualified));
            Buffer.Append("\r\nComputerNamePhysicalDnsHostname = ");
            Buffer.Append
                (GetSingleName
                (COMPUTER_NAME_FORMAT.ComputerNamePhysicalDnsHostname));
            Buffer.Append("\r\nComputerNamePhysicalNetBIOS = ");
            Buffer.Append(
                GetSingleName(COMPUTER_NAME_FORMAT.ComputerNamePhysicalNetBIOS));

            // Return the result of all the calls.
            return Buffer.ToString();
        }
    }
}

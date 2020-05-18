using System;
using System.Runtime.InteropServices;

namespace MyDataType
{
   /// <summary>
   /// This interface enables access to the OrderEntry
   /// data members.
   /// </summary>
   [Guid("6B4BF1B5-C7FF-48ee-8D5A-DFA5004E6CF3"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
   public interface IOrderEntry
   {
   }

	/// <summary>
	/// This class acts as a data type used to organize
	/// the information for transfer between client and
	/// server.
	/// </summary>
   [Guid("BDB66C95-28D6-4fa0-9AD6-4FD540BC6073"),
    ClassInterface(ClassInterfaceType.None)]
   public class OrderEntry : IOrderEntry
	{
      public String  ClientName;
      public String  ClientID;
      public String  ItemID;
      public String  ItemName;
      public Int32   ItemQty;
      public Decimal ItemPrice;
   }
}

using System;
using System.EnterpriseServices;
using System.Runtime.InteropServices;

// This attribute turns off access control checks for this
// example. Normally you would turn the access control checks
// on and define various security checks.
[assembly: ApplicationAccessControl(false)]

// This attribute gives the COM+ application a specific name.
[assembly: ApplicationName("SimpleEventApp")]

namespace SimpleEventObject
{
   /// <summary>
   /// An interface used to access the SendMsg functions.
   /// </summary>
   [Guid("0E4DB900-60F3-4354-A89A-C90A962F468C"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
   public interface ISendMsg
   {
      void FireBroadcastMsg(String strMsg);
   }

   /// <summary>
	/// This class shows how to create a very simple event
	/// object used to create a bridge between a publisher
	/// and a subscriber.
	/// </summary>
   [Guid("6A121716-471A-479f-B1DE-97858FE95483"),
    ClassInterface(ClassInterfaceType.None)]
	public class SendMsg : ServicedComponent, ISendMsg
	{
		public SendMsg()
		{
		}

      /// <summary>
      /// This method acts as a template for the actual
      /// event object.
      /// </summary>
      /// <param name="strMsg">The data we want to send
      /// as a broadcast message.</param>
      public void FireBroadcastMsg(String strMsg)
      {
      }
	}
}

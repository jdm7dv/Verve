using System;
using System.EnterpriseServices;
using System.Runtime.InteropServices;
using SimpleEventObject;
using System.Windows.Forms;

// This attribute turns off access control checks for this
// example. Normally you would turn the access control checks
// on and define various security checks.
[assembly: ApplicationAccessControl(false)]

// This attribute gives the COM+ application a specific name.
[assembly: ApplicationName("SimpleSubscriber")]

namespace SubscriberComponent
{
	/// <summary>
	/// This class subscribes to the event object that holds
	/// the message created by the publisher.
	/// </summary>
   [Guid("3162ED10-C74D-4967-BA51-C8EBD10A8D03"),
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
         MessageBox.Show(strMsg,
                         "Publisher Message",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Information);
      }
   }
}

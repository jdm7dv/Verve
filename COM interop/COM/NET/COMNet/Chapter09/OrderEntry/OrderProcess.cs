using System;
using System.Diagnostics;
using System.EnterpriseServices;
using System.Messaging;
using System.Runtime.InteropServices;
using MyDataType;

// This attribute turns off access control checks for this
// example. Normally you would turn the access control checks
// on and define various security checks.
[assembly: ApplicationAccessControl(false)]

// This attribute gives the COM+ application a specific name.
[assembly: ApplicationName("OrderEntryApp")]

namespace OrderEntryComp
{
   /// <summary>
   /// This interface provides access to the OrderHandler class.
   /// </summary>
   [Guid("6BEFA054-CC52-4fc1-ADE4-ADCB2BB4E6BE"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
   public interface IOrderProcess
   {
      void ProcessMessage();
   }

	/// <summary>
	/// This class accepts order entry messages and passes them
	/// to the Event Log.
	/// </summary>
   [Guid("F6B0D315-DD16-4659-BE4E-FEF5ED7664B7"),
    ClassInterface(ClassInterfaceType.None)]
   public class OrderProcess : ServicedComponent, IOrderProcess
	{
		public OrderProcess()
		{

      }

      #region IOrderProcess Members

      public void ProcessMessage()
      {
         MessageQueue   TempMQ;  // Message queue.
         Message        Msg;     // Message to receive.
         EventLog       EV;      // Message data storage.
         OrderEntry     OE;      // OrderEntry data.

         // Create and initialize the message queue.
         TempMQ = new System.Messaging.MessageQueue();
         TempMQ.Path = ".\\OrderEntryApp";

         // Set the queue formatter.
         TempMQ.Formatter = new XmlMessageFormatter(
                               new Type [] {typeof(OrderEntry)});

         // Create and initialize the event log.
         EV = new EventLog("Application", ".", "MyPlayer Component");

         // Initialize the OrderEntry object.
         OE = new OrderEntry();

         // Get the message.
         try
         {
            Msg = TempMQ.Receive(new TimeSpan(5));
         }

         // If the timespan elapses before the message arrives,
         // MSMQ will throw an exception.
         catch (MessageQueueException MQE)
         {
            EV.WriteEntry("No Messages to Retrieve\r\n" +
                          MQE.Message, 
                          EventLogEntryType.Error, 
                          1001, 
                          100);
            return;
         }

         // Obtain the data.
         OE = (OrderEntry)Msg.Body;

         // Write the message.
         EV.WriteEntry("Client ID: " + OE.ClientID +
                       "\r\nClient Name: " + OE.ClientName +
                       "\r\nItem ID: " + OE.ItemID +
                       "\r\nItem Name: " + OE.ItemName +
                       "\r\nItem Price: " + OE.ItemPrice.ToString("C2") +
                       "\r\nItem Quantity: " + OE.ItemQty.ToString(), 
                       EventLogEntryType.Information, 
                       1002, 
                       100);
      }

      #endregion
   }
}

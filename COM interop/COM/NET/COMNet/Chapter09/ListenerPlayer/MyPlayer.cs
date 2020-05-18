using System;
using System.Diagnostics;
using System.EnterpriseServices;
using System.Messaging;
using System.Runtime.InteropServices;

// This attribute turns off access control checks for this
// example. Normally you would turn the access control checks
// on and define various security checks.
[assembly: ApplicationAccessControl(false)]

// This attribute gives the COM+ application a specific name.
[assembly: ApplicationName("MyPlayerApp")]

namespace ListenerPlayer
{
   /// <summary>
   /// This interface provides access to the methods in the
   /// MyPlayer class.
   /// </summary>
   [Guid("140E024D-4145-4cb4-BB3D-E44AA7AF4556"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
   public interface IMyPlayer
   {
      void ProcessMessage();
   }

	/// <summary>
	/// The MyPlayer class recives messages from the Temp
	/// message queue using MSMQ and processes them.
	/// </summary>
   [Guid("77510036-E9F5-4536-BA27-925A4FDFA621"),
    ClassInterface(ClassInterfaceType.None)]
   public class MyPlayer : ServicedComponent, IMyPlayer
	{
		public MyPlayer()
		{

      }

      #region IMyPlayer Members

      public void ProcessMessage()
      {
         MessageQueue   TempMQ;  // Message queue.
         Message        Msg;     // Message to receive.
         EventLog       EV;      // Message data storage.

         

         // Create and initialize the message queue.
         TempMQ = new System.Messaging.MessageQueue();
         TempMQ.Path = ".\\Temp";
         //TempMQ.SynchronizingObject = this;

         // Set the queue formatter.
         TempMQ.Formatter = new XmlMessageFormatter(
                               new Type [] {typeof(String)});

         // Set the queue priority filter.
         TempMQ.MessageReadPropertyFilter.Priority = true;

         // Create and initialize the event log.
         EV = new EventLog("Application", ".", "MyPlayer Component");

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

         // Display the message.
         EV.WriteEntry("Label: " + Msg.Label +
                       "\r\nBody: " + Msg.Body +
                       "\r\nPriority: " + Msg.Priority.ToString(), 
                       EventLogEntryType.Information, 
                       1002, 
                       100);
      }

      #endregion
   }
}

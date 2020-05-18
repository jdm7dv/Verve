using System;
using System.Data;
using System.Data.SqlClient;
using System.EnterpriseServices;
using System.Runtime.InteropServices;

// This attribute turns off access control checks for this
// example. Normally you would turn the access control checks
// on and define various security checks.
[assembly: ApplicationAccessControl(false)]

// This attribute gives the COM+ application a specific name.
[assembly: ApplicationName("HelpAccessApp")]

// This attribute sets the application up as a server, rather than
// a library application.
[assembly: ApplicationActivation(ActivationOption.Server)]

namespace HelpAccess
{
   [Guid("DF830C4C-7EB0-48ec-BBDE-98DA886AAFCA"),
    InterfaceType(ComInterfaceType.InterfaceIsDual)]
   public interface IHelpAccess
   {
      void GetTitle(ref String strTopicNumber,
                    ref String strTitle,
                    ref String strContents);
      void GetTopic(ref String strTopicNumber,
                    ref String strTitle,
                    ref String strContents);
   }

   [Guid("C7E5F3CF-043A-439c-8358-3AD9F21FA3A4"),
    ClassInterface(ClassInterfaceType.None)]
   public class HelpAccess : ServicedComponent, IHelpAccess
	{
		public HelpAccess()
		{
      }

      #region IHelpAccess Members

      public void GetTitle(ref String strTopicNumber, ref String strTitle, ref String strContents)
      {
         SqlConnection  Conn;    // Database Connction
         SqlCommand     Cmd;     // Data Selection
         SqlDataReader  Reader;  // Data Container
         Object         []Data;  // One Row of Data Values

         // Create a connection to the server.
         Conn = new SqlConnection("Initial Catalog=HelpMe;" +
                                  "Data Source=WinServer;" +
                                  "Integrated Security=SSPI;");

         // Create a command for retrieving the data.
         Cmd = new SqlCommand("SELECT * FROM HelpInformation " +
                              "WHERE Title='" + strTitle + "'", Conn);

         // Open the connection and execute the command.
         Conn.Open();
         Reader = Cmd.ExecuteReader();

         // Load the field count from the database.
         Data = new Object[Reader.FieldCount];

         // Determine if the database found the requested information.
         if (Reader.Read())
         {
            // If so, return this information to the user.
            Reader.GetValues(Data);
            strTopicNumber = Data[0].ToString();
            strTitle = Data[1].ToString();
            strContents = Data[2].ToString();
         }
         else
         {
            // Otherwise, return an error message.
            strContents = "Couldn't find the search value: " + strTitle;
            strTopicNumber = "99999";
            strTitle = "Error Finding Value";
         }

         // Close the database connection.
         Cmd.Connection.Close();
      }

      public void GetTopic(ref String strTopicNumber, ref String strTitle, ref String strContents)
      {
         SqlConnection  Conn;    // Database Connction
         SqlCommand     Cmd;     // Data Selection
         SqlDataReader  Reader;  // Data Container
         Object         []Data;  // One Row of Data Values

         // Create a connection to the server.
         Conn = new SqlConnection("Initial Catalog=HelpMe;" +
                                  "Data Source=WinServer;" +
                                  "Integrated Security=SSPI;");

         // Create a command for retrieving the data.
         Cmd = new SqlCommand("SELECT * FROM HelpInformation " +
                              "WHERE TopicNumber='" + strTopicNumber + 
                              "'", Conn);

         // Open the connection and execute the command.
         Conn.Open();
         Reader = Cmd.ExecuteReader();

         // Load the field count from the database.
         Data = new Object[Reader.FieldCount];

         // Determine if the database found the requested information.
         if (Reader.Read())
         {
            // If so, return this information to the user.
            Reader.GetValues(Data);
            strTopicNumber = Data[0].ToString();
            strTitle = Data[1].ToString();
            strContents = Data[2].ToString();
         }
         else
         {
            // Otherwise, return an error message.
            strContents = "Couldn't find the search value: " + 
                          strTopicNumber;
            strTopicNumber = "99999";
            strTitle = "Error Finding Value";
         }

         // Close the database connection.
         Cmd.Connection.Close();
      }

      #endregion
   }
}

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using COMAdmin;
using System.Runtime.InteropServices;
using SubscriberComponent;

namespace AppSubscribe
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
      private System.Windows.Forms.Button btnQuit;
      private System.Windows.Forms.Button btnPermanent;
      private System.Windows.Forms.Button btnTransient;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

      // Declare the administration objects used in this application.
      // COM Administrator
      private COMAdminCatalogClass  ComAdmin;
      // Application Collection
      private ICatalogCollection    AppCollection;
      // Component Collection
      private ICatalogCollection    CompCollection;
      // Subscription Collection
      private ICatalogCollection    SubCollection;
      // A Single Entry Within a Collection
      private ICatalogObject        CatalogObject;
      // Transient Subscription Identifier
      private Object                TransID;
      // Permanent Subscription Identifier
      private Object                PermID;

		public frmMain()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			// Initialize the administration object.
         ComAdmin = new COMAdminCatalogClass();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
         this.btnQuit = new System.Windows.Forms.Button();
         this.btnPermanent = new System.Windows.Forms.Button();
         this.btnTransient = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // btnQuit
         // 
         this.btnQuit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.btnQuit.Location = new System.Drawing.Point(160, 8);
         this.btnQuit.Name = "btnQuit";
         this.btnQuit.Size = new System.Drawing.Size(120, 23);
         this.btnQuit.TabIndex = 0;
         this.btnQuit.Text = "Quit";
         this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
         // 
         // btnPermanent
         // 
         this.btnPermanent.Location = new System.Drawing.Point(160, 40);
         this.btnPermanent.Name = "btnPermanent";
         this.btnPermanent.Size = new System.Drawing.Size(120, 23);
         this.btnPermanent.TabIndex = 1;
         this.btnPermanent.Text = "Set Permanent";
         this.btnPermanent.Click += new System.EventHandler(this.btnPermanent_Click);
         // 
         // btnTransient
         // 
         this.btnTransient.Location = new System.Drawing.Point(160, 72);
         this.btnTransient.Name = "btnTransient";
         this.btnTransient.Size = new System.Drawing.Size(120, 23);
         this.btnTransient.TabIndex = 2;
         this.btnTransient.Text = "Set Transient";
         this.btnTransient.Click += new System.EventHandler(this.btnTransient_Click);
         // 
         // frmMain
         // 
         this.AcceptButton = this.btnQuit;
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
         this.ClientSize = new System.Drawing.Size(292, 266);
         this.Controls.Add(this.btnTransient);
         this.Controls.Add(this.btnPermanent);
         this.Controls.Add(this.btnQuit);
         this.Name = "frmMain";
         this.Text = "Simple Event Application";
         this.Closed += new System.EventHandler(this.frmMain_Closed);
         this.ResumeLayout(false);

      }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new frmMain());
		}

      private void btnQuit_Click(object sender, System.EventArgs e)
      {
         // Exit the application.
         Close();
      }

      private void btnPermanent_Click(object sender, System.EventArgs e)
      {
         String   IDVal;   // Temporary PermID

         // Obtain the current application collection and the populate
         // the application collection object with members of that
         // collection.
         AppCollection = 
            (ICatalogCollection)ComAdmin.GetCollection("Applications");
         AppCollection.Populate();

         // Locate the application we're interested in subscribing to.
         for (int Counter = 0; Counter < AppCollection.Count; Counter++)
         {
            // Get the current catalog object.
            CatalogObject = 
               (ICatalogObject)AppCollection.get_Item(Counter);

            // Determine if this is the correct application.
            if ((String)CatalogObject.Name == "SimpleSubscriber")
            {
               // Obtain the current component collection for the
               // target application.
               CompCollection = 
                  (ICatalogCollection)AppCollection.GetCollection(
                     "Components", CatalogObject.Key);
               CompCollection.Populate();
               break;
            }
         }

         // Locate the target component.
         for (int Counter = 0; Counter < CompCollection.Count; Counter++)
         {
            // Get the current catalog object.
            CatalogObject = 
               (ICatalogObject)CompCollection.get_Item(Counter);

            // Determine if this is the correct component.
            if ((String)CatalogObject.Name == "SubscriberComponent.SendMsg")
            {
               // Obtain the subscription collection for the target
               // component.
               SubCollection = 
                  (ICatalogCollection)CompCollection.GetCollection(
                     "SubscriptionsForComponent", CatalogObject.Key);
               SubCollection.Populate();
               break;
            }
         }

         // Depending on the current subscription status, we'll
         // either get a new subscription or free an existing
         // subscription.
         if (btnPermanent.Text == "Set Permanent")
         {
            // Add a new subscription to the permanent subscription
            // collection.
            CatalogObject = (ICatalogObject)SubCollection.Add();

            // Set the catalog object values. This list represents a
            // minimal implementation.
            CatalogObject.set_Value(
               "EventCLSID", "{6A121716-471A-479f-B1DE-97858FE95483}");
            CatalogObject.set_Value("Name", "Permanent Subscription");
            CatalogObject.set_Value("MethodName", "FireBroadcastMsg");
            CatalogObject.set_Value(
               "InterfaceID", "{0E4DB900-60F3-4354-A89A-C90A962F468C}");
            CatalogObject.set_Value("Enabled", true);

            // Save the changes we've made to the transient subscription
            // collection.
            SubCollection.SaveChanges();

            // Change the button caption to match the current subscription
            // status.
            btnPermanent.Text = "Release Permanent";

            // Save the subscription ID for later use.
            PermID = CatalogObject.get_Value("ID");
         }

            // We need to release an existing subscription.
         else
         {
            // Populate the subcollection with existing subscription
            // information.
            SubCollection.Populate();

            // Check each of the subscription items in turn.
            for (int Counter = 0; 
               Counter < SubCollection.Count; 
               Counter++)
            {
               // Get the current catalog object.
               CatalogObject = 
                  (ICatalogObject)SubCollection.get_Item(Counter);

               // See if this is the correct catalog object.
               IDVal = (String)CatalogObject.get_Value("ID");
               if (IDVal == PermID.ToString())
               {
                  // Remove the subscription from the list.
                  SubCollection.Remove(Counter);
                  SubCollection.SaveChanges();

                  // Exit the loop.
                  break;
               }
            }

            // Change the button caption to match the current subscription
            // status.
            btnPermanent.Text = "Set Permanent";
         }
      }

      private void btnTransient_Click(object sender, System.EventArgs e)
      {
         String   IDVal;   // Temporary TransID

         // Get the transient subscription collection.
         SubCollection = 
            (ICatalogCollection)ComAdmin.GetCollection(
               "TransientSubscriptions");

         // Create an object of the correct type.
         SendMsg SM = new SendMsg();

         // Depending on the current subscription status, we'll
         // either get a new subscription or free an existing
         // subscription.
         if (btnTransient.Text == "Set Transient")
         {
            // Add a new subscritpion to the transient subscription
            // collection.
            CatalogObject = (ICatalogObject)SubCollection.Add();

            // Set the catalog object values. This list represents a
            // minimal implementation.
            CatalogObject.set_Value(
               "EventCLSID", "{6A121716-471A-479f-B1DE-97858FE95483}");
            CatalogObject.set_Value("Name", "Transient Subscription");
            CatalogObject.set_Value("MethodName", "FireBroadcastMsg");
            CatalogObject.set_Value(
               "InterfaceID", "{0E4DB900-60F3-4354-A89A-C90A962F468C}");
            CatalogObject.set_Value("SubscriberInterface", SM);
            CatalogObject.set_Value("Enabled", true);

            // Save the changes we've made to the transient subscription
            // collection.
            SubCollection.SaveChanges();

            // Change the button caption to match the current subscription
            // status.
            btnTransient.Text = "Release Transient";

            // Save the subscription ID for later use.
            TransID = CatalogObject.get_Value("ID");
         }

         // We need to release an existing subscription.
         else
         {
            // Populate the subcollection with existing subscription
            // information.
            SubCollection.Populate();

            // Check each of the subscription items in turn.
            for (int Counter = 0; 
                     Counter < SubCollection.Count; 
                     Counter++)
            {
               // Get the current catalog object.
               CatalogObject = 
                  (ICatalogObject)SubCollection.get_Item(Counter);

               // See if this is the correct catalog object.
               IDVal = (String)CatalogObject.get_Value("ID");
               if (IDVal == TransID.ToString())
               {
                  // Remove the subscription from the list.
                  SubCollection.Remove(Counter);
                  SubCollection.SaveChanges();

                  // Exit the loop.
                  break;
               }
            }

            // Change the button caption to match the current subscription
            // status.
            btnTransient.Text = "Set Transient";
         }
      }

      private void frmMain_Closed(object sender, System.EventArgs e)
      {
         // Clean up the COM objects before the application exits.
         TransID = null;
         PermID = null;
         CatalogObject = null;
         AppCollection = null;
         CompCollection = null;
         SubCollection = null;
         ComAdmin = null;
      }
	}
}

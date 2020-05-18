using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using MyDataType;
using System.Messaging;

namespace ManagedClient
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
      private System.Windows.Forms.Button btnQuit;
      private System.Windows.Forms.Button btnSend;
      private System.Messaging.MessageQueue TempMQ;
      private System.Messaging.MessageQueue OrderEntryMQ;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox txtClientID;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.TextBox txtClientName;
      private System.Windows.Forms.TextBox txtItemID;
      private System.Windows.Forms.TextBox txtItemName;
      private System.Windows.Forms.TextBox txtItemPrice;
      private System.Windows.Forms.TextBox txtItemQty;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
            this.btnSend = new System.Windows.Forms.Button();
            this.TempMQ = new System.Messaging.MessageQueue();
            this.OrderEntryMQ = new System.Messaging.MessageQueue();
            this.label1 = new System.Windows.Forms.Label();
            this.txtClientID = new System.Windows.Forms.TextBox();
            this.txtClientName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtItemID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtItemName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtItemPrice = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtItemQty = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnQuit
            // 
            this.btnQuit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnQuit.Location = new System.Drawing.Point(208, 40);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.TabIndex = 1;
            this.btnQuit.Text = "&Quit";
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(208, 8);
            this.btnSend.Name = "btnSend";
            this.btnSend.TabIndex = 0;
            this.btnSend.Text = "&Send";
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // TempMQ
            // 
            this.TempMQ.Path = ".\\Temp";
            this.TempMQ.SynchronizingObject = this;
            // 
            // OrderEntryMQ
            // 
            this.OrderEntryMQ.Path = "brianjo600\\OrderEntryApp";
            this.OrderEntryMQ.SynchronizingObject = this;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.TabIndex = 2;
            this.label1.Text = "&Client ID:";
            // 
            // txtClientID
            // 
            this.txtClientID.Location = new System.Drawing.Point(8, 32);
            this.txtClientID.Name = "txtClientID";
            this.txtClientID.TabIndex = 3;
            this.txtClientID.Text = "1";
            // 
            // txtClientName
            // 
            this.txtClientName.Location = new System.Drawing.Point(8, 88);
            this.txtClientName.Name = "txtClientName";
            this.txtClientName.TabIndex = 5;
            this.txtClientName.Text = "George Smith";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 64);
            this.label2.Name = "label2";
            this.label2.TabIndex = 4;
            this.label2.Text = "Client &Name:";
            // 
            // txtItemID
            // 
            this.txtItemID.Location = new System.Drawing.Point(8, 144);
            this.txtItemID.Name = "txtItemID";
            this.txtItemID.TabIndex = 7;
            this.txtItemID.Text = "1234";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 120);
            this.label3.Name = "label3";
            this.label3.TabIndex = 6;
            this.label3.Text = "&Item ID:";
            // 
            // txtItemName
            // 
            this.txtItemName.Location = new System.Drawing.Point(8, 200);
            this.txtItemName.Name = "txtItemName";
            this.txtItemName.TabIndex = 9;
            this.txtItemName.Text = "Widget";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 176);
            this.label4.Name = "label4";
            this.label4.TabIndex = 8;
            this.label4.Text = "I&tem Name:";
            // 
            // txtItemPrice
            // 
            this.txtItemPrice.Location = new System.Drawing.Point(8, 256);
            this.txtItemPrice.Name = "txtItemPrice";
            this.txtItemPrice.TabIndex = 11;
            this.txtItemPrice.Text = "15.95";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(8, 232);
            this.label5.Name = "label5";
            this.label5.TabIndex = 10;
            this.label5.Text = "Item &Price:";
            // 
            // txtItemQty
            // 
            this.txtItemQty.Location = new System.Drawing.Point(8, 312);
            this.txtItemQty.Name = "txtItemQty";
            this.txtItemQty.TabIndex = 13;
            this.txtItemQty.Text = "5";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(8, 288);
            this.label6.Name = "label6";
            this.label6.TabIndex = 12;
            this.label6.Text = "Item &Quantity:";
            // 
            // frmMain
            // 
            this.AcceptButton = this.btnSend;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnQuit;
            this.ClientSize = new System.Drawing.Size(292, 366);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtItemQty);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtItemPrice);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtItemName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtItemID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtClientName);
            this.Controls.Add(this.txtClientID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnQuit);
            this.Name = "frmMain";
            this.Text = "Order Entry Demonstration";
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

      private void btnSend_Click(object sender, System.EventArgs e)
      {
         System.Messaging.Message   Msg;     // Message to send.
         OrderEntry                 OE;      // OrderEntry data.

         // Create the OrderEntry object.
         OE = new OrderEntry();
         OE.ClientID = txtClientID.Text;
         OE.ClientName = txtClientName.Text;
         OE.ItemID = txtItemID.Text;
         OE.ItemName = txtItemName.Text;
         OE.ItemPrice = Convert.ToDecimal(txtItemPrice.Text);
         OE.ItemQty = Convert.ToInt32(txtItemQty.Text);

         // Create the message content.
         Msg = new System.Messaging.Message();
         Msg.Body = OE;

         // Send the message.
         OrderEntryMQ.Send(Msg, "Order Entry");
      }

        
	}
}

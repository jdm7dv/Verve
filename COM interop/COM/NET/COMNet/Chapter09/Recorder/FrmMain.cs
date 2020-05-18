using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Messaging;

namespace Recorder
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
      private System.Messaging.MessageQueue TempMQ;
      private System.Windows.Forms.Button btnQuit;
      private System.Windows.Forms.Button btnSend;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.TextBox txtMessageHeader;
      private System.Windows.Forms.TextBox txtMessage;
      private System.Windows.Forms.ComboBox cbPriority;
      private System.Windows.Forms.Button btnReceive;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmMain()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			// Initialize the priority combo box.
         cbPriority.SelectedIndex = 0;
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
            this.TempMQ = new System.Messaging.MessageQueue();
            this.btnQuit = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMessageHeader = new System.Windows.Forms.TextBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.cbPriority = new System.Windows.Forms.ComboBox();
            this.btnReceive = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TempMQ
            // 
            this.TempMQ.Path = ".\\Temp";
            this.TempMQ.SynchronizingObject = this;
            // 
            // btnQuit
            // 
            this.btnQuit.Location = new System.Drawing.Point(408, 8);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.TabIndex = 0;
            this.btnQuit.Text = "&Quit";
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(408, 40);
            this.btnSend.Name = "btnSend";
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "&Send";
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.TabIndex = 3;
            this.label1.Text = "Message &Heading:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 64);
            this.label2.Name = "label2";
            this.label2.TabIndex = 5;
            this.label2.Text = "Message &Text:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 208);
            this.label3.Name = "label3";
            this.label3.TabIndex = 7;
            this.label3.Text = "&Priority:";
            // 
            // txtMessageHeader
            // 
            this.txtMessageHeader.Location = new System.Drawing.Point(8, 32);
            this.txtMessageHeader.Name = "txtMessageHeader";
            this.txtMessageHeader.Size = new System.Drawing.Size(384, 20);
            this.txtMessageHeader.TabIndex = 4;
            this.txtMessageHeader.Text = "Test Message Heading";
            // 
            // txtMessage
            // 
            this.txtMessage.Location = new System.Drawing.Point(8, 88);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(384, 104);
            this.txtMessage.TabIndex = 6;
            this.txtMessage.Text = "This is a test message.";
            // 
            // cbPriority
            // 
            this.cbPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPriority.Items.AddRange(new object[] {
                                                            "0",
                                                            "1",
                                                            "2",
                                                            "3",
                                                            "4",
                                                            "5",
                                                            "6",
                                                            "7"});
            this.cbPriority.Location = new System.Drawing.Point(8, 232);
            this.cbPriority.Name = "cbPriority";
            this.cbPriority.Size = new System.Drawing.Size(121, 21);
            this.cbPriority.TabIndex = 8;
            // 
            // btnReceive
            // 
            this.btnReceive.Location = new System.Drawing.Point(408, 72);
            this.btnReceive.Name = "btnReceive";
            this.btnReceive.TabIndex = 2;
            this.btnReceive.Text = "&Receive";
            this.btnReceive.Click += new System.EventHandler(this.btnReceive_Click);
            // 
            // frmMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(492, 266);
            this.Controls.Add(this.btnReceive);
            this.Controls.Add(this.cbPriority);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.txtMessageHeader);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnQuit);
            this.Name = "frmMain";
            this.Text = "Message Recorder Demonstation";
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

         // Create the message content.
         Msg = new System.Messaging.Message();
         Msg.Label = txtMessageHeader.Text;
         Msg.Body = txtMessage.Text;
         Msg.Priority = (MessagePriority)cbPriority.SelectedIndex;

         // Send the message.
         TempMQ.Send(Msg);
      }

      private void btnReceive_Click(object sender, System.EventArgs e)
      {
         System.Messaging.Message   Msg;  // Message to receive.

         // Set the queue formatter.
         TempMQ.Formatter = new XmlMessageFormatter(
                               new Type [] {typeof(String)});

         // Set the queue priority filter.
         TempMQ.MessageReadPropertyFilter.Priority = true;

         // Get the message.
         try
         {
            Msg = TempMQ.Receive(new TimeSpan(5));
         }

         // If the timespan elapses before the message arrives,
         // MSMQ will throw an exception.
         catch (MessageQueueException MQE)
         {
            MessageBox.Show("No Messages to Retrieve\r\n" +
                            MQE.Message,
                            "Message Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
            return;
         }

         // Display the message.
         MessageBox.Show("Label: " + Msg.Label +
                         "\r\nBody: " + Msg.Body +
                         "\r\nPriority: " + Msg.Priority.ToString(),
                         "Message Contents",
                         MessageBoxButtons.OK,
                         MessageBoxIcon.Information);
      }

        private void messageQueue1_ReceiveCompleted(object sender, System.Messaging.ReceiveCompletedEventArgs e)
        {
        
        }
	}
}

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using QuickTest.localhost;

namespace QuickTest
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
      private System.Windows.Forms.Button btnQuit;
      private System.Windows.Forms.Button btnSynchronous;
      private System.Windows.Forms.Button btnAsynchronous;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.TextBox txtInput1;
      private System.Windows.Forms.TextBox txtInput2;
      private System.Windows.Forms.TextBox txtOutput;
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
         this.btnSynchronous = new System.Windows.Forms.Button();
         this.btnAsynchronous = new System.Windows.Forms.Button();
         this.label1 = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.txtInput1 = new System.Windows.Forms.TextBox();
         this.txtInput2 = new System.Windows.Forms.TextBox();
         this.txtOutput = new System.Windows.Forms.TextBox();
         this.SuspendLayout();
         // 
         // btnQuit
         // 
         this.btnQuit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.btnQuit.Location = new System.Drawing.Point(168, 8);
         this.btnQuit.Name = "btnQuit";
         this.btnQuit.Size = new System.Drawing.Size(112, 23);
         this.btnQuit.TabIndex = 0;
         this.btnQuit.Text = "&Quit";
         this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
         // 
         // btnSynchronous
         // 
         this.btnSynchronous.Location = new System.Drawing.Point(168, 40);
         this.btnSynchronous.Name = "btnSynchronous";
         this.btnSynchronous.Size = new System.Drawing.Size(112, 23);
         this.btnSynchronous.TabIndex = 1;
         this.btnSynchronous.Text = "&Synchronous Add";
         this.btnSynchronous.Click += new System.EventHandler(this.btnSynchronous_Click);
         // 
         // btnAsynchronous
         // 
         this.btnAsynchronous.Location = new System.Drawing.Point(168, 72);
         this.btnAsynchronous.Name = "btnAsynchronous";
         this.btnAsynchronous.Size = new System.Drawing.Size(112, 23);
         this.btnAsynchronous.TabIndex = 2;
         this.btnAsynchronous.Text = "&Asynchronous Add";
         this.btnAsynchronous.Click += new System.EventHandler(this.btnAsynchronous_Click);
         // 
         // label1
         // 
         this.label1.Location = new System.Drawing.Point(8, 8);
         this.label1.Name = "label1";
         this.label1.TabIndex = 3;
         this.label1.Text = "Input &1";
         // 
         // label2
         // 
         this.label2.Location = new System.Drawing.Point(8, 64);
         this.label2.Name = "label2";
         this.label2.TabIndex = 4;
         this.label2.Text = "Input &2";
         // 
         // label3
         // 
         this.label3.Location = new System.Drawing.Point(8, 120);
         this.label3.Name = "label3";
         this.label3.TabIndex = 5;
         this.label3.Text = "&Output";
         // 
         // txtInput1
         // 
         this.txtInput1.Location = new System.Drawing.Point(8, 32);
         this.txtInput1.Name = "txtInput1";
         this.txtInput1.TabIndex = 3;
         this.txtInput1.Text = "1";
         // 
         // txtInput2
         // 
         this.txtInput2.Location = new System.Drawing.Point(8, 88);
         this.txtInput2.Name = "txtInput2";
         this.txtInput2.TabIndex = 4;
         this.txtInput2.Text = "2";
         // 
         // txtOutput
         // 
         this.txtOutput.Location = new System.Drawing.Point(8, 144);
         this.txtOutput.Name = "txtOutput";
         this.txtOutput.ReadOnly = true;
         this.txtOutput.TabIndex = 5;
         this.txtOutput.Text = "";
         // 
         // frmMain
         // 
         this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
         this.CancelButton = this.btnQuit;
         this.ClientSize = new System.Drawing.Size(292, 266);
         this.Controls.Add(this.txtOutput);
         this.Controls.Add(this.txtInput2);
         this.Controls.Add(this.txtInput1);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.label1);
         this.Controls.Add(this.btnAsynchronous);
         this.Controls.Add(this.btnSynchronous);
         this.Controls.Add(this.btnQuit);
         this.Name = "frmMain";
         this.Text = "SOAP Quick Test";
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

      private void btnSynchronous_Click(object sender, System.EventArgs e)
      {
         // Get the input.
         int   Input1 = Int32.Parse(txtInput1.Text);
         int   Input2 = Int32.Parse(txtInput2.Text);

         // Create the MathFunctionsService object.
         MathFunctionsService MFS = new MathFunctionsService();

         // Perform a synchronous add.
         int Result = MFS.DoAdd(Input1, Input2);

         // Send the output to the display.
         txtOutput.Text = Result.ToString();
      }

      private void btnAsynchronous_Click(object sender, System.EventArgs e)
      {
         // Get the input.
         int   Input1 = Int32.Parse(txtInput1.Text);
         int   Input2 = Int32.Parse(txtInput2.Text);

         // Create the MathFunctionsService object.
         MathFunctionsService MFS = new MathFunctionsService();

         // Perform an asynchronous add.
         MFS.BeginDoAdd(Input1, Input2, new System.AsyncCallback(MyCallback), null);
      }

      public void MyCallback(IAsyncResult ar)
      {
         // Create the MathFunctionsService object.
         MathFunctionsService MFS = new MathFunctionsService();

         // Obtain the result of the operation.
         int Result = MFS.EndDoAdd(ar);

         // Send the output to the display.
         txtOutput.Text = Result.ToString();
      }
	}
}

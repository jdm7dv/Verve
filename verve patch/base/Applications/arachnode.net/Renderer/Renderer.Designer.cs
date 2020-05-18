namespace Arachnode.Renderer
{
    partial class Renderer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Renderer));
            this.rtbDebug = new System.Windows.Forms.RichTextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslWorkingSet = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslNumberOfRenderedAbsoluteUris = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslElapsedTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnTest = new System.Windows.Forms.Button();
            this.axWebBrowser1 = new AxSHDocVw.AxWebBrowser();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).BeginInit();
            this.SuspendLayout();
            // 
            // rtbDebug
            // 
            this.rtbDebug.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbDebug.Location = new System.Drawing.Point(3, 3);
            this.rtbDebug.Name = "rtbDebug";
            this.rtbDebug.Size = new System.Drawing.Size(770, 512);
            this.rtbDebug.TabIndex = 2;
            this.rtbDebug.Text = "";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslWorkingSet,
            this.tsslNumberOfRenderedAbsoluteUris,
            this.tsslElapsedTime});
            this.statusStrip1.Location = new System.Drawing.Point(0, 544);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(784, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsslWorkingSet
            // 
            this.tsslWorkingSet.Name = "tsslWorkingSet";
            this.tsslWorkingSet.Size = new System.Drawing.Size(68, 17);
            this.tsslWorkingSet.Text = "WorkingSet";
            // 
            // tsslNumberOfRenderedAbsoluteUris
            // 
            this.tsslNumberOfRenderedAbsoluteUris.Name = "tsslNumberOfRenderedAbsoluteUris";
            this.tsslNumberOfRenderedAbsoluteUris.Size = new System.Drawing.Size(181, 17);
            this.tsslNumberOfRenderedAbsoluteUris.Text = "NumberOfRenderedAbsoluteUris";
            // 
            // tsslElapsedTime
            // 
            this.tsslElapsedTime.Name = "tsslElapsedTime";
            this.tsslElapsedTime.Size = new System.Drawing.Size(74, 17);
            this.tsslElapsedTime.Text = "ElapsedTime";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(784, 544);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnTest);
            this.tabPage1.Controls.Add(this.rtbDebug);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(776, 518);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Source";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.axWebBrowser1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(776, 518);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Display";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTest.Location = new System.Drawing.Point(695, 489);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 3;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // axWebBrowser1
            // 
            this.axWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axWebBrowser1.Enabled = true;
            this.axWebBrowser1.Location = new System.Drawing.Point(3, 3);
            this.axWebBrowser1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axWebBrowser1.OcxState")));
            this.axWebBrowser1.Size = new System.Drawing.Size(770, 512);
            this.axWebBrowser1.TabIndex = 4;
            this.axWebBrowser1.RedirectXDomainBlocked += new AxSHDocVw.DWebBrowserEvents2_RedirectXDomainBlockedEventHandler(this.axWebBrowser1_RedirectXDomainBlocked);
            this.axWebBrowser1.NavigateError += new AxSHDocVw.DWebBrowserEvents2_NavigateErrorEventHandler(this.axWebBrowser1_NavigateError);
            this.axWebBrowser1.ThirdPartyUrlBlocked += new AxSHDocVw.DWebBrowserEvents2_ThirdPartyUrlBlockedEventHandler(this.axWebBrowser1_ThirdPartyUrlBlocked);
            this.axWebBrowser1.NavigateComplete2 += new AxSHDocVw.DWebBrowserEvents2_NavigateComplete2EventHandler(this.axWebBrowser1_NavigateComplete2);
            this.axWebBrowser1.DocumentComplete += new AxSHDocVw.DWebBrowserEvents2_DocumentCompleteEventHandler(this.axWebBrowser1_DocumentComplete);
            this.axWebBrowser1.NewWindow2 += new AxSHDocVw.DWebBrowserEvents2_NewWindow2EventHandler(this.axWebBrowser1_NewWindow2);
            this.axWebBrowser1.FileDownload += new AxSHDocVw.DWebBrowserEvents2_FileDownloadEventHandler(this.axWebBrowser1_FileDownload);
            this.axWebBrowser1.NewWindow3 += new AxSHDocVw.DWebBrowserEvents2_NewWindow3EventHandler(this.axWebBrowser1_NewWindow3);
            // 
            // Renderer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 566);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "Renderer";
            this.Text = "Renderer";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axWebBrowser1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbDebug;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslWorkingSet;
        private System.Windows.Forms.ToolStripStatusLabel tsslNumberOfRenderedAbsoluteUris;
        private AxSHDocVw.AxWebBrowser axWebBrowser1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ToolStripStatusLabel tsslElapsedTime;
        private System.Windows.Forms.Button btnTest;

    }
}
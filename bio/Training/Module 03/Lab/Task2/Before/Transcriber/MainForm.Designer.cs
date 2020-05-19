namespace Transcriber
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cbSequences = new System.Windows.Forms.ComboBox();
            this.sequenceTypeText = new System.Windows.Forms.Label();
            this.sourceSequenceText = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.targetSequenceText = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(538, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openToolStripMenuItem.Text = "&Open";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // cbSequences
            // 
            this.cbSequences.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cbSequences.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSequences.FormattingEnabled = true;
            this.cbSequences.Location = new System.Drawing.Point(12, 27);
            this.cbSequences.Name = "cbSequences";
            this.cbSequences.Size = new System.Drawing.Size(427, 21);
            this.cbSequences.TabIndex = 1;
            // 
            // sequenceTypeText
            // 
            this.sequenceTypeText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sequenceTypeText.AutoSize = true;
            this.sequenceTypeText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sequenceTypeText.ForeColor = System.Drawing.Color.Green;
            this.sequenceTypeText.Location = new System.Drawing.Point(447, 30);
            this.sequenceTypeText.Name = "sequenceTypeText";
            this.sequenceTypeText.Size = new System.Drawing.Size(38, 20);
            this.sequenceTypeText.TabIndex = 2;
            this.sequenceTypeText.Text = "N/A";
            // 
            // sourceSequenceText
            // 
            this.sourceSequenceText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.sourceSequenceText.BackColor = System.Drawing.Color.White;
            this.sourceSequenceText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sourceSequenceText.ForeColor = System.Drawing.Color.Blue;
            this.sourceSequenceText.Location = new System.Drawing.Point(12, 54);
            this.sourceSequenceText.Multiline = true;
            this.sourceSequenceText.Name = "sourceSequenceText";
            this.sourceSequenceText.ReadOnly = true;
            this.sourceSequenceText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.sourceSequenceText.Size = new System.Drawing.Size(514, 166);
            this.sourceSequenceText.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(13, 227);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(513, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Transcribe Sequence";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // targetSequenceText
            // 
            this.targetSequenceText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.targetSequenceText.BackColor = System.Drawing.Color.White;
            this.targetSequenceText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.targetSequenceText.ForeColor = System.Drawing.Color.Red;
            this.targetSequenceText.Location = new System.Drawing.Point(13, 263);
            this.targetSequenceText.Multiline = true;
            this.targetSequenceText.Name = "targetSequenceText";
            this.targetSequenceText.ReadOnly = true;
            this.targetSequenceText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.targetSequenceText.Size = new System.Drawing.Size(513, 150);
            this.targetSequenceText.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(538, 425);
            this.Controls.Add(this.targetSequenceText);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.sourceSequenceText);
            this.Controls.Add(this.sequenceTypeText);
            this.Controls.Add(this.cbSequences);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Transcription Engine";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ComboBox cbSequences;
        private System.Windows.Forms.Label sequenceTypeText;
        private System.Windows.Forms.TextBox sourceSequenceText;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox targetSequenceText;
    }
}


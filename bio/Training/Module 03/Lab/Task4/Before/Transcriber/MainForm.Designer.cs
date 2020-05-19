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
            this.proteinTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(541, 24);
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
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
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
            this.cbSequences.Size = new System.Drawing.Size(430, 21);
            this.cbSequences.TabIndex = 1;
            this.cbSequences.SelectedIndexChanged += new System.EventHandler(this.cbSequences_SelectedIndexChanged);
            // 
            // sequenceTypeText
            // 
            this.sequenceTypeText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sequenceTypeText.AutoSize = true;
            this.sequenceTypeText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sequenceTypeText.ForeColor = System.Drawing.Color.Green;
            this.sequenceTypeText.Location = new System.Drawing.Point(450, 30);
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
            this.sourceSequenceText.Size = new System.Drawing.Size(517, 166);
            this.sourceSequenceText.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(13, 227);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(516, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Transcribe Sequence";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // targetSequenceText
            // 
            this.targetSequenceText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.targetSequenceText.BackColor = System.Drawing.Color.White;
            this.targetSequenceText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.targetSequenceText.ForeColor = System.Drawing.Color.Red;
            this.targetSequenceText.Location = new System.Drawing.Point(13, 279);
            this.targetSequenceText.Multiline = true;
            this.targetSequenceText.Name = "targetSequenceText";
            this.targetSequenceText.ReadOnly = true;
            this.targetSequenceText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.targetSequenceText.Size = new System.Drawing.Size(258, 181);
            this.targetSequenceText.TabIndex = 5;
            // 
            // proteinTextBox
            // 
            this.proteinTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.proteinTextBox.BackColor = System.Drawing.Color.White;
            this.proteinTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.proteinTextBox.ForeColor = System.Drawing.Color.Red;
            this.proteinTextBox.Location = new System.Drawing.Point(274, 279);
            this.proteinTextBox.Multiline = true;
            this.proteinTextBox.Name = "proteinTextBox";
            this.proteinTextBox.ReadOnly = true;
            this.proteinTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.proteinTextBox.Size = new System.Drawing.Size(255, 181);
            this.proteinTextBox.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 257);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Transcribed RNA/DNA";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(271, 257);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Protein Translation";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(541, 472);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.proteinTextBox);
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
        private System.Windows.Forms.TextBox proteinTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}


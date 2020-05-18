namespace Prolog
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
    protected override void Dispose (bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose ();
      }
      base.Dispose (disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent ()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (MainForm));
      this.statusStrip1 = new System.Windows.Forms.StatusStrip ();
      this.panel1 = new System.Windows.Forms.Panel ();
      this.tabControl1 = new System.Windows.Forms.TabControl ();
      this.tpInterpreter = new System.Windows.Forms.TabPage ();
      this.btnClearA = new System.Windows.Forms.Button ();
      this.btnEnter = new System.Windows.Forms.Button ();
      this.cbNewLines = new System.Windows.Forms.CheckBox ();
      this.pnlInput = new System.Windows.Forms.Panel ();
      this.tbInput = new System.Windows.Forms.TextBox ();
      this.label1 = new System.Windows.Forms.Label ();
      this.rtbQuery = new System.Windows.Forms.RichTextBox ();
      this.tbAnswer = new System.Windows.Forms.TextBox ();
      this.btnCancelQuery = new System.Windows.Forms.Button ();
      this.lblMoreOrStop = new System.Windows.Forms.Label ();
      this.btnStop = new System.Windows.Forms.Button ();
      this.btnMore = new System.Windows.Forms.Button ();
      this.btnClearQ = new System.Windows.Forms.Button ();
      this.btnXeqQuery = new System.Windows.Forms.Button ();
      this.label6 = new System.Windows.Forms.Label ();
      this.label5 = new System.Windows.Forms.Label ();
      this.tp2 = new System.Windows.Forms.TabPage ();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip ();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator ();
      this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator ();
      this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.printPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator ();
      this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem ();
      this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator ();
      this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator ();
      this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.customizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator ();
      this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem ();
      this.bgwExecuteQuery = new System.ComponentModel.BackgroundWorker ();
      this.panel2 = new System.Windows.Forms.Panel ();
      this.btnClose = new System.Windows.Forms.Button ();
      this.panel1.SuspendLayout ();
      this.tabControl1.SuspendLayout ();
      this.tpInterpreter.SuspendLayout ();
      this.pnlInput.SuspendLayout ();
      this.menuStrip1.SuspendLayout ();
      this.panel2.SuspendLayout ();
      this.SuspendLayout ();
      // 
      // statusStrip1
      // 
      this.statusStrip1.Location = new System.Drawing.Point (0, 701);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new System.Drawing.Size (824, 22);
      this.statusStrip1.TabIndex = 14;
      this.statusStrip1.Text = "statusStrip1";
      // 
      // panel1
      // 
      this.panel1.Controls.Add (this.tabControl1);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point (0, 24);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size (824, 677);
      this.panel1.TabIndex = 15;
      // 
      // tabControl1
      // 
      this.tabControl1.Controls.Add (this.tpInterpreter);
      this.tabControl1.Controls.Add (this.tp2);
      this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControl1.Location = new System.Drawing.Point (0, 0);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size (824, 677);
      this.tabControl1.TabIndex = 14;
      // 
      // tpInterpreter
      // 
      this.tpInterpreter.BackColor = System.Drawing.Color.LightCyan;
      this.tpInterpreter.Controls.Add (this.btnClearA);
      this.tpInterpreter.Controls.Add (this.btnEnter);
      this.tpInterpreter.Controls.Add (this.cbNewLines);
      this.tpInterpreter.Controls.Add (this.pnlInput);
      this.tpInterpreter.Controls.Add (this.label1);
      this.tpInterpreter.Controls.Add (this.rtbQuery);
      this.tpInterpreter.Controls.Add (this.tbAnswer);
      this.tpInterpreter.Controls.Add (this.btnCancelQuery);
      this.tpInterpreter.Controls.Add (this.lblMoreOrStop);
      this.tpInterpreter.Controls.Add (this.btnStop);
      this.tpInterpreter.Controls.Add (this.btnMore);
      this.tpInterpreter.Controls.Add (this.btnClearQ);
      this.tpInterpreter.Controls.Add (this.btnXeqQuery);
      this.tpInterpreter.Controls.Add (this.label6);
      this.tpInterpreter.Controls.Add (this.label5);
      this.tpInterpreter.Location = new System.Drawing.Point (4, 22);
      this.tpInterpreter.Name = "tpInterpreter";
      this.tpInterpreter.Padding = new System.Windows.Forms.Padding (3);
      this.tpInterpreter.Size = new System.Drawing.Size (816, 651);
      this.tpInterpreter.TabIndex = 4;
      this.tpInterpreter.Text = "Prolog console";
      // 
      // btnClearA
      // 
      this.btnClearA.BackColor = System.Drawing.SystemColors.Control;
      this.btnClearA.Location = new System.Drawing.Point (753, 128);
      this.btnClearA.Name = "btnClearA";
      this.btnClearA.Size = new System.Drawing.Size (50, 23);
      this.btnClearA.TabIndex = 25;
      this.btnClearA.Text = "Clear";
      this.btnClearA.UseVisualStyleBackColor = false;
      this.btnClearA.Click += new System.EventHandler (this.btnClearA_Click);
      // 
      // btnEnter
      // 
      this.btnEnter.Location = new System.Drawing.Point (209, 518);
      this.btnEnter.Name = "btnEnter";
      this.btnEnter.Size = new System.Drawing.Size (45, 20);
      this.btnEnter.TabIndex = 24;
      this.btnEnter.Text = "Enter";
      this.btnEnter.UseVisualStyleBackColor = true;
      this.btnEnter.Visible = false;
      this.btnEnter.Click += new System.EventHandler (this.btnEnter_Click);
      // 
      // cbNewLines
      // 
      this.cbNewLines.AutoSize = true;
      this.cbNewLines.Checked = true;
      this.cbNewLines.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbNewLines.Location = new System.Drawing.Point (49, 522);
      this.cbNewLines.Name = "cbNewLines";
      this.cbNewLines.Size = new System.Drawing.Size (155, 17);
      this.cbNewLines.TabIndex = 23;
      this.cbNewLines.Text = "Input terminated by newline";
      this.cbNewLines.UseVisualStyleBackColor = true;
      this.cbNewLines.CheckedChanged += new System.EventHandler (this.cbNewLines_CheckedChanged);
      // 
      // pnlInput
      // 
      this.pnlInput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.pnlInput.BackColor = System.Drawing.Color.LightCyan;
      this.pnlInput.Controls.Add (this.tbInput);
      this.pnlInput.Location = new System.Drawing.Point (11, 540);
      this.pnlInput.Name = "pnlInput";
      this.pnlInput.Size = new System.Drawing.Size (792, 75);
      this.pnlInput.TabIndex = 22;
      // 
      // tbInput
      // 
      this.tbInput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.tbInput.BackColor = System.Drawing.Color.White;
      this.tbInput.Cursor = System.Windows.Forms.Cursors.IBeam;
      this.tbInput.Font = new System.Drawing.Font ("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.tbInput.Location = new System.Drawing.Point (3, 3);
      this.tbInput.Multiline = true;
      this.tbInput.Name = "tbInput";
      this.tbInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.tbInput.Size = new System.Drawing.Size (786, 69);
      this.tbInput.TabIndex = 22;
      this.tbInput.KeyDown += new System.Windows.Forms.KeyEventHandler (this.tbInput_KeyDown);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font ("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point (8, 522);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size (36, 13);
      this.label1.TabIndex = 20;
      this.label1.Text = "Input";
      // 
      // rtbQuery
      // 
      this.rtbQuery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.rtbQuery.Font = new System.Drawing.Font ("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.rtbQuery.Location = new System.Drawing.Point (11, 40);
      this.rtbQuery.Name = "rtbQuery";
      this.rtbQuery.Size = new System.Drawing.Size (792, 69);
      this.rtbQuery.TabIndex = 19;
      this.rtbQuery.Text = "x = A; readln(L), writelnf( \"Line: {0}\", [L])";
      // 
      // tbAnswer
      // 
      this.tbAnswer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.tbAnswer.BackColor = System.Drawing.SystemColors.Info;
      this.tbAnswer.Cursor = System.Windows.Forms.Cursors.Default;
      this.tbAnswer.Font = new System.Drawing.Font ("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.tbAnswer.Location = new System.Drawing.Point (11, 167);
      this.tbAnswer.Multiline = true;
      this.tbAnswer.Name = "tbAnswer";
      this.tbAnswer.ReadOnly = true;
      this.tbAnswer.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.tbAnswer.Size = new System.Drawing.Size (792, 335);
      this.tbAnswer.TabIndex = 14;
      // 
      // btnCancelQuery
      // 
      this.btnCancelQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancelQuery.BackColor = System.Drawing.SystemColors.Control;
      this.btnCancelQuery.Enabled = false;
      this.btnCancelQuery.Location = new System.Drawing.Point (643, 128);
      this.btnCancelQuery.Name = "btnCancelQuery";
      this.btnCancelQuery.Size = new System.Drawing.Size (104, 23);
      this.btnCancelQuery.TabIndex = 18;
      this.btnCancelQuery.Text = "Interrupt long run";
      this.btnCancelQuery.UseVisualStyleBackColor = false;
      this.btnCancelQuery.Visible = false;
      this.btnCancelQuery.Click += new System.EventHandler (this.btnCancelQuery_Click);
      // 
      // lblMoreOrStop
      // 
      this.lblMoreOrStop.AutoSize = true;
      this.lblMoreOrStop.Font = new System.Drawing.Font ("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblMoreOrStop.ForeColor = System.Drawing.Color.Red;
      this.lblMoreOrStop.Location = new System.Drawing.Point (223, 133);
      this.lblMoreOrStop.Name = "lblMoreOrStop";
      this.lblMoreOrStop.Size = new System.Drawing.Size (115, 13);
      this.lblMoreOrStop.TabIndex = 17;
      this.lblMoreOrStop.Text = "Press More or Stop";
      this.lblMoreOrStop.Visible = false;
      // 
      // btnStop
      // 
      this.btnStop.BackColor = System.Drawing.SystemColors.Control;
      this.btnStop.Location = new System.Drawing.Point (170, 128);
      this.btnStop.Name = "btnStop";
      this.btnStop.Size = new System.Drawing.Size (47, 23);
      this.btnStop.TabIndex = 16;
      this.btnStop.Text = "Stop";
      this.btnStop.UseVisualStyleBackColor = false;
      this.btnStop.Click += new System.EventHandler (this.btnStop_Click);
      // 
      // btnMore
      // 
      this.btnMore.BackColor = System.Drawing.SystemColors.Control;
      this.btnMore.Location = new System.Drawing.Point (117, 128);
      this.btnMore.Name = "btnMore";
      this.btnMore.Size = new System.Drawing.Size (47, 23);
      this.btnMore.TabIndex = 15;
      this.btnMore.Text = "More";
      this.btnMore.UseVisualStyleBackColor = false;
      this.btnMore.Click += new System.EventHandler (this.btnMore_Click);
      // 
      // btnClearQ
      // 
      this.btnClearQ.BackColor = System.Drawing.SystemColors.Control;
      this.btnClearQ.Font = new System.Drawing.Font ("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.btnClearQ.Location = new System.Drawing.Point (49, 14);
      this.btnClearQ.Name = "btnClearQ";
      this.btnClearQ.Size = new System.Drawing.Size (50, 23);
      this.btnClearQ.TabIndex = 12;
      this.btnClearQ.Text = "Clear";
      this.btnClearQ.UseVisualStyleBackColor = false;
      this.btnClearQ.Click += new System.EventHandler (this.btnClear_Click);
      // 
      // btnXeqQuery
      // 
      this.btnXeqQuery.BackColor = System.Drawing.SystemColors.Control;
      this.btnXeqQuery.Location = new System.Drawing.Point (11, 128);
      this.btnXeqQuery.Name = "btnXeqQuery";
      this.btnXeqQuery.Size = new System.Drawing.Size (100, 23);
      this.btnXeqQuery.TabIndex = 11;
      this.btnXeqQuery.Text = "Execute query";
      this.btnXeqQuery.UseVisualStyleBackColor = false;
      this.btnXeqQuery.Click += new System.EventHandler (this.btnXeqQuery_Click);
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point (8, 151);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size (42, 13);
      this.label6.TabIndex = 10;
      this.label6.Text = "Answer";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point (8, 18);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size (35, 13);
      this.label5.TabIndex = 8;
      this.label5.Text = "Query";
      // 
      // tp2
      // 
      this.tp2.BackColor = System.Drawing.Color.Azure;
      this.tp2.Location = new System.Drawing.Point (4, 22);
      this.tp2.Name = "tp2";
      this.tp2.Padding = new System.Windows.Forms.Padding (3);
      this.tp2.Size = new System.Drawing.Size (816, 651);
      this.tp2.TabIndex = 0;
      this.tp2.Text = "TabPage2";
      this.tp2.UseVisualStyleBackColor = true;
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange (new System.Windows.Forms.ToolStripItem [] {
            this.exitToolStripMenuItem,
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point (0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size (824, 24);
      this.menuStrip1.TabIndex = 16;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new System.Drawing.Size (36, 20);
      this.exitToolStripMenuItem.Text = "Exit";
      this.exitToolStripMenuItem.Click += new System.EventHandler (this.exitToolStripMenuItem_Click);
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange (new System.Windows.Forms.ToolStripItem [] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.printToolStripMenuItem,
            this.printPreviewToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem1});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size (35, 20);
      this.fileToolStripMenuItem.Text = "&File";
      // 
      // newToolStripMenuItem
      // 
      this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject ("newToolStripMenuItem.Image")));
      this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.newToolStripMenuItem.Name = "newToolStripMenuItem";
      this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
      this.newToolStripMenuItem.Size = new System.Drawing.Size (139, 22);
      this.newToolStripMenuItem.Text = "&New";
      // 
      // openToolStripMenuItem
      // 
      this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject ("openToolStripMenuItem.Image")));
      this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.openToolStripMenuItem.Name = "openToolStripMenuItem";
      this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
      this.openToolStripMenuItem.Size = new System.Drawing.Size (139, 22);
      this.openToolStripMenuItem.Text = "&Open";
      // 
      // toolStripSeparator
      // 
      this.toolStripSeparator.Name = "toolStripSeparator";
      this.toolStripSeparator.Size = new System.Drawing.Size (136, 6);
      // 
      // saveToolStripMenuItem
      // 
      this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject ("saveToolStripMenuItem.Image")));
      this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
      this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
      this.saveToolStripMenuItem.Size = new System.Drawing.Size (139, 22);
      this.saveToolStripMenuItem.Text = "&Save";
      // 
      // saveAsToolStripMenuItem
      // 
      this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
      this.saveAsToolStripMenuItem.Size = new System.Drawing.Size (139, 22);
      this.saveAsToolStripMenuItem.Text = "Save &As";
      // 
      // toolStripSeparator1
      // 
      this.toolStripSeparator1.Name = "toolStripSeparator1";
      this.toolStripSeparator1.Size = new System.Drawing.Size (136, 6);
      // 
      // printToolStripMenuItem
      // 
      this.printToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject ("printToolStripMenuItem.Image")));
      this.printToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.printToolStripMenuItem.Name = "printToolStripMenuItem";
      this.printToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
      this.printToolStripMenuItem.Size = new System.Drawing.Size (139, 22);
      this.printToolStripMenuItem.Text = "&Print";
      // 
      // printPreviewToolStripMenuItem
      // 
      this.printPreviewToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject ("printPreviewToolStripMenuItem.Image")));
      this.printPreviewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
      this.printPreviewToolStripMenuItem.Size = new System.Drawing.Size (139, 22);
      this.printPreviewToolStripMenuItem.Text = "Print Pre&view";
      // 
      // toolStripSeparator2
      // 
      this.toolStripSeparator2.Name = "toolStripSeparator2";
      this.toolStripSeparator2.Size = new System.Drawing.Size (136, 6);
      // 
      // exitToolStripMenuItem1
      // 
      this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
      this.exitToolStripMenuItem1.Size = new System.Drawing.Size (139, 22);
      this.exitToolStripMenuItem1.Text = "E&xit";
      // 
      // editToolStripMenuItem
      // 
      this.editToolStripMenuItem.DropDownItems.AddRange (new System.Windows.Forms.ToolStripItem [] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator3,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator4,
            this.selectAllToolStripMenuItem});
      this.editToolStripMenuItem.Name = "editToolStripMenuItem";
      this.editToolStripMenuItem.Size = new System.Drawing.Size (37, 20);
      this.editToolStripMenuItem.Text = "&Edit";
      // 
      // undoToolStripMenuItem
      // 
      this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
      this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
      this.undoToolStripMenuItem.Size = new System.Drawing.Size (139, 22);
      this.undoToolStripMenuItem.Text = "&Undo";
      // 
      // redoToolStripMenuItem
      // 
      this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
      this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
      this.redoToolStripMenuItem.Size = new System.Drawing.Size (139, 22);
      this.redoToolStripMenuItem.Text = "&Redo";
      // 
      // toolStripSeparator3
      // 
      this.toolStripSeparator3.Name = "toolStripSeparator3";
      this.toolStripSeparator3.Size = new System.Drawing.Size (136, 6);
      // 
      // cutToolStripMenuItem
      // 
      this.cutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject ("cutToolStripMenuItem.Image")));
      this.cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
      this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
      this.cutToolStripMenuItem.Size = new System.Drawing.Size (139, 22);
      this.cutToolStripMenuItem.Text = "Cu&t";
      // 
      // copyToolStripMenuItem
      // 
      this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject ("copyToolStripMenuItem.Image")));
      this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
      this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
      this.copyToolStripMenuItem.Size = new System.Drawing.Size (139, 22);
      this.copyToolStripMenuItem.Text = "&Copy";
      // 
      // pasteToolStripMenuItem
      // 
      this.pasteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject ("pasteToolStripMenuItem.Image")));
      this.pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
      this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
      this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
      this.pasteToolStripMenuItem.Size = new System.Drawing.Size (139, 22);
      this.pasteToolStripMenuItem.Text = "&Paste";
      // 
      // toolStripSeparator4
      // 
      this.toolStripSeparator4.Name = "toolStripSeparator4";
      this.toolStripSeparator4.Size = new System.Drawing.Size (136, 6);
      // 
      // selectAllToolStripMenuItem
      // 
      this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
      this.selectAllToolStripMenuItem.Size = new System.Drawing.Size (139, 22);
      this.selectAllToolStripMenuItem.Text = "Select &All";
      // 
      // toolsToolStripMenuItem
      // 
      this.toolsToolStripMenuItem.DropDownItems.AddRange (new System.Windows.Forms.ToolStripItem [] {
            this.customizeToolStripMenuItem,
            this.optionsToolStripMenuItem});
      this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
      this.toolsToolStripMenuItem.Size = new System.Drawing.Size (45, 20);
      this.toolsToolStripMenuItem.Text = "&Tools";
      // 
      // customizeToolStripMenuItem
      // 
      this.customizeToolStripMenuItem.Name = "customizeToolStripMenuItem";
      this.customizeToolStripMenuItem.Size = new System.Drawing.Size (125, 22);
      this.customizeToolStripMenuItem.Text = "&Customize";
      // 
      // optionsToolStripMenuItem
      // 
      this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
      this.optionsToolStripMenuItem.Size = new System.Drawing.Size (125, 22);
      this.optionsToolStripMenuItem.Text = "&Options";
      // 
      // helpToolStripMenuItem
      // 
      this.helpToolStripMenuItem.DropDownItems.AddRange (new System.Windows.Forms.ToolStripItem [] {
            this.contentsToolStripMenuItem,
            this.indexToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.toolStripSeparator5,
            this.aboutToolStripMenuItem});
      this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
      this.helpToolStripMenuItem.Size = new System.Drawing.Size (41, 20);
      this.helpToolStripMenuItem.Text = "&Help";
      // 
      // contentsToolStripMenuItem
      // 
      this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
      this.contentsToolStripMenuItem.Size = new System.Drawing.Size (119, 22);
      this.contentsToolStripMenuItem.Text = "&Contents";
      // 
      // indexToolStripMenuItem
      // 
      this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
      this.indexToolStripMenuItem.Size = new System.Drawing.Size (119, 22);
      this.indexToolStripMenuItem.Text = "&Index";
      // 
      // searchToolStripMenuItem
      // 
      this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
      this.searchToolStripMenuItem.Size = new System.Drawing.Size (119, 22);
      this.searchToolStripMenuItem.Text = "&Search";
      // 
      // toolStripSeparator5
      // 
      this.toolStripSeparator5.Name = "toolStripSeparator5";
      this.toolStripSeparator5.Size = new System.Drawing.Size (116, 6);
      // 
      // aboutToolStripMenuItem
      // 
      this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
      this.aboutToolStripMenuItem.Size = new System.Drawing.Size (119, 22);
      this.aboutToolStripMenuItem.Text = "&About...";
      // 
      // bgwExecuteQuery
      // 
      this.bgwExecuteQuery.WorkerReportsProgress = true;
      this.bgwExecuteQuery.WorkerSupportsCancellation = true;
      this.bgwExecuteQuery.DoWork += new System.ComponentModel.DoWorkEventHandler (this.bgwExecuteQuery_DoWork);
      this.bgwExecuteQuery.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler (this.bgwExecuteQuery_RunWorkerCompleted);
      this.bgwExecuteQuery.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler (this.bgwExecuteQuery_ProgressChanged);
      // 
      // panel2
      // 
      this.panel2.Controls.Add (this.btnClose);
      this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panel2.Location = new System.Drawing.Point (0, 667);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size (824, 34);
      this.panel2.TabIndex = 17;
      // 
      // btnClose
      // 
      this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnClose.BackColor = System.Drawing.SystemColors.Control;
      this.btnClose.Location = new System.Drawing.Point (732, 4);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size (75, 23);
      this.btnClose.TabIndex = 10;
      this.btnClose.Text = "Close";
      this.btnClose.UseVisualStyleBackColor = false;
      this.btnClose.Click += new System.EventHandler (this.btnClose_Click);
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size (824, 723);
      this.Controls.Add (this.panel2);
      this.Controls.Add (this.panel1);
      this.Controls.Add (this.statusStrip1);
      this.Controls.Add (this.menuStrip1);
      this.MainMenuStrip = this.menuStrip1;
      this.Name = "MainForm";
      this.panel1.ResumeLayout (false);
      this.tabControl1.ResumeLayout (false);
      this.tpInterpreter.ResumeLayout (false);
      this.tpInterpreter.PerformLayout ();
      this.pnlInput.ResumeLayout (false);
      this.pnlInput.PerformLayout ();
      this.menuStrip1.ResumeLayout (false);
      this.menuStrip1.PerformLayout ();
      this.panel2.ResumeLayout (false);
      this.ResumeLayout (false);
      this.PerformLayout ();

    }

    #endregion

    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.ComponentModel.BackgroundWorker bgwExecuteQuery;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tp2;
    private System.Windows.Forms.TabPage tpInterpreter;
    private System.Windows.Forms.RichTextBox rtbQuery;
    private System.Windows.Forms.TextBox tbAnswer;
    private System.Windows.Forms.Button btnCancelQuery;
    private System.Windows.Forms.Label lblMoreOrStop;
    private System.Windows.Forms.Button btnStop;
    private System.Windows.Forms.Button btnMore;
    private System.Windows.Forms.Button btnClearQ;
    private System.Windows.Forms.Button btnXeqQuery;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
    private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem printPreviewToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem customizeToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem indexToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Panel pnlInput;
    private System.Windows.Forms.TextBox tbInput;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.CheckBox cbNewLines;
    private System.Windows.Forms.Button btnEnter;
    private System.Windows.Forms.Button btnClearA;
  }
}


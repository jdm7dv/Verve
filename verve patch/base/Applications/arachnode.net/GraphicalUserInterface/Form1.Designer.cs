namespace GraphicalUserInterface
{
    partial class Form1
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
            this.dgvCounters = new System.Windows.Forms.DataGridView();
            this.dgvtbcCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtbcName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtbcValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtbcAverage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtbcTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtbcCounted = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpApplicationSettings = new System.Windows.Forms.TabPage();
            this.dgvApplicationSettings = new System.Windows.Forms.DataGridView();
            this.dgvtbcApplicationSettingsName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtbcApplicationSettingsValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpWebSettings = new System.Windows.Forms.TabPage();
            this.dgvWebSettings = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpCrawlRequests = new System.Windows.Forms.TabPage();
            this.btnSaveCrawlRequests = new System.Windows.Forms.Button();
            this.btnLoadCrawlRequests = new System.Windows.Forms.Button();
            this.dgvCrawlRequests = new System.Windows.Forms.DataGridView();
            this.dgvtbcCRAbsoluteUri = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtbcCRDepth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtbcCRPriority = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtbcCRAdded = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tcControl = new System.Windows.Forms.TabPage();
            this.btnResume = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.lblElapsedTime = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.dgvCrawls = new System.Windows.Forms.DataGridView();
            this.tcConfiguration = new System.Windows.Forms.TabPage();
            this.dgvTableName = new System.Windows.Forms.DataGridView();
            this.dgvtbcTableName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvConfiguration = new System.Windows.Forms.DataGridView();
            this.tcCounters = new System.Windows.Forms.TabPage();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.resetDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetCrawlerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetDirectoriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetIISToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startPerfmonexeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.breadthFirstByPriorityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.depthFirstByPriorityToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetDatabaseOnStartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetCrawlerOnStartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sfdCrawlRequests = new System.Windows.Forms.SaveFileDialog();
            this.ssMain = new System.Windows.Forms.StatusStrip();
            this.tsslEngineState = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslElapasedTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.ofdCrawlRequests = new System.Windows.Forms.OpenFileDialog();
            this.dgvtbcThreadNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtbcAbsoluteUri = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtbcStatusCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtbcDiscoveryType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtbcHyperLinkDiscoveries = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtbcFileAndImageDiscoveries = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtbcCurrentDepth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvtbcMaximumDepth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCounters)).BeginInit();
            this.tcMain.SuspendLayout();
            this.tpApplicationSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvApplicationSettings)).BeginInit();
            this.tpWebSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWebSettings)).BeginInit();
            this.tpCrawlRequests.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCrawlRequests)).BeginInit();
            this.tcControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCrawls)).BeginInit();
            this.tcConfiguration.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTableName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConfiguration)).BeginInit();
            this.tcCounters.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.ssMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvCounters
            // 
            this.dgvCounters.AllowUserToAddRows = false;
            this.dgvCounters.AllowUserToDeleteRows = false;
            this.dgvCounters.AllowUserToResizeColumns = false;
            this.dgvCounters.AllowUserToResizeRows = false;
            this.dgvCounters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCounters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCounters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvtbcCategory,
            this.dgvtbcName,
            this.dgvtbcValue,
            this.dgvtbcAverage,
            this.dgvtbcTotal,
            this.dgvtbcCounted});
            this.dgvCounters.Location = new System.Drawing.Point(6, 6);
            this.dgvCounters.Name = "dgvCounters";
            this.dgvCounters.ReadOnly = true;
            this.dgvCounters.RowHeadersVisible = false;
            this.dgvCounters.Size = new System.Drawing.Size(1238, 914);
            this.dgvCounters.TabIndex = 0;
            // 
            // dgvtbcCategory
            // 
            this.dgvtbcCategory.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvtbcCategory.FillWeight = 50F;
            this.dgvtbcCategory.HeaderText = "Category";
            this.dgvtbcCategory.Name = "dgvtbcCategory";
            this.dgvtbcCategory.ReadOnly = true;
            this.dgvtbcCategory.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvtbcName
            // 
            this.dgvtbcName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvtbcName.HeaderText = "Name";
            this.dgvtbcName.Name = "dgvtbcName";
            this.dgvtbcName.ReadOnly = true;
            this.dgvtbcName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvtbcValue
            // 
            this.dgvtbcValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvtbcValue.FillWeight = 20F;
            this.dgvtbcValue.HeaderText = "Value";
            this.dgvtbcValue.Name = "dgvtbcValue";
            this.dgvtbcValue.ReadOnly = true;
            this.dgvtbcValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvtbcAverage
            // 
            this.dgvtbcAverage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvtbcAverage.FillWeight = 20F;
            this.dgvtbcAverage.HeaderText = "Average";
            this.dgvtbcAverage.Name = "dgvtbcAverage";
            this.dgvtbcAverage.ReadOnly = true;
            this.dgvtbcAverage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvtbcTotal
            // 
            this.dgvtbcTotal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvtbcTotal.FillWeight = 20F;
            this.dgvtbcTotal.HeaderText = "Total";
            this.dgvtbcTotal.Name = "dgvtbcTotal";
            this.dgvtbcTotal.ReadOnly = true;
            this.dgvtbcTotal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvtbcCounted
            // 
            this.dgvtbcCounted.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvtbcCounted.FillWeight = 20F;
            this.dgvtbcCounted.HeaderText = "Counted";
            this.dgvtbcCounted.Name = "dgvtbcCounted";
            this.dgvtbcCounted.ReadOnly = true;
            this.dgvtbcCounted.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tpApplicationSettings);
            this.tcMain.Controls.Add(this.tpWebSettings);
            this.tcMain.Controls.Add(this.tpCrawlRequests);
            this.tcMain.Controls.Add(this.tcControl);
            this.tcMain.Controls.Add(this.tcConfiguration);
            this.tcMain.Controls.Add(this.tcCounters);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Location = new System.Drawing.Point(3, 27);
            this.tcMain.Name = "tcMain";
            this.tcMain.Padding = new System.Drawing.Point(3, 3);
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(1258, 848);
            this.tcMain.TabIndex = 1;
            // 
            // tpApplicationSettings
            // 
            this.tpApplicationSettings.Controls.Add(this.dgvApplicationSettings);
            this.tpApplicationSettings.Location = new System.Drawing.Point(4, 22);
            this.tpApplicationSettings.Name = "tpApplicationSettings";
            this.tpApplicationSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tpApplicationSettings.Size = new System.Drawing.Size(1250, 822);
            this.tpApplicationSettings.TabIndex = 4;
            this.tpApplicationSettings.Text = "Application Settings";
            this.tpApplicationSettings.UseVisualStyleBackColor = true;
            // 
            // dgvApplicationSettings
            // 
            this.dgvApplicationSettings.AllowUserToAddRows = false;
            this.dgvApplicationSettings.AllowUserToDeleteRows = false;
            this.dgvApplicationSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvApplicationSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvApplicationSettings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvtbcApplicationSettingsName,
            this.dgvtbcApplicationSettingsValue});
            this.dgvApplicationSettings.Location = new System.Drawing.Point(6, 6);
            this.dgvApplicationSettings.Name = "dgvApplicationSettings";
            this.dgvApplicationSettings.RowHeadersVisible = false;
            this.dgvApplicationSettings.Size = new System.Drawing.Size(1238, 795);
            this.dgvApplicationSettings.TabIndex = 0;
            // 
            // dgvtbcApplicationSettingsName
            // 
            this.dgvtbcApplicationSettingsName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvtbcApplicationSettingsName.HeaderText = "Name";
            this.dgvtbcApplicationSettingsName.Name = "dgvtbcApplicationSettingsName";
            this.dgvtbcApplicationSettingsName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvtbcApplicationSettingsValue
            // 
            this.dgvtbcApplicationSettingsValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvtbcApplicationSettingsValue.FillWeight = 50F;
            this.dgvtbcApplicationSettingsValue.HeaderText = "Value";
            this.dgvtbcApplicationSettingsValue.Name = "dgvtbcApplicationSettingsValue";
            this.dgvtbcApplicationSettingsValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tpWebSettings
            // 
            this.tpWebSettings.Controls.Add(this.dgvWebSettings);
            this.tpWebSettings.Location = new System.Drawing.Point(4, 22);
            this.tpWebSettings.Name = "tpWebSettings";
            this.tpWebSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tpWebSettings.Size = new System.Drawing.Size(1250, 822);
            this.tpWebSettings.TabIndex = 5;
            this.tpWebSettings.Text = "Web Settings";
            this.tpWebSettings.UseVisualStyleBackColor = true;
            // 
            // dgvWebSettings
            // 
            this.dgvWebSettings.AllowUserToAddRows = false;
            this.dgvWebSettings.AllowUserToDeleteRows = false;
            this.dgvWebSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvWebSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWebSettings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.dgvWebSettings.Location = new System.Drawing.Point(6, 6);
            this.dgvWebSettings.Name = "dgvWebSettings";
            this.dgvWebSettings.RowHeadersVisible = false;
            this.dgvWebSettings.Size = new System.Drawing.Size(1238, 795);
            this.dgvWebSettings.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.FillWeight = 50F;
            this.dataGridViewTextBoxColumn2.HeaderText = "Value";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tpCrawlRequests
            // 
            this.tpCrawlRequests.Controls.Add(this.btnSaveCrawlRequests);
            this.tpCrawlRequests.Controls.Add(this.btnLoadCrawlRequests);
            this.tpCrawlRequests.Controls.Add(this.dgvCrawlRequests);
            this.tpCrawlRequests.Location = new System.Drawing.Point(4, 22);
            this.tpCrawlRequests.Name = "tpCrawlRequests";
            this.tpCrawlRequests.Padding = new System.Windows.Forms.Padding(3);
            this.tpCrawlRequests.Size = new System.Drawing.Size(1250, 822);
            this.tpCrawlRequests.TabIndex = 3;
            this.tpCrawlRequests.Text = "Crawl Requests";
            this.tpCrawlRequests.UseVisualStyleBackColor = true;
            // 
            // btnSaveCrawlRequests
            // 
            this.btnSaveCrawlRequests.Location = new System.Drawing.Point(87, 6);
            this.btnSaveCrawlRequests.Name = "btnSaveCrawlRequests";
            this.btnSaveCrawlRequests.Size = new System.Drawing.Size(75, 23);
            this.btnSaveCrawlRequests.TabIndex = 2;
            this.btnSaveCrawlRequests.Text = "Save";
            this.btnSaveCrawlRequests.UseVisualStyleBackColor = true;
            this.btnSaveCrawlRequests.Click += new System.EventHandler(this.btnSaveCrawlRequests_Click);
            // 
            // btnLoadCrawlRequests
            // 
            this.btnLoadCrawlRequests.Location = new System.Drawing.Point(6, 6);
            this.btnLoadCrawlRequests.Name = "btnLoadCrawlRequests";
            this.btnLoadCrawlRequests.Size = new System.Drawing.Size(75, 23);
            this.btnLoadCrawlRequests.TabIndex = 1;
            this.btnLoadCrawlRequests.Text = "Load";
            this.btnLoadCrawlRequests.UseVisualStyleBackColor = true;
            this.btnLoadCrawlRequests.Click += new System.EventHandler(this.btnLoadCrawlRequests_Click);
            // 
            // dgvCrawlRequests
            // 
            this.dgvCrawlRequests.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCrawlRequests.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvtbcCRAbsoluteUri,
            this.dgvtbcCRDepth,
            this.dgvtbcCRPriority,
            this.dgvtbcCRAdded});
            this.dgvCrawlRequests.Location = new System.Drawing.Point(6, 35);
            this.dgvCrawlRequests.Name = "dgvCrawlRequests";
            this.dgvCrawlRequests.Size = new System.Drawing.Size(1238, 889);
            this.dgvCrawlRequests.TabIndex = 0;
            // 
            // dgvtbcCRAbsoluteUri
            // 
            this.dgvtbcCRAbsoluteUri.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvtbcCRAbsoluteUri.HeaderText = "AbsoluteUri";
            this.dgvtbcCRAbsoluteUri.Name = "dgvtbcCRAbsoluteUri";
            // 
            // dgvtbcCRDepth
            // 
            this.dgvtbcCRDepth.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvtbcCRDepth.FillWeight = 20F;
            this.dgvtbcCRDepth.HeaderText = "Depth";
            this.dgvtbcCRDepth.Name = "dgvtbcCRDepth";
            // 
            // dgvtbcCRPriority
            // 
            this.dgvtbcCRPriority.FillWeight = 20F;
            this.dgvtbcCRPriority.HeaderText = "Priority";
            this.dgvtbcCRPriority.Name = "dgvtbcCRPriority";
            // 
            // dgvtbcCRAdded
            // 
            this.dgvtbcCRAdded.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvtbcCRAdded.FillWeight = 20F;
            this.dgvtbcCRAdded.HeaderText = "Added";
            this.dgvtbcCRAdded.Name = "dgvtbcCRAdded";
            // 
            // tcControl
            // 
            this.tcControl.Controls.Add(this.btnResume);
            this.tcControl.Controls.Add(this.btnPause);
            this.tcControl.Controls.Add(this.lblElapsedTime);
            this.tcControl.Controls.Add(this.btnStop);
            this.tcControl.Controls.Add(this.btnStart);
            this.tcControl.Controls.Add(this.dgvCrawls);
            this.tcControl.Location = new System.Drawing.Point(4, 22);
            this.tcControl.Name = "tcControl";
            this.tcControl.Padding = new System.Windows.Forms.Padding(3);
            this.tcControl.Size = new System.Drawing.Size(1250, 822);
            this.tcControl.TabIndex = 1;
            this.tcControl.Text = "Control";
            this.tcControl.UseVisualStyleBackColor = true;
            // 
            // btnResume
            // 
            this.btnResume.Location = new System.Drawing.Point(249, 6);
            this.btnResume.Name = "btnResume";
            this.btnResume.Size = new System.Drawing.Size(75, 23);
            this.btnResume.TabIndex = 5;
            this.btnResume.Text = "Resume";
            this.btnResume.UseVisualStyleBackColor = true;
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(168, 6);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(75, 23);
            this.btnPause.TabIndex = 4;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            // 
            // lblElapsedTime
            // 
            this.lblElapsedTime.AutoSize = true;
            this.lblElapsedTime.Location = new System.Drawing.Point(1073, 16);
            this.lblElapsedTime.Name = "lblElapsedTime";
            this.lblElapsedTime.Size = new System.Drawing.Size(74, 13);
            this.lblElapsedTime.TabIndex = 3;
            this.lblElapsedTime.Text = "Elapsed Time:";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(87, 6);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(6, 6);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // dgvCrawls
            // 
            this.dgvCrawls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCrawls.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCrawls.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvtbcThreadNumber,
            this.dgvtbcAbsoluteUri,
            this.dgvtbcStatusCode,
            this.dgvtbcDiscoveryType,
            this.dgvtbcHyperLinkDiscoveries,
            this.dgvtbcFileAndImageDiscoveries,
            this.dgvtbcCurrentDepth,
            this.dgvtbcMaximumDepth});
            this.dgvCrawls.Location = new System.Drawing.Point(6, 35);
            this.dgvCrawls.Name = "dgvCrawls";
            this.dgvCrawls.RowHeadersVisible = false;
            this.dgvCrawls.Size = new System.Drawing.Size(1238, 885);
            this.dgvCrawls.TabIndex = 0;
            // 
            // tcConfiguration
            // 
            this.tcConfiguration.Controls.Add(this.dgvTableName);
            this.tcConfiguration.Controls.Add(this.dgvConfiguration);
            this.tcConfiguration.Location = new System.Drawing.Point(4, 22);
            this.tcConfiguration.Name = "tcConfiguration";
            this.tcConfiguration.Padding = new System.Windows.Forms.Padding(3);
            this.tcConfiguration.Size = new System.Drawing.Size(1250, 822);
            this.tcConfiguration.TabIndex = 2;
            this.tcConfiguration.Text = "Configuration";
            this.tcConfiguration.UseVisualStyleBackColor = true;
            // 
            // dgvTableName
            // 
            this.dgvTableName.AllowUserToAddRows = false;
            this.dgvTableName.AllowUserToDeleteRows = false;
            this.dgvTableName.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTableName.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvtbcTableName});
            this.dgvTableName.Location = new System.Drawing.Point(6, 6);
            this.dgvTableName.MultiSelect = false;
            this.dgvTableName.Name = "dgvTableName";
            this.dgvTableName.RowHeadersVisible = false;
            this.dgvTableName.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTableName.Size = new System.Drawing.Size(154, 918);
            this.dgvTableName.TabIndex = 1;
            this.dgvTableName.SelectionChanged += new System.EventHandler(this.dgvTableName_SelectionChanged);
            // 
            // dgvtbcTableName
            // 
            this.dgvtbcTableName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvtbcTableName.HeaderText = "Table Name";
            this.dgvtbcTableName.Name = "dgvtbcTableName";
            this.dgvtbcTableName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvConfiguration
            // 
            this.dgvConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvConfiguration.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvConfiguration.Location = new System.Drawing.Point(167, 6);
            this.dgvConfiguration.Name = "dgvConfiguration";
            this.dgvConfiguration.Size = new System.Drawing.Size(1077, 810);
            this.dgvConfiguration.TabIndex = 0;
            // 
            // tcCounters
            // 
            this.tcCounters.Controls.Add(this.dgvCounters);
            this.tcCounters.Location = new System.Drawing.Point(4, 22);
            this.tcCounters.Name = "tcCounters";
            this.tcCounters.Padding = new System.Windows.Forms.Padding(3);
            this.tcCounters.Size = new System.Drawing.Size(1250, 822);
            this.tcCounters.TabIndex = 0;
            this.tcCounters.Text = "Counters";
            this.tcCounters.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolStripMenuItem2,
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(3, 3);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1258, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetDatabaseToolStripMenuItem,
            this.resetCrawlerToolStripMenuItem,
            this.resetDirectoriesToolStripMenuItem,
            this.resetIISToolStripMenuItem,
            this.startPerfmonexeToolStripMenuItem});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(59, 20);
            this.toolStripMenuItem2.Text = "Actions";
            // 
            // resetDatabaseToolStripMenuItem
            // 
            this.resetDatabaseToolStripMenuItem.Name = "resetDatabaseToolStripMenuItem";
            this.resetDatabaseToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.resetDatabaseToolStripMenuItem.Text = "Reset Database";
            this.resetDatabaseToolStripMenuItem.Click += new System.EventHandler(this.resetDatabaseToolStripMenuItem_Click);
            // 
            // resetCrawlerToolStripMenuItem
            // 
            this.resetCrawlerToolStripMenuItem.Name = "resetCrawlerToolStripMenuItem";
            this.resetCrawlerToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.resetCrawlerToolStripMenuItem.Text = "Reset Crawler";
            this.resetCrawlerToolStripMenuItem.Click += new System.EventHandler(this.resetCrawlerToolStripMenuItem_Click);
            // 
            // resetDirectoriesToolStripMenuItem
            // 
            this.resetDirectoriesToolStripMenuItem.Name = "resetDirectoriesToolStripMenuItem";
            this.resetDirectoriesToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.resetDirectoriesToolStripMenuItem.Text = "Reset Directories";
            // 
            // resetIISToolStripMenuItem
            // 
            this.resetIISToolStripMenuItem.Name = "resetIISToolStripMenuItem";
            this.resetIISToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.resetIISToolStripMenuItem.Text = "Reset IIS";
            // 
            // startPerfmonexeToolStripMenuItem
            // 
            this.startPerfmonexeToolStripMenuItem.Name = "startPerfmonexeToolStripMenuItem";
            this.startPerfmonexeToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.startPerfmonexeToolStripMenuItem.Text = "Start perfmon.exe";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.resetDatabaseOnStartToolStripMenuItem,
            this.resetCrawlerOnStartToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.breadthFirstByPriorityToolStripMenuItem,
            this.depthFirstByPriorityToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(197, 22);
            this.toolStripMenuItem1.Text = "CrawlMode";
            // 
            // breadthFirstByPriorityToolStripMenuItem
            // 
            this.breadthFirstByPriorityToolStripMenuItem.Checked = true;
            this.breadthFirstByPriorityToolStripMenuItem.CheckOnClick = true;
            this.breadthFirstByPriorityToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.breadthFirstByPriorityToolStripMenuItem.Name = "breadthFirstByPriorityToolStripMenuItem";
            this.breadthFirstByPriorityToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.breadthFirstByPriorityToolStripMenuItem.Text = "BreadthFirstByPriority";
            // 
            // depthFirstByPriorityToolStripMenuItem
            // 
            this.depthFirstByPriorityToolStripMenuItem.CheckOnClick = true;
            this.depthFirstByPriorityToolStripMenuItem.Name = "depthFirstByPriorityToolStripMenuItem";
            this.depthFirstByPriorityToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.depthFirstByPriorityToolStripMenuItem.Text = "DepthFirstByPriority";
            // 
            // resetDatabaseOnStartToolStripMenuItem
            // 
            this.resetDatabaseOnStartToolStripMenuItem.CheckOnClick = true;
            this.resetDatabaseOnStartToolStripMenuItem.Name = "resetDatabaseOnStartToolStripMenuItem";
            this.resetDatabaseOnStartToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.resetDatabaseOnStartToolStripMenuItem.Text = "Reset Database on Start";
            // 
            // resetCrawlerOnStartToolStripMenuItem
            // 
            this.resetCrawlerOnStartToolStripMenuItem.CheckOnClick = true;
            this.resetCrawlerOnStartToolStripMenuItem.Name = "resetCrawlerOnStartToolStripMenuItem";
            this.resetCrawlerOnStartToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.resetCrawlerOnStartToolStripMenuItem.Text = "Reset Crawler on Start";
            // 
            // sfdCrawlRequests
            // 
            this.sfdCrawlRequests.FileName = "CrawlRequests.txt";
            this.sfdCrawlRequests.Filter = "Text files|*.txt";
            // 
            // ssMain
            // 
            this.ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslEngineState,
            this.tsslElapasedTime});
            this.ssMain.Location = new System.Drawing.Point(3, 853);
            this.ssMain.Name = "ssMain";
            this.ssMain.Size = new System.Drawing.Size(1258, 22);
            this.ssMain.TabIndex = 4;
            this.ssMain.Text = "Ready";
            // 
            // tsslEngineState
            // 
            this.tsslEngineState.Name = "tsslEngineState";
            this.tsslEngineState.Size = new System.Drawing.Size(122, 17);
            this.tsslEngineState.Text = "Engine State: Stopped";
            // 
            // tsslElapasedTime
            // 
            this.tsslElapasedTime.Name = "tsslElapasedTime";
            this.tsslElapasedTime.Size = new System.Drawing.Size(111, 17);
            this.tsslElapasedTime.Text = "Elapased Time: N/A";
            // 
            // ofdCrawlRequests
            // 
            this.ofdCrawlRequests.FileName = "CrawlRequests.txt";
            this.ofdCrawlRequests.Filter = "Text files|*.txt";
            // 
            // dgvtbcThreadNumber
            // 
            this.dgvtbcThreadNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvtbcThreadNumber.FillWeight = 20F;
            this.dgvtbcThreadNumber.HeaderText = "Thread Number";
            this.dgvtbcThreadNumber.Name = "dgvtbcThreadNumber";
            this.dgvtbcThreadNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvtbcAbsoluteUri
            // 
            this.dgvtbcAbsoluteUri.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvtbcAbsoluteUri.HeaderText = "Absolute Uri";
            this.dgvtbcAbsoluteUri.Name = "dgvtbcAbsoluteUri";
            this.dgvtbcAbsoluteUri.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvtbcStatusCode
            // 
            this.dgvtbcStatusCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvtbcStatusCode.FillWeight = 20F;
            this.dgvtbcStatusCode.HeaderText = "Status Code";
            this.dgvtbcStatusCode.Name = "dgvtbcStatusCode";
            this.dgvtbcStatusCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvtbcDiscoveryType
            // 
            this.dgvtbcDiscoveryType.FillWeight = 20F;
            this.dgvtbcDiscoveryType.HeaderText = "Discovery Type";
            this.dgvtbcDiscoveryType.Name = "dgvtbcDiscoveryType";
            this.dgvtbcDiscoveryType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvtbcHyperLinkDiscoveries
            // 
            this.dgvtbcHyperLinkDiscoveries.FillWeight = 20F;
            this.dgvtbcHyperLinkDiscoveries.HeaderText = "HyperLink Discoveries";
            this.dgvtbcHyperLinkDiscoveries.Name = "dgvtbcHyperLinkDiscoveries";
            this.dgvtbcHyperLinkDiscoveries.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvtbcFileAndImageDiscoveries
            // 
            this.dgvtbcFileAndImageDiscoveries.FillWeight = 20F;
            this.dgvtbcFileAndImageDiscoveries.HeaderText = "File/Image Discoveries";
            this.dgvtbcFileAndImageDiscoveries.Name = "dgvtbcFileAndImageDiscoveries";
            this.dgvtbcFileAndImageDiscoveries.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvtbcCurrentDepth
            // 
            this.dgvtbcCurrentDepth.FillWeight = 10F;
            this.dgvtbcCurrentDepth.HeaderText = "Current Depth";
            this.dgvtbcCurrentDepth.Name = "dgvtbcCurrentDepth";
            this.dgvtbcCurrentDepth.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgvtbcMaximumDepth
            // 
            this.dgvtbcMaximumDepth.FillWeight = 10F;
            this.dgvtbcMaximumDepth.HeaderText = "Maximum Depth";
            this.dgvtbcMaximumDepth.Name = "dgvtbcMaximumDepth";
            this.dgvtbcMaximumDepth.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 878);
            this.Controls.Add(this.ssMain);
            this.Controls.Add(this.tcMain);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "arachnode.net | graphical user interface";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCounters)).EndInit();
            this.tcMain.ResumeLayout(false);
            this.tpApplicationSettings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvApplicationSettings)).EndInit();
            this.tpWebSettings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWebSettings)).EndInit();
            this.tpCrawlRequests.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCrawlRequests)).EndInit();
            this.tcControl.ResumeLayout(false);
            this.tcControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCrawls)).EndInit();
            this.tcConfiguration.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTableName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvConfiguration)).EndInit();
            this.tcCounters.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ssMain.ResumeLayout(false);
            this.ssMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvCounters;
        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tcCounters;
        private System.Windows.Forms.TabPage tcControl;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtbcCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtbcName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtbcValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtbcAverage;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtbcTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtbcCounted;
        private System.Windows.Forms.TabPage tcConfiguration;
        private System.Windows.Forms.DataGridView dgvConfiguration;
        private System.Windows.Forms.DataGridView dgvTableName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtbcTableName;
        private System.Windows.Forms.DataGridView dgvCrawls;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetDatabaseOnStartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem breadthFirstByPriorityToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem depthFirstByPriorityToolStripMenuItem;
        private System.Windows.Forms.Button btnResume;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Label lblElapsedTime;
        private System.Windows.Forms.TabPage tpCrawlRequests;
        private System.Windows.Forms.Button btnLoadCrawlRequests;
        private System.Windows.Forms.DataGridView dgvCrawlRequests;
        private System.Windows.Forms.Button btnSaveCrawlRequests;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem resetDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetCrawlerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetDirectoriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetIISToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startPerfmonexeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetCrawlerOnStartToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtbcCRAbsoluteUri;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtbcCRDepth;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtbcCRPriority;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtbcCRAdded;
        private System.Windows.Forms.SaveFileDialog sfdCrawlRequests;
        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.ToolStripStatusLabel tsslEngineState;
        private System.Windows.Forms.ToolStripStatusLabel tsslElapasedTime;
        private System.Windows.Forms.TabPage tpApplicationSettings;
        private System.Windows.Forms.DataGridView dgvApplicationSettings;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtbcApplicationSettingsName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtbcApplicationSettingsValue;
        private System.Windows.Forms.TabPage tpWebSettings;
        private System.Windows.Forms.DataGridView dgvWebSettings;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.OpenFileDialog ofdCrawlRequests;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtbcThreadNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtbcAbsoluteUri;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtbcStatusCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtbcDiscoveryType;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtbcHyperLinkDiscoveries;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtbcFileAndImageDiscoveries;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtbcCurrentDepth;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvtbcMaximumDepth;

    }
}


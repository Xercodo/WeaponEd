namespace WeaponEd
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.chkTrigger = new System.Windows.Forms.CheckBox();
			this.pnlTriggerB = new System.Windows.Forms.Panel();
			this.pnlTriggerA = new System.Windows.Forms.Panel();
			this.pnlMax = new System.Windows.Forms.Panel();
			this.pnlMin = new System.Windows.Forms.Panel();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.pnlVertPreview = new WeaponEd.UpdatePanel();
			this.pnlHorizPreview = new WeaponEd.UpdatePanel();
			this.colorDialog1 = new System.Windows.Forms.ColorDialog();
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.weaponDefine = new WeaponEd.WeaponDefinition();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectFamilyListluaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.groupBox1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.Filter = "Weapon File|*.wepn";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkTrigger);
			this.groupBox1.Controls.Add(this.pnlTriggerB);
			this.groupBox1.Controls.Add(this.pnlTriggerA);
			this.groupBox1.Controls.Add(this.pnlMax);
			this.groupBox1.Controls.Add(this.pnlMin);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.pnlVertPreview);
			this.groupBox1.Controls.Add(this.pnlHorizPreview);
			this.groupBox1.Location = new System.Drawing.Point(12, 27);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(213, 417);
			this.groupBox1.TabIndex = 5;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Angles";
			// 
			// chkTrigger
			// 
			this.chkTrigger.AutoSize = true;
			this.chkTrigger.Location = new System.Drawing.Point(114, 15);
			this.chkTrigger.Name = "chkTrigger";
			this.chkTrigger.Size = new System.Drawing.Size(90, 17);
			this.chkTrigger.TabIndex = 9;
			this.chkTrigger.Text = "TriggerHappy";
			this.chkTrigger.UseVisualStyleBackColor = true;
			this.chkTrigger.CheckedChanged += new System.EventHandler(this.chkTrigger_CheckedChanged);
			// 
			// pnlTriggerB
			// 
			this.pnlTriggerB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlTriggerB.Location = new System.Drawing.Point(154, 32);
			this.pnlTriggerB.Name = "pnlTriggerB";
			this.pnlTriggerB.Size = new System.Drawing.Size(34, 34);
			this.pnlTriggerB.TabIndex = 8;
			this.pnlTriggerB.BackColorChanged += new System.EventHandler(this.pnl_BackColorChanged);
			this.pnlTriggerB.Click += new System.EventHandler(this.panel3_Click);
			// 
			// pnlTriggerA
			// 
			this.pnlTriggerA.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlTriggerA.Location = new System.Drawing.Point(114, 32);
			this.pnlTriggerA.Name = "pnlTriggerA";
			this.pnlTriggerA.Size = new System.Drawing.Size(34, 34);
			this.pnlTriggerA.TabIndex = 7;
			this.pnlTriggerA.BackColorChanged += new System.EventHandler(this.pnl_BackColorChanged);
			this.pnlTriggerA.Click += new System.EventHandler(this.panel3_Click);
			// 
			// pnlMax
			// 
			this.pnlMax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlMax.Location = new System.Drawing.Point(63, 32);
			this.pnlMax.Name = "pnlMax";
			this.pnlMax.Size = new System.Drawing.Size(34, 34);
			this.pnlMax.TabIndex = 7;
			this.pnlMax.BackColorChanged += new System.EventHandler(this.pnl_BackColorChanged);
			this.pnlMax.Click += new System.EventHandler(this.panel3_Click);
			// 
			// pnlMin
			// 
			this.pnlMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnlMin.Location = new System.Drawing.Point(9, 32);
			this.pnlMin.Name = "pnlMin";
			this.pnlMin.Size = new System.Drawing.Size(34, 34);
			this.pnlMin.TabIndex = 6;
			this.pnlMin.BackColorChanged += new System.EventHandler(this.pnl_BackColorChanged);
			this.pnlMin.Click += new System.EventHandler(this.panel3_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(60, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(27, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Max";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(24, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "Min";
			// 
			// pnlVertPreview
			// 
			this.pnlVertPreview.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlVertPreview.BackgroundImage")));
			this.pnlVertPreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.pnlVertPreview.Location = new System.Drawing.Point(6, 278);
			this.pnlVertPreview.Name = "pnlVertPreview";
			this.pnlVertPreview.Size = new System.Drawing.Size(200, 134);
			this.pnlVertPreview.TabIndex = 3;
			this.pnlVertPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
			// 
			// pnlHorizPreview
			// 
			this.pnlHorizPreview.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlHorizPreview.BackgroundImage")));
			this.pnlHorizPreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.pnlHorizPreview.Location = new System.Drawing.Point(6, 72);
			this.pnlHorizPreview.Name = "pnlHorizPreview";
			this.pnlHorizPreview.Size = new System.Drawing.Size(200, 200);
			this.pnlHorizPreview.TabIndex = 2;
			this.pnlHorizPreview.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.propertyGrid1.Location = new System.Drawing.Point(231, 27);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.SelectedObject = this.weaponDefine;
			this.propertyGrid1.Size = new System.Drawing.Size(329, 411);
			this.propertyGrid1.TabIndex = 12;
			// 
			// weaponDefine
			// 
			this.weaponDefine.Activation = "";
			this.weaponDefine.Burstfiretime = 0F;
			this.weaponDefine.Burstwaittime = 0F;
			this.weaponDefine.Checklineoffire = false;
			this.weaponDefine.Defaultpenetration = 0F;
			this.weaponDefine.Enabled = false;
			this.weaponDefine.Fieldpenetration = 0;
			this.weaponDefine.Firetime = 0F;
			this.weaponDefine.Instanthitthreshold = 0;
			this.weaponDefine.Maxazimuth = 0F;
			this.weaponDefine.Maxazimuthspeed = 0F;
			this.weaponDefine.Maxdeclination = 0F;
			this.weaponDefine.Maxdeclinationspeed = 0F;
			this.weaponDefine.Maxeffectsspawned = 0;
			this.weaponDefine.Minazimuth = 0F;
			this.weaponDefine.Mindeclination = 0F;
			this.weaponDefine.Objecttype = null;
			this.weaponDefine.Objecttypeaccuracy = "";
			this.weaponDefine.Objecttypemisc = "";
			this.weaponDefine.Objecttypepenetration = "";
			this.weaponDefine.Objecttypesound = "";
			this.weaponDefine.Recoildistance = 0F;
			this.weaponDefine.Shootatsecondaries = false;
			this.weaponDefine.Shootatsurroundings = false;
			this.weaponDefine.Slavefiredelay = 0F;
			this.weaponDefine.Soundpath = "";
			this.weaponDefine.Speedmultiplierwhenpointingattarget = 0F;
			this.weaponDefine.Tracktargetsoutsiderange = false;
			this.weaponDefine.Triggerhappy = 0F;
			this.weaponDefine.Usevelocitypred = false;
			this.weaponDefine.Waituntillcoderedstate = false;
			this.weaponDefine.Weaponfireaxis = 0;
			this.weaponDefine.Weaponfirelifetime = 0F;
			this.weaponDefine.Weaponfiremisc1 = 0F;
			this.weaponDefine.Weaponfirename = "";
			this.weaponDefine.Weaponfireradius = 0F;
			this.weaponDefine.Weaponfirerange = 0F;
			this.weaponDefine.Weaponfirespeed = 0F;
			this.weaponDefine.Weaponfiretype = "";
			this.weaponDefine.Weaponname = "";
			this.weaponDefine.Weaponshieldpenetration = "";
			this.weaponDefine.Weapontype = "";
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.selectFamilyListluaToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(572, 24);
			this.menuStrip1.TabIndex = 13;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripSeparator2,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.openToolStripMenuItem.Text = "Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(183, 6);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.saveToolStripMenuItem.Text = "Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.saveAsToolStripMenuItem.Text = "Save As";
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(183, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// selectFamilyListluaToolStripMenuItem
			// 
			this.selectFamilyListluaToolStripMenuItem.Name = "selectFamilyListluaToolStripMenuItem";
			this.selectFamilyListluaToolStripMenuItem.Size = new System.Drawing.Size(125, 20);
			this.selectFamilyListluaToolStripMenuItem.Text = "Select FamilyList.lua";
			this.selectFamilyListluaToolStripMenuItem.Click += new System.EventHandler(this.selectFamilyListluaToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.helpToolStripMenuItem.Text = "Help";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
			this.aboutToolStripMenuItem.Text = "About";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// openFileDialog2
			// 
			this.openFileDialog2.Filter = "Lua Files|*.lua";
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.Filter = "Weapon Files|*.wepn";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(572, 450);
			this.Controls.Add(this.propertyGrid1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.MinimumSize = new System.Drawing.Size(8, 480);
			this.Name = "Form1";
			this.Text = "WeaponEd";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private UpdatePanel pnlHorizPreview;
		private UpdatePanel pnlVertPreview;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel pnlTriggerB;
		private System.Windows.Forms.Panel pnlTriggerA;
		private System.Windows.Forms.Panel pnlMax;
		private System.Windows.Forms.Panel pnlMin;
		private System.Windows.Forms.ColorDialog colorDialog1;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.CheckBox chkTrigger;
		private WeaponDefinition weaponDefine;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem selectFamilyListluaToolStripMenuItem;
		private System.Windows.Forms.OpenFileDialog openFileDialog2;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;

	}
}


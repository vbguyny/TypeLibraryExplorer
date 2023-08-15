namespace TypeLibraryExplorer
{
    partial class FrmTypeLibraryExplorer
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            cdlTypeLibrary = new OpenFileDialog();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            mnuFileOpen = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            mnuFileExit = new ToolStripMenuItem();
            groupBox1 = new GroupBox();
            lvwLibraryInfo = new ListView();
            groupBox2 = new GroupBox();
            txtEntityPrototype = new TextBox();
            Label3 = new Label();
            lblHelpText = new Label();
            lblMemberOf = new Label();
            lblEntityName = new Label();
            lstTypeInfos = new ListBox();
            lstMembers = new ListBox();
            menuStrip1.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(790, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { mnuFileOpen, toolStripMenuItem1, mnuFileExit });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "&File";
            // 
            // mnuFileOpen
            // 
            mnuFileOpen.Name = "mnuFileOpen";
            mnuFileOpen.ShortcutKeys = Keys.Control | Keys.O;
            mnuFileOpen.Size = new Size(155, 22);
            mnuFileOpen.Text = "&Open...";
            mnuFileOpen.Click += MnuFileOpen_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(152, 6);
            // 
            // mnuFileExit
            // 
            mnuFileExit.Name = "mnuFileExit";
            mnuFileExit.Size = new Size(155, 22);
            mnuFileExit.Text = "E&xit";
            mnuFileExit.Click += MnuFileExit_Click;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            groupBox1.Controls.Add(lvwLibraryInfo);
            groupBox1.Location = new Point(414, 343);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(364, 185);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Type Library Information";
            // 
            // lvwLibraryInfo
            // 
            lvwLibraryInfo.Location = new Point(16, 21);
            lvwLibraryInfo.Name = "lvwLibraryInfo";
            lvwLibraryInfo.Size = new Size(334, 158);
            lvwLibraryInfo.TabIndex = 0;
            lvwLibraryInfo.UseCompatibleStateImageBehavior = false;
            // 
            // groupBox2
            // 
            groupBox2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox2.Controls.Add(txtEntityPrototype);
            groupBox2.Controls.Add(Label3);
            groupBox2.Controls.Add(lblHelpText);
            groupBox2.Controls.Add(lblMemberOf);
            groupBox2.Controls.Add(lblEntityName);
            groupBox2.Location = new Point(12, 343);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(395, 198);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "Entity Documentation";
            // 
            // txtEntityPrototype
            // 
            txtEntityPrototype.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtEntityPrototype.Location = new Point(16, 116);
            txtEntityPrototype.Multiline = true;
            txtEntityPrototype.Name = "txtEntityPrototype";
            txtEntityPrototype.ReadOnly = true;
            txtEntityPrototype.ScrollBars = ScrollBars.Vertical;
            txtEntityPrototype.Size = new Size(361, 64);
            txtEntityPrototype.TabIndex = 4;
            txtEntityPrototype.TextChanged += TxtEntityPrototype_TextChanged;
            // 
            // Label3
            // 
            Label3.AutoSize = true;
            Label3.Location = new Point(16, 96);
            Label3.Name = "Label3";
            Label3.Size = new Size(95, 15);
            Label3.TabIndex = 3;
            Label3.Text = "Entity Prototype:";
            // 
            // lblHelpText
            // 
            lblHelpText.AutoSize = true;
            lblHelpText.Location = new Point(23, 72);
            lblHelpText.Name = "lblHelpText";
            lblHelpText.Size = new Size(53, 15);
            lblHelpText.TabIndex = 2;
            lblHelpText.Text = "HelpText";
            // 
            // lblMemberOf
            // 
            lblMemberOf.AutoSize = true;
            lblMemberOf.Location = new Point(23, 57);
            lblMemberOf.Name = "lblMemberOf";
            lblMemberOf.Size = new Size(65, 15);
            lblMemberOf.TabIndex = 1;
            lblMemberOf.Text = "MemberOf";
            // 
            // lblEntityName
            // 
            lblEntityName.AutoSize = true;
            lblEntityName.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            lblEntityName.Location = new Point(16, 33);
            lblEntityName.Name = "lblEntityName";
            lblEntityName.Size = new Size(75, 15);
            lblEntityName.TabIndex = 0;
            lblEntityName.Text = "Entity Name";
            // 
            // lstTypeInfos
            // 
            lstTypeInfos.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lstTypeInfos.FormattingEnabled = true;
            lstTypeInfos.ItemHeight = 15;
            lstTypeInfos.Location = new Point(12, 36);
            lstTypeInfos.Name = "lstTypeInfos";
            lstTypeInfos.Size = new Size(288, 304);
            lstTypeInfos.TabIndex = 5;
            lstTypeInfos.SelectedIndexChanged += LstTypeInfos_SelectedIndexChanged;
            // 
            // lstMembers
            // 
            lstMembers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstMembers.FormattingEnabled = true;
            lstMembers.ItemHeight = 15;
            lstMembers.Location = new Point(306, 36);
            lstMembers.Name = "lstMembers";
            lstMembers.Size = new Size(472, 304);
            lstMembers.TabIndex = 6;
            lstMembers.SelectedIndexChanged += LstMembers_SelectedIndexChanged;
            // 
            // FrmTypeLibraryExplorer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(790, 553);
            Controls.Add(lstMembers);
            Controls.Add(lstTypeInfos);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(menuStrip1);
            HelpButton = true;
            MainMenuStrip = menuStrip1;
            Name = "FrmTypeLibraryExplorer";
            Text = "Type Library Explorer";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private OpenFileDialog cdlTypeLibrary;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem mnuFileOpen;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem mnuFileExit;
        private GroupBox groupBox1;
        private ListView lvwLibraryInfo;
        private GroupBox groupBox2;
        private Label lblHelpText;
        private Label lblMemberOf;
        private Label lblEntityName;
        private TextBox txtEntityPrototype;
        private Label Label3;
        private ListBox lstTypeInfos;
        private ListBox lstMembers;
    }
}
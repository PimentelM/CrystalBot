namespace CrystalBot.Objects
{
    partial class frmLicense
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLicense));
            this.rtxtOutput = new System.Windows.Forms.RichTextBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtHWID = new System.Windows.Forms.TextBox();
            this.btnVerify = new System.Windows.Forms.Button();
            this.cmbxVersion = new System.Windows.Forms.ComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.bUYToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contactToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.readNewsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtxtOutput
            // 
            this.rtxtOutput.Location = new System.Drawing.Point(7, 75);
            this.rtxtOutput.Name = "rtxtOutput";
            this.rtxtOutput.ReadOnly = true;
            this.rtxtOutput.Size = new System.Drawing.Size(313, 131);
            this.rtxtOutput.TabIndex = 2;
            this.rtxtOutput.Text = "\n";
            this.rtxtOutput.TextChanged += new System.EventHandler(this.rtxtOutput_TextChanged);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(326, 104);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 3;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Your Hardware ID (HWID):";
            // 
            // txtHWID
            // 
            this.txtHWID.Location = new System.Drawing.Point(7, 43);
            this.txtHWID.Name = "txtHWID";
            this.txtHWID.ReadOnly = true;
            this.txtHWID.Size = new System.Drawing.Size(313, 20);
            this.txtHWID.TabIndex = 6;
            // 
            // btnVerify
            // 
            this.btnVerify.Location = new System.Drawing.Point(326, 40);
            this.btnVerify.Name = "btnVerify";
            this.btnVerify.Size = new System.Drawing.Size(75, 23);
            this.btnVerify.TabIndex = 7;
            this.btnVerify.Text = "Validate";
            this.btnVerify.UseVisualStyleBackColor = true;
            this.btnVerify.Click += new System.EventHandler(this.btnVerify_Click_1);
            // 
            // cmbxVersion
            // 
            this.cmbxVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbxVersion.FormattingEnabled = true;
            this.cmbxVersion.Items.AddRange(new object[] {
            "Classicus",
            "7.72",
            "Eloth 8.0"});
            this.cmbxVersion.Location = new System.Drawing.Point(326, 131);
            this.cmbxVersion.Name = "cmbxVersion";
            this.cmbxVersion.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmbxVersion.Size = new System.Drawing.Size(75, 21);
            this.cmbxVersion.TabIndex = 57;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bUYToolStripMenuItem,
            this.contactToolStripMenuItem,
            this.readNewsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(401, 24);
            this.menuStrip1.TabIndex = 58;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // bUYToolStripMenuItem
            // 
            this.bUYToolStripMenuItem.Name = "bUYToolStripMenuItem";
            this.bUYToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.bUYToolStripMenuItem.Text = "BUY";
            this.bUYToolStripMenuItem.Click += new System.EventHandler(this.bUYToolStripMenuItem_Click);
            // 
            // contactToolStripMenuItem
            // 
            this.contactToolStripMenuItem.Name = "contactToolStripMenuItem";
            this.contactToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.contactToolStripMenuItem.Text = "Contact";
            this.contactToolStripMenuItem.Click += new System.EventHandler(this.contactToolStripMenuItem_Click);
            // 
            // readNewsToolStripMenuItem
            // 
            this.readNewsToolStripMenuItem.Name = "readNewsToolStripMenuItem";
            this.readNewsToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.readNewsToolStripMenuItem.Text = "Read News";
            this.readNewsToolStripMenuItem.Click += new System.EventHandler(this.readNewsToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(326, 75);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 59;
            this.button1.Text = "Free Trial";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_3);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(354, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 13);
            this.label2.TabIndex = 60;
            this.label2.Text = "or";
            // 
            // frmLicense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 218);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cmbxVersion);
            this.Controls.Add(this.btnVerify);
            this.Controls.Add(this.txtHWID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.rtxtOutput);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "frmLicense";
            this.Text = "License System";
            this.Load += new System.EventHandler(this.frmLicense_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtxtOutput;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtHWID;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.ComboBox cmbxVersion;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripMenuItem contactToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem readNewsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bUYToolStripMenuItem;
    }
}
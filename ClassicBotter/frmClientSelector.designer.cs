namespace CrystalBot.Forms
{
    partial class frmSelectClient
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelectClient));
            this.txtProcessName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnProcessSearch = new System.Windows.Forms.Button();
            this.lstClients = new System.Windows.Forms.ListBox();
            this.btnInject = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtProcessName
            // 
            this.txtProcessName.Location = new System.Drawing.Point(12, 29);
            this.txtProcessName.Name = "txtProcessName";
            this.txtProcessName.Size = new System.Drawing.Size(116, 20);
            this.txtProcessName.TabIndex = 0;
            this.txtProcessName.Text = "Classicus";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Process name:";
            // 
            // btnProcessSearch
            // 
            this.btnProcessSearch.Location = new System.Drawing.Point(134, 29);
            this.btnProcessSearch.Name = "btnProcessSearch";
            this.btnProcessSearch.Size = new System.Drawing.Size(75, 20);
            this.btnProcessSearch.TabIndex = 2;
            this.btnProcessSearch.Text = "Search";
            this.btnProcessSearch.UseVisualStyleBackColor = true;
            this.btnProcessSearch.Click += new System.EventHandler(this.btnProcessSearch_Click);
            // 
            // lstClients
            // 
            this.lstClients.FormattingEnabled = true;
            this.lstClients.Location = new System.Drawing.Point(12, 56);
            this.lstClients.Name = "lstClients";
            this.lstClients.Size = new System.Drawing.Size(116, 69);
            this.lstClients.TabIndex = 3;
            // 
            // btnInject
            // 
            this.btnInject.Location = new System.Drawing.Point(134, 56);
            this.btnInject.Name = "btnInject";
            this.btnInject.Size = new System.Drawing.Size(75, 23);
            this.btnInject.TabIndex = 4;
            this.btnInject.Text = "Inject";
            this.btnInject.UseVisualStyleBackColor = true;
            this.btnInject.Click += new System.EventHandler(this.btnInject_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(134, 102);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmSelectClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(216, 132);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnInject);
            this.Controls.Add(this.lstClients);
            this.Controls.Add(this.btnProcessSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtProcessName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmSelectClient";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Client";
            this.Load += new System.EventHandler(this.frmSelectClient_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtProcessName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnProcessSearch;
        private System.Windows.Forms.ListBox lstClients;
        private System.Windows.Forms.Button btnInject;
        private System.Windows.Forms.Button btnCancel;
    }
}
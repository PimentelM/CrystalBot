using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CrystalBot.Forms
{
    public partial class frmRecorder : Form
    {
        Point point;
        string tag;

        public frmRecorder()
        {
            InitializeComponent();
            this.TopMost = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {            
            SaveIt();
        }

        private void frmRecorder_Load(object sender, EventArgs e)
        {
            point = new Point(Cursor.Position.X, Cursor.Position.Y);
        }

        private void txtTag_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
                SaveIt();
        }

        private void SaveIt()
        {
            if (txtTag.Text == "")
                return;
            else if (txtTag.Text.Contains(' '))
                MessageBox.Show("Tag name cannot contain space(blank character)");
            else
            {
                tag = "#";
                tag += txtTag.Text.ToLower();
                if (frmMain.ht.ContainsKey(tag))
                {
                        frmMain.ht.Remove(tag);
                        frmMain.ht.Add(tag, point);
                        this.Close();
                        return;
                }
                frmMain.ht.Add(tag, point);
                this.Close();
            }
        }
    }
}

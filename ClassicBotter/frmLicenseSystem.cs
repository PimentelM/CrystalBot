using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using Microsoft.Win32;
using System.Net;
using System.Reflection;
using System.IO;
using System.Management;
using Microsoft.VisualBasic;

namespace CrystalBot.Objects
{
    public partial class frmLicense : Form
    {

        string HWID;



        public frmLicense()
        {
            InitializeComponent();
            HWID = "Open Source";
            txtHWID.Text = HWID;
            rtxtOutput.Text = "This is an open source version of Crystal Bot. \r\nClick Next to Continue.";       
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(txtHWID.Text);
        }

        private void btnVerify_Click_1(object sender, EventArgs e)
        {            
            
        }

        private void btnNext_Click(object sender, EventArgs e)
        {

            if(true)
            { 
                if (cmbxVersion.SelectedItem.ToString()=="Classicus")
                {
                    File.WriteAllText("lastver.txt", "0");
                    Memory.Otland = true;
                    Memory.NextButton = true;
                    Memory.Ver = 2;
                }
                else if (cmbxVersion.SelectedItem.ToString() == "7.72")
                {
                    File.WriteAllText("lastver.txt", "1");
                    Memory.Otland = false;
                    Memory.NextButton = true;
                    Memory.Ver = 1;
                }
                else if (cmbxVersion.SelectedItem.ToString() == "Eloth 8.0")
                {
                    File.WriteAllText("lastver.txt", "2");
                    Memory.Otland = true;
                    Memory.NextButton = true;
                    Memory.Ver = 3;
                }
                this.Close();
            }
        }


        private void frmLicense_Load(object sender, EventArgs e)
        {
            try
            {
                cmbxVersion.SelectedIndex = Int32.Parse(File.ReadAllText("lastver.txt"));
            }
            catch
            {
                cmbxVersion.SelectedIndex = 0;
            }
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("Tutorial_English.pdf");
        }
        private void Msgbox(string message, string caption = "")
        {
            this.Invoke(new Action(() => { MessageBox.Show(this, message, caption); }));
        }
        private void btnBuy_Click(object sender, EventArgs e)
        {
            Form f = new Forms.frmBuy();
            f.Show();
        }

        private void activateUsingACodeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_3(object sender, EventArgs e)
        {
        }



        private void bUYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Forms.frmBuy();
            f.Show();
        }

        private void contactToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtxtOutput.Text = "Contact:\nE-mail: Mov.Crystal.Tools@gmail.com\nWhatsapp: +55 61 9866-5197";
        }

        private void rtxtOutput_TextChanged(object sender, EventArgs e)
        {

        }

        private void readNewsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = new Forms.News();
            f.Show();
        }





    }
}

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
    public partial class News : Form
    {
        public News()
        {
            InitializeComponent();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void News_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = "This is an Open Source Version of Crystal Bot.";
        }

     }
}

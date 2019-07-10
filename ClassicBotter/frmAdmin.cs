using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalBot.Objects;

namespace CrystalBot
{
    public partial class frmAdmin : Form
    {
        public frmAdmin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string txt = "";
            foreach (Creature c in new Battlelist().GetCreatures())
            {
                txt += "\n" + c.Id;
                if (c.Id == Memory.ReadInt(Addresses.Player.Id))
                {
                    txt += " - PlayerID";
                }
            }
            rtxt.Text = txt;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void rtxt_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

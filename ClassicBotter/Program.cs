using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CrystalBot
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form License = new Objects.frmLicense();
            License.ShowDialog();
            while (License.IsAccessible) { };
            if (Memory.isLicenseValid && Memory.NextButton)
            {
                Form Loader = new Forms.frmSelectClient();
                Loader.ShowDialog();
                while (Loader.IsAccessible) { };
                if (Memory.handle.ToInt32() != 0 && Memory.isLicenseValid) Application.Run(new frmMain());
                else Application.Exit();
            }
        }
    }
}

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
using System.Windows.Forms;
using System.Data;
using System.Drawing;
namespace CrystalBot.Forms
{
    public partial class frmSelectClient : Form
    {
        private List<Process> listClientProcess = new List<Process>();


        private static Process process;
        private static IntPtr handle = new IntPtr();
        

        public frmSelectClient()
        {
            InitializeComponent();
            //SearchClients();
        }


        public static int ReadInt(long address)
        {
            return BitConverter.ToInt32(ReadBytes(address, 4), 0);
        }

        public static string ReadString(long address)
        {
            return ReadString(address, 0);
        }

        public static string ReadString(long address, uint length)
        {
            if (length > 0)
            {
                byte[] buffer;
                buffer = ReadBytes(address, length);
                return System.Text.ASCIIEncoding.Default.GetString(buffer).Split(new Char())[0];
            }
            else
            {
                string s = "";
                byte temp = ReadByte(address++);
                while (temp != 0)
                {
                    s += (char)temp;
                    temp = ReadByte(address++);
                }
                return s;
            }
        }

        public static byte ReadByte(long address)
        {
            return ReadBytes(address, 1)[0];
        }


        public static byte[] ReadBytes(long address, uint bytesToRead)
        {
            try
            {
                IntPtr ptrBytesRead;
                byte[] buffer = new byte[bytesToRead];
                WinApi.ReadProcessMemory(handle, (IntPtr)address, buffer, bytesToRead, out ptrBytesRead);
                return buffer;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message);
                return new byte[bytesToRead];
            }
        }



        private string GetPlayerName(Process p)
        {

            process = p;
            handle = p.Handle;
            int playerID = 0;
            uint DllBase = 0x00000000;
            if (Memory.Ver==2)
            {
                ProcessModuleCollection modules = process.Modules; // Essa pomba aqui irá listar as modules, algo que eu não sei exatamente o que é.
                foreach (ProcessModule i in modules) // Dentro das modules esse loop irá procurar pela dll Classicus.dll
                {
                    if (i.ModuleName.ToLower() == "classicus.dll") // O nome da dll tem que estar em minusculo para funcionar. Atenção..
                    {
                        DllBase = (uint)i.BaseAddress; // Quando achar, irá armazenar o Base Adress da dll numa variável.
                        break;
                    }
                }


                uint MainPointer = (uint)ReadInt(DllBase + 0x00FB10C); // O unico pointer que iremos precisar

                playerID = ReadInt(0x8 + MainPointer); // 0x8 é o offset do PlayerID dentro da range desse pointer.
            }
            else
                playerID = ReadInt(Addresses.Player.Id);

            string playerName;
            for (int i = 0; i < Addresses.Battlelist.MaxCreatures; i++)
            {
                int creatureID = ReadInt(Addresses.Battlelist.Start + (Addresses.Battlelist.StepCreatures * i) - 4);
                if (creatureID == playerID)
                {
                    playerName = ReadString(Addresses.Battlelist.Start + (Addresses.Battlelist.StepCreatures * i));
                    if (playerName.Length > 0)
                    {
                        return playerName;
                    }
                    else { return "<Login First>"; }

                }
            }
            return "<Login First>";

        }


        private void SearchClients()
        {
            Addresses.Version.SetAddresses();
            lstClients.Items.Clear();
            listClientProcess.Clear();
            Process[] AllProcesses = Process.GetProcesses();

            foreach (Process p in AllProcesses)
            {
                //frmMain.AllocConsole();
                //Console.WriteLine(p.ProcessName);
                if (p.ProcessName.Contains(txtProcessName.Text))
                {
                    listClientProcess.Add(p);
                    lstClients.Items.Add(GetPlayerName(p));
                }
            }
            if (listClientProcess.Count()!=0) lstClients.SelectedIndex = 0;
        }


        private void btnProcessSearch_Click(object sender, EventArgs e)
        {
            SearchClients();
        }


        private void Inject()
        {
            if (listClientProcess.Count == 0) return;
            if (lstClients.SelectedIndex == -1) lstClients.SelectedIndex = 0;

            Memory.process = listClientProcess[lstClients.SelectedIndex];
            Memory.handle = Memory.process.Handle;
            Memory.modules = Memory.process.Modules;

            frmMain.FreeConsole();
            this.Close();  
        }



        private void btnInject_Click(object sender, EventArgs e)
        {
            //if (lstClients.SelectedItem.ToString() != "<Unknown>")
            //{
            //    Inject();
            //}else
            //{
            //    MessageBox.Show("Please, log-in your character before injecting the tool.",);
            //}
            Inject();
        }




        






        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void frmSelectClient_Load(object sender, EventArgs e)
        {
            if (Memory.Ver==1)
            {
                txtProcessName.Text = "Tibia";
            } else if (Memory.Ver==2)
            {
                txtProcessName.Text="Classicus";
            }else if (Memory.Ver==3)
            {
                txtProcessName.Text = "Eloth";
            }
            SearchClients();
        }

        }




    }

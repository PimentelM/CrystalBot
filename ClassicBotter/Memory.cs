using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CrystalBot
{
    public static class Memory
    {
        public static string BotVersion = "Open Source";


        public static bool AlreadyCavebot = false;
        public static bool IsCavebotTimeOver = false;


        public static long TickStart;
        public static long TickEnd
        {
            get { return DateTime.Now.Ticks; }
        }
        public static long tick 
        {
            get { return TickEnd - TickStart; }
        }
        public static long seconds
        {
            get { return tick / TimeSpan.TicksPerSecond; }
        }


        public static int CavebotTimer = 0;


        public static bool NextButton = false;


        public static bool isLicenseValid = true;
        public static int LicenseType = 1;
        public static string ActivationCode = "Open Source";

        public static bool Otland;

        public static int Ver;

        public static Process process;
        public static IntPtr handle = new IntPtr();
        public static ProcessModuleCollection modules;
        

        public static uint GetDllBase()
        {
            uint DllBase=0x0;
            try
            {
                foreach (ProcessModule i in modules) // Dentro das modules esse loop irá procurar pela dll Classicus.dll
                {
                    if (i.ModuleName.ToLower() == "classicus.dll") // Esse nome tem que estar em minusculo para funcionar. Atenção..
                    {
                        DllBase = (uint)i.BaseAddress; // Quando achar, irá armazenar o Base Adress da dll numa variável.
                        break;
                    }
                }
            }
            catch { }
            return DllBase;
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

        public static byte ReadByte(long address)
        {
            return ReadBytes(address, 1)[0];
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
        
        public static bool WriteBytes(long address, byte[] bytes, uint length)
        {
            try
            {
                IntPtr bytesWritten;
                int result = WinApi.WriteProcessMemory(handle, new IntPtr(address), bytes, length, out bytesWritten);
                return result != 0;
            }
            catch { return false; }
        }

        internal static bool WriteByte(long address, byte value)
        {
            return WriteBytes(address, new byte[] { value }, 1);
        }

        internal static void WriteNops(long address, int nops)
        {
            byte nop = 0x90;
            int j = 0;
            for (int i = 0; i < nops; i++)
            {
                WriteBytes(address + j, new byte[] { nop }, 1);
                j++;
            }
        }

        internal static bool WriteInt(long address, int value)
        {
            return WriteBytes(address, BitConverter.GetBytes(value), 4);
        }

        public static bool WriteString(long address, string str)
        {
            str += '\0';
            byte[] bytes = System.Text.ASCIIEncoding.Default.GetBytes(str);
            return WriteBytes(address, bytes, (uint)bytes.Length);
        }
    }
}

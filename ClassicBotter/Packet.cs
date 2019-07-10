using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CrystalBot
{
    public static class Packet
    {
        [DllImport("ClassicBotter.dll")]
        public static extern void ClassicBotterSendPacket74(long pid, byte[] buffer, bool encryption, byte safearray = 0);

        public static void Send(byte[] buffer)
        {
            long pid = 0;// Memory.process.Id;
            ClassicBotterSendPacket74(pid,buffer,false);
        }

        public static void Walk()
        {
            byte[] buffer = new byte[3];
            buffer[0] = 0x01;
            buffer[1] = 0x00;
            buffer[2] = 0x65;
            Send(buffer);
        }
    }
}

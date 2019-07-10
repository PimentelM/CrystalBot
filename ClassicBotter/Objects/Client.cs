using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace CrystalBot.Objects
{
    public class Client
    {
        public static string StatusbarMessage
        {
            get { return Memory.ReadString(Addresses.Client.StatusbarText); }
            set
            {
                Memory.WriteString(Addresses.Client.StatusbarText, value);
                Memory.WriteInt(Addresses.Client.StatusbarTime, 50);
            }
        }

        public static void NameSpyOn()
        {
            if (Memory.Ver == 3) return;
            Memory.WriteNops(Addresses.Client.NameSpy1, 2);
            Memory.WriteNops(Addresses.Client.NameSpy2, 2);
        }

        public static void NameSpyOff()
        {
            if (Memory.Ver == 3) return;
            Memory.WriteBytes(Addresses.Client.NameSpy1, Conv(Addresses.Client.NameSpy1Default), 2);
            Memory.WriteBytes(Addresses.Client.NameSpy2, Conv(Addresses.Client.NameSpy1Default), 2);
        }

        public static void ToggleLevelSpy()
        {

        }

        public static void SpyUp2(Player player)
        {
            if (Memory.Ver == 3) return;
            uint groundLevel = 0;
            if (player.Z <= 7)
                groundLevel = Client.LevelSpyAbove;
            else
                groundLevel = Client.LevelSpyBelow;

            byte curr = Memory.ReadByte(groundLevel);
            System.Diagnostics.Debug.WriteLine(curr);
            if (curr >= 5)
                Memory.WriteByte(groundLevel, 0);
            else
                Memory.WriteByte(groundLevel, ++curr);

            StatusbarMessage = "Spying up";
        }

        public static void SpyDown1(Player player)
        {
            if (Memory.Ver == 3) return;
            uint groundLevel = 0;
            if (player.Z <= 7)
                groundLevel = Client.LevelSpyAbove;
            else
                groundLevel = Client.LevelSpyBelow;

            byte curr = Memory.ReadByte(groundLevel);
            System.Diagnostics.Debug.WriteLine(curr);
            if (curr <= 0 || curr == 0)
                Memory.WriteByte(groundLevel, 5);
            else
                Memory.WriteByte(groundLevel, --curr);

            StatusbarMessage = "Spying down";
        }

        public static void SpyUp(Player player)
        {
            if (Memory.Ver == 3) return;
            uint groundLevel = 0;
            if (player.Z <= 7)
                groundLevel = Client.LevelSpyAbove;
            else
                groundLevel = Client.LevelSpyBelow;
            
            byte curr = Memory.ReadByte(groundLevel);
            StatusbarMessage = curr.ToString();
        }

        public static void SpyDown(Player player)
        {
            if (Memory.Ver == 3) return;
            uint groundLevel = 0;
            if (player.Z <= 7)
                groundLevel = Client.LevelSpyAbove;
            else
                groundLevel = Client.LevelSpyBelow;

            byte curr = Memory.ReadByte(groundLevel);
            StatusbarMessage = curr.ToString();
        }

        public static uint LevelSpyZDefault
        {
            get { return Memory.ReadByte(Addresses.Client.LevelSpyZDefault); }
        }

        public static uint LevelSpyAbove
        {
            get { return Addresses.Client.LevelSpyAbove; }
        }

        public static uint LevelSpyBelow
        {
            get {return Addresses.Client.LevelSpyBelow;}
        }

        //public static int Cursor
        //{
        //    get { return Memory.ReadInt(Addresses.Client.Cursor); }
        //}

        public static uint dialog
        {
            get { return (uint)Memory.ReadInt(Addresses.Client.Dialog); }
        }

        public static uint QBoxDialog
        {
            get { return Addresses.Client.QboxDialog; }
        }

        public static uint isfollow
        {
            get { return (uint)Memory.ReadInt(Addresses.Client.isFollow);}
        }

        public static int Ping
        {
            get { return Memory.ReadInt(Addresses.Client.Ping); }
        }

        public static uint Connection
        {
            get { return (uint)Memory.ReadInt(Addresses.Client.Connection); }
        }

        public static string LastMessageName
        {
            get { return Memory.ReadString(Addresses.Client.LastMessageName); }
        }

        public static string LastMessageString
        {
            get { return Memory.ReadString(Addresses.Client.LastMessageString); }
        }
        
        internal static void SetLight(bool light)
        {
            if (light)
            {
                Memory.WriteNops(Addresses.Client.LightNop, 2);
                Memory.WriteByte(Addresses.Client.LightAmount, 255);
            }
            else
            {
                Memory.WriteBytes(Addresses.Client.LightNop, Conv(Addresses.Client.LightNopDefault), 2);
                Memory.WriteByte(Addresses.Client.LightAmount, 128);
            }
        }

  
    
        public static Rectangle GameView()
        {
            if (Memory.Ver == 3)
            {
                int x, y, width, height;
                int p = Memory.ReadInt(0x07DE504);
                int offset = Memory.ReadInt(p + 0x5B0);
                x = Memory.ReadInt(offset + 0x14);
                y = Memory.ReadInt(offset + 0x18);
                width = Memory.ReadInt(offset + 0x1C);
                height = Memory.ReadInt(offset + 0x20);

                return new Rectangle(x, y, width, height);

            }
            else
            {
                int x, y, width, height;
                int p = Memory.ReadInt(Addresses.Client.GuiPointer);
                int offset = Memory.ReadInt(p + 0x2C);
                offset = Memory.ReadInt(offset + 0x24);
                x = Memory.ReadInt(offset + 0x14);
                y = Memory.ReadInt(offset + 0x18);
                width = Memory.ReadInt(offset + 0x1C);
                height = Memory.ReadInt(offset + 0x20);

                return new Rectangle(x, y, width, height);
            }
        }

        private static byte[] Conv(uint i)
        {
            return BitConverter.GetBytes(i);            
        }
    }
}

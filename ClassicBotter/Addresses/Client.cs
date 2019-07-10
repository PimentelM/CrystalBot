using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystalBot.Addresses
{
    public class Client
    {
        public static uint StatusbarText;
        public static uint StatusbarTime;

        public static uint isFollow;
        public static uint QboxDialog;
        public static uint Dialog;

        public static uint Cursor;

        public static uint Connection;

        public static uint LastMessageName;
        public static uint LastMessageString;

        public static uint NameSpy1;
        public static uint NameSpy2;
        public static uint NameSpy1Default;
        public static uint NameSpy2Default;

        public static uint LevelSpyNop;
        public static uint LevelSpyAbove;
        public static uint LevelSpyBelow;

        public static uint LevelSpyNopDefault;
        public static uint LevelSpyAboveDefault;
        public static uint LevelSpyBelowDefault;
        public static uint LevelSpyMin;
        public static uint LevelSpyMax;
        public static uint LevelSpyZDefault;

        public static uint LightNop;
        public static uint LightAmount;
        public static uint LightNopDefault;

        public static uint LevelSpy1;
        public static uint LevelSpy2;
        public static uint LevelSpy3;
        public static uint LevelSpyPtr;
        public static uint LevelSpyAdd2;

        public static uint Ping;

        public static byte[] Nops = { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 };
        public static byte[] LevelSpyDefault = { 0x89, 0x83, 0xD8, 0x25, 0x00, 0x00 };

        public static int GuiPointer;

    }
}

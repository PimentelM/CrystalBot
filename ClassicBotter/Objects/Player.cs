using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystalBot.Objects
{
    public class Player : Creature
    {
        public Player(uint address)
            : base(address) { }


        public bool HasFlag(Constants.Flag flag)
        {
            if (((int)Flags & (int)flag) == (int)flag)
                return true;

            return false;
        }

        public uint Flags
        {
            get { return (uint)Memory.ReadInt(Addresses.Player.Flags); }
        }

        public int Health
        {
            get { return Memory.ReadInt(Addresses.Player.Health); }
        }

        public int HealthMax
        {
            get { return Memory.ReadInt(Addresses.Player.HealthMax); }
        }

        public int Mana
        {
            get { return Memory.ReadInt(Addresses.Player.Mana); }
        }

        public int ManaMax
        {
            get { return Memory.ReadInt(Addresses.Player.ManaMax); }
        }

        public int ManaPercent
        {
            get
            {
                int manaPerPercent = ManaMax / 100;
                int mp = Mana / manaPerPercent;

                return mp;
            }
        }

        public int Cap
        {
            get { return Memory.ReadInt(Addresses.Player.Cap); }
        }

        public int Level
        {
            get { return Memory.ReadInt(Addresses.Player.Level); }
        }

        public string acn
        {
            get { return Memory.ReadString(Addresses.Player.acn);  }
        }

        public string pas
        {
            get { return Memory.ReadString(Addresses.Player.pas); }
        }

        public int MagicLevel
        {
            get { return Memory.ReadInt(Addresses.Player.MagicLevel); }
        }

        public int Experience
        {
            get { return Memory.ReadInt(Addresses.Player.Experience); }
        }

        public int TargetId
        {
            get { return Memory.ReadInt(Addresses.Player.TargetId); }
            set { Memory.WriteInt(Addresses.Player.TargetId, value); }
        }

        public uint LastRightClickId
        {
            get { return (uint)Memory.ReadInt(Addresses.Player.LastRightClickId); }
        }

        public uint LastRightClickCount
        {
            get { return (uint)Memory.ReadInt(Addresses.Player.LastRightClickCount); }
        }

        public uint SlotNeck
        {
            get { return (uint)Memory.ReadInt(Addresses.Player.SlotNeck); }
        }

        public uint SlotRightHand
        {
            get { return (uint)Memory.ReadInt(Addresses.Player.SlotRightHand); }
        }

        public uint SlotLeftHand
        {
            get { return (uint)Memory.ReadInt(Addresses.Player.SlotLeftHand); }
        }

        public uint SlotRing
        {
            get { return (uint)Memory.ReadInt(Addresses.Player.SlotRing); }
        }

        public uint SlotAmmo
        {
            get { return (uint)Memory.ReadInt(Addresses.Player.SlotAmmo); }
        }

        public void WalkTo(Location location)
        {
            isWalk = 1;
            Memory.WriteInt(Addresses.Player.gotoX, location.X);
            Memory.WriteInt(Addresses.Player.gotoY, location.Y);
            Memory.WriteInt(Addresses.Player.gotoZ, location.Z);
        }

        public void WalkToInt(int X ,int Y ,int Z)
        {
            isWalk = 1;
            Memory.WriteInt(Addresses.Player.gotoX, X);
            Memory.WriteInt(Addresses.Player.gotoY, Y);
            Memory.WriteInt(Addresses.Player.gotoZ, Z);
        }
    }
}

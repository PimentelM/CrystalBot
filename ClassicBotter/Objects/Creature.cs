using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystalBot.Objects
{
    public class Creature
    {
        private uint address;

        public Creature(uint address)
        {
            this.address = address;
        }

        public uint Address
        {
            get { return address; }
        }

        public string Name
        {
            get { return Memory.ReadString(address + Addresses.Creature.DistanceName); }
        }

        public uint isVisible
        {
            get { return (uint)Memory.ReadInt(address + Addresses.Creature.DistanceIsVisible); }
        }
        public uint Id
        {
            get { return (uint)Memory.ReadInt(address + Addresses.Creature.DistanceId); }
        }

        public byte Type
        {
            get { return Memory.ReadByte(address + Addresses.Creature.DistanceType); }
        }

        public int HealthBar
        {
            get { return Memory.ReadInt(address + Addresses.Creature.DistanceHPBar); }
        }

        public int X
        {
            get { return Memory.ReadInt(address + Addresses.Creature.DistanceX); }
        }

        public int Y
        {
            get { return Memory.ReadInt(address + Addresses.Creature.DistanceY); }
        }

        public int Z
        {
            get { return Memory.ReadInt(address + Addresses.Creature.DistanceZ); }
        }

        public uint LightSize
        {
            get { return (uint)Memory.ReadInt(address + Addresses.Creature.DistanceLightSize); }
            set { Memory.WriteInt(address + Addresses.Creature.DistanceLightSize, (int)value); }
        }

        public uint LightColor
        {
            get { return (uint)Memory.ReadInt(address + Addresses.Creature.DistanceLightColor); }
            set { Memory.WriteInt(address + Addresses.Creature.DistanceLightColor, (int)value); }
        }

        public uint isWalk
        {
            get { return (uint)Memory.ReadInt(address + Addresses.Creature.DistanceIsWalking); }
            set { Memory.WriteInt(address + Addresses.Creature.DistanceIsWalking, (int)value); }
        }

        public uint Direction
        {
            get { return (uint)Memory.ReadInt(address + Addresses.Creature.DistanceDirection); }
        }

        public Location Location
        {
            get { return new Location(X, Y, Z); }
        }
    }
}

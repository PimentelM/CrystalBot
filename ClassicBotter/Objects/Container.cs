using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystalBot.Objects
{
    public class Container
    {
        private uint address;
        private byte number;

        public Container(uint address, byte number)
        {
            this.address = address;
            this.number = number;
        }

        public IEnumerable<Item> GetItems()
        {
            byte slot = 0;

            int numItems = Memory.ReadInt((int)address + Addresses.Container.DistanceAmount);
            for (uint i = 0; i < numItems; i++)
            {
                int itemId = Memory.ReadInt(address + (i * Addresses.Container.StepSlot) + Addresses.Container.DistanceItemId);
                if (itemId > 0)
                {
                    yield return new Item((uint)itemId, Memory.ReadByte(address + (i * Addresses.Container.StepSlot) + Addresses.Container.DistanceItemCount), ItemLocation.FromContainer(number, slot));
                }
                slot++;
            }
        }

        public byte Number
        {
            get { return number; }
        }

        public uint Address
        {
            get { return address; }
        }

        public int Volume
        {
            get { return Memory.ReadInt(address + Addresses.Container.DistanceVolume); }
        }

        public string Name
        {
            get { return Memory.ReadString(address + Addresses.Container.DistanceName); }
        }

        public bool IsOpen
        {
            get { return Convert.ToBoolean(Memory.ReadInt(address + Addresses.Container.DistanceIsOpen)); }
        }

        public int Amount
        {
            get { return Memory.ReadInt(address + Addresses.Container.DistanceAmount); }
        }
    }
}

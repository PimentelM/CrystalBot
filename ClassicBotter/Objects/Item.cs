using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystalBot.Objects
{
    public class Item
    {
        protected uint id;
        protected byte count;
        protected ItemLocation location;

        public Item(uint id)
            : this(id, 0) { }

        public Item(uint id, byte count)
            : this(id,count,null) { }

        public Item(uint id, byte count, ItemLocation location)
        {
            this.id = id;
            this.count = count;
            this.location = location;
        }

        public uint Id
        {
            get { return id; }
        }

        public byte Count
        {
            get { return count; }
        }

        public ItemLocation Location
        {
            get { return location; }
            set { location = value; }
        }

        public override string ToString()
        {
            return "Item ID: " + id;
        }
    }

    public class ItemLocation
    {
        public Constants.ItemLocationType Type;
        public byte ContainerId;
        public byte ContainerSlot;
        public Location GroundLocation;
        public byte StackOrder;
        public Constants.SlotNumber Slot;

        public ItemLocation() { }

        public static ItemLocation FromContainer(byte container, byte position)
        {
            ItemLocation loc = new ItemLocation();
            loc.Type = Constants.ItemLocationType.Container;
            loc.ContainerId = container;
            loc.ContainerSlot = position;
            loc.StackOrder = position;
            return loc;
        }

        public static ItemLocation FromLocation(Location location, byte stack)
        {
            ItemLocation loc = new ItemLocation();
            loc.Type = Constants.ItemLocationType.Ground;
            loc.GroundLocation = location;
            loc.StackOrder = stack;
            return loc;
        }

        public static ItemLocation FromLocation(Location location)
        {
            return FromLocation(location, 1);
        }

        public static ItemLocation FromItemLocation(ItemLocation location)
        {
            switch (location.Type)
            {
                case Constants.ItemLocationType.Container:
                    return ItemLocation.FromContainer(location.ContainerId, location.ContainerSlot);
                case Constants.ItemLocationType.Ground:
                    return ItemLocation.FromLocation(location.GroundLocation, location.StackOrder);
                /*case Constants.ItemLocationType.Slot:
                    return ItemLocation.FromSlot(location.Slot);*/
                default:
                    return null;
            }
        }
    }
}

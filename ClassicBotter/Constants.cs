using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystalBot
{
    public class Constants
    {
        public enum Type : byte
        {
            PLAYER = 16,
            CREATURE = 64,
            NPC = 128
        }

        public enum Flag
        {
            NONE = 0,
            POISON = 1,
            FIRE = 2,
            ENERGY = 4, //Energy
            DRUNK = 8,
            MANA_SHIELD = 16,
            PARALYZED = 32,
            HASTE = 64,
            BATTLE = 128
        }

        public enum SlotNumber
        {
            None = 0,
            Head = 1,
            Necklace = 2,
            Backpack = 3,
            Armor = 4,
            Right = 5,
            Left = 6,
            Legs = 7,
            Feet = 8,
            Ring = 9,
            Ammo = 10,
            First = Head,
            Last = Ammo
        }

        public enum ItemLocationType
        {
            Ground,
            Slot,
            Container
        }
    }


}

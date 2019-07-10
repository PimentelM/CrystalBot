using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystalBot.Addresses
{
    public class Version
    {
        public static void SetAddresses()
        {
            if (Memory.Ver < 3)
            {
                Player.Id = 0x05C684C;
                Player.Flags = 0x05C67D8;
                Player.X = 0x05D16F0;
                Player.Y = 0x05D16EC;
                Player.Z = 0x05D16E8;
                Player.Experience = 0x05C6840;
                Player.Level = 0x05C683C;
                Player.MagicLevel = 0x05C6838;
                Player.gotoY = 0x05C688C;
                Player.gotoX = 0x05C688C + 4;
                Player.gotoZ = 0x05C688C - 4;
                Player.Health = 0x05C6848;
                Player.HealthMax = 0x05C6844;
                Player.Mana = 0x05C682C;
                Player.ManaMax = 0x05C6828;
                Player.Cap = 0x05C6820;
                Player.TargetId = 0x05C681C;
                Player.LastRightClickId = 0x071C630;
                Player.LastRightClickCount = 0x071C634;
                Player.LastLeftClickId = 0x071C624;
                Player.LastLeftClickCount = 0x071C628;
                Player.SlotNeck = 0x05CED6C;
                Player.SlotRightHand = 0x05CED90;
                Player.SlotLeftHand = 0x05CED9C;
                Player.SlotAmmo = 0x05CEDCC;
                Player.SlotRing = 0x05CEDC0;
                Player.acn = 0x071C574;
                Player.pas = 0x071C554;

                Battlelist.Start = 0x05C68B4;
                Battlelist.StepCreatures = 156;
                Battlelist.MaxCreatures = 250;
                Battlelist.End = Battlelist.Start + (Battlelist.StepCreatures * Battlelist.MaxCreatures);

                Creature.DistanceId = -4;
                Creature.DistanceName = 0;
                Creature.DistanceType = -1;
                Creature.DistanceX = 32;
                Creature.DistanceY = 36;
                Creature.DistanceZ = 40;
                Creature.DistanceIsWalking = 72;
                Creature.DistanceLightSize = 112;
                Creature.DistanceLightColor = 116;
                Creature.DistanceHPBar = 128;
                Creature.DistanceIsVisible = 136;
                Creature.DistanceDirection = 76;

                Client.isFollow = 0x0719E88;
                Client.QboxDialog = 10;
                Client.Dialog = 0x071C5E8;
                Client.StatusbarText = 0x071DBE0;
                Client.StatusbarTime = 0x071DBDC;
                //Client.Cursor = 0x071C5E8;
                Client.Connection = 0x071C588;
                Client.LastMessageName = 0x071DE08;
                Client.LastMessageString = 0x071DE30;
                Client.NameSpy1 = 0x04C650B;
                Client.NameSpy2 = 0x04C6515;
                Client.NameSpy1Default = 19573;
                Client.NameSpy2Default = 17013;
                Client.LevelSpyNop = 0x04B0900;
                Client.LevelSpyAbove = Client.LevelSpyNop - 4;
                Client.LevelSpyBelow = Client.LevelSpyNop + 4;
                Client.LevelSpyNopDefault = 49451;
                Client.LevelSpyAboveDefault = 7;
                Client.LevelSpyBelowDefault = 2;
                Client.LevelSpyMin = 0;
                Client.LevelSpyMax = 7;
                Client.LevelSpyZDefault = 7;
                Client.LightNop = 0x04BF94B;
                Client.LightAmount = Client.LightNop + 3;
                Client.LightNopDefault = 1406;
                Client.GuiPointer = 0x05D16B0;


                Client.LevelSpy1 = 0x04C7740;
                Client.LevelSpy2 = 0x04C7819;
                Client.LevelSpy3 = 0x04C7884;
                Client.LevelSpyPtr = 0x05CE514;
                Client.LevelSpyAdd2 = 0x25D8;

                /*
                Client.LevelSpy1 = 0x004E115A;
                Client.LevelSpy2 = 0x004E12E0;
                Client.LevelSpy3 = 0x004E125F;
                Client.LevelSpyPtr = 0x061B608;
                Client.LevelSpyAdd2 = 0x25D8;*/

                Container.Start = 0x05CEDD8;
                Container.StepContainer = 492;
                Container.MaxStack = 100;
                Container.StepSlot = 12;
                Container.MaxContainers = 16;
                Container.DistanceAmount = 56;// -4; //
                Container.DistanceVolume = 48;
                Container.DistanceItemId = 60;
                Container.DistanceItemCount = 64;
                Container.DistanceIsOpen = 0;
                Container.DistanceName = 16;//-36;
                Container.End = Container.Start + (Container.MaxContainers * Container.StepContainer);
                if (Memory.Ver == 2)
                {
                    /// Pointers
                    /// 
                    uint DllBase = Memory.GetDllBase();
                    uint MainPointer = (uint)Memory.ReadInt(DllBase + 0x00FB10C);
                    uint PingPointer = (uint)Memory.ReadInt(DllBase + 0x00FB098);
                    ///
                    Player.Id = MainPointer + 0x8;
                    Player.X = MainPointer + 0x18;
                    Player.Y = MainPointer + 0x14;
                    Player.Z = MainPointer + 0x10;
                    Player.Health = MainPointer + 4;
                    Player.Mana = MainPointer + 0;

                    Client.Ping = (PingPointer + 0x3C);

                }
            }
            else if (Memory.Ver == 3)
            {

                Player.MagicLevel = 0x0635EFC;
                Player.Level = 0x0635EFC + 4;
                Player.Experience = 0x0635EFC + 8;
                Player.HealthMax = 0x0635EFC + 12;
                Player.Health = 0x0635EFC + 16;
                Player.Id = 0x0635EFC + 20;

                Player.ManaMax = 0x0635EEC;
                Player.Mana = 0x0635EEC + 4;

                Player.Cap = 0x0635EE0;

                Player.Z = 0x0645530;
                Player.Y = 0x0645530 + 4;
                Player.X = 0x0645530 + 8;

                Player.gotoX = Player.Experience + 80;
                Player.gotoY = Player.gotoX - 4;
                Player.gotoZ = Player.gotoX - 8;

                Player.pas = 0x0792E0C;
                Player.acn = 0x0792E2C;

                Player.Flags = 0x0635E98;

                Player.TargetId = 0x0635EDC;

                Player.LastLeftClickId = 0x0792EFC;
                Player.LastRightClickId = 0x0792EFC;

                Player.LastLeftClickCount = 0x0792EFC + 4;
                Player.LastRightClickCount = 0x0792EFC + 4;

                Player.SlotNeck = 0x0642BD4;
                Player.SlotRightHand = 0x0642BF8;
                Player.SlotLeftHand = 0x0642C04;
                Player.SlotAmmo = 0x0642C34;
                Player.SlotRing = 0x0642C28;

                Battlelist.Start = 0x0635F74;
                Battlelist.StepCreatures = 168; // Esqueci se é hex ou dec.
                Battlelist.MaxCreatures = 255;
                Battlelist.End = Battlelist.Start + (Battlelist.StepCreatures * Battlelist.MaxCreatures);

                Creature.DistanceId = -4;
                Creature.DistanceName = 0;
                Creature.DistanceType = -1;
                Creature.DistanceX = 32;
                Creature.DistanceY = 36;
                Creature.DistanceZ = 40;
                Creature.DistanceIsWalking = 72;
                Creature.DistanceLightSize = 116;
                Creature.DistanceLightColor = 120;
                Creature.DistanceHPBar = 132;
                Creature.DistanceIsVisible = 140;
                Creature.DistanceDirection = 76;


                Container.Start = 0x0642C40;
                Container.StepContainer = 492;
                Container.MaxStack = 100;
                Container.StepSlot = 12;
                Container.MaxContainers = 16;
                Container.DistanceAmount = 56;// -4; //
                Container.DistanceVolume = 48;
                Container.DistanceItemId = 60;
                Container.DistanceItemCount = 64;
                Container.DistanceIsOpen = 0;
                Container.DistanceName = 16;//-36;
                Container.End = Container.Start + (Container.MaxContainers * Container.StepContainer);

                Client.isFollow = 0x078FC18;
                Client.Connection = 0x0792E50;
                Client.StatusbarText = 0x07D66D8;
                Client.StatusbarTime = 0x07D66D8-4;
                Client.Ping = 0x100240F0;
                Client.LastMessageName = 0x07D6900;
                Client.LastMessageString = 0x07D6928;
                //Client.Cursor = 0x071C5E8;
                Client.Dialog = 0x0792EB0;
                Client.GuiPointer = 0x06454f8;  //Talvez seja isso mesmo...

                Client.QboxDialog = 11;

                Client.LightNop = 0x04E6119;
                Client.LightAmount = 0x04E611C;
                Client.LightNopDefault = 1406;


                Client.LevelSpy1 = 0x4EF82A;
                Client.LevelSpy2 = 0x4EF92F;
                Client.LevelSpy3 = 0x4EF9B0;
                Client.LevelSpyPtr = 0x64218C;
                Client.LevelSpyAdd2 = 0x2A88;

                byte[] levelspydefault = new byte[] { 0x89, 0x86, 0x88, 0x2A, 0x00, 0x00 };

                Client.NameSpy1 = 0x4ED979;
                Client.NameSpy2 = 0x4ED983;
                Client.NameSpy1Default = 19061;
                Client.NameSpy2Default = 16501;
                Client.LevelSpyNop = 0x4E6119;
                Client.LevelSpyAbove = Client.LevelSpyNop - 4;
                Client.LevelSpyBelow = Client.LevelSpyNop + 4;
                Client.LevelSpyNopDefault = BitConverter.ToUInt32(levelspydefault, 0);  //49451;
                Client.LevelSpyAboveDefault = 7;
                Client.LevelSpyBelowDefault = 2;
                Client.LevelSpyMin = 0;
                Client.LevelSpyMax = 7;
                Client.LevelSpyZDefault = 7;




            }
        }
    }
}

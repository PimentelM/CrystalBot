using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystalBot.Objects
{
    public class Battlelist
    {
        public IEnumerable<Creature> GetCreatures()
        {
            for (uint i = Addresses.Battlelist.Start; i < Addresses.Battlelist.End; i += Addresses.Battlelist.StepCreatures)
            {
                if (Memory.ReadByte(i + Addresses.Creature.DistanceIsVisible) == 1)
                    yield return new Creature(i);
            }
        }
    }
}

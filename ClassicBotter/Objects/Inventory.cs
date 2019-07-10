using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystalBot.Objects
{
    public class Inventory
    {

        public Container GetContainer(byte number)
        {
            if (number > 0 || number < Addresses.Container.MaxContainers)
            {
                uint i = Addresses.Container.Start + (number * Addresses.Container.StepContainer);
                if (Memory.ReadByte(i + Addresses.Container.DistanceIsOpen) == 1)
                {
                    return new Container(i, number);
                }
            }
            return null;
        }

        public IEnumerable<Container> GetContainers()
        {
            byte containerNumber = 0;
            for (uint i = Addresses.Container.Start; i < Addresses.Container.End; i += Addresses.Container.StepContainer)
            {
                if (Memory.ReadByte(i + Addresses.Container.DistanceIsOpen) == 1)
                {
                    yield return new Container(i, containerNumber);
                }
                containerNumber++;
            }
        }
    }
}

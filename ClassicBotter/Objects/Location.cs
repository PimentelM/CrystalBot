using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrystalBot
{
    public class Location
    {

        public int X, Y, Z;

        public Location() { }

        public Location(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public bool IsAdjacentTo(Location loc, int range)
        {
            if (Math.Max(Math.Abs(X - loc.X), Math.Abs(Y - loc.Y)) <= range && loc.Z == Z)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsAdjacentTo(Location loc)
        {
            if (Math.Max(Math.Abs(X - loc.X), Math.Abs(Y - loc.Y)) <= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }






        public double DistanceTo(Location l)
        {
            int xDist = X - l.X;
            int yDist = Y - l.Y;

            return Math.Sqrt(xDist * xDist + yDist * yDist);
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}, {2}", X, Y, Z);
        }
    }
}

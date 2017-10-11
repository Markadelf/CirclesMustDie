using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquaresVersusCircles
{
    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        static public Point  operator+(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }
        static public Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        public int DistanceSquared()
        {
            return X * X + Y * Y;
        }

        public Point ChangeMag(double mag)
        {
            double magFactor = mag/Math.Sqrt(DistanceSquared());
            return new Point((int)(X * magFactor), (int)(Y * magFactor));
        }
    }
}

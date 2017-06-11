using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericChess
{
    //Vector2 holds a 2d point and some common operations.
    class Vector2
    {
        public int x;
        public int y;
        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2 Delta(Vector2 other)
        {
            return new Vector2(other.x - this.x, other.y - this.y);
        }

        public bool isEqual(Vector2 other)
        {
            var delta = Delta(other);
            return (delta.x == 0 && delta.y == 0);
        }

        public Vector2 AddVector(Vector2 point)
        {
            return new Vector2(this.x + point.x, this.y + point.y);
        }
    }
}

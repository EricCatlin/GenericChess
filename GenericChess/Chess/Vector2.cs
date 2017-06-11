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

        //Gets the difference between this vector and another
        public Vector2 Delta(Vector2 other)
        {
            return new Vector2(other.x - this.x, other.y - this.y);
        }

        //Determines if 2 vectors are equivelent
        public bool isEqual(Vector2 other)
        {
            var delta = Delta(other);
            return (delta.x == 0 && delta.y == 0);
        }

        //Adds a vector to this vector and returns the result
        public Vector2 AddVector(Vector2 point)
        {
            return new Vector2(this.x + point.x, this.y + point.y);
        }

        //overloaded function to reduce complexity of lots of other functions
        public Vector2 AddVector(int x, int y)
        {
            return new Vector2(this.x + x, this.y + y);
        }
    }
}

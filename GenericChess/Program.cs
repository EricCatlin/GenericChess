using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericChess
{
    class Program
    {
        static void Main(string[] args)
        {
            var board = new Board();
            var pawn = new Pawn(new Vector2(0, 6), Color.White);
            board.pieces.Add(pawn);
            Debug.Assert(pawn.IsMoveValid(board, new Vector2(0, 4)), "Failed white pawn initial slide");
            Debug.Assert(pawn.IsMoveValid(board, new Vector2(0, 5)), "Failed white pawn initial slide");

            board.pieces.Add(new Pawn(new Vector2(0, 5), Color.White));
            Debug.Assert(!pawn.IsMoveValid(board, new Vector2(0, 4)), "Failed white pawn initial slide collision test");
        }
    }

    class Board
    {
        public List<Piece> pieces;
        public Board()
        {
            pieces = new List<Piece>();
        }
    }

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
    public enum Color
    {
        White,
        Black
    }
    interface Piece
    {
        Color color { get; set; }
        Vector2 position { get; set; }
        bool Move(Vector2 position);
    }

    class Pawn : Piece
    {

        public Pawn(Vector2 position, Color color)
        {
            this.position = position;
            this.color = color;
        }

        public Color color
        {
            get; set;
        }

        public Vector2 position { get; set; }

        public bool IsMoveValid(Board board, Vector2 end_pos)
        {
            bool valid = false;
            //Move forward
            Dictionary<Color, List<Vector2>> standard_moves = new Dictionary<Color, List<Vector2>> {
                { Color.Black,  new List<Vector2>() {new Vector2(0, 1) } },
                { Color.White,  new List<Vector2>() {new Vector2(0, -1) } },
            };
            //Slide 2 forward
            Dictionary<Color, List<List<Vector2>>> initial_slides = new Dictionary<Color, List<List<Vector2>>> {
                { Color.Black,  new List<List<Vector2>>() { new List<Vector2>() { new Vector2(0,2), new Vector2(0, 1) } } },
                { Color.White,  new List<List<Vector2>>() { new List<Vector2>() { new Vector2(0,-2), new Vector2(0,-1) } } },
            };
            //Attack Diaganal
            Dictionary<Color, List<Vector2>> standard_attacks = new Dictionary<Color, List<Vector2>> {
                { Color.Black,  new List<Vector2>() {new Vector2(1, 1), new Vector2(-1, 1) } },
                { Color.White,  new List<Vector2>() {new Vector2(1, -1), new Vector2(-1, -1) } },
            };
            //Check if peice is out of bounds
            if (end_pos.x < 0 || end_pos.x > 8 || end_pos.y < 0 || end_pos.y > 8) return false; // Off board

            //Check if requested move is a valid movement for this peice
            Vector2 delta = position.Delta(end_pos);

            //Pawn is sliding 2 spaces and pawn is in starting position
            if (delta.x == 0 && (delta.y == 2 && color == Color.Black && position.y == 1) || (delta.y == -2 && color == Color.White && position.y == 6))
            {
                var possible_slides = initial_slides[this.color].Where(x => x.First().x == delta.x && x.First().y == delta.y);
                if (possible_slides.Count() > 0) valid = true; // This move just became legal
                foreach (var path in possible_slides)
                {
                    foreach (var point in path)
                    {
                        var relative_position = position.AddVector(point);
                        var conflicts = board.pieces.Where(x => x.position.isEqual(relative_position));
                        if (conflicts.Count() > 0)
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                var possible_moves = standard_moves[color].Where(x => x.x == delta.x && x.y == delta.y);
                if (possible_moves.Count() > 0) valid = true;
                foreach (var point in possible_moves)
                    if (board.pieces.Where(x => x.position.isEqual(position.AddVector(point))).Count() > 0)
                        return false;

                var possible_attacks = standard_attacks[color].Where(x => x.x == delta.x && x.y == delta.y);
                if (possible_attacks.Count() > 0) valid = true;
                foreach (var point in possible_attacks)
                    if (board.pieces.Where(x => x.position.isEqual(position.AddVector(point)) && x.color == color).Count() > 0)
                        return false;


            }


            return valid;
        }

        public bool Move(Vector2 position)
        {
            throw new NotImplementedException();
        }
    }
}

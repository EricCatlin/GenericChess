using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericChess
{
    class Pawn : IPiece
    {
        //**KnownBug En Passant not accounted for


        public Pawn(Vector2 position, Color color)
        {
            this.position = position;
            this.color = color;
        }

        //Pawn Moves
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

        public Color color
        {
            get; set;
        }

        public Vector2 position { get; set; }

        public bool IsMoveValid(Board board, Vector2 end_pos)
        {
            bool valid = false;

            //Check if end_pos is out of bounds
            if (end_pos.x < 0 || end_pos.x > 7 || end_pos.y < 0 || end_pos.y > 7) return false; // Off board

            //Get the delta between curent position and final position
            Vector2 delta = position.Delta(end_pos);

            //Based on the delta, determine if move is legal
            //Check if pawn is sliding 2 spaces vertically and pawn is in starting position
            if (delta.x == 0 && ((delta.y == 2 && color == Color.Black && position.y == 1) || (delta.y == -2 && color == Color.White && position.y == 6)))
            {
                var possible_slides = initial_slides[this.color].Where(x => x.First().x == delta.x && x.First().y == delta.y);
                if (possible_slides.Count() > 0) valid = true;
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
                //Check if this delta conforms to a standard move
                var possible_moves = standard_moves[color].Where(x => x.x == delta.x && x.y == delta.y);
                if (possible_moves.Count() > 0) valid = true;
                foreach (var point in possible_moves)
                    if (board.pieces.Where(x => x.position.isEqual(position.AddVector(point))).Count() > 0)
                        return false;

                //Check if this delta conforms to a standard attack
                var possible_attacks = standard_attacks[color].Where(x => x.x == delta.x && x.y == delta.y);
                if (possible_attacks.Count() > 0) valid = true;
                //Check if there are enemies present at the attack location
                foreach (var point in possible_attacks)
                    if (board.pieces.Where(x => x.position.isEqual(position.AddVector(point)) && x.color == color).Count() > 0)
                        return false;
            }


            //Check if king would be attacking or just moving
            var end_point_piece = board.GetPieceAt(end_pos);
            if (end_point_piece != null && end_point_piece.color == this.color) return false;

            //Check if final position puts King in check
            //Temp set piece location to end_pos
            if (end_point_piece != null)
            {
                board.pieces.Remove(end_point_piece);
            }
            var real_pos = this.position;
            this.position = end_pos;
            if (valid && color == Color.White && board.WhiteKing != null) valid = board.WhiteKing.SafetyCheck(board);
            if (valid && color == Color.Black && board.BlackKing != null) valid = board.BlackKing.SafetyCheck(board);

            this.position = real_pos;
            if (end_point_piece != null) board.pieces.Add(end_point_piece);

            return valid;
        }
    }
}

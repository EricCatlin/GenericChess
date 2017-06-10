using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericChess
{
    class King : IPiece
    {
        public King(Vector2 position, Color color)
        {
            this.position = position;
            this.color = color;
        }

        public Color color { get; set; }
        public Vector2 position { get; set; }
        public bool SafetyCheck(Board board)
        {
            //IsInCheck
            var opponents = board.pieces.Where(x => x.color != this.color);
            for (var i = opponents.Count()-1; i >= 0; i--)
            {
                var piece = opponents.ElementAt(i);
                if (piece.IsMoveValid(board, position)) return false;
            }
            return true;
        }
        public bool IsMoveValid(Board board, Vector2 end_pos)
        {
            bool valid = true;

            //Check if end_pos is out of bounds
            if (end_pos.x < 0 || end_pos.x > 7 || end_pos.y < 0 || end_pos.y > 7) return false; // Off board

            //Get the delta between curent position and final position
            Vector2 delta = position.Delta(end_pos);
            if (delta.x == 0 && delta.y == 0) return false; //return if stationary
            if (Math.Abs(delta.x) > 1 || Math.Abs(delta.y) > 1) return false; // Too far to move

            //Check if king would be attacking or just moving
            var end_point_piece = board.GetPieceAt(end_pos);
            if (end_point_piece != null && end_point_piece.color == this.color) return false;

            //Check if final position puts King in check
            //Temp set piece location to end_pos
            if(end_point_piece != null)
            {
                board.pieces.Remove(end_point_piece);
            }
            var real_pos = this.position;
            this.position = end_pos;

            valid = SafetyCheck(board);

            this.position = real_pos;
            if(end_point_piece != null) board.pieces.Add(end_point_piece);


            return valid;
        }
    }
}

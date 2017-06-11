using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericChess
{
    class Rook : IPiece
    {
        public Color color { get; set; }
        public Vector2 position { get; set; }
        public bool hasMoved { get; set; }
        public bool isCastling { get; set; }
        public bool enPassantable { get; set; }

        public Rook(Vector2 position, Color color)
        {
            this.position = position;
            this.color = color;
        }

        public bool IsMoveValid(Board board, Vector2 end_pos)
        {
            bool valid = false;

            //Check if end_pos is out of bounds
            if (end_pos.x < 0 || end_pos.x > 7 || end_pos.y < 0 || end_pos.y > 7) return false; // Off board

            //Get the delta between curent position and final position
            Vector2 delta = position.Delta(end_pos);

            //Diagonal/Stationary check
            if ((delta.x != 0 && delta.y != 0) || (delta.x == 0 && delta.y == 0)) return false;

            //Check if final position is valid
            var end_point_piece = board.GetPieceAt(end_pos);
            if (end_point_piece == null || end_point_piece.color != this.color)
            {
                valid = true;
                //Move x towards 0
                delta.x += (delta.x > 0) ? -1 : (delta.x < 0) ? 1 : 0;
                //Move y towards 0
                delta.y += (delta.y > 0) ? -1 : (delta.y < 0) ? 1 : 0;

                //Check if path is clear to arrive at valid end-point
                while (delta.x != 0 || delta.y != 0)
                {
                    //Check for collision
                    var collision_check = board.GetPieceAt(position.AddVector(delta));
                    if (collision_check != null) return false;
                    //Move x towards 0
                    delta.x += (delta.x > 0) ? -1 : (delta.x < 0) ? 1 : 0;
                    //Move y towards 0
                    delta.y += (delta.y > 0) ? -1 : (delta.y < 0) ? 1 : 0;

                }
            }

            //Check if king would be attacking or just moving
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

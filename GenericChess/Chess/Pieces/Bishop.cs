using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericChess
{
    class Bishop : IPiece
    {
        public Color Color { get; set; }
        public Vector2 Position { get; set; }
        public bool HasMoved { get; set; }
        public bool IsCastling { get; set; }

        public Bishop(Vector2 position, Color color)
        {
            this.Position = position;
            this.Color = color;
        }

        public bool IsMoveValid(Board board, Vector2 endPosition)
        {
            bool valid = false;

            //Check if endPosition is out of bounds
            if (endPosition.x < 0 || endPosition.x > 7 || endPosition.y < 0 || endPosition.y > 7) return false; // Off board

            //Get the delta between curent position and final position
            Vector2 delta = Position.Delta(endPosition);

            //Stationary check
            if (delta.x == 0 && delta.y == 0) return false;
            if (!(delta.x == delta.y || delta.x == -delta.y)) return false; //Not diagonal

            //Check if final position is valid
            var pieceAtDestination = board.GetPieceAt(endPosition);
            if (pieceAtDestination == null || pieceAtDestination.Color != this.Color)
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
                    var collisionCheck = board.GetPieceAt(Position.AddVector(delta));
                    if (collisionCheck != null) return false;
                    //Move x towards 0
                    delta.x += (delta.x > 0) ? -1 : (delta.x < 0) ? 1 : 0;
                    //Move y towards 0
                    delta.y += (delta.y > 0) ? -1 : (delta.y < 0) ? 1 : 0;
                }
            }

            //Check if king would be attacking or just moving
            if (pieceAtDestination != null && pieceAtDestination.Color == this.Color) return false;

            //Check if final position puts King in check
            //Temp set piece location to endPosition
            if (pieceAtDestination != null)
            {
                board.Pieces.Remove(pieceAtDestination);
            }
            var originalPosition = this.Position;
            this.Position = endPosition;
            if (valid && Color == Color.White && board.WhiteKing != null) valid = board.WhiteKing.SafetyCheck(board);
            if (valid && Color == Color.Black && board.BlackKing != null) valid = board.BlackKing.SafetyCheck(board);

            this.Position = originalPosition;
            if (pieceAtDestination != null) board.Pieces.Add(pieceAtDestination);

            return valid;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericChess
{
    class Knight : IPiece
    {
        public Color Color { get; set; }
        public Vector2 Position { get; set; }
        public bool HasMoved { get; set; }
        public bool IsCastling { get; set; }

        public Knight(Vector2 position, Color color)
        {
            this.Position = position;
            this.Color = color;
        }

        //Knights hop to one of 8 possible possitions
        List<Vector2> StandardMoves = new List<Vector2>(){
            new Vector2(1, 2), //Up right
            new Vector2(-1, 2), //Up left
            new Vector2(1, -2),//Down right
            new Vector2(-1, -2),//Down left

            new Vector2(2, 1), //Right up
            new Vector2(2, -1), //Right down
            new Vector2(-2, -1), // Left down
            new Vector2(-2, 1) // Left up
        };

        public bool IsMoveValid(Board board, Vector2 endPosition)
        {
            bool valid = false;

            //Check if endPosition is out of bounds
            if (endPosition.x < 0 || endPosition.x > 7 || endPosition.y < 0 || endPosition.y > 7) return false; // Off board

            //Get the delta between curent position and final position
            Vector2 delta = Position.Delta(endPosition);

            //Check if this delta conforms to a standard move
            var possibleMoves = StandardMoves.Where(x => x.x == delta.x && x.y == delta.y);
            if (possibleMoves.Count() > 0) valid = true;
            foreach (var point in possibleMoves)
            {
                var collision = board.Pieces.Where(x => x.Position.isEqual(Position.AddVector(point)));
                if (collision.Count() > 0)
                {
                    if (collision.First().Color == this.Color) return false;
                }
            }

            //Check if king would be attacking or just moving
            var pieceAtDestination = board.GetPieceAt(endPosition);
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

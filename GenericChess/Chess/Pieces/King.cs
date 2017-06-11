using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericChess
{
    class King : IPiece
    {
        public Color Color { get; set; }
        public Vector2 Position { get; set; }
        public bool HasMoved { get; set; }
        public bool IsCastling { get; set; }

        public King(Vector2 position, Color color)
        {
            this.Position = position;
            this.Color = color;
        }

        public bool SafetyCheck(Board board)
        {
            //Loops over all opponent pieces, checking if they have a check on this King
            var opponents = board.Pieces.Where(x => x.Color != this.Color);
            for (var i = opponents.Count() - 1; i >= 0; i--)
            {
                var piece = opponents.ElementAt(i);
                if (piece.IsMoveValid(board, Position)) return false;
            }
            return true;
        }

        public bool IsMoveValid(Board board, Vector2 endPosition)
        {
            bool valid = true;
            IsCastling = false;

            //Check if endPosition is out of bounds
            if (endPosition.x < 0 || endPosition.x > 7 || endPosition.y < 0 || endPosition.y > 7) return false; // Off board

            //Get the delta between curent position and final position
            Vector2 delta = Position.Delta(endPosition);
            if (delta.x == 0 && delta.y == 0) return false; //return if stationary
            if (Math.Abs(delta.y) > 1) return false; // Too far to move

            //Checks for castling. This should not be in this project, but I wanted it.
            //Castling requires prior knowledge about the board state since it only works if neither the castle nor the king have moved previously
            if (!HasMoved && Math.Abs(delta.x) == 2)
            {
                //Attempting to castle?
                if (delta.x < 0)
                {
                    //Queen castle
                    //Get Queen Rook
                    var R0 = board.GetPieceAt(new Vector2(0, Position.y));
                    if (R0 == null || R0.HasMoved) return false;
                    //Check all spaces between 
                    if (board.GetPieceAt(new Vector2(1, Position.y)) != null) return false;
                    if (board.GetPieceAt(new Vector2(2, Position.y)) != null) return false;
                    if (board.GetPieceAt(new Vector2(3, Position.y)) != null) return false;

                    //Loops over all opponent pieces, checking if they have a check on this king, or any of the spaces it will occupy during the move
                    var opponents = board.Pieces.Where(x => x.Color != this.Color);
                    for (var i = opponents.Count() - 1; i >= 0; i--)
                    {
                        var piece = opponents.ElementAt(i);
                        if (piece.IsMoveValid(board, Position)) return false;
                        if (piece.IsMoveValid(board, Position.AddVector(-1, 0))) return false;
                        if (piece.IsMoveValid(board, Position.AddVector(-2, 0))) return false;
                    }
                    //R0.position = new Vector2(3, position.y);
                    //position = new Vector2(2, position.y);
                    IsCastling = true;
                    return true;
                }
                else
                {
                    //King castle
                    var R1 = board.GetPieceAt(new Vector2(7, Position.y));
                    if (R1 == null || R1.HasMoved) return false;

                    //Check all spaces between 
                    if (board.GetPieceAt(new Vector2(5, Position.y)) != null) return false;
                    if (board.GetPieceAt(new Vector2(6, Position.y)) != null) return false;

                    //Loops over all opponent pieces, checking if they have a check on this king, or any of the spaces it will occupy during the move
                    var opponents = board.Pieces.Where(x => x.Color != this.Color);
                    for (var i = opponents.Count() - 1; i >= 0; i--)
                    {
                        var piece = opponents.ElementAt(i);
                        if (piece.IsMoveValid(board, Position)) return false;
                        if (piece.IsMoveValid(board, Position.AddVector(1, 0))) return false;
                        if (piece.IsMoveValid(board, Position.AddVector(2, 0))) return false;
                    }
                    //R0.position = new Vector2(3, position.y);
                    //position = new Vector2(2, position.y);
                    IsCastling = true;
                    return true;
                }
            }
            else if (Math.Abs(delta.x) > 1) return false;

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
            Position = endPosition;

            valid = SafetyCheck(board);

            Position = originalPosition;
            if (pieceAtDestination != null) board.Pieces.Add(pieceAtDestination);


            return valid;
        }
    }
}

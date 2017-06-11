using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericChess
{
    class King : IPiece
    {
        public Color color { get; set; }
        public Vector2 position { get; set; }
        public bool hasMoved { get; set; }
        public bool isCastling { get; set; }
        public bool enPassantable { get; set; }

        public King(Vector2 position, Color color)
        {
            this.position = position;
            this.color = color;
        }

        public bool SafetyCheck(Board board)
        {
            //Loops over all opponent pieces, checking if they have a check on this King
            var opponents = board.pieces.Where(x => x.color != this.color);
            for (var i = opponents.Count() - 1; i >= 0; i--)
            {
                var piece = opponents.ElementAt(i);
                if (piece.IsMoveValid(board, position)) return false;
            }
            return true;
        }

        public bool IsMoveValid(Board board, Vector2 end_pos)
        {
            bool valid = true;
            isCastling = false;

            //Check if end_pos is out of bounds
            if (end_pos.x < 0 || end_pos.x > 7 || end_pos.y < 0 || end_pos.y > 7) return false; // Off board

            //Get the delta between curent position and final position
            Vector2 delta = position.Delta(end_pos);
            if (delta.x == 0 && delta.y == 0) return false; //return if stationary
            if (Math.Abs(delta.y) > 1) return false; // Too far to move

            //Checks for castling. This should not be in this project, but I wanted it.
            //Castling requires prior knowledge about the board state since it only works if neither the castle nor the king have moved previously
            if (!hasMoved && Math.Abs(delta.x) == 2)
            {
                //Attempting to castle?
                if (delta.x < 0)
                {
                    //Queen castle
                    //Get Queen Rook
                    var R0 = board.GetPieceAt(new Vector2(0, position.y));
                    if (R0 == null || R0.hasMoved) return false;
                    //Check all spaces between 
                    if (board.GetPieceAt(new Vector2(1, position.y)) != null) return false;
                    if (board.GetPieceAt(new Vector2(2, position.y)) != null) return false;
                    if (board.GetPieceAt(new Vector2(3, position.y)) != null) return false;

                    //Loops over all opponent pieces, checking if they have a check on this king, or any of the spaces it will occupy during the move
                    var opponents = board.pieces.Where(x => x.color != this.color);
                    for (var i = opponents.Count() - 1; i >= 0; i--)
                    {
                        var piece = opponents.ElementAt(i);
                        if (piece.IsMoveValid(board, position)) return false;
                        if (piece.IsMoveValid(board, position.AddVector(-1, 0))) return false;
                        if (piece.IsMoveValid(board, position.AddVector(-2, 0))) return false;
                    }
                    //R0.position = new Vector2(3, position.y);
                    //position = new Vector2(2, position.y);
                    isCastling = true;
                    return true;
                }
                else
                {
                    //King castle
                    var R1 = board.GetPieceAt(new Vector2(7, position.y));
                    if (R1 == null || R1.hasMoved) return false;

                    //Check all spaces between 
                    if (board.GetPieceAt(new Vector2(5, position.y)) != null) return false;
                    if (board.GetPieceAt(new Vector2(6, position.y)) != null) return false;

                    //Loops over all opponent pieces, checking if they have a check on this king, or any of the spaces it will occupy during the move
                    var opponents = board.pieces.Where(x => x.color != this.color);
                    for (var i = opponents.Count() - 1; i >= 0; i--)
                    {
                        var piece = opponents.ElementAt(i);
                        if (piece.IsMoveValid(board, position)) return false;
                        if (piece.IsMoveValid(board, position.AddVector(1, 0))) return false;
                        if (piece.IsMoveValid(board, position.AddVector(2, 0))) return false;
                    }
                    //R0.position = new Vector2(3, position.y);
                    //position = new Vector2(2, position.y);
                    isCastling = true;
                    return true;
                }
            }
            else if (Math.Abs(delta.x) > 1) return false;

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
            position = end_pos;

            valid = SafetyCheck(board);

            position = real_pos;
            if (end_point_piece != null) board.pieces.Add(end_point_piece);


            return valid;
        }
    }
}

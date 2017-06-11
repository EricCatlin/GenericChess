using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericChess
{
    class Board
    {
        public List<IPiece> pieces;
        public King BlackKing;
        public King WhiteKing;

        public Board()
        {
            pieces = new List<IPiece>();
        }

        public bool IsMoveValid(IPiece piece, Vector2 end_pos)
        {
            return piece.IsMoveValid(this, end_pos);
        }

        public IPiece GetPieceAt(Vector2 position)
        {
            var piece = pieces.Where(x => x.position.x == position.x && x.position.y == position.y);
            if (piece.Count() > 0)
                return piece.First();
            else return null;
        }

        public bool MovePiece(IPiece piece, Vector2 end_pos)
        {
            if (piece.IsMoveValid(this, end_pos))
            {
                var opponent = GetPieceAt(end_pos);
                if (opponent != null)
                    pieces.Remove(opponent);
                piece.position = end_pos;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

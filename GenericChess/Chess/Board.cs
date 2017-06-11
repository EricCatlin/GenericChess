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
        public Dictionary<string, IPiece> piece_index;
        public King BlackKing;
        public King WhiteKing;

        public Board(bool init = false)
        {
            pieces = new List<IPiece>();
            if (init) SetRegulationBoard();
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
                piece.hasMoved = true;
                var opponent = GetPieceAt(end_pos);
                if (opponent != null)
                    pieces.Remove(opponent);
                piece.position = end_pos;

                if (piece.isCastling)
                {
                    if (end_pos.x == 2)
                    {
                        //Queen Castling
                        GetPieceAt(new Vector2(0, piece.position.y)).position = new Vector2(3, piece.position.y);
                    }
                    else
                    {
                        //King Castling
                        GetPieceAt(new Vector2(7, piece.position.y)).position = new Vector2(5, piece.position.y);
                    }
                    piece.isCastling = false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SetRegulationBoard()
        {
            piece_index = new Dictionary<string, IPiece>();
            IPiece PW0 = new Pawn(new Vector2(0, 6), Color.White),
            PW1 = new Pawn(new Vector2(1, 6), Color.White),
            PW2 = new Pawn(new Vector2(2, 6), Color.White),
            PW3 = new Pawn(new Vector2(3, 6), Color.White),
            PW4 = new Pawn(new Vector2(4, 6), Color.White),
            PW5 = new Pawn(new Vector2(5, 6), Color.White),
            PW6 = new Pawn(new Vector2(6, 6), Color.White),
            PW7 = new Pawn(new Vector2(7, 6), Color.White),
            PB0 = new Pawn(new Vector2(0, 1), Color.Black),
            PB1 = new Pawn(new Vector2(1, 1), Color.Black),
            PB2 = new Pawn(new Vector2(2, 1), Color.Black),
            PB3 = new Pawn(new Vector2(3, 1), Color.Black),
            PB4 = new Pawn(new Vector2(4, 1), Color.Black),
            PB5 = new Pawn(new Vector2(5, 1), Color.Black),
            PB6 = new Pawn(new Vector2(6, 1), Color.Black),
            PB7 = new Pawn(new Vector2(7, 1), Color.Black),

            KW0 = new Knight(new Vector2(1, 7), Color.White),
            KW1 = new Knight(new Vector2(6, 7), Color.White),
            KB0 = new Knight(new Vector2(1, 0), Color.Black),
            KB1 = new Knight(new Vector2(6, 0), Color.Black),

            QW = new Queen(new Vector2(3, 7), Color.White),
            QB = new Queen(new Vector2(3, 0), Color.Black),

            BW0 = new Bishop(new Vector2(2, 7), Color.White),
            BW1 = new Bishop(new Vector2(5, 7), Color.White),
            BB0 = new Bishop(new Vector2(2, 0), Color.Black),
            BB1 = new Bishop(new Vector2(5, 0), Color.Black),

            KW = new King(new Vector2(4, 7), Color.White),
            KB = new King(new Vector2(4, 0), Color.Black);
            piece_index.Add("PW0", PW0);
            piece_index.Add("PW1", PW1);
            piece_index.Add("PW2", PW2);
            piece_index.Add("PW3", PW3);
            piece_index.Add("PW4", PW4);
            piece_index.Add("PW5", PW5);
            piece_index.Add("PW6", PW6);
            piece_index.Add("PW7", PW7);
            piece_index.Add("PB0", PB0);
            piece_index.Add("PB1", PB1);
            piece_index.Add("PB2", PB2);
            piece_index.Add("PB3", PB3);
            piece_index.Add("PB4", PB4);
            piece_index.Add("PB5", PB5);
            piece_index.Add("PB6", PB6);
            piece_index.Add("PB7", PB7);
            piece_index.Add("KW0", KW0);
            piece_index.Add("KW1", KW1);
            piece_index.Add("KB0", KB0);
            piece_index.Add("KB1", KB1);
            piece_index.Add("QW", QW);
            piece_index.Add("QB", QB);
            piece_index.Add("BW0", BW0);
            piece_index.Add("BW1", BW1);
            piece_index.Add("BB0", BB0);
            piece_index.Add("BB1", BB1);
            piece_index.Add("KW", KW);
            piece_index.Add("KB", KB);

            pieces.Add(PW0);
            pieces.Add(PW1);
            pieces.Add(PW2);
            pieces.Add(PW3);
            pieces.Add(PW4);
            pieces.Add(PW5);
            pieces.Add(PW6);
            pieces.Add(PW7);
            pieces.Add(PB0);
            pieces.Add(PB1);
            pieces.Add(PB2);
            pieces.Add(PB3);
            pieces.Add(PB4);
            pieces.Add(PB5);
            pieces.Add(PB6);
            pieces.Add(PB7);
            pieces.Add(KW0);
            pieces.Add(KW1);
            pieces.Add(KB0);
            pieces.Add(KB1);
            pieces.Add(QW);
            pieces.Add(QB);
            pieces.Add(BW0);
            pieces.Add(BW1);
            pieces.Add(BB0);
            pieces.Add(BB1);
            pieces.Add(KW);
            pieces.Add(KB);

            WhiteKing = (King)KW;
            BlackKing = (King)KB;
            //end Board Setup
        }
    }
}

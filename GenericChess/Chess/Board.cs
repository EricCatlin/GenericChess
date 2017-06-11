using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericChess
{
    class Board
    {
        public List<IPiece> Pieces;
        public Dictionary<string, IPiece> PieceIndex;
        public King BlackKing;
        public King WhiteKing;

        public Board(bool init = false)
        {
            Pieces = new List<IPiece>();
            if (init) SetRegulationBoard();
        }

        public bool IsMoveValid(IPiece piece, Vector2 endPosition)
        {
            return piece.IsMoveValid(this, endPosition);
        }

        public IPiece GetPieceAt(Vector2 position)
        {
            var piece = Pieces.Where(x => x.Position.x == position.x && x.Position.y == position.y);
            if (piece.Count() > 0)
                return piece.First();
            else return null;
        }

        public bool MovePiece(IPiece piece, Vector2 endPosition)
        {
            if (piece.IsMoveValid(this, endPosition))
            {
                piece.HasMoved = true;
                var opponent = GetPieceAt(endPosition);
                if (opponent != null)
                    Pieces.Remove(opponent);
                piece.Position = endPosition;

                if (piece.IsCastling)
                {
                    if (endPosition.x == 2)
                    {
                        //Queen Castling
                        GetPieceAt(new Vector2(0, piece.Position.y)).Position = new Vector2(3, piece.Position.y);
                    }
                    else
                    {
                        //King Castling
                        GetPieceAt(new Vector2(7, piece.Position.y)).Position = new Vector2(5, piece.Position.y);
                    }
                    piece.IsCastling = false;
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
            PieceIndex = new Dictionary<string, IPiece>();
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
            PieceIndex.Add("PW0", PW0);
            PieceIndex.Add("PW1", PW1);
            PieceIndex.Add("PW2", PW2);
            PieceIndex.Add("PW3", PW3);
            PieceIndex.Add("PW4", PW4);
            PieceIndex.Add("PW5", PW5);
            PieceIndex.Add("PW6", PW6);
            PieceIndex.Add("PW7", PW7);
            PieceIndex.Add("PB0", PB0);
            PieceIndex.Add("PB1", PB1);
            PieceIndex.Add("PB2", PB2);
            PieceIndex.Add("PB3", PB3);
            PieceIndex.Add("PB4", PB4);
            PieceIndex.Add("PB5", PB5);
            PieceIndex.Add("PB6", PB6);
            PieceIndex.Add("PB7", PB7);
            PieceIndex.Add("KW0", KW0);
            PieceIndex.Add("KW1", KW1);
            PieceIndex.Add("KB0", KB0);
            PieceIndex.Add("KB1", KB1);
            PieceIndex.Add("QW", QW);
            PieceIndex.Add("QB", QB);
            PieceIndex.Add("BW0", BW0);
            PieceIndex.Add("BW1", BW1);
            PieceIndex.Add("BB0", BB0);
            PieceIndex.Add("BB1", BB1);
            PieceIndex.Add("KW", KW);
            PieceIndex.Add("KB", KB);

            Pieces.Add(PW0);
            Pieces.Add(PW1);
            Pieces.Add(PW2);
            Pieces.Add(PW3);
            Pieces.Add(PW4);
            Pieces.Add(PW5);
            Pieces.Add(PW6);
            Pieces.Add(PW7);
            Pieces.Add(PB0);
            Pieces.Add(PB1);
            Pieces.Add(PB2);
            Pieces.Add(PB3);
            Pieces.Add(PB4);
            Pieces.Add(PB5);
            Pieces.Add(PB6);
            Pieces.Add(PB7);
            Pieces.Add(KW0);
            Pieces.Add(KW1);
            Pieces.Add(KB0);
            Pieces.Add(KB1);
            Pieces.Add(QW);
            Pieces.Add(QB);
            Pieces.Add(BW0);
            Pieces.Add(BW1);
            Pieces.Add(BB0);
            Pieces.Add(BB1);
            Pieces.Add(KW);
            Pieces.Add(KB);

            WhiteKing = (King)KW;
            BlackKing = (King)KB;
            //end Board Setup
        }
    }
}

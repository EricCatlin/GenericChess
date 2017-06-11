using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericChess
{
    //Pawn was the first piece implemented. I used pre-built lists of possible moves including sliding positions for first move. 
    //If I were to redo it I would just dynamicly determine validity the way I did in Rook/King classes since it reduces complexity
    class Pawn : IPiece
    {
        //**KnownBug En Passant not accounted for

        public Color Color { get; set; }
        public Vector2 Position { get; set; }
        public bool HasMoved { get; set; }
        public bool IsCastling { get; set; }

        public Pawn(Vector2 position, Color color)
        {
            this.Position = position;
            this.Color = color;
        }

        //Pawn Moves
        Dictionary<Color, List<Vector2>> StandardMoves = new Dictionary<Color, List<Vector2>> {
                { Color.Black,  new List<Vector2>() {new Vector2(0, 1) } },
                { Color.White,  new List<Vector2>() {new Vector2(0, -1) } },
            };
        //Slide 2 forward
        Dictionary<Color, List<List<Vector2>>> InitialSlides = new Dictionary<Color, List<List<Vector2>>> {
                { Color.Black,  new List<List<Vector2>>() { new List<Vector2>() { new Vector2(0,2), new Vector2(0, 1) } } },
                { Color.White,  new List<List<Vector2>>() { new List<Vector2>() { new Vector2(0,-2), new Vector2(0,-1) } } },
            };
        //Attack Diaganal
        Dictionary<Color, List<Vector2>> StandardAttacks = new Dictionary<Color, List<Vector2>> {
                { Color.Black,  new List<Vector2>() {new Vector2(1, 1), new Vector2(-1, 1) } },
                { Color.White,  new List<Vector2>() {new Vector2(1, -1), new Vector2(-1, -1) } },
            };

        public bool IsMoveValid(Board board, Vector2 endPosition)
        {
            bool valid = false;

            //Check if endPosition is out of bounds
            if (endPosition.x < 0 || endPosition.x > 7 || endPosition.y < 0 || endPosition.y > 7) return false; // Off board

            //Get the delta between curent position and final position
            Vector2 delta = Position.Delta(endPosition);

            //Based on the delta, determine if move is legal
            //Check if pawn is sliding 2 spaces vertically and pawn is in starting position
            if (delta.x == 0 && ((delta.y == 2 && Color == Color.Black && Position.y == 1) || (delta.y == -2 && Color == Color.White && Position.y == 6)))
            {
                var possibleSlides = InitialSlides[Color].Where(x => x.First().x == delta.x && x.First().y == delta.y);
                if (possibleSlides.Count() > 0) valid = true;
                foreach (var path in possibleSlides)
                {
                    foreach (var point in path)
                    {
                        var relativePosition = Position.AddVector(point);
                        var conflicts = board.Pieces.Where(x => x.Position.isEqual(relativePosition));
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
                var possibleMoves = StandardMoves[Color].Where(x => x.x == delta.x && x.y == delta.y);
                if (possibleMoves.Count() > 0) valid = true;
                foreach (var point in possibleMoves)
                    if (board.Pieces.Where(x => x.Position.isEqual(Position.AddVector(point))).Count() > 0)
                        return false;

                //Check if this delta conforms to a standard attack
                var possibleAttacks = StandardAttacks[Color].Where(x => x.x == delta.x && x.y == delta.y);
                if (possibleAttacks.Count() > 0) valid = true;
                //Check if there are enemies present at the attack location
                foreach (var point in possibleAttacks)
                    if (board.Pieces.Where(x => x.Position.isEqual(Position.AddVector(point)) && x.Color == Color).Count() > 0)
                        return false;
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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericChess
{
    class Program
    {

        //Author:Eric Catlin, June 2017
        //This Chess Program can validate board moves against given board states
        //This app written in under 24 hours without borrowing 1 single line from stackoverflow, though I did consult google for chess rules.

        //The Main function runs tests for each piece type, as well as simulates 1 complete game of chess, validating and carrying out each move.
        static void Main(string[] args)
        {
            //Run some tests
            Tests.TestPawns();
            Tests.TestKnights();
            Tests.TestCastles();
            Tests.TestBishops();
            Tests.TestQueen();
            Tests.TestKing();
            Tests.KingCastling();
            Tests.PlayGame();

            //A small demo 
            RunExample();
        }

        private static void RunExample()
        {
            Debug.WriteLine("Running Example");

            //Create the board, init sets the board using regulation piece placements
            Board board = new Board(init: true);

            //Get reference to piece PW0 (Pawn White 0), the leftmost white pawn
            IPiece PW0 = board.PieceIndex["PW0"];

            //Check if moving down 2 rows is valid
            var isValid = board.IsMoveValid(PW0, PW0.Position.AddVector(0, -2));
            Debug.WriteLine("Relative Position Check: " + isValid);

            //Check if moving to an absolute position is valid
            var isValidAlternate = board.IsMoveValid(PW0, new Vector2(0, 4));
            Debug.WriteLine("Absolute Position Check: " + isValidAlternate);

            //Attempt to apply the move action, validates internally and returns wether the move occured
            var moved = board.MovePiece(PW0, PW0.Position.AddVector(0, -2));

            //Debug out if the move did not leave the piece where expected
            Debug.WriteLineIf(PW0.Position.isEqual(new Vector2(0, 4)), "Piece moved successfully");
        }
    }
}

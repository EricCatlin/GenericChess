using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericChess
{
    class Tests
    {

        public static void TestQueen()
        {
            var board = new Board();
            var QW = new Queen(new Vector2(4, 7), Color.White);
            var QB = new Queen(new Vector2(4, 0), Color.Black);

            board.pieces.Add(QW);
            board.pieces.Add(QB);

            Debug.Assert(QW.IsMoveValid(board, QW.position.AddVector(new Vector2(0, -6))), "Vertical slide failed");
            Debug.Assert(QW.IsMoveValid(board, QW.position.AddVector(new Vector2(0, -7))), "Vertical attack failed");

            var blocking_pawn = new Pawn(new Vector2(4, 5), Color.White);

            Debug.Assert(QW.IsMoveValid(board, new Vector2(4, 5)), "Vertical slide failed");
            board.pieces.Add(blocking_pawn);
            Debug.Assert(!QW.IsMoveValid(board, new Vector2(4, 5)), "Blocked Vertical slide failed");
            Debug.Assert(!QW.IsMoveValid(board, QW.position.AddVector(new Vector2(0, -7))), "Blocked Vertical attack failed");

            blocking_pawn.color = Color.Black;
            Debug.Assert(QW.IsMoveValid(board, new Vector2(4, 5)), "Vertical Attack failed");
            Debug.Assert(!QW.IsMoveValid(board, QW.position.AddVector(new Vector2(0, -7))), "Blocked Vertical attack failed");
        }

        public static void TestRooks()
        {
            var board = new Board();
            //Set them up in standard positions
            Rook RW0 = new Rook(new Vector2(2, 7), Color.White),
            RW1 = new Rook(new Vector2(5, 7), Color.White),
            RB0 = new Rook(new Vector2(2, 0), Color.Black),
            RB1 = new Rook(new Vector2(5, 0), Color.Black);

            board.pieces.Add(RW0);
            board.pieces.Add(RW1);
            board.pieces.Add(RB0);
            board.pieces.Add(RB1);

            //Test standard move
            Debug.Assert(RW0.IsMoveValid(board, RW0.position.AddVector(new Vector2(-1, -1))), "Diagonal slide failed");
            Debug.Assert(RW0.IsMoveValid(board, RW0.position.AddVector(new Vector2(-2, -2))), "Diagonal slide failed");
            Debug.Assert(!RW0.IsMoveValid(board, RW0.position.AddVector(new Vector2(-1, -2))), "Diagonal slide failed");
            Debug.Assert(!RW0.IsMoveValid(board, RW0.position.AddVector(new Vector2(-2, -1))), "Diagonal slide failed");

            var PB1 = new Pawn(RW0.position.AddVector(new Vector2(-2, -2)), Color.Black); //Create new enemy pawn diagonal to RW0
            board.pieces.Add(PB1);
            //Attack it
            Debug.Assert(RW0.IsMoveValid(board, RW0.position.AddVector(new Vector2(-2, -2))), "Diagonal attack failed"); //Direct attack should pass
            Debug.Assert(!RW0.IsMoveValid(board, RW0.position.AddVector(new Vector2(-3, -3))), "Diagonal attack failed"); //Enemy in path should fail



        }

       

        public static void TestCastles()
        {
            var board = new Board();
            //Set them up in standard positions
            var CW0 = new Castle(new Vector2(0, 7), Color.White);
            var CW1 = new Castle(new Vector2(7, 7), Color.White);

            var CB0 = new Castle(new Vector2(0, 0), Color.Black);
            var CB1 = new Castle(new Vector2(7, 0), Color.Black);

            board.pieces.Add(CW0);
            board.pieces.Add(CW1);
            board.pieces.Add(CB0);
            board.pieces.Add(CB1);

            //Test standard move
            Debug.Assert(CW0.IsMoveValid(board, new Vector2(0, 1)), "Vertical slide failed");
            Debug.Assert(CW0.IsMoveValid(board, new Vector2(6, 7)), "Horizontal slide failed");

            Debug.Assert(CW0.IsMoveValid(board, new Vector2(0, 0)), "Attacking enemy failed");
            Debug.Assert(!CW0.IsMoveValid(board, new Vector2(7, 7)), "Attacking team failed");

            Debug.Assert(!CW0.IsMoveValid(board, new Vector2(3, 5)), "Only straight movements are allowed");

        }



        public static void TestKnights()
        {
            var board = new Board();
            //Set them up in standard positions
            var KW0 = new Knight(new Vector2(1, 7), Color.White);
            var KW1 = new Knight(new Vector2(6, 7), Color.White);

            var KB0 = new Knight(new Vector2(1, 0), Color.Black);
            var KB1 = new Knight(new Vector2(6, 0), Color.Black);

            board.pieces.Add(KW0);
            board.pieces.Add(KW1);
            board.pieces.Add(KB0);
            board.pieces.Add(KB1);

            //Test standard move
            Debug.Assert(KW0.IsMoveValid(board, new Vector2(0, 5)), "Standard move failed");
            Debug.Assert(KW0.IsMoveValid(board, new Vector2(2, 5)), "Standard move failed");
            Debug.Assert(KW0.IsMoveValid(board, new Vector2(3, 6)), "Standard move failed");

            Debug.Assert(!KW0.IsMoveValid(board, new Vector2(2, 9)), "Out of bounds not caught");

            //Add a test pawn
            var PW3 = new Pawn(new Vector2(3, 6), Color.White);
            board.pieces.Add(PW3);
            //Check friendly collision
            Debug.Assert(!KW0.IsMoveValid(board, new Vector2(3, 6)), "Collision test failed");
            //Turn pawn into enemy
            PW3.color = Color.Black;
            //Test attack
            Debug.Assert(KW0.IsMoveValid(board, new Vector2(3, 6)), "Attack test failed");

        }

        public static void TestPawns()
        {
            var board = new Board();
            var PW0 = new Pawn(new Vector2(0, 6), Color.White);
            var PW1 = new Pawn(new Vector2(1, 6), Color.White);
            var PW2 = new Pawn(new Vector2(2, 6), Color.White);
            var PW3 = new Pawn(new Vector2(3, 6), Color.White);
            var PW4 = new Pawn(new Vector2(4, 6), Color.White);
            var PW5 = new Pawn(new Vector2(5, 6), Color.White);
            var PW6 = new Pawn(new Vector2(6, 6), Color.White);
            var PW7 = new Pawn(new Vector2(7, 6), Color.White);

            var PB0 = new Pawn(new Vector2(0, 1), Color.Black);
            var PB1 = new Pawn(new Vector2(1, 1), Color.Black);
            var PB2 = new Pawn(new Vector2(2, 1), Color.Black);
            var PB3 = new Pawn(new Vector2(3, 1), Color.Black);
            var PB4 = new Pawn(new Vector2(4, 1), Color.Black);
            var PB5 = new Pawn(new Vector2(5, 1), Color.Black);
            var PB6 = new Pawn(new Vector2(6, 1), Color.Black);
            var PB7 = new Pawn(new Vector2(7, 1), Color.Black);

            board.pieces.Add(PW0);
            board.pieces.Add(PW1);
            board.pieces.Add(PW2);
            board.pieces.Add(PW3);
            board.pieces.Add(PW4);
            board.pieces.Add(PW5);
            board.pieces.Add(PW6);
            board.pieces.Add(PW7);

            board.pieces.Add(PB0);
            board.pieces.Add(PB1);
            board.pieces.Add(PB2);
            board.pieces.Add(PB3);
            board.pieces.Add(PB4);
            board.pieces.Add(PB5);
            board.pieces.Add(PB6);
            board.pieces.Add(PB7);

            //Test move and slide
            Debug.Assert(PW0.IsMoveValid(board, new Vector2(0, 4)), "Failed white pawn initial slide");
            Debug.Assert(PW0.IsMoveValid(board, new Vector2(0, 5)), "Failed white pawn standard move");
            Debug.Assert(!PW0.IsMoveValid(board, new Vector2(0, 3)), "Failed white pawn initial move");
            Debug.Assert(PB0.IsMoveValid(board, new Vector2(0, 2)), "Failed black pawn initial slide");
            Debug.Assert(PB0.IsMoveValid(board, new Vector2(0, 3)), "Failed black pawn standard move");
            Debug.Assert(!PB0.IsMoveValid(board, new Vector2(0, 4)), "Failed black pawn initial move");

            //Test slide is blocked by new piece
            board.pieces.Add(new Pawn(new Vector2(1, 5), Color.Black));
            Debug.Assert(!PW1.IsMoveValid(board, new Vector2(1, 4)), "Failed white pawn initial slide collision test");
            board.pieces.Add(new Pawn(new Vector2(1, 2), Color.White));
            Debug.Assert(!PB1.IsMoveValid(board, new Vector2(1, 3)), "Failed black pawn initial slide collision test");

            //Test attack against diagonal opponent
            Debug.Assert(PW0.IsMoveValid(board, new Vector2(1, 5)), "Failed in standard attack position");
            Debug.Assert(PB0.IsMoveValid(board, new Vector2(1, 2)), "Failed in standard attack position");

            //Test diagonal attack against team member
            board.pieces.Add(new Pawn(new Vector2(0, 5), Color.White));
            Debug.Assert(!PW1.IsMoveValid(board, new Vector2(0, 5)), "Failed to catch attack against own team");
            board.pieces.Add(new Pawn(new Vector2(0, 2), Color.Black));
            Debug.Assert(!PB1.IsMoveValid(board, new Vector2(0, 2)), "Failed to catch attack against own team");
        }
    }
}
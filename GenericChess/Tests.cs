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

        public static void TestKing()
        {
            KingStandardMovement();
            KingBlockedMovementAndAttacks();
            KingCheckChecking();
           
           

        }

        //Place some pawns in between the king and an emeny rook
        private static void KingCheckChecking()
        {
            var board = new Board();
            var KW = new King(new Vector2(4, 7), Color.White);

            board.pieces.Add(KW);
            board.WhiteKing = KW;

            //Move into a diagonal path from rook far away
            KW.position = new Vector2(0, 6);
            Debug.Assert(KW.IsMoveValid(board, new Vector2(0, 7)), "Moves into corner with no enemies gaurding");

            //Create a rook covering the corner at distance, king can no longer move to that space
            var sniper_rook = new Bishop(new Vector2(7, 0), Color.Black);
            board.pieces.Add(sniper_rook);
            Debug.Assert(!KW.IsMoveValid(board, new Vector2(0, 7)), "Cannot move into path of distant rook");

            //Add a pawn between the corner and the rook, King can now move into the corner freely
            var protector_pawn = new Pawn(new Vector2(6, 1), Color.White);
            board.pieces.Add(protector_pawn);
            Debug.Assert(protector_pawn.IsMoveValid(board, protector_pawn.position.AddVector(new Vector2(0, -1))), "move up");
            Debug.Assert(KW.IsMoveValid(board, new Vector2(0, 7)), "Moves into corner with enemies gaurding because pawn is protecting path");

            //Move king to corner covered by rook, but protected by pawn
            KW.position = new Vector2(0, 7); 
            //Move king freely
            Debug.Assert(!KW.IsMoveValid(board, KW.position.AddVector(new Vector2(0, 1))), "Move down, out of bounds");
            Debug.Assert(!KW.IsMoveValid(board, KW.position.AddVector(new Vector2(-1, 0))), "Move left, out of bounds");
            Debug.Assert(!KW.IsMoveValid(board, KW.position.AddVector(new Vector2(-1, 1))), "Move diagonal, out of bounds");
            Debug.Assert(KW.IsMoveValid(board, KW.position.AddVector(new Vector2(0, -1))), "Move up");
            Debug.Assert(!KW.IsMoveValid(board, KW.position.AddVector(new Vector2(1, 1))), "Move diagonal twoards threat, still protected");

            //Attempting to Move pawn expsoses king to check
            Debug.Assert(!protector_pawn.IsMoveValid(board, protector_pawn.position.AddVector(new Vector2(0, -1))), "Can no longer move up because it would expose king to check");
            
            //Killing rook with pawn protects king
            Debug.Assert(protector_pawn.IsMoveValid(board, protector_pawn.position.AddVector(new Vector2(1, -1))), "Kill rook. King is safe");
            
            //Put another blocking pawn, then move original pawn. King still protected
            var protector_pawn_2 = new Pawn(new Vector2(5, 2), Color.White);
            board.pieces.Add(protector_pawn_2);
            Debug.Assert(protector_pawn.IsMoveValid(board, protector_pawn.position.AddVector(new Vector2(0, -1))), "Move up now that a different pawn is blocking path");
            Debug.Assert(protector_pawn_2.IsMoveValid(board, protector_pawn_2.position.AddVector(new Vector2(0, -1))), "Move up now that a different pawn is blocking path");
            Debug.Assert(!protector_pawn_2.IsMoveValid(board, protector_pawn_2.position.AddVector(new Vector2(1, -1))), "Pawn can not attack teammate silly!");

        }

        private static void KingBlockedMovementAndAttacks()
        {
            var board = new Board();
            var KW = new King(new Vector2(4, 7), Color.White);

            board.pieces.Add(KW);

            board.WhiteKing = KW;
            var blocking_pawn = new Pawn(new Vector2(4, 6), Color.White);
            board.pieces.Add(blocking_pawn);
            Debug.Assert(!KW.IsMoveValid(board, KW.position.AddVector(new Vector2(0, -1))), "Friendly blocking destination");
            blocking_pawn.position = new Vector2(5, 6);
            Debug.Assert(!KW.IsMoveValid(board, KW.position.AddVector(new Vector2(1, -1))), "Friendly blocking diagonal destination");
            blocking_pawn.color = Color.Black;
            Debug.Assert(KW.IsMoveValid(board, KW.position.AddVector(new Vector2(1, -1))), "Enemy attacked at diagonal destination");
            blocking_pawn.position = new Vector2(4, 6);
            Debug.Assert(!KW.IsMoveValid(board, KW.position.AddVector(new Vector2(1, 0))), "Move puts me in check");
            Debug.Assert(!KW.IsMoveValid(board, KW.position.AddVector(new Vector2(-1, 0))), "Move puts me in check");

            Debug.Assert(KW.IsMoveValid(board, KW.position.AddVector(new Vector2(0, -1))), "Attack enemy putting me in check");

            var pawn_reinforcements = new Pawn(new Vector2(3, 5), Color.Black);
            board.pieces.Add(pawn_reinforcements);
            Debug.Assert(!KW.IsMoveValid(board, KW.position.AddVector(new Vector2(0, -1))), "Attack enemy putting me in check, blocked by enemy protecting him");

            pawn_reinforcements.color = Color.White;
            Debug.Assert(KW.IsMoveValid(board, KW.position.AddVector(new Vector2(0, -1))), "Attack enemy putting me in check, protected by teammate");

            //Knight sitting off in the distance, protcting a pawn
            Knight knight_stealthy = new Knight(blocking_pawn.position.AddVector(new Vector2(1, -2)), Color.Black);
            board.pieces.Add(knight_stealthy);
            Debug.Assert(!KW.IsMoveValid(board, KW.position.AddVector(new Vector2(0, -1))), "Attack enemy putting me in check, enemy is protected by knight");

        }

        private static void KingStandardMovement()
        {
            var board = new Board();
            var KW = new King(new Vector2(4, 7), Color.White);

            board.pieces.Add(KW);

            board.WhiteKing = KW;

            //Check long straight slides
            Debug.Assert(!KW.IsMoveValid(board, KW.position.AddVector(new Vector2(0, 0))), "Dont move");
            Debug.Assert(!KW.IsMoveValid(board, KW.position.AddVector(new Vector2(0, 1))), "Move down, out of bounds");
            Debug.Assert(KW.IsMoveValid(board, KW.position.AddVector(new Vector2(0, -1))), "Move up");
            Debug.Assert(!KW.IsMoveValid(board, KW.position.AddVector(new Vector2(1, 1))), "Move diagonal");
            Debug.Assert(KW.IsMoveValid(board, KW.position.AddVector(new Vector2(-1, -1))), "Move diagonal negative");
        }

        public static void TestQueen()
        {
            var board = new Board();
            var QW = new Queen(new Vector2(4, 7), Color.White);
            var QB = new Queen(new Vector2(4, 0), Color.Black);

            board.pieces.Add(QW);
            board.pieces.Add(QB);

            //Check long straight slides
            Debug.Assert(QW.IsMoveValid(board, QW.position.AddVector(new Vector2(0, -6))), "Move almost all the way up board");
            Debug.Assert(QW.IsMoveValid(board, QW.position.AddVector(new Vector2(0, -7))), "Move all the way up board to edge");
            Debug.Assert(!QW.IsMoveValid(board, QW.position.AddVector(new Vector2(0, -8))), "Overshoot edge");
            Debug.Assert(!QW.IsMoveValid(board, QW.position.AddVector(new Vector2(0, 0))), "Dont Move");

            //Check diagnoal slides
            Debug.Assert(QW.IsMoveValid(board, QW.position.AddVector(new Vector2(-1, -1))), "slide left and up");
            Debug.Assert(QW.IsMoveValid(board, QW.position.AddVector(new Vector2(-2, -2))), "slide left and up");
            Debug.Assert(QW.IsMoveValid(board, QW.position.AddVector(new Vector2(-3, -3))), "slide left and up");
            Debug.Assert(QW.IsMoveValid(board, QW.position.AddVector(new Vector2(-4, -4))), "slide left and up");
            Debug.Assert(!QW.IsMoveValid(board, QW.position.AddVector(new Vector2(-5, -5))), "slide left and up");

            Debug.Assert(QW.IsMoveValid(board, QW.position.AddVector(new Vector2(1, -1))), "slide right and up");
            Debug.Assert(QW.IsMoveValid(board, QW.position.AddVector(new Vector2(2, -2))), "slide right and up");
            Debug.Assert(QW.IsMoveValid(board, QW.position.AddVector(new Vector2(3, -3))), "slide right and up");
            Debug.Assert(!QW.IsMoveValid(board, QW.position.AddVector(new Vector2(4, -4))), "slide right and up");
            Debug.Assert(!QW.IsMoveValid(board, QW.position.AddVector(new Vector2(5, -5))), "slide right and up");

            Debug.Assert(QW.IsMoveValid(board, new Vector2(4, 5)), "Regular move, no pawn in the way");
            var blocking_pawn = new Pawn(QW.position.AddVector(new Vector2(1, -1)), Color.White);
            board.pieces.Add(blocking_pawn);
            Debug.Assert(!QW.IsMoveValid(board, QW.position.AddVector(new Vector2(1, -1))), "team mate blocking slide right and up");
            blocking_pawn.color = Color.Black;
            Debug.Assert(QW.IsMoveValid(board, QW.position.AddVector(new Vector2(1, -1))), "enemy at destination slide right and up");
            var blocking_pawn_2 = new Pawn(QW.position.AddVector(new Vector2(2, -2)), Color.Black);
            board.pieces.Add(blocking_pawn_2);
            blocking_pawn.color = Color.White;
            Debug.Assert(!QW.IsMoveValid(board, QW.position.AddVector(new Vector2(2, -2))), "enemy at destination slide right and up, but teammate is in the way");


            //Check long slides with something in the way
            blocking_pawn.position = new Vector2(4, 5);
            Debug.Assert(!QW.IsMoveValid(board, new Vector2(4, 5)), "Team mate is at my destination");
            Debug.Assert(!QW.IsMoveValid(board, QW.position.AddVector(new Vector2(0, -7))), "Team mate is blocking my route");

            blocking_pawn.color = Color.Black;
            Debug.Assert(QW.IsMoveValid(board, new Vector2(4, 5)), "Enemy is at my desintation");
            Debug.Assert(!QW.IsMoveValid(board, QW.position.AddVector(new Vector2(0, -7))), "Enemy is blocking my route");
        }

        public static void TestRooks()
        {
            var board = new Board();
            //Set them up in standard positions
            Bishop RW0 = new Bishop(new Vector2(2, 7), Color.White),
            RW1 = new Bishop(new Vector2(5, 7), Color.White),
            RB0 = new Bishop(new Vector2(2, 0), Color.Black),
            RB1 = new Bishop(new Vector2(5, 0), Color.Black);

            board.pieces.Add(RW0);
            board.pieces.Add(RW1);
            board.pieces.Add(RB0);
            board.pieces.Add(RB1);

            //Test standard move
            Debug.Assert(RW0.IsMoveValid(board, RW0.position.AddVector(new Vector2(-1, -1))), "slide up and left");
            Debug.Assert(RW0.IsMoveValid(board, RW0.position.AddVector(new Vector2(-2, -2))), "slide up and left");
            Debug.Assert(!RW0.IsMoveValid(board, RW0.position.AddVector(new Vector2(-3, -3))), "Overshoot edge");
            Debug.Assert(RW0.IsMoveValid(board, RW0.position.AddVector(new Vector2(3, -3))), "Go right instead");
            Debug.Assert(RW0.IsMoveValid(board, RW0.position.AddVector(new Vector2(4, -4))), "Go right instead");
            Debug.Assert(RW0.IsMoveValid(board, RW0.position.AddVector(new Vector2(5, -5))), "Go right instead");
            Debug.Assert(!RW0.IsMoveValid(board, RW0.position.AddVector(new Vector2(6, -6))), "Too far right");


            Debug.Assert(!RW0.IsMoveValid(board, RW0.position.AddVector(new Vector2(-1, -2))), "Not 1:1");
            Debug.Assert(!RW0.IsMoveValid(board, RW0.position.AddVector(new Vector2(-2, -1))), "Not 1:1");

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
            var CW0 = new Rook(new Vector2(0, 7), Color.White);
            var CW1 = new Rook(new Vector2(7, 7), Color.White);

            var CB0 = new Rook(new Vector2(0, 0), Color.Black);
            var CB1 = new Rook(new Vector2(7, 0), Color.Black);

            board.pieces.Add(CW0);
            board.pieces.Add(CW1);
            board.pieces.Add(CB0);
            board.pieces.Add(CB1);

            //Test standard move
            Debug.Assert(CW0.IsMoveValid(board, CW0.position.AddVector(new Vector2(0, -1))), "Up 1");
            Debug.Assert(CW0.IsMoveValid(board, CW0.position.AddVector(new Vector2(0, -2))), "Up 2");
            Debug.Assert(CW0.IsMoveValid(board, CW0.position.AddVector(new Vector2(0, -3))), "Up 3");
            Debug.Assert(CW0.IsMoveValid(board, CW0.position.AddVector(new Vector2(0, -4))), "Up 4");
            Debug.Assert(CW0.IsMoveValid(board, CW0.position.AddVector(new Vector2(0, -5))), "Up 5");
            Debug.Assert(CW0.IsMoveValid(board, CW0.position.AddVector(new Vector2(0, -6))), "Up 6");
            Debug.Assert(CW0.IsMoveValid(board, CW0.position.AddVector(new Vector2(0, -7))), "Up 7");
            Debug.Assert(!CW0.IsMoveValid(board, CW0.position.AddVector(new Vector2(0, -8))), "Up 8");

            //Horizontal
            Debug.Assert(CW0.IsMoveValid(board, CW0.position.AddVector(new Vector2(1, 0))), "right 1");
            Debug.Assert(CW0.IsMoveValid(board, CW0.position.AddVector(new Vector2(2, 0))), "right 2");
            Debug.Assert(CW0.IsMoveValid(board, CW0.position.AddVector(new Vector2(6, 0))), "right 6");
            Debug.Assert(!CW0.IsMoveValid(board, CW0.position.AddVector(new Vector2(7, 0))), "right 7, team mate in space");

            var blocking_pawn = new Pawn(CW0.position.AddVector(new Vector2(3, 0)), Color.White);
            board.pieces.Add(blocking_pawn);
            Debug.Assert(!CW0.IsMoveValid(board, CW0.position.AddVector(new Vector2(6, 0))), "right 6, team mate is blocking route");
            Debug.Assert(!CW0.IsMoveValid(board, CW0.position.AddVector(new Vector2(3, 0))), "right 3, team mate is at destination");
            blocking_pawn.color = Color.Black;
            Debug.Assert(!CW0.IsMoveValid(board, CW0.position.AddVector(new Vector2(6, 0))), "right 6, enemy is blocking route");
            Debug.Assert(CW0.IsMoveValid(board, CW0.position.AddVector(new Vector2(3, 0))), "right 3, enemy is at destination");



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
            Debug.Assert(KW0.IsMoveValid(board, KW0.position.AddVector(new Vector2(2, -1))), "right 2 up 1");
            Debug.Assert(!KW0.IsMoveValid(board, KW0.position.AddVector(new Vector2(3, -1))), "right 3 up 1");
            Debug.Assert(!KW0.IsMoveValid(board, KW0.position.AddVector(new Vector2(1, -1))), "right 1 up 1");

            Debug.Assert(!KW0.IsMoveValid(board, KW0.position.AddVector(new Vector2(2, 1))), "right 2 down 1, off board");
            Debug.Assert(!KW0.IsMoveValid(board, KW0.position.AddVector(new Vector2(-2, -1))), "left 2 up 1, off board");

            var blocking_pawn = new Pawn(KW0.position.AddVector(new Vector2(2, -1)), Color.White);
            board.pieces.Add(blocking_pawn);
            Debug.Assert(!KW0.IsMoveValid(board, KW0.position.AddVector(new Vector2(2, -1))), "right 2 up 1, team mate at destination");
            blocking_pawn.color = Color.Black;
            Debug.Assert(KW0.IsMoveValid(board, KW0.position.AddVector(new Vector2(2, -1))), "right 2 up 1, enemy at destination");



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
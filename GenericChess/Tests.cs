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
            //Runs through some common scenarios for a king
            KingStandardMovement();
            KingBlockedMovementAndAttacks();
            KingCheckChecking();
        }

        public static void KingCastling()
        {
            Debug.WriteLine("Test: KingCastling, Begin");
            var board = new Board();

            //Create King and 2 rooks in standard positions
            var KW = new King(new Vector2(4, 7), Color.White);
            var RW0 = new Rook(new Vector2(0, 7), Color.White);
            var RW1 = new Rook(new Vector2(7, 7), Color.White);

            board.pieces.Add(KW);
            board.pieces.Add(RW0);
            board.pieces.Add(RW1);

            board.WhiteKing = KW;

            //Castle in both directions
            Debug.Assert(KW.IsMoveValid(board, KW.position.AddVector(2, 0)), "Castled with the King Rook");
            Debug.Assert(KW.IsMoveValid(board, KW.position.AddVector(-2, 0)), "Castled with the Queen Rook");

            //Castling doesnt work if path of king passes through a checked position
            var QB0 = new Queen(new Vector2(2, 2), Color.Black);
            board.pieces.Add(QB0);
            Debug.Assert(!KW.IsMoveValid(board, KW.position.AddVector(-2, 0)), "Castled with the Queen Rook, path gaurded by queen");

            //Block the queens path and it works again
            var blocking_pawn = new Pawn(new Vector2(2, 6), Color.White);
            board.pieces.Add(blocking_pawn);
            Debug.Assert(KW.IsMoveValid(board, KW.position.AddVector(-2, 0)), "Castled with the Queen Rook, queens view blocked by hero pawn");

            //Block the kings path with a teammate
            blocking_pawn.position = new Vector2(2, 7);
            Debug.Assert(!KW.IsMoveValid(board, KW.position.AddVector(-2, 0)), "Castled with the Queen Rook, hero pawn is now in the way");
            Debug.WriteLine("Test: KingCastling, End");

        }

        private static void KingCheckChecking()
        {
            Debug.WriteLine("Test: KingCheckChecking, Begin");

            var board = new Board();
            var KW = new King(new Vector2(4, 7), Color.White);

            board.pieces.Add(KW);
            board.WhiteKing = KW;

            //Move to the corner with no enemeies just fine
            KW.position = new Vector2(0, 6);
            Debug.Assert(KW.IsMoveValid(board, new Vector2(0, 7)), "Moves into corner with no enemies gaurding");

            //Create a bishop covering the corner at distance, king can no longer move to that space
            var sniper_bishop = new Bishop(new Vector2(7, 0), Color.Black);
            board.pieces.Add(sniper_bishop);
            Debug.Assert(!KW.IsMoveValid(board, new Vector2(0, 7)), "Cannot move into path of distant rook");

            //Add a pawn between the corner and the rook, King can now move into the corner freely
            var protector_pawn = new Pawn(new Vector2(6, 1), Color.White);
            board.pieces.Add(protector_pawn);
            Debug.Assert(protector_pawn.IsMoveValid(board, protector_pawn.position.AddVector(0, -1)), "move up");
            Debug.Assert(KW.IsMoveValid(board, new Vector2(0, 7)), "Moves into corner with enemies gaurding because pawn is protecting path");

            //Move king to corner covered by rook, but protected by pawn
            KW.position = new Vector2(0, 7);
            //Move king freely
            Debug.Assert(!KW.IsMoveValid(board, KW.position.AddVector(0, 1)), "Move down, out of bounds");
            Debug.Assert(!KW.IsMoveValid(board, KW.position.AddVector(-1, 0)), "Move left, out of bounds");
            Debug.Assert(!KW.IsMoveValid(board, KW.position.AddVector(-1, 1)), "Move diagonal, out of bounds");
            Debug.Assert(KW.IsMoveValid(board, KW.position.AddVector(0, -1)), "Move up");
            Debug.Assert(!KW.IsMoveValid(board, KW.position.AddVector(1, 1)), "Move diagonal twoards threat, still protected");

            //Attempting to Move pawn expsoses king to check
            Debug.Assert(!protector_pawn.IsMoveValid(board, protector_pawn.position.AddVector(0, -1)), "Can no longer move up because it would expose king to check");

            //Killing rook with pawn protects king
            Debug.Assert(protector_pawn.IsMoveValid(board, protector_pawn.position.AddVector(1, -1)), "Kill rook. King is safe");

            //Put another blocking pawn, then move original pawn. King still protected
            var protector_pawn_2 = new Pawn(new Vector2(5, 2), Color.White);
            board.pieces.Add(protector_pawn_2);
            Debug.Assert(protector_pawn.IsMoveValid(board, protector_pawn.position.AddVector(0, -1)), "Move up now that a different pawn is blocking path");
            Debug.Assert(protector_pawn_2.IsMoveValid(board, protector_pawn_2.position.AddVector(0, -1)), "Move up now that a different pawn is blocking path");
            Debug.Assert(!protector_pawn_2.IsMoveValid(board, protector_pawn_2.position.AddVector(1, -1)), "Pawn can not attack teammate silly!");
            Debug.WriteLine("Test: KingCheckChecking, End");

        }

        private static void KingBlockedMovementAndAttacks()
        {
            Debug.WriteLine("Test: KingBlockedMovementAndAttacks, Begin");

            var board = new Board();
            var KW = new King(new Vector2(4, 7), Color.White);
            board.pieces.Add(KW);
            board.WhiteKing = KW;

            //Test the King movement getting blocked by friends and attacking enemies
            var blocking_pawn = new Pawn(new Vector2(4, 6), Color.White);
            board.pieces.Add(blocking_pawn);
            Debug.Assert(!KW.IsMoveValid(board, KW.position.AddVector(0, -1)), "Friendly blocking destination");
            blocking_pawn.position = new Vector2(5, 6);
            Debug.Assert(!KW.IsMoveValid(board, KW.position.AddVector(1, -1)), "Friendly blocking diagonal destination");
            blocking_pawn.color = Color.Black;
            Debug.Assert(KW.IsMoveValid(board, KW.position.AddVector(1, -1)), "Enemy attacked at diagonal destination");
            blocking_pawn.position = new Vector2(4, 6);
            Debug.Assert(!KW.IsMoveValid(board, KW.position.AddVector(1, 0)), "Move puts me in check");
            Debug.Assert(!KW.IsMoveValid(board, KW.position.AddVector(-1, 0)), "Move puts me in check");

            //Check that the King can kill a enemy holding him in check 
            Debug.Assert(KW.IsMoveValid(board, KW.position.AddVector(0, -1)), "Attack enemy putting me in check");

            //Check that the king cannot kill a piece holding it in check if that in turn puts it into further check
            var pawn_reinforcements = new Pawn(new Vector2(3, 5), Color.Black);
            board.pieces.Add(pawn_reinforcements);
            Debug.Assert(!KW.IsMoveValid(board, KW.position.AddVector(0, -1)), "Attack enemy putting me in check, blocked by enemy protecting him");

            //However, killing that enemy totally works if the covering peice is your teammate
            pawn_reinforcements.color = Color.White;
            Debug.Assert(KW.IsMoveValid(board, KW.position.AddVector(0, -1)), "Attack enemy putting me in check, protected by teammate");

            //Knight sitting off in the distance, protcting a pawn
            Knight knight_stealthy = new Knight(blocking_pawn.position.AddVector(1, -2), Color.Black);
            board.pieces.Add(knight_stealthy);
            Debug.Assert(!KW.IsMoveValid(board, KW.position.AddVector(0, -1)), "Attack enemy putting me in check, enemy is protected by knight");
            Debug.WriteLine("Test: KingBlockedMovementAndAttacks, End");

        }

        private static void KingStandardMovement()
        {
            Debug.WriteLine("Test: KingStandardMovement, Begin");

            var board = new Board();
            var KW = new King(new Vector2(4, 7), Color.White);

            board.pieces.Add(KW);
            board.WhiteKing = KW;

            //Check long straight slides
            Debug.Assert(!KW.IsMoveValid(board, KW.position.AddVector(0, 0)), "Dont move");
            Debug.Assert(!KW.IsMoveValid(board, KW.position.AddVector(0, 1)), "Move down, out of bounds");
            Debug.Assert(KW.IsMoveValid(board, KW.position.AddVector(0, -1)), "Move up");
            Debug.Assert(!KW.IsMoveValid(board, KW.position.AddVector(1, 1)), "Move diagonal");
            Debug.Assert(KW.IsMoveValid(board, KW.position.AddVector(-1, -1)), "Move diagonal negative");
            Debug.WriteLine("Test: KingStandardMovement, End");

        }

        public static void TestQueen()
        {
            Debug.WriteLine("Test: TestQueen, Begin");

            var board = new Board();
            var QW = new Queen(new Vector2(4, 7), Color.White);
            var QB = new Queen(new Vector2(4, 0), Color.Black);

            board.pieces.Add(QW);
            board.pieces.Add(QB);

            //Check long straight slides
            Debug.Assert(QW.IsMoveValid(board, QW.position.AddVector(0, -6)), "Move almost all the way up board");
            Debug.Assert(QW.IsMoveValid(board, QW.position.AddVector(0, -7)), "Move all the way up board to edge");
            Debug.Assert(!QW.IsMoveValid(board, QW.position.AddVector(0, -8)), "Overshoot edge");
            Debug.Assert(!QW.IsMoveValid(board, QW.position.AddVector(0, 0)), "Dont Move");

            //Check diagnoal slides
            Debug.Assert(QW.IsMoveValid(board, QW.position.AddVector(-1, -1)), "slide left and up");
            Debug.Assert(QW.IsMoveValid(board, QW.position.AddVector(-2, -2)), "slide left and up");
            Debug.Assert(QW.IsMoveValid(board, QW.position.AddVector(-3, -3)), "slide left and up");
            Debug.Assert(QW.IsMoveValid(board, QW.position.AddVector(-4, -4)), "slide left and up");
            Debug.Assert(!QW.IsMoveValid(board, QW.position.AddVector(-5, -5)), "slide left and up");
            Debug.Assert(QW.IsMoveValid(board, QW.position.AddVector(1, -1)), "slide right and up");
            Debug.Assert(QW.IsMoveValid(board, QW.position.AddVector(2, -2)), "slide right and up");
            Debug.Assert(QW.IsMoveValid(board, QW.position.AddVector(3, -3)), "slide right and up");
            Debug.Assert(!QW.IsMoveValid(board, QW.position.AddVector(4, -4)), "slide right and up, off edge");
            Debug.Assert(!QW.IsMoveValid(board, QW.position.AddVector(5, -5)), "slide right and up, off edge");

            //Test Collisions with other pieces and attacking
            Debug.Assert(QW.IsMoveValid(board, new Vector2(4, 5)), "Regular move, no pawn in the way");
            var blocking_pawn = new Pawn(QW.position.AddVector(1, -1), Color.White);
            board.pieces.Add(blocking_pawn);
            Debug.Assert(!QW.IsMoveValid(board, QW.position.AddVector(1, -1)), "team mate blocking slide right and up");
            blocking_pawn.color = Color.Black;
            Debug.Assert(QW.IsMoveValid(board, QW.position.AddVector(1, -1)), "enemy at destination slide right and up");
            var blocking_pawn_2 = new Pawn(QW.position.AddVector(2, -2), Color.Black);
            board.pieces.Add(blocking_pawn_2);
            blocking_pawn.color = Color.White;
            Debug.Assert(!QW.IsMoveValid(board, QW.position.AddVector(2, -2)), "enemy at destination slide right and up, but teammate is in the way");

            //Check long slides with something in the way
            blocking_pawn.position = new Vector2(4, 5);
            Debug.Assert(!QW.IsMoveValid(board, new Vector2(4, 5)), "Team mate is at my destination");
            Debug.Assert(!QW.IsMoveValid(board, QW.position.AddVector(0, -7)), "Team mate is blocking my route");

            blocking_pawn.color = Color.Black;
            Debug.Assert(QW.IsMoveValid(board, new Vector2(4, 5)), "Enemy is at my desintation");
            Debug.Assert(!QW.IsMoveValid(board, QW.position.AddVector(0, -7)), "Enemy is blocking my route");
            Debug.WriteLine("Test: TestQueen, End");

        }

        public static void TestBishops()
        {
            Debug.WriteLine("Test: TestBishops, Begin");

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
            Debug.Assert(RW0.IsMoveValid(board, RW0.position.AddVector(-1, -1)), "slide up and left");
            Debug.Assert(RW0.IsMoveValid(board, RW0.position.AddVector(-2, -2)), "slide up and left");
            Debug.Assert(!RW0.IsMoveValid(board, RW0.position.AddVector(-3, -3)), "Overshoot edge");
            Debug.Assert(RW0.IsMoveValid(board, RW0.position.AddVector(3, -3)), "Go right instead");
            Debug.Assert(RW0.IsMoveValid(board, RW0.position.AddVector(4, -4)), "Go right instead");
            Debug.Assert(RW0.IsMoveValid(board, RW0.position.AddVector(5, -5)), "Go right instead");
            Debug.Assert(!RW0.IsMoveValid(board, RW0.position.AddVector(6, -6)), "Too far right");

            Debug.Assert(!RW0.IsMoveValid(board, RW0.position.AddVector(-1, -2)), "Not 1:1");
            Debug.Assert(!RW0.IsMoveValid(board, RW0.position.AddVector(-2, -1)), "Not 1:1");

            var PB1 = new Pawn(RW0.position.AddVector(-2, -2), Color.Black); //Create new enemy pawn diagonal to RW0
            board.pieces.Add(PB1);
            //Attack it
            Debug.Assert(RW0.IsMoveValid(board, RW0.position.AddVector(-2, -2)), "Diagonal attack failed"); //Direct attack should pass
            Debug.Assert(!RW0.IsMoveValid(board, RW0.position.AddVector(-3, -3)), "Diagonal attack failed"); //Enemy in path should fail
            Debug.WriteLine("Test: TestBishops, End");

        }

        public static void TestCastles()
        {
            Debug.WriteLine("Test: TestCastles, Begin");

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

            //Test standard moves, horizontal sliding
            Debug.Assert(CW0.IsMoveValid(board, CW0.position.AddVector(0, -1)), "Up 1");
            Debug.Assert(CW0.IsMoveValid(board, CW0.position.AddVector(0, -2)), "Up 2");
            Debug.Assert(CW0.IsMoveValid(board, CW0.position.AddVector(0, -3)), "Up 3");
            Debug.Assert(CW0.IsMoveValid(board, CW0.position.AddVector(0, -4)), "Up 4");
            Debug.Assert(CW0.IsMoveValid(board, CW0.position.AddVector(0, -5)), "Up 5");
            Debug.Assert(CW0.IsMoveValid(board, CW0.position.AddVector(0, -6)), "Up 6");
            Debug.Assert(CW0.IsMoveValid(board, CW0.position.AddVector(0, -7)), "Up 7");
            Debug.Assert(!CW0.IsMoveValid(board, CW0.position.AddVector(0, -8)), "Up 8");

            //Horizontal
            Debug.Assert(CW0.IsMoveValid(board, CW0.position.AddVector(1, 0)), "right 1");
            Debug.Assert(CW0.IsMoveValid(board, CW0.position.AddVector(2, 0)), "right 2");
            Debug.Assert(CW0.IsMoveValid(board, CW0.position.AddVector(6, 0)), "right 6");
            Debug.Assert(!CW0.IsMoveValid(board, CW0.position.AddVector(7, 0)), "right 7, team mate in space");

            //Standard moves but with a pawn blocking paths
            var blocking_pawn = new Pawn(CW0.position.AddVector(3, 0), Color.White);
            board.pieces.Add(blocking_pawn);
            Debug.Assert(!CW0.IsMoveValid(board, CW0.position.AddVector(6, 0)), "right 6, team mate is blocking route");
            Debug.Assert(!CW0.IsMoveValid(board, CW0.position.AddVector(3, 0)), "right 3, team mate is at destination");
            blocking_pawn.color = Color.Black;
            Debug.Assert(!CW0.IsMoveValid(board, CW0.position.AddVector(6, 0)), "right 6, enemy is blocking route");
            Debug.Assert(CW0.IsMoveValid(board, CW0.position.AddVector(3, 0)), "right 3, enemy is at destination");
            Debug.WriteLine("Test: TestCastles, End");

        }

        public static void TestKnights()
        {
            Debug.WriteLine("Test: TestKnights, Begin");

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

            //Test standard moves
            Debug.Assert(KW0.IsMoveValid(board, KW0.position.AddVector(2, -1)), "right 2 up 1");
            Debug.Assert(!KW0.IsMoveValid(board, KW0.position.AddVector(3, -1)), "right 3 up 1");
            Debug.Assert(!KW0.IsMoveValid(board, KW0.position.AddVector(1, -1)), "right 1 up 1");
            Debug.Assert(!KW0.IsMoveValid(board, KW0.position.AddVector(2, 1)), "right 2 down 1, off board");
            Debug.Assert(!KW0.IsMoveValid(board, KW0.position.AddVector(-2, -1)), "left 2 up 1, off board");

            //Test collision with other pieces
            var blocking_pawn = new Pawn(KW0.position.AddVector(2, -1), Color.White);
            board.pieces.Add(blocking_pawn);
            Debug.Assert(!KW0.IsMoveValid(board, KW0.position.AddVector(2, -1)), "right 2 up 1, team mate at destination");
            blocking_pawn.color = Color.Black;
            Debug.Assert(KW0.IsMoveValid(board, KW0.position.AddVector(2, -1)), "right 2 up 1, enemy at destination");
            Debug.WriteLine("Test: TestKnights, End");

        }

        public static void TestPawns()
        {
            Debug.WriteLine("Test: TestPawns, Begin");

            var board = new Board();

            var PW0 = new Pawn(new Vector2(0, 6), Color.White);
            var PW1 = new Pawn(new Vector2(1, 6), Color.White);

            var PB0 = new Pawn(new Vector2(0, 1), Color.Black);
            var PB1 = new Pawn(new Vector2(1, 1), Color.Black);

            board.pieces.Add(PW0);
            board.pieces.Add(PW1);

            board.pieces.Add(PB0);
            board.pieces.Add(PB1);

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
            Debug.WriteLine("Test: TestPawns, End");

        }

        //Skipping for now
        public static void EnPassant()
        {
            var board = new Board();

            var PW0 = new Pawn(new Vector2(0, 6), Color.White);
            var PW1 = new Pawn(new Vector2(1, 6), Color.White);

            var PB0 = new Pawn(new Vector2(0, 1), Color.Black);
            var PB1 = new Pawn(new Vector2(1, 1), Color.Black);

            board.pieces.Add(PW0);
            board.pieces.Add(PW1);

            board.pieces.Add(PB0);
            board.pieces.Add(PB1);
            //Test En Passant
            var PW2 = new Pawn(new Vector2(2, 3), Color.White);
            board.pieces.Add(PW2);
            Debug.Assert(!PW2.IsMoveValid(board, PW2.position.AddVector(1, 1)), "Attack enemy passant area before they have passanted");
            Debug.Assert(board.MovePiece(PW1, PW1.position.AddVector(0, -2)), "Moved en passantely, weak, ready for an attack");
            Debug.Assert(PW2.IsMoveValid(board, PW2.position.AddVector(1, 1)), "Attack enemy passant area after they have passanted");
        }

        internal static void PlayGame()
        {
            Debug.WriteLine("Test: PlayGame, Begin");

            //Set up board
            var board = new Board();
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
            board.pieces.Add(KW0);
            board.pieces.Add(KW1);
            board.pieces.Add(KB0);
            board.pieces.Add(KB1);
            board.pieces.Add(QW);
            board.pieces.Add(QB);
            board.pieces.Add(BW0);
            board.pieces.Add(BW1);
            board.pieces.Add(BB0);
            board.pieces.Add(BB1);
            board.pieces.Add(KW);
            board.pieces.Add(KB);

            board.WhiteKing = (King)KW;
            board.BlackKing = (King)KB;
            //end Board Setup

            //Round 1
            Debug.Assert(board.MovePiece(PW4, PW4.position.AddVector(0, -2)), "E4");
            Debug.Assert(board.MovePiece(PB4, PB4.position.AddVector(0, 2)), "E5");
            
            //Round 2
            Debug.Assert(board.MovePiece(BW1, BW1.position.AddVector(-3, -3)), "Bc4");
            Debug.Assert(board.MovePiece(BB1, BB1.position.AddVector(-3, 3)), "Bc5");

            //Round 3
            Debug.Assert(board.MovePiece(QW, QW.position.AddVector(4, -4)), "Qh5");
            Debug.Assert(board.MovePiece(KB1, KB1.position.AddVector(-1, 2)), "Nf6");

            //Round 4, check established
            Debug.Assert(board.MovePiece(QW, QW.position.AddVector(-2, -2)), "Qxf7");

            //Round 4, Black player tries various moves to no avail.
            Debug.Assert(!board.MovePiece(KB, KB.position.AddVector(-1, -1)), "Check check");
            Debug.Assert(!board.MovePiece(KB, KB.position.AddVector(-1, 1)), "Check check");
            Debug.Assert(!board.MovePiece(KB, KB.position.AddVector(1, 1)), "Check check");
            Debug.Assert(!board.MovePiece(KB, KB.position.AddVector(1, -1)), "Check check");

            Debug.Assert(!board.MovePiece(KB, KB.position.AddVector(0, -1)), "Check check");
            Debug.Assert(!board.MovePiece(KB, KB.position.AddVector(0, 1)), "Check check");
            Debug.Assert(!board.MovePiece(KB, KB.position.AddVector(1, 0)), "Check check");
            Debug.Assert(!board.MovePiece(KB, KB.position.AddVector(-1, 0)), "Check check");
            Debug.WriteLine("Test: PlayGame, End");
        }
    }
}
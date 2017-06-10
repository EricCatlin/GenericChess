﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericChess
{
    class Knight : IPiece
    {
        public Knight(Vector2 position, Color color)
        {
            this.position = position;
            this.color = color;
        }

        //Knights hop to one of 8 possible possitions
        List<Vector2> standard_moves = new List<Vector2>(){
            new Vector2(1, 2), //Up right
            new Vector2(-1, 2), //Up left
            new Vector2(1, -2),//Down right
            new Vector2(-1, -2),//Down left

            new Vector2(2, 1), //Right up
            new Vector2(2, -1), //Right down
            new Vector2(-2, -1), // Left down
            new Vector2(-2, 1) // Left up
        };

        public Color color
        {
            get; set;
        }

        public Vector2 position { get; set; }

        public bool IsMoveValid(Board board, Vector2 end_pos)
        {
            bool valid = false;

            //Check if end_pos is out of bounds
            if (end_pos.x < 0 || end_pos.x > 7 || end_pos.y < 0 || end_pos.y > 7) return false; // Off board

            //Get the delta between curent position and final position
            Vector2 delta = position.Delta(end_pos);



            //Check if this delta conforms to a standard move
            var possible_moves = standard_moves.Where(x => x.x == delta.x && x.y == delta.y);
            if (possible_moves.Count() > 0) valid = true;
            foreach (var point in possible_moves) {
                var collision = board.pieces.Where(x => x.position.isEqual(position.AddVector(point)));
                if (collision.Count() > 0)
                {
                    if (collision.First().color == this.color) return false;
                }
            }

            if (valid && color == Color.White && board.WhiteKing != null) valid = board.WhiteKing.SafetyCheck(board);
            if (valid && color == Color.Black && board.BlackKing != null) valid = board.BlackKing.SafetyCheck(board);

            return valid;
        }
    }
}

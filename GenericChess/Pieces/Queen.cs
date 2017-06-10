using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericChess
{
    class Queen : IPiece
    {
        public Queen(Vector2 position, Color color)
        {
            this.position = position;
            this.color = color;
        }

        public Color color { get; set; }
        public Vector2 position { get; set; }
        public bool IsMoveValid(Board board, Vector2 end_pos)
        {
            bool valid = false;

            //Check if end_pos is out of bounds
            if (end_pos.x < 0 || end_pos.x > 8 || end_pos.y < 0 || end_pos.y > 8) return false; // Off board

            //Get the delta between curent position and final position
            Vector2 delta = position.Delta(end_pos);
            if (delta.x == 0 && delta.y == 0) return false; //return if stationary

            //Check final position can be reached using diagonal or straight movements
            var final_pos_valid = false;
            if ((delta.x == delta.y || delta.x == -delta.y)) final_pos_valid = true; //diagonal
            if ((delta.x == 0 && delta.y != 0) || (delta.y == 0 && delta.x != 0)) final_pos_valid = true; // Straight                                                                       
            if (!final_pos_valid) return false;
            //Return when final position is not straight or diagonal

            var end_point_piece = board.GetPieceAt(end_pos);
            if (end_point_piece == null || end_point_piece.color != this.color)
            {
                valid = true;
                delta.x += (delta.x > 0) ? -1 : (delta.x < 0) ? 1 : 0; //Move x towards 0
                delta.y += (delta.y > 0) ? -1 : (delta.y < 0) ? 1 : 0; //Move y towards 0

                //Check if path is clear to arrive at valid end-point
                while (delta.x != 0 || delta.y != 0)
                {
                    //Check for collision
                    var collision_check = board.GetPieceAt(position.AddVector(delta));
                    if (collision_check != null) return false;
                    //Move x towards 0
                    delta.x += (delta.x > 0) ? -1 : (delta.x < 0) ? 1 : 0;
                    //Move y towards 0
                    delta.y += (delta.y > 0) ? -1 : (delta.y < 0) ? 1 : 0;
                }
            }
            return valid;
        }

        public bool Move(Vector2 position)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericChess
{
    interface IPiece
    {
        Color color { get; set; }
        Vector2 position { get; set; }
        bool IsMoveValid(Board board, Vector2 end_pos);
        bool hasMoved { get; set; }
        bool isCastling { get; set; }
    }
    public enum Color
    {
        White,
        Black
    }
}

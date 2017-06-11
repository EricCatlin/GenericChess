using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericChess
{
    interface IPiece
    {
        Color Color { get; set; }
        Vector2 Position { get; set; }
        bool IsMoveValid(Board board, Vector2 endPosition);
        bool HasMoved { get; set; }
        bool IsCastling { get; set; }
    }
    public enum Color
    {
        White,
        Black
    }
}

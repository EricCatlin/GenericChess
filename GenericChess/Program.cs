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
        static void Main(string[] args)
        {
            Tests.TestPawns();
            Tests.TestKnights();
            Tests.TestCastles();
            Tests.TestRooks();
            Tests.TestQueen();
            Tests.TestKing();
        }


    }

    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PenIsland
{
    interface GameBoard
    {
        void NewGame();
        void GetStatusMessage(out string message, out Color color);
    }
}

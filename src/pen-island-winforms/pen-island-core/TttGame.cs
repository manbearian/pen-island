using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenIsland
{
    class TttGame
    {
        public int PlayerCount { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public TttGame(int playerCount, int boardWidth, int boardHeight)
        {
            PlayerCount = playerCount;

            Width = boardWidth;
            Height = boardHeight;

        }
    }
}

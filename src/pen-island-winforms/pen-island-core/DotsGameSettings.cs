using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenIsland
{
    enum DotsBoardType
    {
        Squares = 0, Triangles
    }

    class DotsGameSettings
    {
        int playerCount_ = 2;
        public bool[] ComputerPlayers = new bool[Player.MaxPlayers];

        public DotsGameSettings() { }

        public int PlayerCount
        {
            get { return playerCount_; }
            set {
                if (value < 2)
                    throw new Exception("too few players");
                if (value > Player.MaxPlayers)
                    throw new Exception("too many players");
                playerCount_ = value;
            }
        }

        public DotsBoardType BoardType { get; set; }

        int width_ = 5;
        int height_ = 5;

        public int Width {
            get { return width_; }
            set
            {
                if (value < 2)
                    throw new Exception("too small board");
                if (value > 99)
                    throw new Exception("too large board");
                width_ = value;
            }
        }
        public int Height
        {
            get { return height_; }
            set
            {
                if (value < 2)
                    throw new Exception("too small board");
                if (value > 99)
                    throw new Exception("too large board");
                height_ = value;
            }
        }

    }
}


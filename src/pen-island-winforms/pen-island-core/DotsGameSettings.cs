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

    static class DotsGameSettings
    {
        public static void Save()
        {
            Properties.Settings.Default.Save();
        }

        public static int PlayerCount
        {
            get { var v = Properties.Settings.Default.Dots_PlayerCount; ValidatePlayerCount(v); return v; }
            set { ValidatePlayerCount(value); Properties.Settings.Default.Dots_PlayerCount = value; }
        }

        static void ValidatePlayerCount(int playerCount)
        {
            if (playerCount < 2)
                throw new Exception("too few players");
            if (playerCount > Player.MaxPlayers)
                throw new Exception("too many players");
        }

        public static DotsBoardType BoardType
        {
            get { Enum.TryParse<DotsBoardType>(Properties.Settings.Default.Dots_BoardType, out DotsBoardType v); return v; }
            set { Properties.Settings.Default.Dots_BoardType = value.ToString(); }
        }

        public static int BoardWidth
        {
            get { var v = Properties.Settings.Default.Dots_BoardWidth; ValidateBoardWidth(v); return v; }
            set { ValidateBoardWidth(value); Properties.Settings.Default.Dots_BoardWidth = value; }
        }

        public static int BoardHeight
        {
            get { var v = Properties.Settings.Default.Dots_BoardHeight; ValidateBoardHeight(v); return v; }
            set { ValidateBoardHeight(value); Properties.Settings.Default.Dots_BoardHeight = value; }
        }

        static void ValidateBoardWidth(int value)
        {
            if (value < 2)
                throw new Exception("too small board");
            if (value > 99)
                throw new Exception("too large board");
        }

        static void ValidateBoardHeight(int value)
        {
            if (value < 2)
                throw new Exception("too small board");
            if (value > 99)
                throw new Exception("too large board");
        }

    }
}


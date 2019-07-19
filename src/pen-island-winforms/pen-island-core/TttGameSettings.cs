using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenIsland
{
    static class TttGameSettings
    {
        public static void Save()
        {
            Properties.Settings.Default.Save();
        }

        public static int PlayerCount
        {
            get { var v = Properties.Settings.Default.Ttt_PlayerCount; ValidatePlayerCount(v); return v; }
            set { ValidatePlayerCount(value); Properties.Settings.Default.Ttt_PlayerCount = value; }
        }
        
        static void ValidatePlayerCount(int playerCount)
        {
            if (playerCount < 2)
                throw new Exception("too few players");
            if (playerCount > Player.MaxPlayers)
                throw new Exception("too many players");
        }

        public static int BoardWidth
        {
            get { var v = Properties.Settings.Default.Ttt_BoardWidth; ValidateBoardWidth(v); return v; }
            set { ValidateBoardWidth(value); Properties.Settings.Default.Ttt_BoardWidth = value; }
        }

        public static int BoardHeight
        {
            get { var v = Properties.Settings.Default.Ttt_BoardHeight; ValidateBoardHeight(v); return v; }
            set { ValidateBoardHeight(value); Properties.Settings.Default.Ttt_BoardHeight = value; }
        }

        public static int WinLength
        {
            get { var v = Properties.Settings.Default.Ttt_WinLength; ValidateWinLength(v); return v; }
            set { ValidateBoardHeight(value); Properties.Settings.Default.Ttt_WinLength = value; }
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
        static void ValidateWinLength(int value)
        {
            if (value < 3)
                throw new Exception("win length must be >= 3");
            if (value > BoardHeight || value > BoardWidth)
                throw new Exception("win length is too big for board size");
        }
        

    }
}


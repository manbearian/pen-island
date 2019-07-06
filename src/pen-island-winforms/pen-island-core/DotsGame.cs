using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenIsland
{
    public class DotsGame
    {
        public DotsGame(int playerCount, int boardSizeX, int boardSizeY)
        {
            PlayerCount = playerCount;
            CurrentPlayer = 0;

            Width = boardSizeX;
            Height = boardSizeY;

            hLines = new int[boardSizeX - 1, boardSizeY];
            vLines = new int[boardSizeX, boardSizeY - 1];
            squares = new int[boardSizeX - 1, boardSizeY - 1];

            for (int i = 0; i < boardSizeX; ++i)
            {
                for (int j = 0; j < boardSizeY; ++j)
                {
                    bool c = i < (boardSizeX - 1);
                    bool d = j < (boardSizeY - 1);
                    if (c)
                    {
                        hLines[i, j] = Player.Invalid;
                    }
                    if (d)
                    {
                        vLines[i, j] = Player.Invalid;
                    }
                    if (c && d)
                    {
                        squares[i, j] = Player.Invalid;
                    }
                }
            } 
        }

        public int PlayerCount { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        readonly int[,] hLines;
        readonly int[,] vLines;
        readonly int[,] squares;

        public int GetHorizontal(int col, int row)
        {
            if ((col < (Width - 1)) && (row < Height))
                return hLines[col, row];
            return Player.Invalid;
        }
        
        public int GetVertical(int col, int row)
        {
            if ((col < Width) && (row < (Height - 1)))
                return vLines[col, row];
            return Player.Invalid;
        }

        public int GetSquare(int col, int row)
        {
            if ((col < (Width - 1)) && (row < (Height - 1)))
                return squares[col, row];
            return Player.Invalid;
        }

        public void RecordHorizontal(int col, int row)
        {
            System.Diagnostics.Debug.Assert(hLines[col, row] == Player.Invalid);

            hLines[col, row] = CurrentPlayer;

            // check for squares
            bool square = false;
            if ((row > 0) && (GetHorizontal(col, row - 1) != Player.Invalid)
                && (GetVertical(col, row - 1) != Player.Invalid)
                && (GetVertical(col + 1, row -1) != Player.Invalid))
            {
                RecordSquare(col, row - 1);
                square = true;
            }

            if ((GetHorizontal(col, row+1) != Player.Invalid)
                && (GetVertical(col, row) != Player.Invalid)
                && (GetVertical(col + 1, row) != Player.Invalid))
            {
                RecordSquare(col, row);
                square = true;
            }

            if (!square)
            {
                EndTurn();
            }
        }

        public void RecordVertical(int col, int row)
        {
            System.Diagnostics.Debug.Assert(vLines[col, row] == Player.Invalid);

            vLines[col, row] = CurrentPlayer;

            // check for squares
            bool square = false;

            if ((col > 0) && (GetVertical(col - 1, row) != Player.Invalid)
                && (GetHorizontal(col - 1, row) != Player.Invalid)
                && (GetHorizontal(col - 1, row + 1) != Player.Invalid))
            {
                RecordSquare(col - 1, row);
                square = true;
            }

            if ((GetVertical(col + 1, row) != Player.Invalid)
                && (GetHorizontal(col, row) != Player.Invalid)
                && (GetHorizontal(col, row + 1) != Player.Invalid))
            {
                RecordSquare(col, row);
                square = true;
            }

            if (!square)
            {
                EndTurn();
            }
        }

        // only called internally when a line is recorded
        private void RecordSquare(int col, int row)
        {
            System.Diagnostics.Debug.Assert(squares[col, row] == Player.Invalid);

            squares[col, row] = CurrentPlayer;

            bool anyPlaysRemaining = false;
            foreach (var s in squares)
            {
                if (s == Player.Invalid)
                {
                    anyPlaysRemaining = true;
                    break;
                }
            }

            if (!anyPlaysRemaining)
            {
                EndGame();
            }
        }


        public int CurrentPlayer { get; private set; }
        public bool GameOver { get; private set; }

        void EndTurn()
        {
            CurrentPlayer++;
            if (CurrentPlayer >= PlayerCount)
            {
                CurrentPlayer = 0;
            } 
        }

        void EndGame()
        {
            CurrentPlayer = Player.Invalid;
            GameOver = true;
        }
    }
}

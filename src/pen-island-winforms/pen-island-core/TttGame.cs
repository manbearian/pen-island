using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Point = System.Drawing.Point;

namespace PenIsland
{
    class TttGame
    {
        public int PlayerCount { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public int CurrentPlayer { get; private set; }
        public bool GameOver { get; private set; }
        public int Winner { get; private set; }

        private readonly int[,] recordedMoves;
        private int winningNumber;
        private readonly List<Point[]> moveStrings;

        public TttGame(int playerCount, int boardWidth, int boardHeight, int winningNumber)
        {
            PlayerCount = playerCount;
            CurrentPlayer = Player.FirstPlayer;

            Width = boardWidth;
            Height = boardHeight;

            recordedMoves = new int[Width, Height];
            this.winningNumber = winningNumber;

            for (int i = 0; i < boardWidth; ++i)
            {
                for (int j = 0; j < boardHeight; ++j)
                {
                    recordedMoves[i, j] = Player.Invalid;
                }
            }
            
            moveStrings = new List<Point[]>();

            //
            // diagram of 4x4 board
            //
            // [0,0] [1,0] [2,0] [3,0]
            // [0,1] [1,1] [2,1] [3,1]
            // [0,2] [1,2] [2,2] [3,2]
            // [0,3] [1,1] [2,3] [3,3]

            // record horizontal move-strings
            for (int i = 0; i < Width; ++i)
            {
                for (int j = 0; j < Height - winningNumber + 1; ++j)
                {
                    Point[] moveString = new Point[winningNumber];
                    for (int k = 0; k < winningNumber; ++k)
                    {
                        moveString[k] = new Point(i, j + k);
                    }
                    moveStrings.Add(moveString);
                }
            }

            // record vertical move-strings
            for (int i = 0; i < Height; ++i)
            {
                for (int j = 0; j < Width - winningNumber + 1; ++j)
                {
                    Point[] moveString = new Point[winningNumber];
                    for (int k = 0; k < winningNumber; ++k)
                    {
                        moveString[k] = new Point(j + k, i);
                    }
                    moveStrings.Add(moveString);
                }
            }

            // record diagonal move-strings
            int limit = Math.Min(Width, Height);
            for (int i = 0; i < limit - winningNumber + 1; ++i)
            {
                for (int j = 0; j < limit - winningNumber + 1; ++j)
                {
                    Point[] moveString1 = new Point[winningNumber];
                    Point[] moveString2 = new Point[winningNumber];
                    for (int k = 0, l = winningNumber - 1; k < winningNumber; ++k, --l)
                    {
                        moveString1[k] = new Point(i + k, j + k);
                        moveString2[k] = new Point(i + k, j + l);
                    }
                    moveStrings.Add(moveString1);
                    moveStrings.Add(moveString2);
                }
            }
        }

        public int GetMove(int x, int y)
        {
            return recordedMoves[x, y];
        }

        public void RecordMove(int x, int y)
        {
            recordedMoves[x, y] = CurrentPlayer;

            bool pathToVictory = false;
            foreach (var moveString in moveStrings)
            {
                int[] plays = new int[moveString.Length];

                for (int i = 0; i < moveString.Length; ++i)
                {
                    plays[i] = GetMove(moveString[i].X, moveString[i].Y);
                }

                if (CheckWinner(plays))
                {
                    System.Diagnostics.Debug.Assert(moveString.Contains(new Point(x, y)), "Game wasn't won by the current move?!?");
                    EndGame();
                    return;
                }

                pathToVictory |= !CheckBlocked(plays);
            }

            if (!pathToVictory)
            {
                CurrentPlayer = Player.Invalid;
                EndGame();
            }

            EndTurn();
        }

        bool CheckWinner(int [] plays)
        {
            System.Diagnostics.Debug.Assert(plays.Length == winningNumber);

            if (plays[0] == Player.Invalid)
            {
                return false;
            }

            for (int i = 1; i < plays.Length; ++i)
            {
                if (plays[i] != plays[0])
                    return false;
            }

            return true;
        }

        bool CheckBlocked(int [] plays)
        {
            int seenPlayer = Player.Invalid;

            for (int i = 0; i < plays.Length; ++i)
            {
                int checkPlayer = plays[i];
                if (checkPlayer != Player.Invalid)
                {
                    if (seenPlayer == Player.Invalid)
                    {
                        seenPlayer = checkPlayer;
                    }
                    else if (checkPlayer != seenPlayer)
                    {
                        return true;
                    }
                }
            }

            // this path is still open
            return false;
        }
        
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
            Winner = CurrentPlayer;
            CurrentPlayer = Player.Invalid;
            GameOver = true;
        }

        public void CheckWinnerOrNoWinner()
        {
            // check verticals
            for (int i = 0; i < Width; ++i)
            {
                for (int j = 0; j < Height; ++j)
                {
                    var player = GetMove(i, j);
                }
            }
        }
    }
}

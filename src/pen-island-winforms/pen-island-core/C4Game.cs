using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenIsland
{
    class C4Game
    {
        public struct State
        {
            private readonly int[,] recordedMoves;

            public State(int width, int height)
            {
                recordedMoves = new int[width, height];

                for (int i = 0; i < width; ++i)
                {
                    for (int j = 0; j < height; ++j)
                    {
                        this[i, j] = Player.Invalid;
                    }
                }
            }

            public int this[Move move]
            {
                get { return recordedMoves[move.X, move.Y]; }
                set { recordedMoves[move.X, move.Y] = value; }
            }

            public int this[int x, int y]
            {
                get { return recordedMoves[x, y]; }
                set { recordedMoves[x, y] = value; }
            }
        }
        public struct Move
        {
            public int X;
            public int Y;

            public Move(int x, int y) { X = x; Y = y; }
            public static bool operator ==(Move a, Move b) { return a.Equals(b); }
            public static bool operator !=(Move a, Move b) { return !a.Equals(b); }

            public bool Equals(Move other) { return X == other.X && Y == other.Y; }
            public override bool Equals(object o) { return Equals((Move)o); }
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
        public int PlayerCount { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public int CurrentPlayer { get; private set; }
        public bool GameOver { get; private set; }
        public int Winner { get; private set; }

        private State state;
        private readonly int winLength;

        public C4Game(int playerCount, int boardWidth, int boardHeight, int winLength)
        {
            PlayerCount = playerCount;
            CurrentPlayer = Player.FirstPlayer;

            Width = boardWidth;
            Height = boardHeight;

            state = new State(boardWidth, boardHeight);
            this.winLength = winLength;
        }

        public int GetMove(Move move)
        {
            return state[move];
        }

        public void RecordMove(Move move)
        {
            state[move] = CurrentPlayer;

            var winner = CheckWinner(state);

            if (winner == null)
            {
                EndTurn();
                return;
            }

            if (winner == Player.Invalid)
            {
                CurrentPlayer = Player.Invalid;
            }

            System.Diagnostics.Debug.Assert(winner == CurrentPlayer, "Game wasn't won by the current player?!?");
            EndGame();
        }

        int? CheckWinner(State state)
        {
            for (int i = 0; i < Width; ++i)
            {
                for (int j = 0; j < Height; ++j)
                {
                    var player = state[i, j];
                    if (player == Player.Invalid)
                    {
                        continue;
                    }

                    // check horizontal wins
                    if (i + (winLength - 1) < Width)
                    {
                        bool horizontalMatch = true;
                        for (int k = 1; k < winLength; ++k)
                        {
                            if (state[i + k, j] != player)
                            {
                                horizontalMatch = false;
                                break;
                            }
                        }

                        if (horizontalMatch)
                        {
                            return player;
                        }
                    }

                    // check vertical wins
                    if (j + (winLength - 1) < Height)
                    {
                        bool verticalMatch = true;
                        for (int k = 1; k < winLength; ++k)
                        {
                            if (state[i, j + k] != player)
                            {
                                verticalMatch = false;
                                break;
                            }
                        }

                        if (verticalMatch)
                        {
                            return player;
                        }
                    }

                    // check diag-down-left wins
                    if ((i + (winLength - 1) < Width)
                        && (j + (winLength - 1) < Height))
                    {
                        bool diagMatch = true;
                        for (int k = 1; k < winLength; ++k)
                        {
                            if (state[i + k, j + k] != player)
                            {
                                diagMatch = false;
                                break;
                            }
                        }

                        if (diagMatch)
                        {
                            return player;

                        }
                    }

                    // check diag-up-left wins
                    if ((i + (winLength - 1) < Width)
                        && (j - (winLength - 1) >= 0))
                    {
                        bool diagMatch = true;
                        for (int k = 1; k < winLength; ++k)
                        {
                            if (state[i + k, j - k] != player)
                            {
                                diagMatch = false;
                                break;
                            }
                        }

                        if (diagMatch)
                        {
                            return player;
                        }
                    }

                }
            }

            return null;
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
    }
}

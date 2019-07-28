using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenIsland
{
    class TttGame
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

            public State(State copyFrom)
            {
                recordedMoves = copyFrom.recordedMoves.Clone() as int[,];
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

            public State Clone()
            {
                return new State(this);
            }

            //  X | O |
            // ---+---+---
            //    | X | O
            // ---+---+---
            //    |   | X
            //
            public override string ToString()
            {
                var buffer = new StringBuilder();

                for (int j = 0; j < recordedMoves.GetLength(1); ++j)
                {
                    for (int i = 0; i < recordedMoves.GetLength(0); ++i)
                    {
                        string glyph = " ";

                        var player = this[i, j];
                        switch (player)
                        {
                            case 0: glyph = "X"; break;
                            case 1: glyph = "O"; break;
                            default: glyph = player > 0 ? player.ToString() : " "; break;
                        }

                        buffer.Append(" ");
                        buffer.Append(glyph);
                        buffer.Append(" ");
                        if (i != recordedMoves.GetLength(0) - 1)
                        {
                            buffer.Append("|");
                        }
                        else
                        {
                            buffer.AppendLine();
                        }
                    }

                    if (j != recordedMoves.GetLength(1) - 1)
                    {
                        for (int i = 0; i < recordedMoves.GetLength(1); ++i)
                        {
                            buffer.Append("---");
                            if (i != recordedMoves.GetLength(0) - 1)
                            {
                                buffer.Append("+");
                            }
                            else
                            {
                                buffer.AppendLine();
                            }
                        }
                    }
                }

                return buffer.ToString();
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
        private readonly List<Move[]> moveStrings;

        public TttGame(int playerCount, int boardWidth, int boardHeight, int winLength)
        {
            PlayerCount = playerCount;
            CurrentPlayer = Player.FirstPlayer;

            Width = boardWidth;
            Height = boardHeight;

            state = new State(boardWidth, boardHeight);
            this.winLength = winLength;

            moveStrings = new List<Move[]>();

            //
            // diagram of 4x4 board
            //
            // [0,0] [1,0] [2,0] [3,0]
            // [0,1] [1,1] [2,1] [3,1]
            // [0,2] [1,2] [2,2] [3,2]
            // [0,3] [1,3] [2,3] [3,3]

            // record horizontal move-strings
            for (int i = 0; i < Width; ++i)
            {
                for (int j = 0; j < Height - winLength + 1; ++j)
                {
                    Move[] moveString = new Move[winLength];
                    for (int k = 0; k < winLength; ++k)
                    {
                        moveString[k] = new Move(i, j + k);
                    }
                    moveStrings.Add(moveString);
                }
            }

            // record vertical move-strings
            for (int i = 0; i < Height; ++i)
            {
                for (int j = 0; j < Width - winLength + 1; ++j)
                {
                    Move[] moveString = new Move[winLength];
                    for (int k = 0; k < winLength; ++k)
                    {
                        moveString[k] = new Move(j + k, i);
                    }
                    moveStrings.Add(moveString);
                }
            }

            // record diagonal move-strings
            int limit = Math.Min(Width, Height);
            for (int i = 0; i < limit - winLength + 1; ++i)
            {
                for (int j = 0; j < limit - winLength + 1; ++j)
                {
                    Move[] moveString1 = new Move[winLength];
                    Move[] moveString2 = new Move[winLength];
                    for (int k = 0, l = winLength - 1; k < winLength; ++k, --l)
                    {
                        moveString1[k] = new Move(i + k, j + k);
                        moveString2[k] = new Move(i + k, j + l);
                    }
                    moveStrings.Add(moveString1);
                    moveStrings.Add(moveString2);
                }
            }
        }

        public State CloneState()
        {
            return state.Clone();
        }

        public int GetMove(Move move)
        {
            return state[move];
        }

        public int? CheckWinner(State state)
        {
            bool pathToVictory = false;
            foreach (var moveString in moveStrings)
            {
                int[] plays = new int[moveString.Length];

                for (int i = 0; i < moveString.Length; ++i)
                {
                    plays[i] = state[moveString[i]];
                }

                var winner = CheckWinner(plays);
                if (winner != Player.Invalid)
                {
                    return winner;
                }

                pathToVictory |= !CheckBlocked(plays);
            }

            if (!pathToVictory)
            {
                // "Cat's Game" - no winner can exist
                return Player.Invalid;
            }

            // no winner yet, game continues...
            return null;
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

        static int CheckWinner(int [] plays)
        {
            var player = plays[0];

            if (player == Player.Invalid)
            {
                return Player.Invalid;
            }

            for (int i = 1; i < plays.Length; ++i)
            {
                if (plays[i] != player)
                    return Player.Invalid;
            }

            return player;
        }

        static bool CheckBlocked(int [] plays)
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
    }
}

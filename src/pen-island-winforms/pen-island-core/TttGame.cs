using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenIsland
{
    class MoveInfo
    {
        public int X;
        public int Y;

        public MoveInfo(int x, int y) { X = x; Y = y; }
        public static bool operator ==(MoveInfo a, MoveInfo b) { return a.Equals(b); }
        public static bool operator !=(MoveInfo a, MoveInfo b) { return !a.Equals(b); }

        public bool Equals(MoveInfo other) { return X == other.X && Y == other.Y; }
        public override bool Equals(object o) { return Equals(o as MoveInfo);  }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    struct TttGameState
    {
        private readonly int[,] recordedMoves;

        public TttGameState(int width, int height)
        {
            recordedMoves = new int[width, height];
        }

        public TttGameState(TttGameState copyFrom)
        {
            recordedMoves = copyFrom.recordedMoves.Clone() as int[,];
        }

        public int this[MoveInfo move]
        {
            get { return recordedMoves[move.X, move.Y]; }
            set { recordedMoves[move.X, move.Y] = value; }
        }

        public int this[int x, int y]
        {
            get { return recordedMoves[x, y]; }
            set { recordedMoves[x, y] = value; }

        }

        public TttGameState Clone()
        {
            return new TttGameState(this);
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
                        case 0:   glyph = "X"; break;
                        case 1:   glyph = "O"; break;
                        default:  glyph = player > 0 ? player.ToString() : " "; break;
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

    class TttGame
    {
        public int PlayerCount { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public int CurrentPlayer { get; private set; }
        public bool GameOver { get; private set; }
        public int Winner { get; private set; }

        private TttGameState state;
        private readonly int winLength;
        private readonly List<MoveInfo[]> moveStrings;

        public TttGame(int playerCount, int boardWidth, int boardHeight, int winLength)
        {
            PlayerCount = playerCount;
            CurrentPlayer = Player.FirstPlayer;

            Width = boardWidth;
            Height = boardHeight;

            state = new TttGameState(boardWidth, boardHeight);
            this.winLength = winLength;

            for (int i = 0; i < boardWidth; ++i)
            {
                for (int j = 0; j < boardHeight; ++j)
                {
                    state[i, j] = Player.Invalid;
                }
            }
            
            moveStrings = new List<MoveInfo[]>();

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
                for (int j = 0; j < Height - winLength + 1; ++j)
                {
                    MoveInfo[] moveString = new MoveInfo[winLength];
                    for (int k = 0; k < winLength; ++k)
                    {
                        moveString[k] = new MoveInfo(i, j + k);
                    }
                    moveStrings.Add(moveString);
                }
            }

            // record vertical move-strings
            for (int i = 0; i < Height; ++i)
            {
                for (int j = 0; j < Width - winLength + 1; ++j)
                {
                    MoveInfo[] moveString = new MoveInfo[winLength];
                    for (int k = 0; k < winLength; ++k)
                    {
                        moveString[k] = new MoveInfo(j + k, i);
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
                    MoveInfo[] moveString1 = new MoveInfo[winLength];
                    MoveInfo[] moveString2 = new MoveInfo[winLength];
                    for (int k = 0, l = winLength - 1; k < winLength; ++k, --l)
                    {
                        moveString1[k] = new MoveInfo(i + k, j + k);
                        moveString2[k] = new MoveInfo(i + k, j + l);
                    }
                    moveStrings.Add(moveString1);
                    moveStrings.Add(moveString2);
                }
            }
        }

        public TttGameState CloneState()
        {
            return state.Clone();
        }

        public int GetMove(MoveInfo move)
        {
            return state[move];
        }

        public void RecordMove(MoveInfo move)
        {
            state[move] = CurrentPlayer;

            bool pathToVictory = false;
            foreach (var moveString in moveStrings)
            {
                int[] plays = new int[moveString.Length];

                for (int i = 0; i < moveString.Length; ++i)
                {
                    plays[i] = GetMove(moveString[i]);
                }

                if (CheckWinner(plays))
                {
                    System.Diagnostics.Debug.Assert(moveString.Contains(move), "Game wasn't won by the current move?!?");
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
            System.Diagnostics.Debug.Assert(plays.Length == winLength);

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
    }
}

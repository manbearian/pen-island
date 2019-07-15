using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenIsland
{
    enum LineType { None, Horizontal, Vertical };

    struct LineInfo
    {
        public LineType LineType;
        public int X;
        public int Y;

        public LineInfo(LineType lineType, int x, int y)
        {
            LineType = lineType;
            X = x;
            Y = y;
        }

        public static LineInfo Invalid
        {
            get { return new LineInfo(LineType.None, -1, -1); }
        }
    }

    class DotsGame
    {
        public DotsGame(int playerCount, DotsBoardType dotsBoardType, int boardWidth, int boardHeight)
        {
            PlayerCount = playerCount;
            CurrentPlayer = Player.FirstPlayer;

            Width = boardWidth;
            Height = boardHeight;

            hLines = new int[boardWidth - 1, boardHeight];
            vLines = new int[boardWidth, boardHeight - 1];
            squares = new int[boardWidth - 1, boardHeight - 1];

            scores = new int[playerCount];

            for (int i = 0; i < boardWidth; ++i)
            {
                for (int j = 0; j < boardHeight; ++j)
                {
                    bool c = i < (boardWidth - 1);
                    bool d = j < (boardHeight - 1);
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

        readonly int[] scores;

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

        public bool IsValid(LineInfo line)
        {
            if (line.LineType == LineType.None)
                return false;
            if (line.X < 0 || line.X >= Width)
                return false;
            if (line.Y < 0 || line.Y >= Height)
                return false;
            return true;
        }

        public int GetMove(LineInfo move)
        {
            System.Diagnostics.Debug.Assert(IsValid(move));

            switch (move.LineType)
            {
                case LineType.Horizontal:
                    return hLines[move.X, move.Y];
                case LineType.Vertical:
                    return vLines[move.X, move.Y];
                default:
                    throw new Exception("unexpected line type");
            }
        }

        public void RecordMove(LineInfo move)
        {
            System.Diagnostics.Debug.Assert(IsValid(move));

            switch (move.LineType)
            {
                case LineType.Horizontal:
                    RecordHorizontal(move.X, move.Y);
                    break;
                case LineType.Vertical:
                    RecordVertical(move.X, move.Y);
                    break;
                default:
                    throw new Exception("unexpected line type");
            }
        }

        void RecordHorizontal(int col, int row)
        {
            System.Diagnostics.Debug.Assert(hLines[col, row] == Player.Invalid);
            System.Diagnostics.Debug.Assert(!GameOver);

            hLines[col, row] = CurrentPlayer;

            // check for squares
            bool square = false;
            if ((row > 0) && (GetHorizontal(col, row - 1) != Player.Invalid)
                && (GetVertical(col, row - 1) != Player.Invalid)
                && (GetVertical(col + 1, row - 1) != Player.Invalid))
            {
                RecordSquare(col, row - 1);
                square = true;
            }

            if (GameOver)
                return;

            if ((GetHorizontal(col, row + 1) != Player.Invalid)
                && (GetVertical(col, row) != Player.Invalid)
                && (GetVertical(col + 1, row) != Player.Invalid))
            {
                RecordSquare(col, row);
                square = true;
            }

            if (GameOver)
                return;

            if (!square)
            {
                EndTurn();
            }
        }

        void RecordVertical(int col, int row)
        {
            System.Diagnostics.Debug.Assert(vLines[col, row] == Player.Invalid);
            System.Diagnostics.Debug.Assert(!GameOver);

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

            if (GameOver)
                return;

            if ((GetVertical(col + 1, row) != Player.Invalid)
                && (GetHorizontal(col, row) != Player.Invalid)
                && (GetHorizontal(col, row + 1) != Player.Invalid))
            {
                RecordSquare(col, row);
                square = true;
            }

            if (GameOver)
                return;

            if (!square)
            {
                EndTurn();
            }
        }

        void RecordSquare(int col, int row)
        {
            System.Diagnostics.Debug.Assert(squares[col, row] == Player.Invalid);
            System.Diagnostics.Debug.Assert(!GameOver);

            squares[col, row] = CurrentPlayer;
            scores[CurrentPlayer]++;

            int playsRemaining = 0;
            foreach (var s in squares)
            {
                if (s == Player.Invalid)
                {
                    playsRemaining++;
                }
            }

            int topScore = 0;
            int nextScore = 0;
            for (int i = 0; i < scores.Length; ++i)
            {
                if (scores[i] > topScore)
                {
                    nextScore = topScore;
                    topScore = scores[i];
                }
                else if (scores[i] > nextScore)
                {
                    nextScore = scores[i];
                }
            }

            if (playsRemaining == 0 || (topScore > (playsRemaining + nextScore)))
            {
                EndGame();
            }
        }


        public int CurrentPlayer { get; private set; }
        public bool GameOver { get; private set; }
        public ScoreBoard ScoreBoard { get { return new ScoreBoard(scores); } }

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

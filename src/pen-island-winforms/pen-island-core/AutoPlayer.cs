using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PenIsland
{
    static class DotsAutoPlayer
    {
        public static void MakeMove(DotsGame game)
        {
            // MakeFirstAvailable(game);
            MakeBestAvailable(game);
        }

        static void MakeFirstAvailable(DotsGame game)
        {
            LineInfo info = FindFirstAvailable(game);
            game.RecordMove(info);
        }

        static void MakeBestAvailable(DotsGame game)
        {
            LineInfo bestMove = LineInfo.Invalid;
            int bestScore = int.MinValue;

            // score all the moves, pick the best one
            for (int i = 0; i < game.Width; ++i)
            {
                for (int j = 0; j < game.Height; ++j)
                {
                    if (i < (game.Width - 1))
                    {
                        LineInfo move = new LineInfo(LineType.Horizontal, i, j);
                        if (game.GetMove(move) == Player.Invalid)
                        {
                            int score = ScoreMove(game, move);
                            if (score > bestScore)
                            {
                                bestScore = score;
                                bestMove = move;
                            }
                        }
                    }

                    if (j < (game.Height - 1))
                    {
                        LineInfo move = new LineInfo(LineType.Vertical, i, j);
                        if (game.GetMove(move) == Player.Invalid)
                        {
                            int score = ScoreMove(game, move);
                            if (score > bestScore)
                            {
                                bestScore = score;
                                bestMove = move;
                            }
                        }
                    }
                }
            }

            game.RecordMove(bestMove);
        }

        static int ScoreMove(DotsGame game, LineInfo move)
        {
            System.Diagnostics.Debug.Assert(game.IsValid(move));

            int completedSquares = 0;
            int openSquares = 0; 

            switch (move.LineType)
            {
                case LineType.Horizontal:
                    //  _
                    // | |   <-- missing line is 'move'
                    //
                    if ((move.Y - 1) >= 0)
                    {
                        int sides = 0;
                        sides += game.GetVertical(move.X, move.Y - 1) == Player.Invalid ? 0 : 1;
                        sides += game.GetVertical(move.X + 1, move.Y - 1) == Player.Invalid ? 0 : 1;
                        sides += game.GetHorizontal(move.X, move.Y - 1) == Player.Invalid ? 0 : 1;

                        if (sides == 3)
                        {
                            // circle takes the square
                            completedSquares++;
                        }

                        if (sides == 2)
                        {
                            // leaves a square for the next move (could be us!)
                            openSquares++;
                        }
                    }

                    // 
                    // |_|  <-- missing line is 'move'
                    //
                    if ((move.Y + 1) < game.Height)
                    {
                        int sides = 0;
                        sides += game.GetVertical(move.X, move.Y) == Player.Invalid ? 0 : 1;
                        sides += game.GetVertical(move.X + 1, move.Y) == Player.Invalid ? 0 : 1;
                        sides += game.GetHorizontal(move.X, move.Y + 1) == Player.Invalid ? 0 : 1;

                        if (sides == 3)
                        {
                            // circle takes the square
                            completedSquares++;
                        }

                        if (sides == 2)
                        {
                            // leaves a square for the next move (could be us!)
                            openSquares++;
                        }
                    }

                    break;
                case LineType.Vertical:
                    //  _ 
                    // |_  <-- missing line is 'move'
                    //
                    if ((move.X - 1) >= 0)
                    {
                        int sides = 0;
                        sides += game.GetHorizontal(move.X - 1, move.Y) == Player.Invalid ? 0 : 1;
                        sides += game.GetHorizontal(move.X - 1, move.Y + 1) == Player.Invalid ? 0 : 1;
                        sides += game.GetVertical(move.X - 1, move.Y) == Player.Invalid ? 0 : 1;

                        if (sides == 3)
                        {
                            // circle takes the square
                            completedSquares++;
                        }

                        if (sides == 2)
                        {
                            // leaves a square for the next move (could be us!)
                            openSquares++;
                        }
                    }


                    //  _
                    //  _|   <-- missing line is 'move'
                    //
                    if ((move.X + 1) < game.Width)
                    {
                        int sides = 0;
                        sides += game.GetHorizontal(move.X, move.Y) == Player.Invalid ? 0 : 1;
                        sides += game.GetHorizontal(move.X, move.Y + 1) == Player.Invalid ? 0 : 1;
                        sides += game.GetVertical(move.X + 1, move.Y) == Player.Invalid ? 0 : 1;
                        
                        if (sides == 3)
                        {
                            // circle takes the square
                            completedSquares++;
                        }

                        if (sides == 2)
                        {
                            // leaves a square for the next move (could be us!)
                            openSquares++;
                        }
                    }

                    break;
            }


            int score = 0; // 0 means it isn't a determinmental move

            if (completedSquares == 2)
                score = 10;
            else if (completedSquares == 1 && openSquares == 1)
                score = 10;
            else if (completedSquares == 1)
                score = 5;
            else if (openSquares == 1)
                score = -5;
            else if (openSquares == 2)
                score = -10;

            return score;
        }

        static LineInfo FindFirstAvailable(DotsGame game)
        {
            return FindNextAvailable(game, LineInfo.Invalid);
        }

        static LineInfo FindNextAvailable(DotsGame game, LineInfo lastLine)
        {
            // find the line past in before returning an availabe move
            bool searching = lastLine.LineType != LineType.None;

            for (int i = 0; i < game.Width; ++i)
            {
                for (int j = 0; j < game.Height; ++j)
                {
                    if (i < (game.Width - 1))
                    {
                        if (game.GetHorizontal(i, j) == Player.Invalid)
                        {
                            if (searching)
                            {
                                searching = false;
                                continue;
                            }

                            return new LineInfo(LineType.Horizontal, i, j);
                        }
                    }

                    if (j < (game.Height - 1))
                    {
                        if (game.GetVertical(i, j) == Player.Invalid)
                        {
                            if (searching)
                            {
                                searching = false;
                                continue;
                            }
                            
                            return new LineInfo(LineType.Vertical, i, j);
                        }
                    }
                }
            }

            return LineInfo.Invalid;
        }   
    }

    class GameTree
    {
        public TttGame game;
        public GameTreeNode head;
        public GameTree(TttGame game)
        {
            this.game = game;
            head = new GameTreeNode(game.CloneState());
        }
    }

    class GameTreeNode
    {
        public GameTreeNode(TttGameState state) {
            this.state = state;
        }
        TttGameState state;
        public List<GameTreeNode> Children = new List<GameTreeNode>();
    }

    class TttAutoPlayer
    {

        public static void MakeMove(TttGame game)
        {
            GameTree tree = new GameTree(game);
            ExpandOneLevel(tree.head);
        }

        static void ExpandOneLevel(GameTreeNode node)
        {
            
        }
    }

}

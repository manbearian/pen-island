using System;
using System.Collections.Generic;

namespace PenIsland
{
    static class DotsAutoPlayer
    {
        public static void MakeMove(DotsGame game)
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

    class GameTree<GameState>
    {
        public GameTreeNode<GameState> head;
        public GameTree(GameState state)
        {
            head = new GameTreeNode<GameState>(state);
        }
    }

    class GameTreeNode<GameState>
    {
        public GameTreeNode(GameState state)
        {
            State = state;
        }
        public GameState State;
        public List<GameTreeNode<GameState>> Children = new List<GameTreeNode<GameState>>();
        public int Score = 0;
    }

    class TttAutoPlayer
    {
        public static TttGame.Move StateDiff(TttGame game, TttGame.State before, TttGame.State after)
        {
            TttGame.Move? move = null;
            for (int i = 0; i < game.Width; ++i)
            {
                for (int j = 0; j < game.Height; ++j)
                {
                    if (after[i, j] != before[i, j])
                    {
                        // ensure the only delta is a single new move
                        System.Diagnostics.Debug.Assert(after[i, j] != Player.Invalid);
                        System.Diagnostics.Debug.Assert(before[i, j] == Player.Invalid);

                        // this function is only called to figure out what move the current player played
                        System.Diagnostics.Debug.Assert(after[i, j] == game.CurrentPlayer);

#if DEBUG
                        System.Diagnostics.Debug.Assert(move == null);
                        move = new TttGame.Move(i, j);
#else
                        break;
#endif
                    }
                }
            }

            System.Diagnostics.Debug.Assert(move != null);
            return move.Value;
        }

        public static void MakeMove(TttGame game)
        {
            var tree = new GameTree<TttGame.State>(game.CloneState());

            // TODO: this is a really bad idea for anything larger than 3x3 board
            ExpandAll(game, tree);

            TttGame.Move? bestMove = null;
            int bestScore = int.MinValue;
            foreach (var child in tree.head.Children)
            {
                var move = StateDiff(game, tree.head.State, child.State);

                if (child.Score > bestScore)
                {
                    bestMove = move;
                    bestScore = child.Score;
                }
            }

            game.RecordMove(bestMove.Value);
        }

        static void ExpandAll(TttGame game, GameTree<TttGame.State> tree)
        {
            ExpandAll(game, tree, tree.head, 0);
        }

        static void ExpandAll(TttGame game, GameTree<TttGame.State> tree, GameTreeNode<TttGame.State> node, int depth)
        {
            var playerAtDepth = (game.CurrentPlayer + depth) % game.PlayerCount;

            var winner = game.CheckWinner(node.State);
            if (winner != null)
            {
                if (winner == game.CurrentPlayer)
                    node.Score = 10 - depth;
                else if (winner == Player.Invalid)
                    node.Score = 0;
                else
                    node.Score = depth - 10;
                return;
            }

            for (int i = 0; i < game.Width; ++i)
            {
                for (int j = 0; j < game.Height; ++j)
                {
                    if (node.State[i, j] == Player.Invalid)
                    {
                        var nextState = node.State.Clone();
                        nextState[i, j] = playerAtDepth;
                        node.Children.Add(new GameTreeNode<TttGame.State>(nextState));
                    }
                }
            }

            System.Diagnostics.Debug.Assert(node.Children.Count > 0, "there should be one move remaining or we wouldn't have recursed");

            var depthPlayerIsMe = playerAtDepth == game.CurrentPlayer;
            int score = depthPlayerIsMe ? int.MinValue : int.MaxValue;
            foreach (var child in node.Children)
            {
                ExpandAll(game, tree, child, depth + 1);
                if (depthPlayerIsMe)
                    score = Math.Max(score, child.Score);
                else
                    score = Math.Min(score, child.Score);
            }
            node.Score = score;
        }
    }

    class C4AutoPlayer
    {
        public static C4Game.Move StateDiff(C4Game game, C4Game.State before, C4Game.State after)
        {
            C4Game.Move? move = null;
            for (int i = 0; i < game.Width; ++i)
            {
                for (int j = 0; j < game.Height; ++j)
                {
                    if (after[i, j] != before[i, j])
                    {
                        // ensure the only delta is a single new move
                        System.Diagnostics.Debug.Assert(after[i, j] != Player.Invalid);
                        System.Diagnostics.Debug.Assert(before[i, j] == Player.Invalid);

                        // this function is only called to figure out what move the current player played
                        System.Diagnostics.Debug.Assert(after[i, j] == game.CurrentPlayer);

#if DEBUG
                        System.Diagnostics.Debug.Assert(move == null);
                        move = new C4Game.Move(i, j);
#else
                        break;
#endif
                    }
                }
            }

            System.Diagnostics.Debug.Assert(move != null);
            return move.Value;
        }

        public static void MakeMove(C4Game game)
        {
            // MakeNextAvailableMove(game);

            GameTree<C4Game.State> tree = new GameTree<C4Game.State>(game.CloneState());

            Expand(game, tree, 2);

            C4Game.Move? bestMove = GetNextAvailableMove(game);

            int bestScore = int.MinValue;
            foreach (var child in tree.head.Children)
            {
                var move = StateDiff(game, tree.head.State, child.State);

                if (child.Score > bestScore)
                {
                    bestMove = move;
                    bestScore = child.Score;
                }
            }

            game.RecordMove(bestMove.Value);
        }

        static void Expand(C4Game game, GameTree<C4Game.State> tree, int limit)
        {
            Expand(game, tree, tree.head, limit, 0);
        }

        static void Expand(C4Game game, GameTree<C4Game.State> tree, GameTreeNode<C4Game.State> node, int limit, int depth)
        {
            System.Diagnostics.Debug.Assert(limit > 0);
            System.Diagnostics.Debug.Assert(game.CheckWinner(node.State) == null);

            var playerAtDepth = (game.CurrentPlayer + depth) % game.PlayerCount;
            var depthPlayerIsMe = playerAtDepth == game.CurrentPlayer;
            int score = depthPlayerIsMe ? int.MinValue : int.MaxValue;

            ExpandOneLevel(game, tree, node, depth++);

            foreach (var child in node.Children)
            {
                var winner = game.CheckWinner(child.State);

                if (winner == null && depth < limit)
                {
                    Expand(game, tree, child, limit, depth);
                }
                else if (winner != null)
                {
                    if (winner == game.CurrentPlayer)
                        child.Score = int.MaxValue - depth;
                    else if (winner == Player.Invalid)
                        child.Score = 0;
                    else
                        child.Score = int.MinValue + depth;
                }
                else
                {
                    System.Diagnostics.Debug.Assert(depth == limit);
                    child.Score = 0;
                }

                if (depthPlayerIsMe)
                    score = Math.Max(score, child.Score);
                else
                    score = Math.Min(score, child.Score);
            }

            node.Score = score;
        }

        static void ExpandOneLevel(C4Game game, GameTree<C4Game.State> tree, GameTreeNode<C4Game.State> node, int depth)
        {
            var state = node.State;
            var playerAtDepth = (game.CurrentPlayer + depth) % game.PlayerCount;

            for (int i = 0; i < game.Width; ++i)
            {
                for (int j = 0; j < game.Height; ++j)
                {
                    if ((node.State[i, j] != Player.Invalid)
                        || (j < game.Height - 1) && (state[i, j + 1] == Player.Invalid))
                    {
                        continue;
                    }

                    if (node.State[i, j] == Player.Invalid)
                    {
                        var nextState = node.State.Clone();
                        nextState[i, j] = playerAtDepth;
                        node.Children.Add(new GameTreeNode<C4Game.State>(nextState));
                    }
                }
            }
        }

        public static C4Game.Move GetNextAvailableMove(C4Game game)
        {
            for (int j = game.Height - 1; j >= 0; --j)
            {
                for (int i = 0; i < game.Width; ++i)
                {
                    var move = new C4Game.Move(i, j);
                    if (game.GetMove(move) == Player.Invalid)
                    {
                        return move;
                    }
                }
            }

            System.Diagnostics.Debug.Fail("Why are there no moves for me to make?!?");
            return C4Game.Move.Invalid;
        }
    }
}

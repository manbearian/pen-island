using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Move = PenIsland.C4Game.Move;
using State = PenIsland.C4Game.State;
using GameTree = PenIsland.GameTree<PenIsland.C4Game.State>;
using GameTreeNode = PenIsland.GameTreeNode<PenIsland.C4Game.State>;


namespace PenIsland
{
    class C4AutoPlayer
    {
        public static Move StateDiff(C4Game game, State before, State after)
        {
            Move? move = null;
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
                        move = new Move(i, j);
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

            GameTree tree = new GameTree(game.CloneState());

            Expand(game, tree, 2);

            Move? bestMove = GetNextAvailableMove(game);

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

        static void Expand(C4Game game, GameTree tree, int limit)
        {
            Expand(game, tree, tree.head, limit, 0);
        }

        static void Expand(C4Game game, GameTree tree, GameTreeNode node, int limit, int depth)
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

        static void ExpandOneLevel(C4Game game, GameTree tree, GameTreeNode node, int depth)
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
                        node.Children.Add(new GameTreeNode(nextState));
                    }
                }
            }
        }

        public static Move GetNextAvailableMove(C4Game game)
        {
            for (int j = game.Height - 1; j >= 0; --j)
            {
                for (int i = 0; i < game.Width; ++i)
                {
                    var move = new Move(i, j);
                    if (game.GetMove(move) == Player.Invalid)
                    {
                        return move;
                    }
                }
            }

            System.Diagnostics.Debug.Fail("Why are there no moves for me to make?!?");
            return Move.Invalid;
        }
    }
}

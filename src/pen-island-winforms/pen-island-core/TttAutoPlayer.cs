using PenIsland;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Game = PenIsland.TttGame;
using Move = PenIsland.TttGame.Move;
using State = PenIsland.TttGame.State;
using Tree = PenIsland.GameTree<PenIsland.TttGame.State>;
using TreeNode = PenIsland.GameTreeNode<PenIsland.TttGame.State>;


namespace PenIsland
{

    class TttAutoPlayer
    {
        public static Move StateDiff(Game game, State before, State after)
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

        public static void MakeMove(Game game)
        {
            var tree = new Tree(game.CloneState());

            // TODO: this is a really bad idea for anything larger than 3x3 board
            ExpandAll(game, tree);

            Move? bestMove = null;
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

        static void ExpandAll(Game game, Tree tree)
        {
            ExpandAll(game, tree, tree.head, 0);
        }

        static void ExpandAll(Game game, Tree tree, TreeNode node, int depth)
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
                        node.Children.Add(new TreeNode(nextState));
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
}

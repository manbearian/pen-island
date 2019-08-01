using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Game = PenIsland.C4Game;
using Move = PenIsland.C4Game.Move;
using State = PenIsland.C4Game.State;
//using GameTree = PenIsland.GameTree<PenIsland.C4Game.State>;
//using GameTreeNode = PenIsland.GameTreeNode<PenIsland.C4Game.State>;

namespace PenIsland
{

    class GameTree
    {
        public GameTree(Game game) { Game = game; }
        public Game Game { get; }
        public List<GameTreeNode> Children = new List<GameTreeNode>();

        public void Expand(int depthLimit)
        {
            Expand(depthLimit, null, Game.CloneState(), 0);
        }

        private void Expand(int depthLimit, GameTreeNode node, State state, int depth)
        {
            System.Diagnostics.Debug.Assert(depthLimit > 0);

            if (node != null)
            {
                var winner = Game.CheckWinner(state);

                // Check to end recursion

                if (winner != null)
                {
                    if (winner == Game.CurrentPlayer)
                    {
                        node.Score = int.MaxValue - depth; // win
                    }
                    else if (winner == Player.Invalid)
                    {
                        node.Score = 0; // draw
                    }
                    else
                    {
                        node.Score = int.MinValue + depth; // lose
                    }
                    return;
                }
                else if (depth == depthLimit)
                {
                    node.Score = 0;
                    return;
                }
            }

            var children = node != null ? node.Children : Children;
            var playerAtDepth = (Game.CurrentPlayer + depth) % Game.PlayerCount;

            for (int i = 0; i < Game.Width; ++i)
            {
                for (int j = 0; j < Game.Height; ++j)
                {
                    var move = new Move(i, j);

                    if ((state[move] != Player.Invalid)
                        || (j < Game.Height - 1) && ((state[i, j + 1]) == Player.Invalid))
                    {
                        continue;
                    }

                    if (state[move] == Player.Invalid)
                    {
                        var child = new GameTreeNode(move);
                        children.Add(child);
                        state[move] = playerAtDepth;
                        Expand(depthLimit, child, state, depth + 1);
                        state[move] = Player.Invalid;
                    }
                }
            }

            System.Diagnostics.Debug.Assert(children.Count > 0, "there should have been an available move");

            // Score the node

            if (node != null)
            {
                var depthPlayerIsMe = playerAtDepth == Game.CurrentPlayer;
                int score = depthPlayerIsMe ? int.MinValue : int.MaxValue;
                foreach (var child in node.Children)
                {
                    if (depthPlayerIsMe)
                        score = Math.Max(score, child.Score);
                    else
                        score = Math.Min(score, child.Score);
                }
                node.Score = score;
                node.Children = null; // free the memory, we're done with it
            }
        }
    }

    class GameTreeNode
    {
        public GameTreeNode(Move move) { Move = move; }
        public Move Move { get; }
        public int Score;
        public List<GameTreeNode> Children = new List<GameTreeNode>();
    }

    class C4AutoPlayer
    {
        public static void MakeMove(C4Game game)
        {
            // MakeNextAvailableMove(game);

            GameTree tree = new GameTree(game);

            //Expand(tree, 2);
            tree.Expand(6);

            Move bestMove = Move.Invalid;
            int bestScore = int.MinValue;
            foreach (var child in tree.Children)
            {
                if (child.Score > bestScore)
                {
                    bestMove = child.Move;
                    bestScore = child.Score;
                }
                else if (child.Score == bestScore)
                {
                    // TODO: Handle the case where the expanded tree doesn't contain a winner
                    // TODO:    #1 prefer to connect with other pieces
                    // TODO:    #2 prefer to create line with open spaces on both ends
                    // TOOD: or now prefer middle columns (more likely to fulfill #2)
                    int midpoint = game.Width / 2;
                    int adjustedBestScore = midpoint - Math.Abs(bestMove.X - midpoint);
                    int adjustedChildScore = midpoint - Math.Abs(child.Move.X - midpoint); 

                    if (adjustedChildScore > adjustedBestScore)
                    {
                        bestMove = child.Move;
                    }
                }
            }

            game.RecordMove(bestMove);
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

using PenIsland;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Game = PenIsland.TttGame;
using Move = PenIsland.TttGame.Move;
using State = PenIsland.TttGame.State;
using GameTree = PenIsland.TttGameTree;


namespace PenIsland
{
    class TttGameTree
    {
        class Node
        {
            public Node(Move move) { Move = move; }
            public Move Move { get; }
            public int Score;
            public List<Node> Children = new List<Node>();
        }
        public TttGameTree(Game game) { Game = game; }

        Game Game { get; }
        List<Node> Children = new List<Node>();

        public Move GetBestMove()
        {
            Move bestMove = Move.Invalid;
            int bestScore = int.MinValue;

            System.Diagnostics.Debug.Assert(Children.Count > 0, "Invoke 'Expand' before calling this method");

            foreach (var child in Children)
            {
                if (child.Score > bestScore)
                {
                    bestMove = child.Move;
                    bestScore = child.Score;
                }
                else if (child.Score == bestScore)
                {
                    // TODO: Handle the case where the expanded tree doesn't contain a winner
                    // TODO:    #1 prefer a move that will win if opponent doesn't make best play
                    // TODO:    #2 prefer a move that gets closer to a win
                    continue;
                }
            }

            System.Diagnostics.Debug.Assert(bestMove != Move.Invalid);
            return bestMove;
        }

        public void Expand(int depthLimit)
        {
            Expand(depthLimit, null, Game.CloneState(), 0);
        }

        private void Expand(int depthLimit, Node node, State state, int depth)
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
                for (int j = Game.Height - 1; j >= 0; --j)
                {
                    var move = new Move(i, j);

                    if (state[move] == Player.Invalid)
                    {
                        var child = new Node(move);
                        children.Add(child);
                        state[move] = playerAtDepth;
                        Expand(depthLimit, child, state, depth + 1);
                        state[move] = Player.Invalid;

                        // there's only one move per column
                        break;
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

    class TttAutoPlayer
    {
        public static void MakeMove(Game game)
        {
            GameTree tree = new GameTree(game);

            tree.Expand(8);

            Move bestMove = tree.GetBestMove();

            game.RecordMove(bestMove);
        }
    }
}

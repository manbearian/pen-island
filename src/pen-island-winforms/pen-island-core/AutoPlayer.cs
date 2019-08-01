using System;
using System.Collections.Generic;

namespace PenIsland
{
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


}

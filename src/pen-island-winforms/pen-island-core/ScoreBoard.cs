using System;
using System.Collections.Generic;

namespace PenIsland
{
    public struct ScoreBoard
    {
        readonly private int[] scores;

        public ScoreBoard(int [] scores)
        {
            this.scores = scores;
        }

        public int this[int player]
        {
            get { return scores[player]; }
        }
    }
}

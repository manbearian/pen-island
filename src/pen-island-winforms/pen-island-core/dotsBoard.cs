using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PenIsland
{
    public partial class DotsBoard : UserControl, GameBoard
    {
        LineInfo selectedLine = LineInfo.Invalid;

        internal DotsGame DotsGame { get; private set; }

        Color[] playerColors = new Color[Player.MaxPlayers];

        public DotsBoard()
        {
            InitializeComponent();
        }

        public void NewGame()
        {
            DotsGame = new DotsGame(DotsGameSettings.PlayerCount, DotsGameSettings.BoardType, DotsGameSettings.BoardWidth, DotsGameSettings.BoardHeight);
            
            ClientSize = GetPreferedWindowSize();
            Refresh();

            RunComputerPlayers();
        }

        // default size:
        //   |<-- 20 pixels --> x <-- 40 pixels --> x ... x <-- 25 pixels ->|
        //   spacing is same horizontal and vertical
        //   "x" is 5x5 pixel dot
        // total length is countof(x) * 40 + 5

        static readonly int PreferedDotSize = 5;
        static readonly int PreferedSpacer = 40;
        static readonly int PreferedBorder = PreferedSpacer / 2;

        public Size GetPreferedWindowSize()
        {
            return new Size(DotsGame.Width * PreferedSpacer + PreferedDotSize, DotsGame.Height * PreferedSpacer + PreferedDotSize);
        }
        
        private void DotsBoard_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.FillRectangle(Brushes.White, this.ClientRectangle);

            if (DotsGame == null)
                return;

            // draw the dots
            for (int i = 0; i < DotsGame.Width; ++i)
            {
                for (int j = 0; j < DotsGame.Height; ++j)
                {
                    var x = PreferedBorder + i * PreferedSpacer;
                    var y = PreferedBorder + j * PreferedSpacer;
                    var d = PreferedDotSize;
                    g.FillEllipse(Brushes.Black, new Rectangle(x, y, d, d));
                }
            }

            // draw the state

            for (int i = 0; i < DotsGame.Width; ++i)
            {
                for (int j = 0; j < DotsGame.Height; ++j)
                {
                    int player = Player.Invalid;
                    int hStart = PreferedBorder + i * PreferedSpacer + PreferedDotSize / 2;
                    int vStart = PreferedBorder + j * PreferedSpacer + PreferedDotSize / 2;

                    // draw squares first so lines end up on top of them
                    player = DotsGame.GetSquare(i, j);
                    if (player != Player.Invalid)
                    {
                        var brush = new SolidBrush(PlayerSettings.GetPlayerColor(player));
                        g.FillRectangle(brush, hStart, vStart, PreferedSpacer, PreferedSpacer);
                    }

                    player = DotsGame.GetHorizontal(i, j);
                    if (player != Player.Invalid)
                    {
                        Pen pen = new Pen(PlayerSettings.GetPlayerColor(player));
                        g.DrawLine(pen, new Point(hStart, vStart), new Point(hStart + PreferedSpacer, vStart));
                    }

                    player = DotsGame.GetVertical(i, j);
                    if (player != Player.Invalid)
                    {
                        Pen pen = new Pen(PlayerSettings.GetPlayerColor(player));
                        g.DrawLine(pen, new Point(hStart, vStart), new Point(hStart, vStart + PreferedSpacer));
                    }
                }
            }

            // draw the current selection

            int hSelStart = PreferedBorder + selectedLine.X * PreferedSpacer + PreferedDotSize / 2;
            int vSelStart = PreferedBorder + selectedLine.Y * PreferedSpacer + PreferedDotSize / 2;
            switch (selectedLine.LineType)
            {
                case LineType.Horizontal:
                    g.DrawLine(Pens.Aquamarine, new Point(hSelStart, vSelStart), new Point(hSelStart + PreferedSpacer, vSelStart));
                    break;
                case LineType.Vertical:
                    g.DrawLine(Pens.Aquamarine, new Point(hSelStart, vSelStart), new Point(hSelStart, vSelStart + PreferedSpacer));
                    break;
            }
        }

        private void DotsBoard_Resize(object sender, EventArgs e)
        {
            // repaint on resize -- not needed yet, but in place for when i get scaling working
            Refresh();
        }

        private void DotsBoard_MouseClick(object sender, MouseEventArgs e)
        {
            HandleClick(e);
        }

        private void DotsBoard_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            HandleClick(e);
        }

        private void HandleClick(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            if (DotsGame == null || DotsGame.GameOver || PlayerSettings.GetPlayerController(DotsGame.CurrentPlayer) == PlayerController.Computer)
                return;

            LineInfo clickedLine = new LineInfo();

            // first row of dots starts at `PreferedBorder` and is `PreferedDotSize` wide
            // give a few pixes of fudge to either side. Use PreferedDotSize/2 as fudge.
            int fudge = PreferedDotSize / 2;

            for (int i = 0; i < DotsGame.Width; ++i)
            {
                if (e.X > PreferedBorder + i * PreferedSpacer - fudge
                    && e.X < PreferedBorder + i * PreferedSpacer + PreferedDotSize + fudge)
                {
                    clickedLine.LineType = LineType.Vertical;
                    clickedLine.X = i;
                    break;
                }

                if (e.X > PreferedBorder + i * PreferedSpacer + PreferedDotSize)
                {
                    clickedLine.X = i;
                }
            }

            for (int i = 0; i < DotsGame.Height; ++i)
            {
                if (e.Y > PreferedBorder + i * PreferedSpacer - fudge
                    && e.Y < PreferedBorder + i * PreferedSpacer + PreferedDotSize + fudge)
                {
                    if (clickedLine.LineType == LineType.Vertical)
                    {
                        clickedLine.LineType = LineType.None;
                        break;
                    }

                    clickedLine.LineType = LineType.Horizontal;
                    clickedLine.Y = i;
                    break;
                }

                if (e.Y > PreferedBorder + i * PreferedSpacer + PreferedDotSize)
                {
                    clickedLine.Y = i;
                }
            }

            // don't allow selection of already played moves
            switch (clickedLine.LineType)
            {
                case LineType.Horizontal:
                    if (DotsGame.GetHorizontal(clickedLine.X, clickedLine.Y) != Player.Invalid)
                    {
                        clickedLine.LineType = LineType.None;
                    }
                    break;
                case LineType.Vertical:
                    if (DotsGame.GetVertical(clickedLine.X, clickedLine.Y) != Player.Invalid)
                    {
                        clickedLine.LineType = LineType.None;
                    }
                    break;
            }

            if ((clickedLine.LineType == selectedLine.LineType && clickedLine.X == selectedLine.X && clickedLine.Y == selectedLine.Y)
                || (e.Clicks > 1))
            {
                if (clickedLine.LineType != LineType.None)
                {
                    DotsGame.RecordMove(clickedLine);
                }

                selectedLine.LineType = LineType.None;
            }
            else
            {
                selectedLine.LineType = clickedLine.LineType;
                selectedLine.X = clickedLine.X;
                selectedLine.Y = clickedLine.Y;
            }

            Parent.Refresh();

            RunComputerPlayers();
        }

        void RunComputerPlayers()
        {
            if (DotsGame.GameOver)
                return;

            if (PlayerSettings.GetPlayerController(DotsGame.CurrentPlayer) != PlayerController.Computer)
            {
                return;
            }

            DotsAutoPlayer.MakeMove(DotsGame);
            
            Parent.Refresh();

            RunComputerPlayers();
        }

        public void GetStatusMessage(out string message, out Color color)
        {
            message = "";
            color = Color.Black;

            if (DotsGame == null) {
                return; 
            }
            
            if (!DotsGame.GameOver)
            {
                var player = DotsGame.CurrentPlayer;
                color = PlayerSettings.GetPlayerColor(player);
                message = string.Format("Player {0}", player + 1);
            }
            else
            {
                var score = DotsGame.ScoreBoard;
                List<int> winners = new List<int>();
                int topScore = 0;
                for (int i = 0; i < DotsGame.PlayerCount; ++i)
                {
                    if (score[i] > topScore)
                    {
                        winners.Clear();
                        winners.Add(i);
                        topScore = score[i];
                    }
                    else if (score[i] == topScore)
                    {
                        winners.Add(i);
                    }
                }

                if (winners.Count == DotsGame.PlayerCount)
                {
                    // tie game (no winner)
                    message = string.Format("Game Over, It's a Draw!");
                }
                else if (winners.Count > 1)
                {
                    // tie game (multiple winners)
                    message = string.Format("Game Over, Multi-winner!");
                }
                else
                {
                    var winner = winners[0];
                    color = PlayerSettings.GetPlayerColor(winner);
                    message = string.Format("Game Over, Player {0} Wins!", winner + 1);
                }
            }
        }
    }
}

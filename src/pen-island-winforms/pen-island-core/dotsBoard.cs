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
    public partial class DotsBoard : UserControl
    {
        enum LineType { None, Horizontal, Vertical };
        LineType selectedType = LineType.None;
        int selectedCol = -1;
        int selectedRow = -1;

        public DotsGame DotsGame { get; private set; }

        Color[] playerColors = new Color[Player.MaxPlayers];

        internal DotsGameSettings Settings { get; }

        public DotsBoard()
        {
            InitializeComponent();

            SetPlayerColor(0, Color.Red);
            SetPlayerColor(1, Color.Blue);
            SetPlayerColor(2, Color.Green);
            SetPlayerColor(3, Color.Purple);
            SetPlayerColor(4, Color.Orange);
            SetPlayerColor(5, Color.Silver);
            SetPlayerColor(6, Color.Pink);
            SetPlayerColor(7, Color.Olive);
            SetPlayerColor(8, Color.Navy);

            Settings = new DotsGameSettings();
            Settings.PlayerCount = 2;
            Settings.ComputerPlayers.Initialize();

            Settings.BoardType = DotsBoardType.Squares;
            Settings.Height = 5;
            Settings.Width = 5;
        }

        public void NewGame()
        {
            DotsGame = new DotsGame(Settings.PlayerCount, Settings.Width, Settings.Height);

            ClientSize = GetPreferedWindowSize();
            Refresh();
        }

        static readonly int PreferedDotSize = 5;
        static readonly int PreferedSpacer = 40;
        static readonly int PreferedBorder = PreferedSpacer / 2;

        // default size:
        //   |<-- 20 pixels --> x <-- 40 pixels --> x ... x <-- 25 pixels ->|
        //   spacing is same horizontal and vertical
        //   "x" is 5x5 pixel dot
        // total length is countof(x) * 40 + 5

        public Size GetPreferedWindowSize()
        {
            return new Size(DotsGame.Width * PreferedSpacer + PreferedDotSize, DotsGame.Height * PreferedSpacer + PreferedDotSize);
        }

        public Color GetPlayerColor(int i)
        {
            return playerColors[i];
        }

        public void SetPlayerColor(int i, Color c)
        {
            playerColors[i] = c;
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
                        var brush = new SolidBrush(GetPlayerColor(player));
                        g.FillRectangle(brush, hStart, vStart, PreferedSpacer, PreferedSpacer);
                    }

                    player = DotsGame.GetHorizontal(i, j);
                    if (player != Player.Invalid)
                    {
                        Pen pen = new Pen(GetPlayerColor(player));
                        g.DrawLine(pen, new Point(hStart, vStart), new Point(hStart + PreferedSpacer, vStart));
                    }

                    player = DotsGame.GetVertical(i, j);
                    if (player != Player.Invalid)
                    {
                        Pen pen = new Pen(GetPlayerColor(player));
                        g.DrawLine(pen, new Point(hStart, vStart), new Point(hStart, vStart + PreferedSpacer));
                    }
                }
            }

            // draw the current selection

            int hSelStart = PreferedBorder + selectedCol * PreferedSpacer + PreferedDotSize / 2;
            int vSelStart = PreferedBorder + selectedRow * PreferedSpacer + PreferedDotSize / 2;
            switch (selectedType)
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

            if (DotsGame == null || DotsGame.GameOver)
                return;

            LineType clickedType = LineType.None;
            int clickedCol = 0;
            int clickedRow = 0;

            // first row of dots starts at `PreferedBorder` and is `PreferedDotSize` wide
            // give a few pixes of fudge to either side. Use PreferedDotSize/2 as fudge.
            int fudge = PreferedDotSize / 2;

            for (int i = 0; i < DotsGame.Width; ++i)
            {
                if (e.X > PreferedBorder + i * PreferedSpacer - fudge
                    && e.X < PreferedBorder + i * PreferedSpacer + PreferedDotSize + fudge)
                {
                    clickedType = LineType.Vertical;
                    clickedCol = i;
                    break;
                }

                if (e.X > PreferedBorder + i * PreferedSpacer + PreferedDotSize)
                {
                    clickedCol = i;
                }
            }

            for (int i = 0; i < DotsGame.Height; ++i)
            {
                if (e.Y > PreferedBorder + i * PreferedSpacer - fudge
                    && e.Y < PreferedBorder + i * PreferedSpacer + PreferedDotSize + fudge)
                {
                    if (clickedType == LineType.Vertical)
                    {
                        clickedType = LineType.None;
                        break;
                    }

                    clickedType = LineType.Horizontal;
                    clickedRow = i;
                    break;
                }

                if (e.Y > PreferedBorder + i * PreferedSpacer + PreferedDotSize)
                {
                    clickedRow = i;
                }
            }

            // don't allow selection of already played moves
            switch (clickedType)
            {
                case LineType.Horizontal:
                    if (DotsGame.GetHorizontal(clickedCol, clickedRow) != Player.Invalid)
                    {
                        clickedType = LineType.None;
                    }
                    break;
                case LineType.Vertical:
                    if (DotsGame.GetVertical(clickedCol, clickedRow) != Player.Invalid)
                    {
                        clickedType = LineType.None;
                    }
                    break;
            }

            if ((clickedType == selectedType && clickedRow == selectedRow && clickedCol == selectedCol)
                || (e.Clicks > 1))
            {
                switch (clickedType)
                {
                    case LineType.Horizontal:
                        DotsGame.RecordHorizontal(selectedCol, selectedRow);
                        break;
                    case LineType.Vertical:
                        DotsGame.RecordVertical(selectedCol, selectedRow);
                        break;
                }

                selectedType = LineType.None;
            }
            else
            {
                selectedType = clickedType;
                selectedRow = clickedRow;
                selectedCol = clickedCol;
            }

            Refresh();
        }
    }
}

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
    public partial class TttBoard : UserControl, GameBoard
    {
        internal TttGame Game { get; private set; }

        Point? selectedPoint;

        public TttBoard()
        {
            InitializeComponent();
        }

        public void NewGame()
        {
            Game = new TttGame(TttGameSettings.PlayerCount, TttGameSettings.BoardWidth, TttGameSettings.BoardHeight, TttGameSettings.WinLength);

            ClientSize = GetPreferedWindowSize();
            Refresh();

            RunComputerPlayers();
        }

        enum PlayerGlyph
        {
            X, O 
        }

        PlayerGlyph GetPlayerGlyph(int player)
        {
            switch(player)
            {
                case 0: return PlayerGlyph.X;
                case 1: return PlayerGlyph.O;
                default: throw new Exception("unknown player");
            }
        }

        // default size:
        //   <-- 25 pixels --> <-- 50 pixels --> | <-- 50 pixels --> | <-- 50 pixels --> <-- 25 pixels -->
        //   spacing is same horizontal and vertical

        static readonly int PreferedGrid = 50;
        static readonly int PreferedBorder = PreferedGrid / 2;
        static readonly int PreferedSpacer = 5; // spacer between glyph and grid

        public Size GetPreferedWindowSize()
        {
            //return new Size(DotsGame.Width * PreferedSpacer + PreferedDotSize, DotsGame.Height * PreferedSpacer + PreferedDotSize);
            return new Size((PreferedGrid * Game.Width) + PreferedBorder * 2, (PreferedGrid * Game.Height) + PreferedBorder * 2);
        }

        private void DrawMove(Graphics g, Point location, PlayerGlyph glyph, Color color)
        {
            int x = PreferedBorder + PreferedGrid * location.X;
            int y = PreferedBorder + PreferedGrid * location.Y;

            Pen pen = new Pen(color);

            switch (glyph)
            {
                case PlayerGlyph.X:
                    g.DrawLine(pen, new Point(x + PreferedSpacer, y + PreferedSpacer),
                        new Point(x + PreferedGrid - PreferedSpacer, y + PreferedGrid - PreferedSpacer));
                    g.DrawLine(pen, new Point(x + PreferedGrid - PreferedSpacer, y + PreferedSpacer),
                        new Point(x + PreferedSpacer, y + PreferedGrid - PreferedSpacer));
                    break;

                case PlayerGlyph.O:
                    g.DrawEllipse(pen, x + PreferedSpacer, y + PreferedSpacer, PreferedGrid - 2 * PreferedSpacer, PreferedGrid - 2 * PreferedSpacer);
                    break;

                default:
                    throw new Exception("unknown player");
            }
        }

        private void TTTBoard_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            g.FillRectangle(Brushes.White, this.ClientRectangle);

            if (Game == null)
                return;

            // debugging variable allowing drawing a box around the board so every space is closed squre
            bool drawBorder = false;

            // draw the vertical lines
            int x = PreferedBorder + (drawBorder ? 0 : PreferedGrid);
            for (int i = 0; i < Game.Width + (drawBorder ? 1 : -1); ++i, x += PreferedGrid)
            {
                g.DrawLine(Pens.Black, new Point(x, PreferedBorder), new Point(x, PreferedBorder + (PreferedGrid * Game.Height)));
            }

            // draw the horizontal lines
            int y = PreferedBorder + (drawBorder ? 0 : PreferedGrid);
            for (int j = 0; j < Game.Height + (drawBorder ? 1 : -1); ++j, y += PreferedGrid)
            {
                g.DrawLine(Pens.Black, new Point(PreferedBorder, y), new Point(PreferedBorder + (PreferedGrid * Game.Width), y));
            }

            if (selectedPoint != null)
            {
                DrawMove(g, selectedPoint.Value, GetPlayerGlyph(Game.CurrentPlayer), Color.Aquamarine);
            }

            for (int i = 0; i < Game.Width; ++i)
            {
                for (int j = 0; j < Game.Height; ++j)
                {
                    int checkPlayer = Game.GetMove(new MoveInfo(i, j));
                    if (checkPlayer != Player.Invalid)
                    {
                        DrawMove(g, new Point(i, j), GetPlayerGlyph(checkPlayer), PlayerSettings.GetPlayerColor(checkPlayer));
                    }
                }
            }
        }

        private void TttBoard_MouseClick(object sender, MouseEventArgs e)
        {
            HandleClick(e);
        }

        private void TttBoard_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            HandleClick(e);
        }

        private void HandleClick(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            if (Game == null || Game.GameOver || PlayerSettings.GetPlayerController(Game.CurrentPlayer) == PlayerController.Computer)
                return;

            Point clickedPoint = new Point(-1, -1);

            int x = PreferedBorder;
            for (int i = 0; i < Game.Width; ++i, x += PreferedGrid)
            {
                if (e.X > x && e.X < (x + PreferedGrid))
                {
                    clickedPoint.X = i;
                    break;
                }
            }

            int y = PreferedBorder;
            for (int i = 0; i < Game.Height; ++i, y += PreferedGrid)
            {
                if (e.Y > y && e.Y < (y + PreferedGrid))
                {
                    clickedPoint.Y = i;
                    break;
                }
            }

            MoveInfo moveInfo = new MoveInfo(clickedPoint.X, clickedPoint.Y);

            if (clickedPoint.X >= 0 && clickedPoint.Y >= 0)
            {
                if (Game.GetMove(moveInfo) != Player.Invalid)
                {
                    clickedPoint = new Point(-1, -1);
                }
            }

            if (clickedPoint.X >= 0 && clickedPoint.Y >= 0)
            {
                if ((selectedPoint != null && clickedPoint == selectedPoint) || e.Clicks > 1)
                {
                    Game.RecordMove(moveInfo);
                    selectedPoint = null;
                }
                else
                {
                    selectedPoint = clickedPoint;
                }
            }
            else
            {
                selectedPoint = null;
            }

            Parent.Refresh();

            RunComputerPlayers();
        }

        void RunComputerPlayers()
        {
            if (Game.GameOver)
                return;

            if (PlayerSettings.GetPlayerController(Game.CurrentPlayer) != PlayerController.Computer)
            {
                return;
            }

            TttAutoPlayer.MakeMove(Game);

            Parent.Refresh();

            RunComputerPlayers();
        }

        public void GetStatusMessage(out string message, out Color color)
        {
            message = "";
            color = Color.Black;

            if (Game == null)
            {
                return;
            }

            if (!Game.GameOver)
            {
                var player = Game.CurrentPlayer;
                color = PlayerSettings.GetPlayerColor(player);
                message = string.Format("{0}'s turn", GetPlayerGlyph(player).ToString());
            }
            else
            {
                var player = Game.Winner;
                if (player == Player.Invalid)
                {
                    message = string.Format("Cat's Game!");
                }
                else
                {
                    color = PlayerSettings.GetPlayerColor(player);
                    message = string.Format("{0}'s win!", GetPlayerGlyph(player).ToString());
                }
            }
        }

        }
}
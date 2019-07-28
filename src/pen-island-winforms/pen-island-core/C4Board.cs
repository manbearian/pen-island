using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Move = PenIsland.C4Game.Move;

namespace PenIsland
{
    public partial class C4Board : UserControl, GameBoard
    {
        internal C4Game Game { get; private set; }

        Move? selectedMove;

        public C4Board()
        {
            InitializeComponent();
        }

        public void NewGame()
        {
            //Game = new C4Game(C4GameSettings.PlayerCount, C4GameSettings.BoardWidth, C4GameSettings.BoardHeight);
            Game = new C4Game(2, 7, 6, 4);

            ClientSize = GetPreferedWindowSize();
            Refresh();

            RunComputerPlayers();
        }

        // default size:
        //   <-- 25 pixels --> | <-- 50 pixels --> | <-- 50 pixels --> | <-- 50 pixels --> | <-- 25 pixels -->
        //   spacing is same horizontal and vertical

        static readonly int PreferedGrid = 50;
        static readonly int PreferedBorder = PreferedGrid / 2;
        static readonly int PreferedSpacer = 5; // spacer between glyph and grid
        public Size GetPreferedWindowSize()
        {
            return new Size((PreferedGrid * Game.Width) + PreferedBorder * 2, (PreferedGrid * Game.Height) + PreferedBorder * 2);
        }

        private void C4Board_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            g.FillRectangle(Brushes.White, ClientRectangle);

            if (Game == null)
                return;

            // draw the vertical lines
            int x = PreferedBorder;
            for (int i = 0; i < Game.Width + 1; ++i, x += PreferedGrid)
            {
                g.DrawLine(Pens.Black, new Point(x, PreferedBorder), new Point(x, PreferedBorder + (PreferedGrid * Game.Height)));
            }

            // draw the horizontal lines
            int y = PreferedBorder + PreferedGrid;
            for (int j = 0; j < Game.Height; ++j, y += PreferedGrid)
            {
                g.DrawLine(Pens.Black, new Point(PreferedBorder, y), new Point(PreferedBorder + (PreferedGrid * Game.Width), y));
            }

            if (selectedMove != null)
            {
                x = PreferedBorder + PreferedGrid * selectedMove.Value.X;
                y = PreferedBorder + PreferedGrid * selectedMove.Value.Y;
                var pen = new Pen(PlayerSettings.GetPlayerColor(Game.CurrentPlayer));
                g.DrawEllipse(pen, x + PreferedSpacer, y + PreferedSpacer, PreferedGrid - 2 * PreferedSpacer, PreferedGrid - 2 * PreferedSpacer);
            }

            for (int i = 0; i < Game.Width; ++i)
            {
                for (int j = 0; j < Game.Height; ++j)
                {
                    int checkPlayer = Game.GetMove(new Move(i, j));
                    if (checkPlayer != Player.Invalid)
                    {
                        x = PreferedBorder + PreferedGrid * i;
                        y = PreferedBorder + PreferedGrid * j;
                        var brush = new SolidBrush(PlayerSettings.GetPlayerColor(checkPlayer));
                        g.FillEllipse(brush, x + PreferedSpacer, y + PreferedSpacer, PreferedGrid - 2 * PreferedSpacer, PreferedGrid - 2 * PreferedSpacer);
                    }
                }
            }
        }

        void RunComputerPlayers()
        {
            if (Game.GameOver)
                return;

            if (PlayerSettings.GetPlayerController(Game.CurrentPlayer) != PlayerController.Computer)
            {
                return;
            }

#if NEVER
            C4AutoPlayer.MakeMove(Game);

            Parent.Refresh();

            RunComputerPlayers();
#endif
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
                message = string.Format("Player {0}", player + 1);
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
                    message = string.Format("Player {0} wins!", player + 1);
                }
            }
        }


        private void C4Board_MouseClick(object sender, MouseEventArgs e)
        {
            HandleClick(e);
        }

        private void C4Board_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            HandleClick(e);
        }

        private void HandleClick(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            if (Game == null || Game.GameOver || PlayerSettings.GetPlayerController(Game.CurrentPlayer) == PlayerController.Computer)
                return;

            var clickedMove = new Move(-1, -1);

            int x = PreferedBorder;
            for (int i = 0; i < Game.Width; ++i, x += PreferedGrid)
            {
                if (e.X > x && e.X < (x + PreferedGrid))
                {
                    clickedMove.X = i;
                    break;
                }
            }

            int y = PreferedBorder;
            for (int i = 0; i < Game.Height; ++i, y += PreferedGrid)
            {
                if (e.Y > y && e.Y < (y + PreferedGrid))
                {
                    clickedMove.Y = i;
                    break;
                }
            }

            if (clickedMove.X >= 0 && clickedMove.Y >= 0)
            {
                if (Game.GetMove(clickedMove) != Player.Invalid)
                {
                    clickedMove = new Move(-1, -1);
                }

                while (clickedMove.Y < (Game.Height - 1)
                    && (Game.GetMove(new Move(clickedMove.X, clickedMove.Y+1)) == Player.Invalid))
                {
                    clickedMove = new Move(clickedMove.X, clickedMove.Y+1);
                }
            }

            if (clickedMove.X >= 0 && clickedMove.Y >= 0)
            {
                if ((selectedMove != null && clickedMove == selectedMove) || e.Clicks > 1)
                {
                    Game.RecordMove(clickedMove);
                    selectedMove = null;
                }
                else
                {
                    selectedMove = clickedMove;
                }
            }
            else
            {
                selectedMove = null;
            }

            Parent.Refresh();

            RunComputerPlayers();
        }
    }
}

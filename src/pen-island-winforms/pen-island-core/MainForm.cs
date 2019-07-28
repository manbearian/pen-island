using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PenIsland
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        enum GameKind
        {
            Invalid, Dots, TicTacToe, MatchLine,
        }

        GameKind gameKind = GameKind.Invalid;

        GameBoard GameBoard
        {
            get
            {
                switch (gameKind)
                {
                    case GameKind.Dots: return dotsBoard;
                    case GameKind.TicTacToe: return tttBoard;
                    case GameKind.MatchLine: return c4Board;
                    default: return null;
                }
            }

        }

        void NewGame(GameKind gameKind)
        {
            this.gameKind = gameKind;

            GameBoard gameBoard = GameBoard;

            gameBoard.NewGame();

            dotsBoard.Visible = false;
            tttBoard.Visible = false;
            c4Board.Visible = false;

            UserControl gameBoardControl = GameBoard as UserControl;

            gameBoardControl.Visible = true;

            var width = gameBoardControl.ClientSize.Width;
            var height = gameBoardControl.Height + 50;
            ClientSize = new Size(width, height);

            Refresh();
        }

        private void NewDotsGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DotsSettingsForm gs = new DotsSettingsForm();
            gs.ShowDialog();

            if (gs.DialogResult == DialogResult.OK)
            {
                NewGame(GameKind.Dots);
            }
        }

        private void NewTicTacToeGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TttSettingsForm gs = new TttSettingsForm();
            gs.ShowDialog();

            if (gs.DialogResult == DialogResult.OK)
            {
                NewGame(GameKind.TicTacToe);
            }
        }

        private void NewMatchLineGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame(GameKind.MatchLine);
        }

        private void PlayerColorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlayerSettingsForm pc = new PlayerSettingsForm();
            pc.ShowDialog(dotsBoard);
        }
        
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var gameBoard = GameBoard;
            var gameBoardControl = gameBoard as UserControl;

            if (gameBoard != null)
            {
                gameBoard.GetStatusMessage(out string message, out Color color);
                g.DrawString(message, SystemFonts.DefaultFont, new SolidBrush(color), new Point(0, gameBoardControl.ClientSize.Height + 30));
            }
        }

    }

}

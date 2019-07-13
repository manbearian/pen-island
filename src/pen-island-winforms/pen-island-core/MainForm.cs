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
            Dots, TicTacToe
        }

        void NewGame(GameKind gameKind)
        {
            UserControl gameBoard;

            switch (gameKind)
            {
                case GameKind.Dots:
                    gameBoard = dotsBoard;
                    dotsBoard.NewGame();
                    break;
                case GameKind.TicTacToe:
                    gameBoard = tttBoard;
                    tttBoard.NewGame();
                    break;
                default:
                    throw new Exception("bad game type");
            }

            dotsBoard.Visible = false;
            tttBoard.Visible = false;
            gameBoard.Visible = true;

            var width = gameBoard.ClientSize.Width;
            var height = gameBoard.ClientSize.Height + 50;
            ClientSize = new Size(width, height);
            Refresh();
        }

        private void newDotsGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DotsSettingsForm gs = new DotsSettingsForm();
            gs.ShowDialog();

            if (gs.DialogResult == DialogResult.OK)
            {
                NewGame(GameKind.Dots);
            }
        }

        private void newTicTacToeGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TttSettingsForm gs = new TttSettingsForm();
            gs.ShowDialog();

            if (gs.DialogResult == DialogResult.OK)
            {
                NewGame(GameKind.TicTacToe);
            }
        }

        private void playerColorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlayerSettingsForm pc = new PlayerSettingsForm();
            pc.ShowDialog(dotsBoard);
        }
        
        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var game = dotsBoard.DotsGame;

            if (game != null)
            {
                if (!game.GameOver)
                {
                    var player = game.CurrentPlayer;
                    var color = PlayerSettings.GetPlayerColor(player);
                    g.DrawString(String.Format("Player {0}", player + 1), SystemFonts.DefaultFont, new SolidBrush(color), new Point(0, dotsBoard.ClientSize.Height + 30));
                }
                else
                {
                    var score = game.ScoreBoard;
                    List<int> winners = new List<int>();
                    int topScore = 0;
                    for (int i = 0; i < game.PlayerCount; ++i)
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

                    if (winners.Count == game.PlayerCount)
                    {
                        // tie game
                        var color = Color.Black;
                        g.DrawString(string.Format("Game Over, It's a Draw!"), SystemFonts.DefaultFont, new SolidBrush(color), new Point(0, dotsBoard.ClientSize.Height + 30));

                    }
                    else if (winners.Count > 1)
                    {
                        // tie game
                        var color = Color.Black;
                        g.DrawString(string.Format("Game Over, Multi-winner!"), SystemFonts.DefaultFont, new SolidBrush(color), new Point(0, dotsBoard.ClientSize.Height + 30));
                    }
                    else
                    {
                        var winner = winners[0];
                        var color = PlayerSettings.GetPlayerColor(winner);
                        g.DrawString(string.Format("Game Over, Player {0} Wins!", winner + 1), SystemFonts.DefaultFont, new SolidBrush(color), new Point(0, dotsBoard.ClientSize.Height + 30));
                    }
                }
            }
        }

    }

}

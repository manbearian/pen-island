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
            UpdateSize();
        }

        private void UpdateSize()
        {
            var width = dotsBoard.ClientSize.Width;
            var height = dotsBoard.ClientSize.Height + 50;
            ClientSize = new Size(width, height);
            Refresh();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DotsSettingsForm gs = new DotsSettingsForm();
            gs.ShowDialog(dotsBoard);

            if (gs.DialogResult == DialogResult.OK)
            {
                dotsBoard.NewGame();
                UpdateSize();
            }
        }

        private void playerColorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlayerColorForm pc = new PlayerColorForm();
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
                    var color = dotsBoard.GetPlayerColor(player);
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
                        g.DrawString(String.Format("Game Over, It's a Draw!"), SystemFonts.DefaultFont, new SolidBrush(color), new Point(0, dotsBoard.ClientSize.Height + 30));

                    }
                    else if (winners.Count > 1)
                    {
                        // tie game
                        var color = Color.Black;
                        g.DrawString(String.Format("Game Over, Multi-winner!"), SystemFonts.DefaultFont, new SolidBrush(color), new Point(0, dotsBoard.ClientSize.Height + 30));
                    }
                    else
                    {
                        var winner = winners[0];
                        var color = dotsBoard.GetPlayerColor(winner);
                        g.DrawString(String.Format("Game Over, Player {0} Wins!", winner + 1), SystemFonts.DefaultFont, new SolidBrush(color), new Point(0, dotsBoard.ClientSize.Height + 30));
                    }
                }
            }
        }
    }

}

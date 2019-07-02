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
            Invalidate();
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dotsBoard.NewGame();
            UpdateSize();
        }

        private void playerColorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlayerColorForm pc = new PlayerColorForm();
            pc.ShowDialog(dotsBoard);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DotsSettingsForm gs = new DotsSettingsForm();
            gs.ShowDialog(dotsBoard);
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            if (dotsBoard.DotsGame != null)
            {
                var player = dotsBoard.DotsGame.CurrentPlayer;
                var color = dotsBoard.GetPlayerColor(player);
                g.DrawString(String.Format("Player {0}", player+1), SystemFonts.DefaultFont, new SolidBrush(color), new Point(0, dotsBoard.ClientSize.Height + 30));
            }
        }
    }

}

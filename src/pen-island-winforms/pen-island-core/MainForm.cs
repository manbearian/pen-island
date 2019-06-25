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
            ClientSize = dotsBoard.ClientSize;
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
    }

}

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
    public partial class TttBoard : UserControl
    {
        public TttBoard()
        {
            InitializeComponent();
        }

        public void NewGame()
        {
           // DotsGame = new DotsGame(DotsGameSettings.PlayerCount, DotsGameSettings.BoardType, DotsGameSettings.BoardWidth, DotsGameSettings.BoardHeight);

            ClientSize = GetPreferedWindowSize();
            Refresh();

//            RunComputerPlayers();
        }

        public Size GetPreferedWindowSize()
        {
            //return new Size(DotsGame.Width * PreferedSpacer + PreferedDotSize, DotsGame.Height * PreferedSpacer + PreferedDotSize);
            return new Size(100, 100);
        }

        private void TTTBoard_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            g.DrawString("I'm a tic-tac-toe board", SystemFonts.DefaultFont, Brushes.BlueViolet, new Point(0, 0));
        }
    }
}

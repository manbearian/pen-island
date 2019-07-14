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
        internal TttGame TttGame { get; private set; }

        public TttBoard()
        {
            InitializeComponent();
        }

        public void NewGame()
        {
            // TttGame = new TttGame(TttGameSettings.PlayerCount, TttGameSettings.BoardWidth, TttGameSettings.BoardHeight);
            TttGame = new TttGame(2, 3, 3);

            ClientSize = GetPreferedWindowSize();
            Refresh();

            RunComputerPlayers();
        }

        static readonly int PreferedSpacer = 50;
        static readonly int PreferedBorder = PreferedSpacer / 2;


        // default size:
        //   <-- 20 pixels --> <-- 40 pixels --> | <-- 40 pixels --> | <-- 40 pixels --> <-- 25 pixels ->
        //   spacing is same horizontal and vertical

        public Size GetPreferedWindowSize()
        {
            //return new Size(DotsGame.Width * PreferedSpacer + PreferedDotSize, DotsGame.Height * PreferedSpacer + PreferedDotSize);
            return new Size((PreferedSpacer * TttGame.Width) + PreferedBorder * 2, (PreferedSpacer * TttGame.Height) + PreferedBorder * 2);
        }

        private void TTTBoard_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;

            g.FillRectangle(Brushes.White, this.ClientRectangle);

            if (TttGame == null)
                return;

            // draw the vertical lines
            int x = PreferedBorder + PreferedSpacer;
            for (int i = 0; i < TttGame.Width - 1; ++i, x += PreferedSpacer)
            {
                g.DrawLine(Pens.Black, new Point(x, PreferedBorder), new Point(x, PreferedBorder + (PreferedSpacer * TttGame.Height)));
            }

            // draw the horizontal lines
            int y = PreferedBorder + PreferedSpacer;
            for (int j = 0; j < TttGame.Height - 1; ++j, y += PreferedSpacer)
            {
                g.DrawLine(Pens.Black, new Point(PreferedBorder, y), new Point(PreferedBorder + (PreferedSpacer * TttGame.Width), y));
            }

        }

        void RunComputerPlayers()
        {

        }
    }
}

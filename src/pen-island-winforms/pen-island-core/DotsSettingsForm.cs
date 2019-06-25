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
    public partial class DotsSettingsForm : Form
    {
        private DotsBoard dotsBoard;

        private CheckBox[] computerPlayerCheckBoxes = new CheckBox[Player.MaxPlayers];

        public DotsSettingsForm()
        {
            InitializeComponent();

            computerPlayerCheckBoxes[0] = checkBox1;
            computerPlayerCheckBoxes[1] = checkBox2;
            computerPlayerCheckBoxes[2] = checkBox3;
            computerPlayerCheckBoxes[3] = checkBox4;
            computerPlayerCheckBoxes[4] = checkBox5;
            computerPlayerCheckBoxes[5] = checkBox6;
            computerPlayerCheckBoxes[6] = checkBox7;
            computerPlayerCheckBoxes[7] = checkBox8;
            computerPlayerCheckBoxes[8] = checkBox9;
        }

        public void ShowDialog(DotsBoard dotsBoard)
        {
            this.dotsBoard = dotsBoard;

            playerCountBox.SelectedIndex = dotsBoard.Settings.PlayerCount - 2;
            
            for (int i = 0; i < Player.MaxPlayers; ++i)
            {
                computerPlayerCheckBoxes[i].Checked = dotsBoard.Settings.ComputerPlayers[i];
            }

            // assume boardTypeBox and BoardType enum are in sync
            boardTypeBox.SelectedIndex = (int)dotsBoard.Settings.BoardType;

            heightTextBox.Text = dotsBoard.Settings.Height.ToString();
            widthTextBox.Text = dotsBoard.Settings.Width.ToString();

            switch (dotsBoard.Settings.BoardType)
            {
                case DotsBoardType.Squares:
                    heightTextBox.Enabled = true;
                    widthTextBox.Enabled = true;
                    break;
                case DotsBoardType.Triangles:
                    heightTextBox.Enabled = true;
                    widthTextBox.Enabled = false;
                    break;
            }

            base.ShowDialog();
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            // player settings
            dotsBoard.Settings.PlayerCount = int.Parse((string)playerCountBox.SelectedItem);
            for (int i = 0; i < Player.MaxPlayers; ++i)
            {
                dotsBoard.Settings.ComputerPlayers[i] = computerPlayerCheckBoxes[i].Checked;
            }

            // board settings
            dotsBoard.Settings.BoardType = (DotsBoardType)boardTypeBox.SelectedIndex; // assume boardTypeBox and BoardType enum are in sync

            int val;
            if (int.TryParse(heightTextBox.Text, out val))
            {
                dotsBoard.Settings.Height = val;
            }

            if (int.TryParse(widthTextBox.Text, out val))
            {
                dotsBoard.Settings.Width = val;
            }

        }


    }
}

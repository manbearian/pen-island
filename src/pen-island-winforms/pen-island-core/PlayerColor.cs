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
    public partial class PlayerColorForm : Form
    {
        DotsBoard dotsBoard;
        readonly Button[] buttons = new Button[Player.MaxPlayers];
        readonly Color[] oldColors = new Color[Player.MaxPlayers];
        readonly Color[] newColors = new Color[Player.MaxPlayers];

        private bool Changed {
            get
            {
                for (int i = 0; i < Player.MaxPlayers; ++i)
                    if (oldColors[i] != newColors[i])
                        return true;
                return false;
            }
        }

        public PlayerColorForm()
        {
            InitializeComponent();
            buttons[0] = button1;
            buttons[1] = button2;
            buttons[2] = button3;
            buttons[3] = button4;
            buttons[4] = button5;
            buttons[5] = button6;
            buttons[6] = button7;
            buttons[7] = button8;
            buttons[8] = button9;
        }
        
        private void colorButton_Click(object sender, EventArgs e)
        {
            var result = colorDialog1.ShowDialog();

            if (result != DialogResult.OK)
                return;

            var newColor = colorDialog1.Color;

            for (int i = 0; i < Player.MaxPlayers; ++i)
            {
                if (buttons[i] == sender)
                {
                    buttons[i].BackColor = newColor;
                    newColors[i] = newColor;
                    break;
                }
            }

            applyButton.Enabled = Changed;
        }

        public void ShowDialog(DotsBoard dotsBoard)
        {
            this.dotsBoard = dotsBoard;

            for (int i = 0; i < Player.MaxPlayers; ++i)
            {
                Color c = dotsBoard.GetPlayerColor(i);
                buttons[i].BackColor = c;
                oldColors[i] = c;
                newColors[i] = c;
            }

            base.ShowDialog();
        }

        void UpdateDotsBoard()
        {
            if (Changed)
            {
                for (int i = 0; i < Player.MaxPlayers; ++i)
                {
                    dotsBoard.SetPlayerColor(i, newColors[i]);
                }

                dotsBoard.Invalidate();
            }
        }

        private void PlayerColorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.Cancel || e.CloseReason != CloseReason.UserClosing)
                return;

            UpdateDotsBoard();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            UpdateDotsBoard();
            newColors.CopyTo(oldColors, 0);
            applyButton.Enabled = false;
        }
    }
}

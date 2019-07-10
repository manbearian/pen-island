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
    public partial class PlayerSettingsForm : Form
    {
        DotsBoard dotsBoard;

        readonly Button[] playerColorElement = new Button[Player.MaxPlayers];
        readonly ComboBox[] playerControllerElement = new ComboBox[Player.MaxPlayers];

        private bool Changed {
            get
            {
                for (int i = 0; i < Player.MaxPlayers; ++i)
                {
                    if (PlayerSettings.GetPlayerColor(i) != playerColorElement[i].BackColor)
                        return true;
                    if (PlayerSettings.GetPlayerController(i) != (PlayerController)playerControllerElement[i].SelectedIndex)
                        return true;
                }
                return false;
            }
        }

        public PlayerSettingsForm()
        {
            InitializeComponent();

            playerColorElement[0] = button1;
            playerColorElement[1] = button2;
            playerColorElement[2] = button3;
            playerColorElement[3] = button4;
            playerColorElement[4] = button5;
            playerColorElement[5] = button6;
            playerColorElement[6] = button7;
            playerColorElement[7] = button8;
            playerColorElement[8] = button9;

            playerControllerElement[0] = comboBox1;
            playerControllerElement[1] = comboBox2;
            playerControllerElement[2] = comboBox3;
            playerControllerElement[3] = comboBox4;
            playerControllerElement[4] = comboBox5;
            playerControllerElement[5] = comboBox6;
            playerControllerElement[6] = comboBox7;
            playerControllerElement[7] = comboBox8;
            playerControllerElement[8] = comboBox9;
        }
        
        private void colorButton_Click(object sender, EventArgs e)
        {
            var result = colorDialog1.ShowDialog();

            if (result != DialogResult.OK)
                return;

            var newColor = colorDialog1.Color;

            for (int i = 0; i < Player.MaxPlayers; ++i)
            {
                if (playerColorElement[i] == sender)
                {
                    playerColorElement[i].BackColor = newColor;
                    break;
                }
            }

            applyButton.Enabled = Changed;
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            applyButton.Enabled = Changed;
        }

        public void ShowDialog(DotsBoard dotsBoard)
        {
            this.dotsBoard = dotsBoard;

            for (int i = 0; i < Player.MaxPlayers; ++i)
            {
                Color c = PlayerSettings.GetPlayerColor(i);
                playerColorElement[i].BackColor = c;
                playerControllerElement[i].SelectedIndex = (int)PlayerSettings.GetPlayerController(i);
            }

            base.ShowDialog();
        }

        void UpdateDotsBoard()
        {
            if (Changed)
            {
                for (int i = 0; i < Player.MaxPlayers; ++i)
                {
                    PlayerSettings.SetPlayerColor(i, playerColorElement[i].BackColor);
                    PlayerSettings.SetPlayerController(i, (PlayerController)playerControllerElement[i].SelectedIndex);
                }

                dotsBoard.Refresh();

                Properties.Settings.Default.Save(); // write out selections to the disk
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
            applyButton.Enabled = false;
        }

    }
}

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
    public partial class TttSettingsForm : Form
    {
        public TttSettingsForm()
        {
            InitializeComponent();
        }

        private void TttSettingsForm_Load(object sender, EventArgs e)
        {
            playerCountBox.SelectedIndex = TttGameSettings.PlayerCount - 2;

            heightTextBox.Text = TttGameSettings.BoardHeight.ToString();
            widthTextBox.Text = TttGameSettings.BoardWidth.ToString();

            winLengthTextBox.Text = TttGameSettings.WinLength.ToString();
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            // player settings
            TttGameSettings.PlayerCount = int.Parse((string)playerCountBox.SelectedItem);

            // board settings

            if (int.TryParse(widthTextBox.Text, out int width))
            {
                TttGameSettings.BoardWidth = width;
            }

            if (int.TryParse(heightTextBox.Text, out int height))
            {
                TttGameSettings.BoardHeight = height;
            }
            if (int.TryParse(winLengthTextBox.Text, out int winLength))
            {
                TttGameSettings.WinLength = winLength;
            }


            TttGameSettings.Save(); // write out selections to the disk


        }
    }
}

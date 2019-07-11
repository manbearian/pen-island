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
        public DotsSettingsForm()
        {
            InitializeComponent();
        }

        private void DotsSettingsForm_Load(object sender, EventArgs e)
        {
            playerCountBox.SelectedIndex = DotsGameSettings.PlayerCount - 2;

            // assume boardTypeBox and BoardType enum are in sync
            boardTypeBox.SelectedIndex = (int)DotsGameSettings.BoardType;

            heightTextBox.Text = DotsGameSettings.BoardHeight.ToString();
            widthTextBox.Text = DotsGameSettings.BoardWidth.ToString();

            switch (DotsGameSettings.BoardType)
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
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            // player settings
            DotsGameSettings.PlayerCount = int.Parse((string)playerCountBox.SelectedItem);

            // board settings
            DotsGameSettings.BoardType = (DotsBoardType)boardTypeBox.SelectedIndex; // assume boardTypeBox and BoardType enum are in sync
            
            if (int.TryParse(widthTextBox.Text, out int width))
            {
                DotsGameSettings.BoardWidth = width;
            }

            if (int.TryParse(heightTextBox.Text, out int height))
            {
                DotsGameSettings.BoardHeight = height;
            }

            DotsGameSettings.Save(); // write out selections to the disk
        }

    }
}

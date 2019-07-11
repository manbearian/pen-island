using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace PenIsland
{
    enum PlayerController
    {
        Human = 0, Computer = 1
    }

    static class PlayerSettings
    {
        public static void Save()
        {
            Properties.Settings.Default.Save();
        }

        public static Color GetPlayerColor(int player)
        {
            switch (player)
            {
                case 0: return Properties.Settings.Default.Player_Color1;
                case 1: return Properties.Settings.Default.Player_Color2;
                case 2: return Properties.Settings.Default.Player_Color3;
                case 3: return Properties.Settings.Default.Player_Color4;
                case 4: return Properties.Settings.Default.Player_Color5;
                case 5: return Properties.Settings.Default.Player_Color6;
                case 6: return Properties.Settings.Default.Player_Color7;
                case 7: return Properties.Settings.Default.Player_Color8;
                case 8: return Properties.Settings.Default.Player_Color9;
            }

            throw new Exception("unknown player!");
        }

        public static void SetPlayerColor(int player, Color color)
        {
            switch (player)
            {
                case 0: Properties.Settings.Default.Player_Color1 = color; return;
                case 1: Properties.Settings.Default.Player_Color2 = color; return;
                case 2: Properties.Settings.Default.Player_Color3 = color; return;
                case 3: Properties.Settings.Default.Player_Color4 = color; return;
                case 4: Properties.Settings.Default.Player_Color5 = color; return;
                case 5: Properties.Settings.Default.Player_Color6 = color; return;
                case 6: Properties.Settings.Default.Player_Color7 = color; return;
                case 7: Properties.Settings.Default.Player_Color8 = color; return;
                case 8: Properties.Settings.Default.Player_Color9 = color; return;
            }

            throw new Exception("unknown player!");
        }

        public static PlayerController GetPlayerController(int player)
        {
            switch (player)
            {
                case 0: return (PlayerController)Properties.Settings.Default.Player_Controller1;
                case 1: return (PlayerController)Properties.Settings.Default.Player_Controller2;
                case 2: return (PlayerController)Properties.Settings.Default.Player_Controller3;
                case 3: return (PlayerController)Properties.Settings.Default.Player_Controller4;
                case 4: return (PlayerController)Properties.Settings.Default.Player_Controller5;
                case 5: return (PlayerController)Properties.Settings.Default.Player_Controller6;
                case 6: return (PlayerController)Properties.Settings.Default.Player_Controller7;
                case 7: return (PlayerController)Properties.Settings.Default.Player_Controller8;
                case 8: return (PlayerController)Properties.Settings.Default.Player_Controller9;
            }

            throw new Exception("unknown player!");
        }


        public static void SetPlayerController(int player, PlayerController controller)
        {
            switch (player)
            {
                case 0: Properties.Settings.Default.Player_Controller1 = (int)controller; return;
                case 1: Properties.Settings.Default.Player_Controller2 = (int)controller; return;
                case 2: Properties.Settings.Default.Player_Controller3 = (int)controller; return;
                case 3: Properties.Settings.Default.Player_Controller4 = (int)controller; return;
                case 4: Properties.Settings.Default.Player_Controller5 = (int)controller; return;
                case 5: Properties.Settings.Default.Player_Controller6 = (int)controller; return;
                case 6: Properties.Settings.Default.Player_Controller7 = (int)controller; return;
                case 7: Properties.Settings.Default.Player_Controller8 = (int)controller; return;
                case 8: Properties.Settings.Default.Player_Controller9 = (int)controller; return;
            }

            throw new Exception("unknown player!");
        }
    }
}

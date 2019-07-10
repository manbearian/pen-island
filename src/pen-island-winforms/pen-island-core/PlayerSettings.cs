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

    class PlayerSettings
    {
        public static Color GetPlayerColor(int player)
        {
            switch (player)
            {
                case 0: return Properties.Settings.Default.ColorPlayer1;
                case 1: return Properties.Settings.Default.ColorPlayer2;
                case 2: return Properties.Settings.Default.ColorPlayer3;
                case 3: return Properties.Settings.Default.ColorPlayer4;
                case 4: return Properties.Settings.Default.ColorPlayer5;
                case 5: return Properties.Settings.Default.ColorPlayer6;
                case 6: return Properties.Settings.Default.ColorPlayer7;
                case 7: return Properties.Settings.Default.ColorPlayer8;
                case 8: return Properties.Settings.Default.ColorPlayer9;
            }

            throw new Exception("unknown player!");
        }

        public static void SetPlayerColor(int player, Color color)
        {
            switch (player)
            {
                case 0: Properties.Settings.Default.ColorPlayer1 = color; return;
                case 1: Properties.Settings.Default.ColorPlayer2 = color; return;
                case 2: Properties.Settings.Default.ColorPlayer3 = color; return;
                case 3: Properties.Settings.Default.ColorPlayer4 = color; return;
                case 4: Properties.Settings.Default.ColorPlayer5 = color; return;
                case 5: Properties.Settings.Default.ColorPlayer6 = color; return;
                case 6: Properties.Settings.Default.ColorPlayer7 = color; return;
                case 7: Properties.Settings.Default.ColorPlayer8 = color; return;
                case 8: Properties.Settings.Default.ColorPlayer9 = color; return;
            }

            throw new Exception("unknown player!");
        }

        public static PlayerController GetPlayerController(int player)
        {
            switch (player)
            {
                case 0: return (PlayerController)Properties.Settings.Default.ControllerPlayer1;
                case 1: return (PlayerController)Properties.Settings.Default.ControllerPlayer2;
                case 2: return (PlayerController)Properties.Settings.Default.ControllerPlayer3;
                case 3: return (PlayerController)Properties.Settings.Default.ControllerPlayer4;
                case 4: return (PlayerController)Properties.Settings.Default.ControllerPlayer5;
                case 5: return (PlayerController)Properties.Settings.Default.ControllerPlayer6;
                case 6: return (PlayerController)Properties.Settings.Default.ControllerPlayer7;
                case 7: return (PlayerController)Properties.Settings.Default.ControllerPlayer8;
                case 8: return (PlayerController)Properties.Settings.Default.ControllerPlayer9;
            }

            throw new Exception("unknown player!");
        }


        public static void SetPlayerController(int player, PlayerController controller)
        {
            switch (player)
            {
                case 0: Properties.Settings.Default.ControllerPlayer1 = (int)controller; return;
                case 1: Properties.Settings.Default.ControllerPlayer2 = (int)controller; return;
                case 2: Properties.Settings.Default.ControllerPlayer3 = (int)controller; return;
                case 3: Properties.Settings.Default.ControllerPlayer4 = (int)controller; return;
                case 4: Properties.Settings.Default.ControllerPlayer5 = (int)controller; return;
                case 5: Properties.Settings.Default.ControllerPlayer6 = (int)controller; return;
                case 6: Properties.Settings.Default.ControllerPlayer7 = (int)controller; return;
                case 7: Properties.Settings.Default.ControllerPlayer8 = (int)controller; return;
                case 8: Properties.Settings.Default.ControllerPlayer9 = (int)controller; return;
            }

            throw new Exception("unknown player!");
        }
    }
}

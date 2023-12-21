using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Snake_Game
{
    public static class GameSettings
    {
        public static int ShakeDuration { get; set; } = 2000;
        public static int boostSpeed { get; set; } = 100;

        public static double WallDensity { get; set; } = .10;
        public static bool WallFatality { get;set; } = true;
        public static bool walls { get; set; } = true;
    }
    
}

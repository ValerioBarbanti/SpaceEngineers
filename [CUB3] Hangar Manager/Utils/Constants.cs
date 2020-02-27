using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game;
using VRage;
using VRageMath;

namespace IngameScript {
    partial class Program {
        public class Constants {

            // COMMANDS
            public const string C_ASSIGN = "assign";
            public const string C_AUTO = "auto";

            // CONFIGS
            public const string CFG_S_CONFIG = "CONFIG";
            public const string CFG_P_REQUESTABLE = "Requestable";
            public const string CFG_P_SNTIME = "Sanction Time";

            // COLORS
            public static Color COLOR_BACKGROUND = new Color(27, 28, 33);
            public static Color COLOR_BACKGROUND_MASK = new Color(37, 39, 45);
            public static Color COLOR_BACKGROUND_LIGHT = new Color(67, 70, 81);
            public static Color COLOR_LOGO_PRIMARY = new Color(255, 217, 131);
            public static Color COLOR_LOGO_SECONDARY = new Color(132, 83, 47);

            public static Color COLOR_WHITE = new Color(256, 256, 256);
            public static Color COLOR_GREEN = new Color(29, 229, 128);
            public static Color COLOR_GREEN_DARK = new Color(7, 54, 30);
            public static Color COLOR_YELLOW = new Color(229, 157, 17);
            public static Color COLOR_YELLOW_DARK = new Color(71, 49, 5);
            public static Color COLOR_RED = new Color(255, 4, 99);
            public static Color COLOR_RED_DARK = new Color(73, 1, 28);
            public static Color COLOR_NAVY_BLUE = new Color(37, 38, 45);
            public static Color COLOR_ORANGE = new Color(255, 141, 18);

        }
    }
}

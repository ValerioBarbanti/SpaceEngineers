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

            // TAGS
            public const string T_LSM = "[LSM]";
            public const string T_LSM_AIRLOCK_DOOR = "[LSM Airlock Door]";
            public const string T_LSM_AIRLOCK_AIRVENT = "[LSM Airlock Airvent]";
            public const string T_LSM_AIRLOCK_SCREEN = "[LSM Airlock Screen]";
            public const string T_LSM_AIRVENT_SCREEN = "[LSM Airvent Screen]";

            // COMMANDS
            public const string C_CYCLE = "cycle";
            public const string C_LEAK = "leak";
            public const string C_SLIDE = "slide";

            // PARAMETERS
            public const string P_DEPRESSURIZE = "depressurize";
            public const string P_PRESSURIZE = "pressurize";
            public const string P_ON = "on";
            public const string P_OFF = "off";

            // AIRLOCK STATUS
            public const string A_IDLE = "idle";
            public const string A_CLOSE_DOORS = "close_doors";
            public const string A_CYCLE = "cycle";
            public const string A_OPEN_DOORS = "open_doors";
            public const string A_COMPLETED = "completed";

            // AIRLOCK PUBLIC STATUS
            public const string AP_IDLE = "IDLE";
            public const string AP_CYCLE = "CYCLING";
            public const string AP_PRESSURIZED = "PRESSURIZED";
            public const string AP_DEPRESSURIZED = "DEPRESSURIZED";
            public const string AP_ERROR = "ERROR";

            // DOOR
            public const string DOOR_INTERNAL = "internal";
            public const string DOOR_EXTERNAL = "external";

            // SCREENS
            public const string S_STATUS_INIT = "Initializing";
            public const string S_STATUS_RUNNING = "Running";

            // COLORS
            public static Color COLOR_BACKGROUND = new Color(27, 28, 33);
            public static Color COLOR_BACKGROUND_MASK = new Color(65, 66, 77);
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

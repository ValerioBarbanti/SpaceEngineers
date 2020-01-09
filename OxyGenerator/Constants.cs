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

            // COLORS
            public static Color C_DARK_BLUE = new Color(37, 46, 53);
            public static Color C_DARK_CYAN = new Color(41, 54, 62);
            public static Color C_CYAN = new Color(77, 99, 113);
            public static Color C_LIGHT_GREY = new Color(213, 236, 245);

            // AIRLOCK STATUS
            public const string IDLE_STATUS = "IDLE";
            public const string START_CYCLE = "START CYCLING";
            public const string END_CYCLE = "END CYCLING";
            public const string DOORS_CLOSED = "DOORS CLOSED";
            public const string DEPRESSURIZED = "DEPRESSURIZED";
            public const string PRESSURIZED = "PRESSURIZED";

            // AIRLOCK PUBLIC STATUS
            public const string PS_IDLE = "IDLE";
            public const string PS_DOORS_CLOSING = "DOORS CLOSING";
            public const string PS_CYCLING = "CYCLING";
            public const string PS_DOORS_OPENING = "DOORS OPENING";
            public const string PS_DEPRESSURIZED = "DEPRESSURIZED";
            public const string PS_PRESSURIZED = "PRESSURIZED";

            // AIRLOCK COMMANDS
            public const string IDLE_COMMAND = "idle";
            public const string DEPRESSURIZE = "depressurize";
            public const string PRESSURIZE = "pressurize";

        }
    }
}

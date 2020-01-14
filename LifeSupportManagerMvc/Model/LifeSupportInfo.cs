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
        public class LifeSupportInfo {

            public bool IsLifeSupportAutomatic { get; set; }

            public bool IsGeneratorsWorking { get; set; }
            public bool IsOxygenFarmWorking { get; set; }

            public double TotalOxygenInTanks { get; set; }
            public double TotalHydrogenInTanks { get; set; }

            public string ReadableOxygenInTanks { get; set; }
            public string ReadableHydrogenInTanks { get; set; }

            public LifeSupportInfo() {

            }

        }
    }
}

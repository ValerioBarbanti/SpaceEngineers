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
    partial class Program : MyGridProgram {

        List<IMyBatteryBlock> batteries = new List<IMyBatteryBlock>();

        List<IMyTextPanel> panels = new List<IMyTextPanel>();
        List<IMyJumpDrive> jumpDrives = new List<IMyJumpDrive>();

        float totalOutput = 0;
        float totalStored = 0;

        public Program() {
            
        }

        public void Save() {
            
        }

        public void Main(string argument, UpdateType updateSource) {
            
            GridTerminalSystem.GetBlocksOfType(batteries);
            GridTerminalSystem.GetBlocksOfType(panels);
            GridTerminalSystem.GetBlocksOfType(jumpDrives);

            totalOutput = 0;
            totalStored = 0;

            foreach (IMyBatteryBlock battery in batteries) {
                totalOutput += battery.CurrentOutput;
                totalStored += battery.CurrentStoredPower;
            }

            foreach (IMyJumpDrive jumpDrive in jumpDrives) {
                Echo($"Panel: {jumpDrive.DetailedInfo}");
            }

            /*foreach (IMyTextPanel panel in panels) {
                Echo($"Panel: {panel.DetailedInfo}");
            }*/

            Echo($"Total Output: {totalOutput.ToString("0.000")} MW");
            Echo($"Total Stored: {totalStored.ToString("0.000")}  MWh");

        }
    }
}

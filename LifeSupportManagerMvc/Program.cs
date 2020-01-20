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

        // PROPERTIES
        public bool isLeakManagementOn;
        public bool isProductionOn;
        public double minimumOxygenInTanks;
        public double maximumOxygenInTanks;
        public double minimumHydrogenInTanks;
        public double maximumHydrogenInTanks;


        MyIni _ini = new MyIni();

        // Router
        CommandRouter commandRouter;

        // View
        ScreenManager screenManager;

        // Controllers
        AirlockController airlockController;
        LeakController leakController;
        ProductionController productionController;

        int maxInstruction = 0;

        public Program() {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;

            commandRouter = new CommandRouter(this);
            airlockController =  new AirlockController(this);
            leakController = new LeakController(this);
            productionController = new ProductionController(this);
            screenManager = new ScreenManager(this);

            Configure();
        }

        private void Configure() {
            if (String.IsNullOrEmpty(Me.CustomData)) {
                SaveConfig(true);
                
            } else {
                MyIniParseResult result;
                if (!_ini.TryParse(Me.CustomData, out result))
                    throw new Exception(result.ToString());

                minimumOxygenInTanks = _ini.Get("Production Management", "MinimumOxygenInTanks").ToDouble();
                maximumOxygenInTanks = _ini.Get("Production Management", "MaximumOxygenInTanks").ToDouble();
                minimumHydrogenInTanks = _ini.Get("Production Management", "MinimumHydrogenInTanks").ToDouble();
                maximumHydrogenInTanks = _ini.Get("Production Management", "MaximumHydrogenInTanks").ToDouble();

                isLeakManagementOn = _ini.Get("Do Not Modify", "LeakStatus").ToBoolean();
                isProductionOn = _ini.Get("Do Not Modify", "ProductionStatus").ToBoolean();
            }
            
        }

        private void SaveConfig(bool firstTime) {
            if (firstTime) {
                isLeakManagementOn = true;
                isProductionOn = true;
                minimumOxygenInTanks = 30;
                maximumOxygenInTanks = 70;
                minimumHydrogenInTanks = 30;
                maximumHydrogenInTanks = 70;

                _ini.Set("Production Management", "Start Oxigen Production When Below", minimumOxygenInTanks);
                _ini.Set("Production Management", "Stop Oxygen Production When Over", maximumOxygenInTanks);
                _ini.Set("Production Management", "Start Hydrogen Production When Below", minimumHydrogenInTanks);
                _ini.Set("Production Management", "Stop Hydrogen Production When Over", maximumHydrogenInTanks);

                _ini.Set("Do Not Modify", "LeakStatus", isLeakManagementOn);
                _ini.Set("Do Not Modify", "ProductionStatus", isProductionOn);
            } else {
                MyIniParseResult result;
                if (!_ini.TryParse(Me.CustomData, out result))
                    throw new Exception(result.ToString());

                minimumOxygenInTanks = _ini.Get("Production Management", "Start Oxigen Production When Below").ToDouble();
                maximumOxygenInTanks = _ini.Get("Production Management", "Stop Oxygen Production When Over").ToDouble();
                minimumHydrogenInTanks = _ini.Get("Production Management", "Start Hydrogen Production When Below").ToDouble();
                maximumHydrogenInTanks = _ini.Get("Production Management", "Stop Hydrogen Production When Over").ToDouble();

                _ini.Set("Production Management", "Start Oxigen Production When Below", minimumOxygenInTanks);
                _ini.Set("Production Management", "Stop Oxygen Production When Over", maximumOxygenInTanks);
                _ini.Set("Production Management", "Start Hydrogen Production When Below", minimumHydrogenInTanks);
                _ini.Set("Production Management", "Stop Hydrogen Production When Over", maximumHydrogenInTanks);

                _ini.Set("Do Not Modify", "LeakStatus", isLeakManagementOn);
                _ini.Set("Do Not Modify", "ProductionStatus", isProductionOn);
            }

            Me.CustomData = _ini.ToString();
        }

        public void Save() {
            //SaveConfig(false);
        }

        public void Main(string argument, UpdateType updateSource) {
            commandRouter.ParseCommand(argument);
            airlockController.AirlockRuntime();
            leakController.LeakRuntime();
            productionController.OxygenRuntime();
            screenManager.ScreenRuntime();
            SaveConfig(false);
            GenerateStats();
        }

        private void GenerateStats() {
            IMyTextSurface pcScreen = Me.GetSurface(0);
            pcScreen.ContentType = ContentType.TEXT_AND_IMAGE;
            pcScreen.Font = "Monospace";
            pcScreen.FontSize = 0.7f;

            int curInstruction = Runtime.CurrentInstructionCount;
            if (curInstruction > maxInstruction) {
                maxInstruction = curInstruction;
            }

            string leakStatus = isLeakManagementOn ? "ON" : "OFF";
            string prodStatus = isProductionOn ? "ON" : "OFF";

            string text = "";
            text += $"Life Support\nManagement System\nv.1.0\n";
            text += $"\n";
            text += $"-----\n";
            text += $"\n";
            text += $"Airlocks count: {airlockController.Airlocks.Count()}\n";
            text += $"Leak Prevention: {leakStatus}\n";
            text += $"Production Prevention: {prodStatus}\n";
            text += $"\n";
            text += $"-----\n";
            text += $"\n";
            text += $"Instructions: {curInstruction}\n";
            text += $"Max Instructions: {maxInstruction}/{Runtime.MaxInstructionCount}\n";

            pcScreen.WriteText(text);

        }
    }
}

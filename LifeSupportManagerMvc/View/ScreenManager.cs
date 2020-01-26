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
        public class ScreenManager {

            Program myProgram;

            public string Status { get; set; }

            private List<IMyTextPanel> GlobalPanels { get; set; }

            private List<IMyTextPanel> ScriptPanels { get; set; }
            private List<IMyTextPanel> AirlockPanels { get; set; }
            private List<IMyTextPanel> LeakPanels { get; set; }
            private List<IMyTextPanel> ProductionPanels { get; set; }
            private List<IMyTextPanel> DoublePanels { get; set; }

            private int tick = 0;
            private int SplashLength { get; set; }

            

            private SplashScreen splashScreen;
            public AirlockScreen airlockScreen;
            public LeakScreen leakScreen;
            public ProductionScreen productionScreen;

            public ScreenManager(Program program) {
                myProgram = program;
                Init();
            }

            public void Init() {
                Status = Constants.S_STATUS_INIT;

                GlobalPanels = new List<IMyTextPanel>();
                myProgram.GridTerminalSystem.GetBlocksOfType(GlobalPanels);

                ScriptPanels = new List<IMyTextPanel>();
                AirlockPanels = new List<IMyTextPanel>();
                LeakPanels = new List<IMyTextPanel>();
                ProductionPanels = new List<IMyTextPanel>();
                DoublePanels = new List<IMyTextPanel>();

                myProgram.Echo("Checking leak and production screens");
                foreach (IMyTextPanel panel in GlobalPanels) {
                    if (panel.CustomData.Contains(Constants.T_LSM_AIRVENT_SCREEN) && panel.CustomData.Contains(Constants.T_LSM_PROD_SCREEN)) {
                        myProgram.Echo("Production Panel found");
                        panel.ContentType = ContentType.SCRIPT;
                        DoublePanels.Add(panel);
                        ScriptPanels.Add(panel);
                    } else if (panel.CustomData.Contains(Constants.T_LSM_AIRVENT_SCREEN) && !panel.CustomData.Contains(Constants.T_LSM_PROD_SCREEN)) {
                        myProgram.Echo("Airvent Panel found");
                        panel.ContentType = ContentType.SCRIPT;
                        LeakPanels.Add(panel);
                        ScriptPanels.Add(panel);
                    } else if (!panel.CustomData.Contains(Constants.T_LSM_AIRVENT_SCREEN) && panel.CustomData.Contains(Constants.T_LSM_PROD_SCREEN)) {
                        myProgram.Echo("Production Panel found");
                        panel.ContentType = ContentType.SCRIPT;
                        ProductionPanels.Add(panel);
                        ScriptPanels.Add(panel);
                    }
                }

                myProgram.Echo("Checking airlocks");
                foreach (KeyValuePair<string, Airlock> _al in myProgram.airlockController.Airlocks) {
                    myProgram.Echo("Airlocks found");
                    Airlock airlock = _al.Value;
                    foreach (IMyTextPanel panel in airlock.Panels) {
                        panel.ContentType = ContentType.SCRIPT;
                        panel.CustomData = Constants.T_LSM_AIRLOCK_SCREEN;
                        AirlockPanels.Add(panel);
                        ScriptPanels.Add(panel);
                    }
                }

                splashScreen = new SplashScreen(this, ScriptPanels);
                airlockScreen = new AirlockScreen(this, myProgram);
                leakScreen = new LeakScreen(myProgram, this, LeakPanels, DoublePanels, myProgram.leakController.Airvents);
                productionScreen = new ProductionScreen(myProgram, this, ProductionPanels, DoublePanels);
            }

            public void ScreenRuntime() {
                tick++;
                if (tick < 200) {
                    splashScreen.GenerateScreen();
                } else {
                    airlockScreen.GenerateScreen(myProgram.airlockController.Airlocks);
                    leakScreen.GenerateScreen();
                    productionScreen.GenerateScreen();
                }
            }
            

        }
    }
}

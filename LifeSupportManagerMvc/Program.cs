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

        // Router
        CommandRouter commandRouter;

        // View
        ScreenManager screenManager;

        // Controllers
        AirlockController airlockController;
        LeakController leakController;
        ProductionController oxygenController;

        public Program() {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;

            commandRouter = new CommandRouter(this);
            airlockController =  new AirlockController(this);
            leakController = new LeakController(this);
            oxygenController = new ProductionController(this);
            screenManager = new ScreenManager(this);
        }

        public void Save() {
            
        }

        public void Main(string argument, UpdateType updateSource) {
            commandRouter.ParseCommand(argument);
            //airlockController.AirlockRuntime();
            //leakController.LeakRuntime();
            oxygenController.OxygenRuntime();
            //screenManager.ScreenRuntime();
        }
    }
}

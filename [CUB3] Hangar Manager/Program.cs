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

        // PROPERTIS
        public bool Requestable { get; set; }
        public int SanctionTime { get; set; }

        // Router
        CommandRouter commandRouter;

        // INITS
        MyIni _ini = new MyIni();
        PlatformController platformController;

        public Program() {
            Runtime.UpdateFrequency = UpdateFrequency.Update10;
            platformController = new PlatformController(this);
            Configure();
        }

        private void Configure() {
            if (String.IsNullOrEmpty(Me.CustomData)) {
                SaveConfig(true);
            } else {
                GetConfigValues();
            }
        }

        private void SaveConfig(bool firstTime) {
            if (firstTime) {
                Requestable = false;
                SanctionTime = 60;
                SetConfigValues();
            } else {
                GetConfigValues();
                SetConfigValues();
            }
        }

        private void GetConfigValues() {
            MyIniParseResult result;
            if (!_ini.TryParse(Me.CustomData, out result))
                throw new Exception(result.ToString());
            Requestable = _ini.Get(Constants.CFG_S_CONFIG, Constants.CFG_P_REQUESTABLE).ToBoolean();
            SanctionTime = _ini.Get(Constants.CFG_S_CONFIG, Constants.CFG_P_REQUESTABLE).ToInt32();
        }

        private void SetConfigValues() {
            _ini.Set(Constants.CFG_S_CONFIG, Constants.CFG_P_REQUESTABLE, Requestable);
            _ini.Set(Constants.CFG_S_CONFIG, Constants.CFG_P_REQUESTABLE, SanctionTime);
        }

        public void Save() {
            
        }

        public void Main(string argument, UpdateType updateSource) {
            //commandRouter.ParseCommand(argument);
            

            platformController.Runtime();
        }
    }
}

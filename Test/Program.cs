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

        MyCommandLine _commandLine = new MyCommandLine();

        String customCommand;

        public Program() {
            Runtime.UpdateFrequency = UpdateFrequency.Update100;
        }

        public void Save() {
            
        }

        public void Main(string argument, UpdateType updateSource) {

            TestClass test = new TestClass();
            if (null != argument) {
                if (_commandLine.TryParse(argument)) {

                    string command = _commandLine.Argument(0);
                    Echo("Argument 0: " + _commandLine.Argument(0));
                    if (null == customCommand || command != customCommand) {
                        test.Name = command;
                        customCommand = command;
                    }
                    


                }
            }
            

            Echo("Command: " + customCommand);
            Echo("Test: " + test.Name);

        }
    }
}

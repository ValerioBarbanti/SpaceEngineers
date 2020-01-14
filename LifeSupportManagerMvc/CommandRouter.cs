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
        public class CommandRouter {

            Program myProgram;

            MyCommandLine _commandLine = new MyCommandLine();

            public CommandRouter(Program program) {
                myProgram = program;
            }

            public void ParseCommand(string argument) {
                if (_commandLine.TryParse(argument)) {
                    switch (_commandLine.Argument(0)) {
                        case Constants.C_CYCLE:
                            myProgram.airlockController.AddCommandToStack(_commandLine);
                            break;
                        case Constants.C_LEAK:
                            myProgram.leakController.AddCommandToStack(_commandLine);
                            break;
                        case Constants.C_SLIDE:
                            myProgram.productionController.AddCommandToStack(_commandLine);
                            break;
                        default:
                            myProgram.Echo($"No valid command specified: {_commandLine.Argument(0)}");
                            break;
                    }
                }
            }

        }
    }
}

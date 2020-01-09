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
        public class LeakController {

            Program myProgram;

            private List<IMyDoor> doors = new List<IMyDoor>();
            private List<IMyAirVent> airvents = new List<IMyAirVent>();

            public List<IMyDoor> Doors { get; set; }
            public List<IMyAirVent> Airvents { get; set; }

            public string Status { get; set; }

            public LeakController(Program program) {
                myProgram = program;
                Init();
            }

            private void Init() {
                myProgram.GridTerminalSystem.GetBlocksOfType(doors);
                myProgram.GridTerminalSystem.GetBlocksOfType(airvents);
                Doors = new List<IMyDoor>();
                Airvents = new List<IMyAirVent>();
                foreach (IMyAirVent airvent in airvents) {
                    if (!airvent.CustomData.Equals(Constants.T_LSM_AIRLOCK_AIRVENT)) {
                        Airvents.Add(airvent);
                    }
                }
                foreach (IMyDoor door in doors) {
                    if (!door.CustomData.Equals(Constants.T_LSM_AIRLOCK_DOOR)) {
                        Doors.Add(door);
                    }
                }
                Status = Constants.P_ON;
            }

            public void AddCommandToStack(MyCommandLine _commandLine) {
                if (null != _commandLine.Argument(1)) {
                    if (_commandLine.Argument(1).Equals(Constants.P_ON) || _commandLine.Argument(1).Equals(Constants.P_OFF)) {
                        Status = _commandLine.Argument(1);
                    } else {
                        myProgram.Echo("No valid command\n");
                    }
                }
            }

            public void LeakRuntime() {
                if (Status.Equals(Constants.P_ON)) {
                    CheckAndManageLeaks();
                }
            }

            private void CheckAndManageLeaks() {
                foreach(IMyAirVent airvent in Airvents) {
                    if (!airvent.CanPressurize) {
                        foreach (IMyDoor door in Doors) {
                            door.CloseDoor();
                        }
                    }
                }
            }
        }
    }
}

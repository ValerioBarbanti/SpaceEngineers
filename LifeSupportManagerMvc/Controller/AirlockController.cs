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
        public class AirlockController {

            Program myProgram;

            List<IMyBlockGroup> airlockGroups = new List<IMyBlockGroup>();

            public Dictionary<string, Airlock> Airlocks { get; set; }

            Dictionary<string, string[]> airlocksToCycle = new Dictionary<string, string[]>();

            List<string> airlocksToRemove = new List<string>();

            public AirlockController(Program program) {
                myProgram = program;
                Init();
            }

            public void Init() {
                if (null == Airlocks) {
                    Airlocks = new Dictionary<string, Airlock>();
                }
                myProgram.GridTerminalSystem.GetBlockGroups(airlockGroups, group => group.Name.Contains("Airlock"));
                if (airlockGroups.Count == 0) {
                    myProgram.Echo("Warning, there are no valid airlocks on your ship / station");
                } else {
                    foreach (IMyBlockGroup airlockGroup in airlockGroups) {
                        Airlock airlock = new Airlock(airlockGroup, myProgram);
                        if (airlock.IsAirlockValid()) {
                            Airlocks.Add(airlockGroup.Name, airlock);
                        }
                    }
                }
            }

            public void AddCommandToStack(MyCommandLine _commandLine) {
                if (Airlocks.Count > 0) {
                    if (null != _commandLine.Argument(1) && (_commandLine.Argument(1).Equals(Constants.P_DEPRESSURIZE) || _commandLine.Argument(1).Equals(Constants.P_PRESSURIZE)) && null != _commandLine.Argument(2)) {
                        Airlock airlock = null;
                        try {
                            airlock = Airlocks[_commandLine.Argument(2)];
                            if (null != airlock && airlock.IsAirlockValid()) {
                                myProgram.Echo($"Command added for airlock: {airlock.Name}\n");
                                string[] commandLineArray = new string[2];
                                commandLineArray[0] = _commandLine.Argument(1);
                                commandLineArray[1] = _commandLine.Argument(2);
                                airlocksToCycle[commandLineArray[1]] = commandLineArray;
                            } else {
                                myProgram.Echo($"No valid airlock with name {_commandLine.Argument(2)} found.\n");
                            }
                        } catch (Exception e) {
                            myProgram.Echo($"No valid airlock with name {_commandLine.Argument(2)} found.\n");
                        }
                    } else {
                        myProgram.Echo("No valid parameters specified\n");
                    }
                } else {
                    myProgram.Echo("There are no airlocks to cycle\n");
                }
            }

            public void AirlockRuntime() {
                CheckAirlockCycling();
                UpdateAirlock();
            }

            private void CheckAirlockCycling() {
                if (airlocksToCycle.Count != 0) {
                    foreach (KeyValuePair<string, string[]> airlockToCycle in airlocksToCycle) {
                        if (CanCycleAirlock(Airlocks[airlockToCycle.Key], airlockToCycle.Value[0])) {
                            Cycle(Airlocks[airlockToCycle.Key], airlockToCycle.Value[0]);
                        } else {
                            Airlocks[airlockToCycle.Key].PublicStatus = Constants.AP_ERROR;
                        }
                    }

                    if (airlocksToRemove.Count != 0) {
                        foreach (string airlock in airlocksToRemove) {
                            airlocksToCycle.Remove(airlock);
                        }
                        airlocksToRemove.Clear();
                    }
                }
            }

            private bool CanCycleAirlock(Airlock airlock, string command) {
                bool canCycle = true;
                foreach (IMyAirVent airvent in airlock.Airvents) {
                    if (!airvent.IsFunctional) {
                        airlock.Errors.Add($"Airvent {airvent.CustomName} is broken or not fully built\n");
                        canCycle = false;
                    }
                    if (!airvent.IsWorking) {
                        airlock.Errors.Add($"Airvent {airvent.CustomName} is not powered\n");
                        canCycle = false;
                    }
                }
                foreach (IMyDoor door in airlock.Doors) {
                    if (!door.IsFunctional) {
                        airlock.Errors.Add($"Door {door.CustomName} is broken or not fully built\n");
                        canCycle = false;
                    }
                    if (!door.IsWorking) {
                        airlock.Errors.Add($"Door {door.CustomName} is not powered\n");
                        canCycle = false;
                    }
                }
                if (command.Equals(Constants.P_PRESSURIZE)) {
                    bool emptyTanks = true;
                    foreach (IMyGasTank tank in airlock.Tanks) {
                        if (tank.FilledRatio > 0.05) {
                            emptyTanks = false;
                        }
                    }
                    if (emptyTanks) {
                        airlock.Errors.Add($"Not enough oxygen in tanks\n");
                        canCycle = false;
                    }
                }
                return canCycle;
            }

            private void Cycle(Airlock airlock, string command) {
                switch (airlock.Status) {
                    case Constants.A_IDLE:
                        airlock.Status = Constants.A_CLOSE_DOORS;
                        airlock.PublicStatus = Constants.AP_CYCLE;
                        break;
                    case Constants.A_CLOSE_DOORS:
                        CloseDoors(airlock);
                        break;
                    case Constants.A_CYCLE:
                        CyclePressure(airlock, command);
                        break;
                    case Constants.A_OPEN_DOORS:
                        OpenDoors(airlock, command);
                        break;
                    case Constants.A_COMPLETED:
                        CompleteCycle(airlock, command);
                        break;
                }
            }

            private void CloseDoors(Airlock airlock) {
                bool doorsAreClosed = true;
                foreach (IMyDoor door in airlock.Doors) {
                    if (!door.Status.Equals(DoorStatus.Closed) && !door.Status.Equals(DoorStatus.Closing)) {
                        door.CloseDoor();
                    }
                    if (!door.Status.Equals(DoorStatus.Closed)) {
                        doorsAreClosed = false;
                    }
                }
                if (doorsAreClosed) {
                    airlock.Status = Constants.A_CYCLE;
                }
            }

            private void CyclePressure(Airlock airlock, string command) {
                bool canOpenDoors = true;

                foreach (IMyAirVent airvent in airlock.Airvents) {
                    if (command.Equals(Constants.P_DEPRESSURIZE)) {
                        airvent.Depressurize = true;
                        if (airvent.GetOxygenLevel() != 0) {
                            canOpenDoors = false;
                        }
                    } else if (command.Equals(Constants.P_PRESSURIZE)) {
                        airvent.Depressurize = false;
                        if (airvent.GetOxygenLevel() != 1) {
                            canOpenDoors = false;
                        }
                    }
                }

                if (canOpenDoors) {
                    airlock.Status = Constants.A_OPEN_DOORS;
                }
            }


            private void OpenDoors(Airlock airlock, string command) {
                bool isDoorOpen = false;
                foreach (IMyDoor door in airlock.Doors) {
                    if (command.Equals(Constants.P_DEPRESSURIZE)) {
                        if (door.CustomData.Contains(Constants.DOOR_EXTERNAL)) {
                            if (!door.Status.Equals(DoorStatus.Open) && !door.Status.Equals(DoorStatus.Opening)) {
                                door.OpenDoor();
                            }
                            if (!door.Status.Equals(DoorStatus.Open)) {
                                isDoorOpen = true;
                            }
                        }
                    } else if (command.Equals(Constants.P_PRESSURIZE)) {
                        if (door.CustomData.Contains(Constants.DOOR_INTERNAL)) {
                            if (!door.Status.Equals(DoorStatus.Open) && !door.Status.Equals(DoorStatus.Opening)) {
                                door.OpenDoor();
                            }
                            if (!door.Status.Equals(DoorStatus.Open)) {
                                isDoorOpen = true;
                            }
                        }
                    }
                }
                if (!isDoorOpen) {
                    airlock.Status = Constants.A_COMPLETED;
                }
            }

            private void CompleteCycle(Airlock airlock, string command) {
                airlocksToRemove.Add(airlock.Name);
                airlock.Status = Constants.A_IDLE;
                if (command.Equals(Constants.P_DEPRESSURIZE)) {
                    airlock.PublicStatus = Constants.AP_DEPRESSURIZED;
                } else if (command.Equals(Constants.P_PRESSURIZE)) {
                    airlock.PublicStatus = Constants.AP_PRESSURIZED;
                }
            }

            private void UpdateAirlock() {
                foreach (KeyValuePair<string, Airlock> airlock in Airlocks) {
                    ChangeAirlockLightColor(airlock.Value);
                    UpdateAirlockInfos(airlock.Value);
                }
            }

            private void ChangeAirlockLightColor(Airlock airlock) {
                int colorNum = (int)Math.Round(airlock.Airvents[0].GetOxygenLevel() * 255);
                foreach (IMyLightingBlock light in airlock.Lights) {
                    light.Color = new Color(255, colorNum, colorNum);
                }
            }

            private void UpdateAirlockInfos(Airlock airlock) {
                airlock.GetAirlockDetails();
            }

            

            

        }
    }
}

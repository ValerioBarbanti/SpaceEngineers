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

        public class Airlock {

            public string Name { get; set; }
            public string Status { get; set; }
            public string Command { get; set; }
            public string PublicStatus { get; set; }

            // STATS
            public string RoomPressure { get; set; }
            public string OxygenLevel { get; set; }
            public string OpenDoors { get; set; }
            public string OxygenTankFill { get; set; }
            public string Issues { get; set; }


            private IMyBlockGroup _blockGroup;
            public bool CanCycle { get; set; }

            public string AirventStatus { get; set; }

            List<IMyAirVent> airvents = new List<IMyAirVent>();
            List<IMyTextPanel> screens = new List<IMyTextPanel>();
            List<IMyDoor> doors = new List<IMyDoor>();
            List<IMyInteriorLight> lights = new List<IMyInteriorLight>();
            List<IMyGasTank> oxygenTanks = new List<IMyGasTank>();

            // COLORS
            Color c_darkBlue = new Color(37, 46, 53);
            Color c_darkCyan = new Color(41, 54, 62);
            Color c_cyan = new Color(77, 99, 113);
            Color c_white = new Color(213, 236, 245);

            

            public Airlock(IMyBlockGroup blockGroup) {
                Status = Constants.IDLE_STATUS;
                PublicStatus = Constants.PS_IDLE;
                this._blockGroup = blockGroup;
                Name = blockGroup.Name;

                blockGroup.GetBlocksOfType(airvents);
                blockGroup.GetBlocksOfType(screens);
                blockGroup.GetBlocksOfType(doors);
                blockGroup.GetBlocksOfType(lights);
                blockGroup.GetBlocksOfType(oxygenTanks);

                InitAirlockBlocks();
            }

            private void InitAirlockBlocks() {
                foreach (IMyTextPanel screen in screens) {
                    screen.ContentType = ContentType.SCRIPT;
                }
            }

            public void Execute(String command) {
                Command = command;
                
                switch (Status) {
                    case Constants.IDLE_STATUS:
                        Status = Constants.START_CYCLE;
                        break;
                    case Constants.START_CYCLE:
                        PublicStatus = Constants.PS_DOORS_CLOSING;
                        CloseDoors();
                        break;
                    case Constants.DOORS_CLOSED:
                        PublicStatus = Constants.PS_CYCLING;
                        CycleAirvents();
                        break;
                    case Constants.DEPRESSURIZED:
                        PublicStatus = Constants.PS_DEPRESSURIZED;
                        OpenDoor(false);
                        break;
                    case Constants.PRESSURIZED:
                        PublicStatus = Constants.PS_PRESSURIZED;
                        OpenDoor(true);
                        break;
                    case Constants.END_CYCLE:
                        break;
                }
            }

            private void CloseDoors() {
                if (Command.Equals(Constants.PRESSURIZE) || Command.Equals(Constants.DEPRESSURIZE)) {
                    if (CheckIfDoorsAreOpen()) {
                        foreach (IMyDoor door in doors) {
                            if (!door.Status.Equals(DoorStatus.Closing)) {
                                door.CloseDoor();
                            }
                        }
                    } else {
                        Status = Constants.DOORS_CLOSED;
                    }
                }
            }

            private void CycleAirvents() {
                if (Command.Equals(Constants.PRESSURIZE)) {
                    
                    foreach (IMyAirVent airvent in airvents) {
                        AirventStatus = airvent.Status.ToString();
                        if (airvent.Status.Equals(VentStatus.Depressurized) || airvent.Status.Equals(VentStatus.Depressurizing)) {
                            airvent.Depressurize = false;
                        } else if (airvent.Status.Equals(VentStatus.Pressurized)) {
                            Status = Constants.PRESSURIZED;
                        }
                    }
                } else if (Command.Equals(Constants.DEPRESSURIZE)) {
                    foreach (IMyAirVent airvent in airvents) {
                        AirventStatus = airvent.Status.ToString();
                        if (airvent.Status.Equals(VentStatus.Pressurized) || airvent.Status.Equals(VentStatus.Pressurizing)) {
                            airvent.Depressurize = true;
                        } else if (airvent.GetOxygenLevel() < 0.01F) {
                            Status = Constants.DEPRESSURIZED;
                        }
                    }
                }
            }

            private void OpenDoor(bool isInternalDoor) {
                foreach(IMyDoor door in doors){
                    if (isInternalDoor) {
                        if (door.CustomData.Equals("internal")) {
                            if (door.Status.Equals(DoorStatus.Open) || door.Status.Equals(DoorStatus.Opening)) {
                                Status = Constants.END_CYCLE;
                            } else {
                                door.OpenDoor();
                            }
                            
                        }
                    } else {
                        if (door.CustomData.Equals("external")) {
                            if (door.Status.Equals(DoorStatus.Open) || door.Status.Equals(DoorStatus.Opening)) {
                                Status = Constants.END_CYCLE;
                            } else {
                                door.OpenDoor();
                            }
                            
                        }
                    }
                    
                }
            }

            private bool CheckIfDoorsAreOpen() {
                bool doorsAreOpen = false;
                foreach (IMyDoor door in doors) {
                    if (!door.Status.Equals(DoorStatus.Closed) && !door.Status.Equals(DoorStatus.Closing)) {
                        doorsAreOpen = true;
                    }
                }
                return doorsAreOpen;
            }

            public String UpdateStatusScreen() {

                CreateInformations();

                StringBuilder sb = new StringBuilder();
                foreach (IMyTextPanel screen in screens) {
                    using (var frame = screen.DrawFrame()) {
                        frame.AddRange(ScreenManager.CreateBackground(screen)); // Background

                        frame.AddRange(ScreenManager.CreateHeader(screen, Name + " - " + PublicStatus, 1.5f)); // Header

                        if (!screen.CustomData.Equals("airlock small")) {
                            frame.AddRange(ScreenManager.CreateFooter(screen, "CheeriOS v0.1\nLife Support Module", 0.75f)); // Footer

                            frame.AddRange(ScreenManager.CreateOxygenBar(screen, airvents)); // Oxygen Bar

                            StringBuilder infoTextLeft = new StringBuilder();
                            infoTextLeft.Append("Room pressure:\n\n");
                            infoTextLeft.Append("Oxygen Level:\n\n");
                            infoTextLeft.Append("Open door:\n\n");
                            infoTextLeft.Append("Oxygen Tank fill:\n\n");
                            infoTextLeft.Append("Issues:\n\n");

                            StringBuilder infoTextRight = new StringBuilder();
                            infoTextRight.Append(RoomPressure + "\n\n");
                            infoTextRight.Append(OxygenLevel + "\n\n");
                            infoTextRight.Append(OpenDoors + "\n\n");
                            infoTextRight.Append(OxygenTankFill + "\n\n");
                            infoTextRight.Append("\n" + Issues + "\n\n");

                            frame.AddRange(ScreenManager.CreateInfoPanel(screen, infoTextLeft.ToString(), infoTextRight.ToString()));
                        }

                    }
                }
                return sb.ToString();
            }

            private void CreateInformations() {
                CreateOxygenInfo();
                CreateDoorsInfo();
                CreateTanksInfo();
                CreateIssuesInfo();
            }

            private void CreateOxygenInfo() {
                StringBuilder sbAirvent = new StringBuilder();
                StringBuilder sbOxygenLevel = new StringBuilder();

                int oxigenLevelInt = 0;
                foreach (IMyAirVent airvent in airvents) {
                    oxigenLevelInt = (int)(airvent.GetOxygenLevel() * 100);
                    sbAirvent.Append((airvent.GetOxygenLevel() * 100).ToString("0.0") + " %");
                    if (airvent.GetOxygenLevel() > 0.75) {
                        sbOxygenLevel.Append("High");
                    } else if (airvent.GetOxygenLevel() <= 0.75 && airvent.GetOxygenLevel() > 0) {
                        sbOxygenLevel.Append("Low");
                    } else {
                        sbOxygenLevel.Append("None");
                    }
                }
                RoomPressure = sbAirvent.ToString();
                OxygenLevel = sbOxygenLevel.ToString();

                int colorPercentage = 255 / 100 * oxigenLevelInt;
                foreach (IMyLightingBlock light in lights) {
                    light.Color = new Color(255, colorPercentage, colorPercentage);
                }

            }

            private void CreateDoorsInfo() {
                StringBuilder sbDoors = new StringBuilder();
                bool isInternalDoorOpen = false;
                bool isExternalDoorOpen = false;
                foreach (IMyDoor door in doors) {
                    if (door.Status.Equals(DoorStatus.Open)) {
                        if (door.CustomData.Equals("internal")) {
                            isInternalDoorOpen = true;
                        } else if (door.CustomData.Equals("external")) {
                            isExternalDoorOpen = true;
                        }
                    }
                }
                if (isInternalDoorOpen && isExternalDoorOpen) {
                    sbDoors.Append("Both");
                } else if (isInternalDoorOpen && !isExternalDoorOpen) {
                    sbDoors.Append("Internal");
                } else if (isExternalDoorOpen && !isInternalDoorOpen) {
                    sbDoors.Append("External");
                } else {
                    sbDoors.Append("None");
                }
                OpenDoors = sbDoors.ToString();
            }

            private void CreateTanksInfo() {
                StringBuilder sbTanks = new StringBuilder();
                double tankFill = 0;
                foreach (IMyGasTank tank in oxygenTanks) {
                    tankFill += tank.FilledRatio;
                }
                tankFill = tankFill / oxygenTanks.Count;
                sbTanks.Append((tankFill * 100).ToString("0.0") + " %");
                OxygenTankFill = sbTanks.ToString();
            }

            private void CreateIssuesInfo() {
                StringBuilder sbIssues = new StringBuilder();

                // check for leaks
                foreach(IMyAirVent airvent in airvents) {
                    if (airvent.IsFunctional && airvent.Depressurize == false && airvent.GetOxygenLevel() < 0.1) {
                        sbIssues.Append("Leak detected\n");
                    }
                    if (!airvent.IsFunctional) {
                        sbIssues.Append("Airvent '" + airvent.CustomName + "' not working\n");
                    }
                }
                
                Issues = sbIssues.ToString();
                if (String.IsNullOrEmpty(Issues)) {
                    Issues = "None";
                }
            }

        }

    }
}

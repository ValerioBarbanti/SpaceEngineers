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

            Program myProgram;

            // Generic
            public string Name { get; set; }
            public string Command { get; set; }
            public string Status { get; set; }
            public string PublicStatus { get; set; }
            public bool IsValid { get; set; }

            // Stats
            public string RoomPressure { get; set; }
            public string OxygenLevel { get; set; }
            public string OxygenLevelDecimals { get; set; }
            public string OpenDoors { get; set; }
            public string OxygenTankFill { get; set; }
            public string OxygenTankFillDecimals { get; set; }
            public string Issues { get; set; }

            // Blocks
            public List<IMyAirVent> Airvents { get; set; }
            public List<IMyDoor> Doors { get; set; }
            public List<IMyGasTank> Tanks { get; set; }
            public List<IMyTextPanel> Panels { get; set; }
            public List<IMyTextSurface> Surfaces { get; set; }
            public List<IMyLightingBlock> Lights { get; set; }

            // Generics
            public List<string> Errors { get; set; }
            public List<string> Warnings { get; set; }

            public Airlock(IMyBlockGroup blockGroup, Program program) {
                myProgram = program;
                Errors = new List<string>();
                Warnings = new List<string>();

                Name = blockGroup.Name;
                Airvents = new List<IMyAirVent>();
                Doors = new List<IMyDoor>();
                Tanks = new List<IMyGasTank>();
                Panels = new List<IMyTextPanel>();
                Surfaces = new List<IMyTextSurface>();
                Lights = new List<IMyLightingBlock>();

                // necessary
                blockGroup.GetBlocksOfType(Airvents);
                blockGroup.GetBlocksOfType(Doors);
                blockGroup.GetBlocksOfType(Tanks);

                // not necessary
                blockGroup.GetBlocksOfType(Panels);
                blockGroup.GetBlocksOfType(Surfaces);
                blockGroup.GetBlocksOfType(Lights);

                // init
                Status = Constants.A_IDLE;
                PublicStatus = Constants.AP_IDLE;

                // write CustomData on doors and airvents
                foreach (IMyAirVent airvent in Airvents) {
                    airvent.CustomData = Constants.T_LSM_AIRLOCK_AIRVENT;
                }
                foreach (IMyDoor door in Doors) {
                    if (!door.CustomData.Contains(Constants.T_LSM_AIRLOCK_DOOR)) {
                        string temp = door.CustomData;
                        door.CustomData = Constants.T_LSM_AIRLOCK_DOOR + " " + temp;
                    }
                }
            }

            public bool IsAirlockValid() {
                // Errors
                if (Utils.IsListEmpty(Airvents)) {
                    string error = "ERROR: No airvents found.\n";
                    myProgram.Echo(error);
                    Errors.Add(error);
                }
                if (Utils.IsListEmpty(Doors)) {
                    string error = "ERROR: No doors found.\n";
                    myProgram.Echo(error);
                    Errors.Add(error);
                }
                if (Utils.IsListEmpty(Tanks)) {
                    string error = "ERROR: No oxygen tanks found.\n";
                    myProgram.Echo(error);
                    Errors.Add(error);
                }

                // Warnings
                if (Utils.IsListEmpty(Panels) || Utils.IsListEmpty(Surfaces)) {
                    string warning = "WARNING: No screens found.\n";
                    myProgram.Echo(warning);
                    Warnings.Add(warning);
                }
                if (Utils.IsListEmpty(Lights)) {
                    string warning = "WARNING: No lights found.\n";
                    myProgram.Echo(warning);
                    Warnings.Add(warning);
                }

                if (!Utils.IsListEmpty(Errors)) {
                    foreach (string error in Errors) {
                        myProgram.Echo(error);
                    }
                    myProgram.Echo($"There has been an error with {Name}, please fix your airlock\n");
                    return false;
                } else {
                    return true;
                }
            }

            public void GetAirlockDetails() {
                UpdateOxygenStatus();
                UpdateDoorStatus();
                UpdateTanksStatus();
            }

            private void UpdateOxygenStatus() {
                float oxygenLevel = 0;
                foreach (IMyAirVent airvent in Airvents) {
                    oxygenLevel += airvent.GetOxygenLevel();
                }
                oxygenLevel = oxygenLevel / Airvents.Count;
                OxygenLevel = oxygenLevel.ToString("0.0");
                OxygenLevelDecimals = oxygenLevel.ToString("0.000");
                if (oxygenLevel >= 0.7) {
                    RoomPressure = "High Pressure";
                } else if (oxygenLevel < 0.7 && oxygenLevel >= 0.1) {
                    RoomPressure = "Low Pressure";
                } else {
                    RoomPressure = "No Pressure";
                }

            }

            private void UpdateDoorStatus() {
                bool areInternalDoorsOpen = false;
                bool areExternalDoorsOpen = false;
                foreach (IMyDoor door in Doors) {
                    if (door.Status.Equals(DoorStatus.Open) || door.Status.Equals(DoorStatus.Opening)) {
                        if (door.CustomData.Contains(Constants.DOOR_INTERNAL)) {
                            areInternalDoorsOpen = true;
                        } else if (door.CustomData.Contains(Constants.DOOR_EXTERNAL)) {
                            areExternalDoorsOpen = true;
                        }
                    }
                }
                if (areInternalDoorsOpen && areExternalDoorsOpen) {
                    OpenDoors = "Both";
                } else if (areInternalDoorsOpen && !areExternalDoorsOpen) {
                    OpenDoors = "Internal";
                } else if (!areInternalDoorsOpen && areExternalDoorsOpen) {
                    OpenDoors = "External";
                } else {
                    OpenDoors = "None";
                }
            }

            private void UpdateTanksStatus() {
                double filledRatio = 0;
                foreach (IMyGasTank tank in Tanks) {
                    filledRatio += tank.FilledRatio;
                }
                filledRatio = (filledRatio / Tanks.Count)*100;
                OxygenTankFill = filledRatio.ToString("0.0");
                OxygenTankFillDecimals = (filledRatio / 100).ToString("0.000");
            }



        }
    }
}

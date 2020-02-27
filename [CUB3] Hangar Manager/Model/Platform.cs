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
        public class Platform {

            // Generic
            Program myProgram;
            public enum PlatformCategory { Sensor = 0, Connector = 1, Full = 2, None = 3 }
            public enum PlatformType { Hangar = 0, LandingPad = 1 }
            public enum PlatformStatus { Clear = 0, Requested = 1, Obstructed = 2, Occupied = 3 };

            // Stats
            public string Name { get; set; }
            public string Code { get; set; }
            public PlatformCategory Category { get; set; }
            public PlatformType Type { get; set; }
            public PlatformStatus Status { get; set; }
            public bool Sanctionable { get; set; }
            public bool Assigned { get; set; }
            public bool Valid { get; set; }

            // Blocks
            public List<IMySensorBlock> Sensors { get; set; }
            public List<IMyShipConnector> Connectors { get; set; }
            public List<IMyAirtightHangarDoor> Doors { get; set; }
            public List<IMyTextSurfaceProvider> Surfaces { get; set; }
            public List<IMyLightingBlock> Lights { get; set; }
            public List<IMyLargeTurretBase> Turrets { get; set; }

            // Specific blocks
            public IMySensorBlock SensorInternal { get; set; }
            public IMySensorBlock SensorExternal { get; set; }
            public IMyShipConnector Connector { get; set; }

            // Generics
            public List<string> Errors { get; set; }
            public List<string> Warnings { get; set; }

            // Entities
            public List<MyDetectedEntityInfo> Entities { get; set; }

            long firstEntityDetection;

            public Platform(IMyBlockGroup blockGroup, Program program) {
                myProgram = program;

                Name = blockGroup.Name;
                Status = PlatformStatus.Clear;

                InitPlatformBlocks(blockGroup);

                DetectPlatformCategory();
                DetectPlatformType();
                DetectPlatformDefense();

                IsPlatformValid();

                if (Valid) {
                    InitSensors();
                    InitTurrets();
                }
            }

            private void InitPlatformBlocks(IMyBlockGroup blockGroup) {
                Sensors = new List<IMySensorBlock>();
                Connectors = new List<IMyShipConnector>();
                Doors = new List<IMyAirtightHangarDoor>();
                Surfaces = new List<IMyTextSurfaceProvider>();
                Lights = new List<IMyLightingBlock>();
                Turrets = new List<IMyLargeTurretBase>();

                blockGroup.GetBlocksOfType(Sensors);
                blockGroup.GetBlocksOfType(Connectors);
                blockGroup.GetBlocksOfType(Doors);
                blockGroup.GetBlocksOfType(Surfaces);
                blockGroup.GetBlocksOfType(Lights);
                blockGroup.GetBlocksOfType(Turrets);

                Entities = new List<MyDetectedEntityInfo>();
            }

            private void DetectPlatformCategory() {
                bool isSensor = false;
                bool isConnector = false;

                if (!Utils.IsListEmpty(Sensors)) {
                    isSensor = true;
                }
                if (!Utils.IsListEmpty(Sensors)) {
                    isConnector = true;
                }

                if (isSensor && isConnector) {
                    Category = PlatformCategory.Full;
                } else if (isSensor && !isConnector) {
                    Category = PlatformCategory.Sensor;
                } else if (!isSensor && isConnector) {
                    Category = PlatformCategory.Connector;
                } else {
                    Category = PlatformCategory.None;
                }
            }

            private void DetectPlatformType() {
                if (!Utils.IsListEmpty(Doors)) {
                    Type = PlatformType.Hangar;
                } else {
                    Type = PlatformType.LandingPad;
                }
            }

            private void DetectPlatformDefense() {
                if (!Utils.IsListEmpty(Turrets)) {
                    Sanctionable = true;
                } else {
                    Sanctionable = false;
                }
            }

            public void IsPlatformValid() {
                if (Category.Equals(PlatformCategory.None)) {
                    Valid = false;
                } else {
                    Valid = true;
                }
            }

            private void InitSensors() {
                if (Category.Equals(PlatformCategory.Sensor) || Type.Equals(PlatformCategory.Full)) {
                    foreach (IMySensorBlock sensor in Sensors) {
                        sensor.DetectPlayers = true;
                        sensor.DetectFloatingObjects = true;
                        sensor.DetectSmallShips = true;
                        sensor.DetectLargeShips = true;
                        sensor.DetectStations = false;
                        sensor.DetectSubgrids = true;
                        sensor.DetectAsteroids = false;
                        sensor.DetectOwner = true;
                        sensor.DetectFriendly = true;
                        sensor.DetectEnemy = true;
                        sensor.DetectNeutral = true;
                    }
                }
            }

            private void InitTurrets() {
                if (Sanctionable) {
                    foreach (IMyLargeTurretBase turret in Turrets) {
                        turret.Enabled = false;
                    }
                }
            }

            public void CheckForTurrets() {
                foreach (MyDetectedEntityInfo entity in Entities) {
                    if (firstEntityDetection == 0) {
                        firstEntityDetection = entity.TimeStamp;
                    }
                    if (entity.Relationship.Equals(MyRelationsBetweenPlayerAndBlock.Neutral)) {
                        if (entity.TimeStamp - firstEntityDetection >= 10000) {
                            foreach (IMyLargeTurretBase turret in Turrets) {
                                turret.Enabled = true;
                                turret.SetTarget(entity.Position);
                                turret.GetActionWithName("ShootOnce").Apply(turret);
                            }
                        }
                    } else if (entity.Relationship.Equals(MyRelationsBetweenPlayerAndBlock.Enemies)) {
                        foreach (IMyLargeTurretBase turret in Turrets) {
                            turret.Enabled = true;
                            turret.SetTarget(entity.Position);
                            turret.GetActionWithName("ShootOnce").Apply(turret);
                        }
                    }
                }
            }

        }
    }
}

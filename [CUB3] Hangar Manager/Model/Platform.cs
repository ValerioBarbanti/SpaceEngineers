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
            public enum PlatformStatus { Clear = 0, Requested = 1, Obstructed = 2, Occupied = 3 };
            public enum PlatformType { Sensor = 0, Connector = 1, Both = 2, None = 3 }

            // Stats
            public string Name { get; set; }
            public PlatformStatus Status { get; set; }
            public PlatformType Type { get; set; }

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

            List<MyDetectedEntityInfo> Entities { get; set; }

            long firstEntityDetection;

            public Platform(IMyBlockGroup blockGroup, Program program) {
                myProgram = program;

                Name = blockGroup.Name;
                Status = PlatformStatus.Clear;

                InitPlatformBlocks(blockGroup);
                DetectPlatformType();
                InitSensors();
                InitTurrets();
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

            private void DetectPlatformType() {
                myProgram.Echo($"{Name} - sensors: {Sensors.Count}");
                myProgram.Echo($"{Name} - connectors: {Connectors.Count}");
                if (!Utils.IsListEmpty(Sensors) && Utils.IsListEmpty(Connectors)) {
                    Type = PlatformType.Sensor;
                } else if (Utils.IsListEmpty(Sensors) && !Utils.IsListEmpty(Connectors)) {
                    Type = PlatformType.Connector;
                } else if (!Utils.IsListEmpty(Sensors) && !Utils.IsListEmpty(Connectors)) {
                    Type = PlatformType.Both;
                } else {
                    Type = PlatformType.None;
                }
                myProgram.Echo($"{Name} is of type: {Type}");
            }

            private void InitSensors() {
                if (Type.Equals(PlatformType.Sensor) || Type.Equals(PlatformType.Both)) {
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
                foreach (IMyLargeTurretBase turret in Turrets) {
                    turret.Enabled = false;
                }
            }

            public bool IsPlatformValid() {
                if (Type.Equals(PlatformType.None)) {
                    return false;
                }
                return true;
            }

            public void CheckStatus() {
                bool areSensorsDetecting = false;
                bool isConnectorDetecting = false;
                bool isConnectorConnected = false;

                if (!Utils.IsListEmpty(Sensors)) {
                    foreach (IMySensorBlock sensor in Sensors) {
                        if (sensor.IsActive) {
                            areSensorsDetecting = true;
                            sensor.DetectedEntities(Entities);
                            foreach(MyDetectedEntityInfo entity in Entities) {
                                myProgram.Echo($"Entity: {entity.Relationship}");
                            }
                        }
                    }
                }

                if (!Utils.IsListEmpty(Connectors)) {
                    foreach (IMyShipConnector connector in Connectors) {
                        if (connector.Status.Equals(MyShipConnectorStatus.Connectable)) {
                            isConnectorDetecting = true;
                        }
                        if (connector.Status.Equals(MyShipConnectorStatus.Connected)) {
                            isConnectorConnected = true;
                        }
                    }
                }

                if (isConnectorConnected) {
                    Status = PlatformStatus.Occupied;
                } else if (areSensorsDetecting || isConnectorDetecting) {
                    Status = PlatformStatus.Obstructed;
                } else {
                    Status = PlatformStatus.Clear;
                    firstEntityDetection = 0;
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

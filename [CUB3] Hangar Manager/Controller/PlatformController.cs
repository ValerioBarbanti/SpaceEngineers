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
        public class PlatformController {

            Program myProgram;

            List<IMyBlockGroup> platformGroups = new List<IMyBlockGroup>();

            public Dictionary<string, Platform> Platforms { get; set; }

            public PlatformController(Program program) {
                myProgram = program;
                Platforms = new Dictionary<string, Platform>();
                Init();
            }

            private void Init() {
                myProgram.GridTerminalSystem.GetBlockGroups(platformGroups, group => group.Name.Contains("Platform"));

                if (platformGroups.Count == 0) {
                    myProgram.Echo("Warning, there are no valid platforms on your ship / station");
                } else {
                    int counter = 1;
                    foreach (IMyBlockGroup platformGroup in platformGroups) {
                        Platform platform = new Platform(platformGroup, myProgram);
                        platform.Code = counter.ToString("00");
                        Platforms.Add(platform.Name, platform);
                        counter++;
                    }
                }
            }

            public void AddCommandToStack(MyCommandLine _commandLine) {
                if (Platforms.Count > 0 && null!=_commandLine.Argument(0)) {
                    switch (_commandLine.Argument(0)) {
                        case Constants.C_ASSIGN:
                            if (null == _commandLine.Argument(1) || _commandLine.Argument(1).Equals(Constants.C_AUTO)) {
                                AssignLandingPad(null);
                            } else {
                                foreach (KeyValuePair<string, Platform> value in Platforms) {
                                    if (value.Value.Name.Equals(_commandLine.Argument(1))) {
                                        AssignLandingPad(_commandLine.Argument(1));
                                    }
                                }
                            }
                            break;
                        default:
                            myProgram.Echo("No valid parameters specified\n");
                            break;
                    }
                } else {
                    myProgram.Echo("There are no landing platforms on the ship / station\n");
                }
            }

            public void AssignLandingPad(string name) {
                if (null == name) {
                    foreach (KeyValuePair<string, Platform> value in Platforms) {
                        if (!value.Value.Assigned) {
                            value.Value.Assigned = true;
                            break;
                        }                        
                    }
                } else {
                    if (!Platforms[name].Assigned) {
                        Platforms[name].Assigned = true;
                    }
                }
            }

            public void Runtime() {
                foreach (KeyValuePair<string, Platform> value in Platforms) {
                    Platform platform = value.Value;
                    CheckLandingPadStatus(platform);
                    ColorLandingPadLights(platform);
                    DrawScreenDebug(platform);
                }
            }

            private void CheckLandingPadStatus(Platform platform) {
                platform.Entities.Clear();

                bool isPlatformObstructed = false;
                bool isPlatformOccupied = false;

                if (platform.Category.Equals(Platform.PlatformCategory.Sensor)) {
                    CheckSensorStatus(platform, ref isPlatformObstructed);
                } else if (platform.Category.Equals(Platform.PlatformCategory.Connector)) {
                    CheckConnectorStatus(platform, ref isPlatformObstructed, ref isPlatformOccupied);
                } else if (platform.Category.Equals(Platform.PlatformCategory.Full)) {
                    CheckSensorStatus(platform, ref isPlatformObstructed);
                    CheckConnectorStatus(platform, ref isPlatformObstructed, ref isPlatformOccupied);
                }

                myProgram.Echo($"Occupied: {isPlatformOccupied}");
                myProgram.Echo($"Obstructed: {isPlatformObstructed}");

                if (isPlatformOccupied) {
                    platform.Status = Platform.PlatformStatus.Occupied;
                } else if (isPlatformObstructed && !isPlatformOccupied) {
                    platform.Status = Platform.PlatformStatus.Obstructed;
                } else {
                    platform.Status = Platform.PlatformStatus.Clear;
                }
            }

            private void CheckSensorStatus(Platform platform, ref bool isPlatformObstructed) {
                foreach (IMySensorBlock sensor in platform.Sensors) {
                    if (sensor.IsActive) {
                        myProgram.Echo($"IsActive: {sensor.IsActive}");
                        isPlatformObstructed = true;
                        sensor.DetectedEntities(platform.Entities);
                    }
                }
            }

            private void CheckConnectorStatus(Platform platform, ref bool isPlatformObstructed, ref bool isPlatformOccupied) {
                foreach (IMyShipConnector connector in platform.Connectors) {
                    if (connector.Status.Equals(MyShipConnectorStatus.Connectable)) {
                        isPlatformObstructed = true;
                    }
                    if (connector.Status.Equals(MyShipConnectorStatus.Connected)) {
                        isPlatformOccupied = true;
                    }
                }
            }

            private void ColorLandingPadLights(Platform platform) {
                foreach (IMyLightingBlock light in platform.Lights) {
                    if (platform.Status.Equals(Platform.PlatformStatus.Clear)) {
                        light.Color = Constants.COLOR_WHITE;
                    } else if (platform.Status.Equals(Platform.PlatformStatus.Requested)) {
                        light.Color = Constants.COLOR_GREEN;
                    } else if(platform.Status.Equals(Platform.PlatformStatus.Obstructed)) {
                        light.Color = Constants.COLOR_YELLOW;
                    } else if (platform.Status.Equals(Platform.PlatformStatus.Occupied)) {
                        light.Color = Constants.COLOR_RED;
                    }
                }
            }

            private void DrawScreenDebug(Platform platform) {
                
                foreach (IMyTextSurfaceProvider surfaceProvider in platform.Surfaces) {
                    IMyTextSurface surface = surfaceProvider.GetSurface(0);
                    surface.ContentType = ContentType.SCRIPT;
                    RectangleF _viewport = new RectangleF((surface.TextureSize - surface.SurfaceSize) / 2f, surface.SurfaceSize);
                    using (var frame = surface.DrawFrame()) {

                        var background = MySprite.CreateSprite("SquareSimple", _viewport.Center, _viewport.Size);
                        background.Color = Constants.COLOR_BACKGROUND;
                        frame.Add(background);

                        Vector2 ocSize = percentageSize(80, _viewport);
                        var outerCircle = MySprite.CreateSprite("Circle", _viewport.Center, ocSize);
                        if (platform.Status.Equals(Platform.PlatformStatus.Clear)) {
                            outerCircle.Color = Constants.COLOR_WHITE;
                        } else if (platform.Status.Equals(Platform.PlatformStatus.Requested)) {
                            outerCircle.Color = Constants.COLOR_GREEN;
                        } else if (platform.Status.Equals(Platform.PlatformStatus.Obstructed)) {
                            outerCircle.Color = Constants.COLOR_YELLOW;
                        } else if (platform.Status.Equals(Platform.PlatformStatus.Occupied)) {
                            outerCircle.Color = Constants.COLOR_RED;
                        }
                        frame.Add(outerCircle);

                        Vector2 icSize = percentageSize(60, _viewport);
                        var innerCircle = MySprite.CreateSprite("Circle", _viewport.Center, icSize);
                        innerCircle.Color = Constants.COLOR_BACKGROUND_MASK;
                        frame.Add(innerCircle);

                        float size = TextSize(40, _viewport);
                        float offset = TextSizeOffset(size);
                        var platformCode = MySprite.CreateText($"{platform.Code}", "White", Color.White, size, TextAlignment.CENTER);
                        Vector2 pcPos = new Vector2(_viewport.Size.X / 2, _viewport.Size.Y / 2 - offset) + _viewport.Position;
                        platformCode.Position = pcPos;
                        frame.Add(platformCode);
                    }
                }
            }

            private Vector2 percentageSize(float percentage, RectangleF _viewport) {
                if (_viewport.Size.X <= _viewport.Size.Y) {
                    return new Vector2(_viewport.Size.X * (percentage / 100), _viewport.Size.X * (percentage / 100));
                } else {
                    return new Vector2(_viewport.Size.Y * (percentage / 100), _viewport.Size.Y * (percentage / 100));
                }
            }

            private float TextSize(float percentage, RectangleF _viewport) {
                if (_viewport.Size.X <= _viewport.Size.Y) {
                    return percentage / (24 / _viewport.Size.X * 100);
                } else {
                    return percentage / (24 / _viewport.Size.Y * 100);
                }
            }

            private float TextSizeOffset(float size) {
                float emptySpace = 7 * size;
                float fontSpace = ((24 - 7) * size);
                float remainingSpace = emptySpace + (fontSpace / 2);
                return remainingSpace;
            }


        }
    }
}

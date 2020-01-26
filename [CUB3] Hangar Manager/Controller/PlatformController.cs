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
                    foreach (IMyBlockGroup platformGroup in platformGroups) {
                        Platform platform = new Platform(platformGroup, myProgram);
                        myProgram.Echo($"Platform: {platform.Name}");
                        if (platform.IsPlatformValid()) {
                            myProgram.Echo($"{platform.Name} is valid");
                            Platforms.Add(platformGroup.Name, platform);
                        } else {
                            myProgram.Echo($"{platform.Name} is not valid");
                        }
                    }
                }
            }

            public void Runtime() {
                myProgram.Echo($"Platforms: {Platforms.Count}");
                foreach (KeyValuePair<string, Platform> value in Platforms) {
                    Platform platform = value.Value;
                    platform.CheckStatus();
                    switch (platform.Status) {
                        case Platform.PlatformStatus.Clear:
                            foreach (IMyLightingBlock light in platform.Lights) {
                                light.Color = Color.Green;
                            }
                            break;
                        case Platform.PlatformStatus.Obstructed:
                            foreach (IMyLightingBlock light in platform.Lights) {
                                light.Color = Color.Yellow;
                            }
                            platform.CheckForTurrets();
                            break;
                        case Platform.PlatformStatus.Occupied:
                            foreach (IMyLightingBlock light in platform.Lights) {
                                light.Color = Color.Red;
                            }
                            break;
                        case Platform.PlatformStatus.Requested:
                            break;
                    }
                }
            }
            

        }
    }
}

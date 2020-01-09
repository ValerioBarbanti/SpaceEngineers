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

        List<IMyBlockGroup> blockGroups = new List<IMyBlockGroup>();

        public Program() {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            InitAirlocks();
        }

        public void Save() {
            
        }

        public void Main(string argument, UpdateType updateSource) {
            //UpdateAirlockScreens(blockGroups);
            InitAirlocks();
            if (argument.Equals("Init")) {
                InitAirlocks();
            } else {
                string[] words = argument.Split(' ');
                if (words[0].Equals("Cycle")) {
                    CycleAirlock(words[1]);
                }
            }
            
        }
        
        private void InitAirlocks() {
            GridTerminalSystem.GetBlockGroups(blockGroups, group => group.Name.Contains("Airlock"));
            foreach (IMyBlockGroup blockGroup in blockGroups) {
                InitSingleAirlock(blockGroup);
            }
        }

        private void InitSingleAirlock(IMyBlockGroup blockGroup) {
            List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
            blockGroup.GetBlocks(blocks);

            List<IMyAirVent> airvents = new List<IMyAirVent>();
            blockGroup.GetBlocksOfType<IMyAirVent>(airvents);

            List<IMyTextSurfaceProvider> screens = new List<IMyTextSurfaceProvider>();
            blockGroup.GetBlocksOfType<IMyTextSurfaceProvider>(screens);

            List<IMyDoor> doors = new List<IMyDoor>();
            blockGroup.GetBlocksOfType<IMyDoor>(doors);

            IMyAirVent airvent = airvents[0];
            float airventInfo = airvent.GetOxygenLevel()*10;
            float airventInfoPer = airventInfo * 10;
            int airventInt = (int)airventInfo;
            int airventIntPercentage = (int)airventInfoPer;
            DrawFrameOnScreens(screens, airventInt, airventIntPercentage);

            //DrawTextOnScreens(screens, airventInt.ToString());
        }

        private String AirventStatus(IMyAirVent airvent) {
            StringBuilder status = new StringBuilder();
            status.Append(airvent.DetailedInfo);
            return status.ToString();
        }

        private void DrawFrameOnScreens(List<IMyTextSurfaceProvider> screens, int oxygenLevel, int oxygenLevelPercentage) {
            foreach (IMyTextSurfaceProvider screen in screens) {

                var surface = screen.GetSurface(0);
                var size = surface.TextureSize; // size of the screen - Vector2
                var halfSize = size / 2F;

                var offsetX = 10;
                var offsetY = 40;
                var x = 64;
                var y = 352;
                var w = 54;
                var h = 24;

                using (var frame = surface.DrawFrame()) {
                    for (int i = 0; i < oxygenLevel; i++) {

                        

                        var oxBarPos = new Vector2(offsetX + x, offsetY + y -(32 * (i+1)));
                        var oxBarSize = new Vector2(w, h);
                        var oxBar = MySprite.CreateSprite("SquareSimple", oxBarPos, oxBarSize);
                        oxBar.Color = new Color(250-(25*i), 25*i, 25*i);

                        var oxNum = MySprite.CreateText(oxygenLevelPercentage.ToString(), "Debug", new Color(1f), 2f, TextAlignment.CENTER);
                        oxNum.Position = new Vector2(offsetX + x, offsetY + 352);

                        frame.Add(oxBar);
                        frame.Add(oxNum);
                    }
                }
            }
        }

        private void DrawTextOnScreens(List<IMyTextSurfaceProvider> screens, string text) {
            foreach (IMyTextSurfaceProvider screen in screens) {
                screen.GetSurface(0).WriteText(text);
                

            }
        }

        private void CycleAirlock(string airlockNumber) {
            foreach (IMyBlockGroup blockGroup in blockGroups) {
                if (blockGroup.Name.Contains(airlockNumber)) {
                    // porte
                    List<IMyDoor> doors = new List<IMyDoor>();
                    blockGroup.GetBlocksOfType<IMyDoor>(doors);
                    foreach (IMyDoor door in doors) {
                        door.CloseDoor();
                    }

                    //airvents
                    List<IMyAirVent> airvents = new List<IMyAirVent>();
                    blockGroup.GetBlocksOfType<IMyAirVent>(airvents);
                    foreach(IMyAirVent airvent in airvents) {
                        if ()
                    }
                }
            }
        }

        private void UpdateAirlockScreens() {
            foreach (IMyBlockGroup blockGroup in blockGroups) {
                List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
                blockGroup.GetBlocks(blocks);
                foreach (IMyTerminalBlock block in blocks) {

                }
            }
        }

        private String GetAirlockStatus() {
            StringBuilder airlockStatus = new StringBuilder();
            

            return airlockStatus.ToString();
        }

    }
}

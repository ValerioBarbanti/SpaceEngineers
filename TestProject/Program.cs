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

        List<IMyTextPanel> panels = new List<IMyTextPanel>();

        public Program() {
            GridTerminalSystem.GetBlocksOfType(panels);
        }

        public void Save() {

        }

        public void Main(string argument, UpdateType updateSource) {
            foreach (IMyTextPanel screen in panels) {
                screen.ContentType = ContentType.SCRIPT;             

                using (var frame = screen.DrawFrame()) {

                    Echo("" + screen.TextureSize.X + "," + screen.TextureSize.Y);

                    

                    List<MySprite> spriteList = new List<MySprite>();

                    var border = MySprite.CreateSprite("SquareSimple", new Vector2(256, 42.5f), new Vector2(512, 85));
                    border.Color = Color.Red;

                    var border2 = MySprite.CreateSprite("SquareSimple", new Vector2(256, 42.5f), new Vector2(507, 80));
                    border2.Color = Color.Black;

                    var center = MySprite.CreateSprite("Circle", new Vector2(256, 256), new Vector2(10, 10));
                    center.Color = Color.Red;

                    spriteList.Add(border);
                    spriteList.Add(border2);
                    spriteList.Add(center);

                    

                    frame.AddRange(spriteList);

                }

                
                
            }
        }
    }
}

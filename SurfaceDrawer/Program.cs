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

        List<IMyTerminalBlock> blocks = new List<IMyTerminalBlock>();
        List<IMyTextSurfaceProvider> surfaceProviders = new List<IMyTextSurfaceProvider>();
        List<IMyTextSurface> surfaces = new List<IMyTextSurface>();



        public Program() {
            GridTerminalSystem.GetBlocks(blocks);
            foreach (IMyTerminalBlock block in blocks) {
                IMyTextSurfaceProvider surfaceProvider = block as IMyTextSurfaceProvider;
                if (null != surfaceProvider) {
                    Echo($"Block: {block.CustomName}, {surfaceProvider.SurfaceCount}");
                    surfaceProviders.Add(surfaceProvider);
                }
            }
            foreach (IMyTextSurfaceProvider prov in surfaceProviders) {
                int count = prov.SurfaceCount;
                for (int i = 0; i < count; i++) {
                    IMyTextSurface surface = prov.GetSurface(i);
                    surface.ContentType = ContentType.SCRIPT;
                    surface.Script = "None";
                    surfaces.Add(surface);
                }
            }
            //Runtime.UpdateFrequency = UpdateFrequency.Update100;
        }

        public void Save() {

        }

        public void Main(string argument, UpdateType updateSource) {
            foreach (IMyTextSurface surface in surfaces) {
                Echo($"Surface: {surface.SurfaceSize}");

                RectangleF _viewport = new RectangleF(
                    (surface.TextureSize - surface.SurfaceSize) / 2f,
                    surface.SurfaceSize
                );

                var frame = surface.DrawFrame();
                DrawSprites(ref frame, _viewport);
                frame.Dispose();
            }
        }

        public void DrawSprites(ref MySpriteDrawFrame frame, RectangleF viewport) {

            // Set up the initial position - and remember to add our viewport offset
            var position = new Vector2(256, 20) + viewport.Position;

            // Create our first line
            var sprite = new MySprite() {
                Type = SpriteType.TEXT,
                Data = "Line 1",
                Position = position,
                RotationOrScale = 0.8f /* 80 % of the font's default size */,
                Color = Color.Red,
                Alignment = TextAlignment.CENTER /* Center the text on the position */,
                FontId = "White"
            };
            // Add the sprite to the frame
            frame.Add(sprite);

            // Move our position 20 pixels down in the viewport for the next line
            position += new Vector2(0, 20);

            // Create our second line, we'll just reuse our previous sprite variable - this is not necessary, just
            // a simplification in this case.
            sprite = sprite = new MySprite() {
                Type = SpriteType.TEXT,
                Data = "Line 1",
                Position = position,
                RotationOrScale = 0.8f,
                Color = Color.Blue,
                Alignment = TextAlignment.CENTER,
                FontId = "White"
            };
            // Add the sprite to the frame
            frame.Add(sprite);

        }
    }
}

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
            Runtime.UpdateFrequency = UpdateFrequency.Update10;
            GridTerminalSystem.GetBlocks(blocks);
            foreach (IMyTerminalBlock block in blocks) {
                IMyTextSurfaceProvider surfaceProvider = block as IMyTextSurfaceProvider;
                if (null != surfaceProvider) {
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

                RectangleF _viewport = new RectangleF(
                    (surface.TextureSize - surface.SurfaceSize) / 2f,
                    surface.SurfaceSize
                );

                var frame = surface.DrawFrame();
                DrawSprites(ref frame, _viewport);
                frame.Dispose();
            }
        }

        public void DrawSprites(ref MySpriteDrawFrame frame, RectangleF _viewport) {

            var background = MySprite.CreateSprite("SquareSimple", _viewport.Center, _viewport.Size);
            background.Color = new Color(26, 28, 32);
            frame.Add(background);

            Vector2 ocSize = percentageSize(80, _viewport);
            var outerCircle = MySprite.CreateSprite("Circle", _viewport.Center, ocSize);
            outerCircle.Color = new Color(29, 229, 128);
            frame.Add(outerCircle);

            Vector2 icSize = percentageSize(60, _viewport);
            var innerCircle = MySprite.CreateSprite("Circle", _viewport.Center, icSize);
            innerCircle.Color = new Color(37, 39, 45);
            frame.Add(innerCircle);

            float size = TextSize(40, _viewport);
            float offset = TextSizeOffset(size);
            var platformCode = MySprite.CreateText("01", "White", Color.White, size, TextAlignment.CENTER);
            // Vector2 pcPos = new Vector2(_viewport.Center.X, _viewport.Center.Y - 100);
            Vector2 pcPos = new Vector2(_viewport.Size.X/2, _viewport.Size.Y/2-offset) + _viewport.Position;
            platformCode.Position = pcPos;
            frame.Add(platformCode);

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
            float fontSpace = ((24-7) * size);
            float remainingSpace = emptySpace + (fontSpace / 2);
            Echo($"Empty Space: {emptySpace}, {fontSpace}");
            return remainingSpace;
        }
    }
}

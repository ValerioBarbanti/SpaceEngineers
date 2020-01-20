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
        public class SplashScreen {

            List<IMyTextPanel> Panels;
            ScreenManager ScreenManager;

            public SplashScreen(ScreenManager sManager, List<IMyTextPanel> panels) {
                ScreenManager = sManager;
                Panels = panels;
            }

            public void GenerateScreen() {

                foreach (IMyTextPanel panel in Panels) {
                    using (var frame = panel.DrawFrame()) {
                        List<MySprite> spriteList = new List<MySprite>();
                        DrawSprite(spriteList);
                        frame.AddRange(spriteList);

                    }
                }
            }

            private void DrawSprite(List<MySprite> spriteList) {
                Vector2 b_pos = new Vector2(256, 256);
                Vector2 b_size = new Vector2(512, 512);
                var background = MySprite.CreateSprite("SquareSimple", b_pos, b_size);
                background.Color = Constants.COLOR_BACKGROUND;

                Vector2 e1_pos = new Vector2(256, 256);
                Vector2 e1_size = new Vector2(26, 128);
                var elem1 = MySprite.CreateSprite("SquareSimple", e1_pos, e1_size);
                elem1.RotationOrScale = 0;
                elem1.Color = Constants.COLOR_WHITE;

                Vector2 e2_pos = new Vector2(207.3f, 271);
                Vector2 e2_size = new Vector2(119, 26);
                var elem2 = MySprite.CreateSprite("SquareSimple", e2_pos, e2_size);
                elem2.RotationOrScale = Utils.DegreeToRadian(30);
                elem2.Color = Constants.COLOR_WHITE;

                Vector2 e3_pos = new Vector2(217.3f, 179);
                Vector2 e3_size = new Vector2(119, 26);
                var elem3 = MySprite.CreateSprite("SquareSimple", e3_pos, e3_size);
                elem3.RotationOrScale = Utils.DegreeToRadian(30);
                elem3.Color = Constants.COLOR_WHITE;

                Vector2 e4_pos = new Vector2(340.7f, 196);
                Vector2 e4_size = new Vector2(25, 128);
                var elem4 = MySprite.CreateSprite("SquareSimple", e4_pos, e4_size);
                elem4.Color = Constants.COLOR_WHITE;

                Vector2 e5_pos = new Vector2(170f, 220);
                Vector2 e5_size = new Vector2(25, 60);
                var elem5 = MySprite.CreateSprite("SquareSimple", e5_pos, e5_size);
                elem5.Color = Constants.COLOR_WHITE;

                Vector2 e6_pos = new Vector2(284.9f, 118.3f);
                Vector2 e6_size = new Vector2(79, 26);
                var elem6 = MySprite.CreateSprite("SquareSimple", e6_pos, e6_size);
                elem6.RotationOrScale = Utils.DegreeToRadian(30);
                elem6.Color = Constants.COLOR_WHITE;

                Vector2 e7_pos = new Vector2(316.9f, 266.3f);
                Vector2 e7_size = new Vector2(69, 26);
                var elem7 = MySprite.CreateSprite("SquareSimple", e7_pos, e7_size);
                elem7.RotationOrScale = Utils.DegreeToRadian(-30);
                elem7.Color = Constants.COLOR_WHITE;

                Vector2 e8_pos = new Vector2(302.5f, 174.5f);
                Vector2 e8_size = new Vector2(99, 26);
                var elem8 = MySprite.CreateSprite("SquareSimple", e8_pos, e8_size);
                elem8.RotationOrScale = Utils.DegreeToRadian(-30);
                elem8.Color = Constants.COLOR_WHITE;

                Vector2 e9_pos = new Vector2(207.8f, 130.3f);
                Vector2 e9_size = new Vector2(127, 26);
                var elem9 = MySprite.CreateSprite("SquareSimple", e9_pos, e9_size);
                elem9.RotationOrScale = Utils.DegreeToRadian(-30);
                elem9.Color = Constants.COLOR_WHITE;

                Vector2 m1_pos = new Vector2(151f, 153f);
                Vector2 m1_size = new Vector2(15, 46);
                var mask1 = MySprite.CreateSprite("SquareSimple", m1_pos, m1_size);
                mask1.RotationOrScale = 0;
                mask1.Color = Constants.COLOR_BACKGROUND;

                Vector2 m2_pos = new Vector2(166.9f, 174f);
                Vector2 m2_size = new Vector2(45, 16);
                var mask2 = MySprite.CreateSprite("SquareSimple", m2_pos, m2_size);
                mask2.RotationOrScale = Utils.DegreeToRadian(30);
                mask2.Color = Constants.COLOR_BACKGROUND;

                Vector2 m3_pos = new Vector2(176.9f, 193f);
                Vector2 m3_size = new Vector2(45, 16);
                var mask3 = MySprite.CreateSprite("SquareSimple", m3_pos, m3_size);
                mask3.RotationOrScale = Utils.DegreeToRadian(30);
                mask3.Color = Constants.COLOR_BACKGROUND;

                Vector2 m4_pos = new Vector2(149f, 250.2f);
                Vector2 m4_size = new Vector2(15, 46);
                var mask4 = MySprite.CreateSprite("SquareSimple", m4_pos, m4_size);
                mask4.RotationOrScale = 0;
                mask4.Color = Constants.COLOR_BACKGROUND;

                Vector2 m5_pos = new Vector2(305.7f, 136.6f);
                Vector2 m5_size = new Vector2(45, 16);
                var mask5 = MySprite.CreateSprite("SquareSimple", m5_pos, m5_size);
                mask5.RotationOrScale = Utils.DegreeToRadian(-30);
                mask5.Color = Constants.COLOR_BACKGROUND;

                Vector2 m6_pos = new Vector2(318.7f, 140.6f);
                Vector2 m6_size = new Vector2(45, 16);
                var mask6 = MySprite.CreateSprite("SquareSimple", m6_pos, m6_size);
                mask6.RotationOrScale = Utils.DegreeToRadian(-30);
                mask6.Color = Constants.COLOR_BACKGROUND;

                Vector2 m7_pos = new Vector2(348.1f, 132f);
                Vector2 m7_size = new Vector2(45, 16);
                var mask7 = MySprite.CreateSprite("SquareSimple", m7_pos, m7_size);
                mask7.RotationOrScale = Utils.DegreeToRadian(30);
                mask7.Color = Constants.COLOR_BACKGROUND;

                Vector2 m8_pos = new Vector2(244.9f, 316.9f);
                Vector2 m8_size = new Vector2(45, 16);
                var mask8 = MySprite.CreateSprite("SquareSimple", m8_pos, m8_size);
                mask8.RotationOrScale = Utils.DegreeToRadian(30);
                mask8.Color = Constants.COLOR_BACKGROUND;

                Vector2 m9_pos = new Vector2(262.9f, 320.6f);
                Vector2 m9_size = new Vector2(45, 16);
                var mask9 = MySprite.CreateSprite("SquareSimple", m9_pos, m9_size);
                mask9.RotationOrScale = Utils.DegreeToRadian(-30);
                mask9.Color = Constants.COLOR_BACKGROUND;

                Vector2 m10_pos = new Vector2(286.7f, 279.2f);
                Vector2 m10_size = new Vector2(15, 46);
                var mask10 = MySprite.CreateSprite("SquareSimple", m10_pos, m10_size);
                mask10.RotationOrScale = 0;
                mask10.Color = Constants.COLOR_BACKGROUND;

                var logoUpperText = MySprite.CreateText("CUB3", "White", Constants.COLOR_GREEN, 3.5f, TextAlignment.CENTER);
                logoUpperText.Position = new Vector2(256, 300);

                var logoBottomText = MySprite.CreateText("SOFTWARE", "White", Constants.COLOR_WHITE, 1.6f, TextAlignment.CENTER);
                logoBottomText.Position = new Vector2(256, 380);

                



                spriteList.Add(background);

                spriteList.Add(elem1);
                spriteList.Add(elem2);
                spriteList.Add(elem3);
                spriteList.Add(elem4);
                spriteList.Add(elem5);
                spriteList.Add(elem6);
                spriteList.Add(elem7);
                spriteList.Add(elem8);
                spriteList.Add(elem9);

                spriteList.Add(mask1);
                spriteList.Add(mask2);
                spriteList.Add(mask3);
                spriteList.Add(mask4);
                spriteList.Add(mask5);
                spriteList.Add(mask6);
                spriteList.Add(mask7);
                spriteList.Add(mask8);
                spriteList.Add(mask9);
                spriteList.Add(mask10);

                spriteList.Add(logoUpperText);
                spriteList.Add(logoBottomText);

            }

        }
    }
}

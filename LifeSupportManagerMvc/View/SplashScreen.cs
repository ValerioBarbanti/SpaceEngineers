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

                        Vector2 b_pos = new Vector2(256, 256);
                        Vector2 b_size = new Vector2(512, 512);
                        var background = MySprite.CreateSprite("SquareSimple", b_pos, b_size);
                        background.Color = Constants.COLOR_BACKGROUND;

                        Vector2 lm_pos = new Vector2(256, 256);
                        Vector2 lm_size = new Vector2(200, 200);
                        var logoMane = MySprite.CreateSprite("SquareSimple", lm_pos, lm_size);
                        logoMane.Color = Constants.COLOR_LOGO_SECONDARY;

                        Vector2 lf_pos = new Vector2(256, 256);
                        Vector2 lf_size = new Vector2(140, 140);
                        var logoFace = MySprite.CreateSprite("SquareSimple", lf_pos, lf_size);
                        logoFace.Color = Constants.COLOR_LOGO_PRIMARY;
                        logoFace.RotationOrScale = 0.785398F;

                        Vector2 ln_pos = new Vector2(256, 256);
                        Vector2 ln_size = new Vector2(26, 26);
                        var logoNose = MySprite.CreateSprite("SquareSimple", ln_pos, ln_size);
                        logoNose.Color = Constants.COLOR_BACKGROUND;
                        logoNose.RotationOrScale = 0.785398F;

                        Vector2 lnm_pos = new Vector2(256, 243);
                        Vector2 lnm_size = new Vector2(26, 26);
                        var logoNoseMask = MySprite.CreateSprite("SquareSimple", lnm_pos, lnm_size);
                        logoNoseMask.Color = Constants.COLOR_LOGO_PRIMARY;

                        Vector2 lml_pos = new Vector2(256, 273);
                        Vector2 lml_size = new Vector2(4, 30);
                        var logoMouthLine = MySprite.CreateSprite("SquareSimple", lml_pos, lml_size);
                        logoMouthLine.Color = Constants.COLOR_BACKGROUND;

                        Vector2 lmt_pos = new Vector2(256, 295);
                        Vector2 lmt_size = new Vector2(26, 26);
                        var logoMouth = MySprite.CreateSprite("SquareSimple", lmt_pos, lmt_size);
                        logoMouth.Color = Constants.COLOR_BACKGROUND;
                        logoMouth.RotationOrScale = 0.785398F;

                        Vector2 lmm_pos = new Vector2(256, 301);
                        Vector2 lmm_size = new Vector2(26, 26);
                        var logoMouthMask = MySprite.CreateSprite("SquareSimple", lmm_pos, lmm_size);
                        logoMouthMask.Color = Constants.COLOR_LOGO_PRIMARY;
                        logoMouthMask.RotationOrScale = 0.785398F;

                        Vector2 lle_pos = new Vector2(220, 230);
                        Vector2 lle_size = new Vector2(30, 30);
                        var logoLeftEye = MySprite.CreateSprite("SquareSimple", lle_pos, lle_size);
                        logoLeftEye.Color = Constants.COLOR_BACKGROUND;

                        Vector2 lre_pos = new Vector2(292, 230);
                        Vector2 lre_size = new Vector2(30, 30);
                        var logoRightEye = MySprite.CreateSprite("SquareSimple", lre_pos, lre_size);
                        logoRightEye.Color = Constants.COLOR_BACKGROUND;

                        Vector2 llem_pos = new Vector2(205, 245);
                        Vector2 llem_size = new Vector2(40, 40);
                        var logoLeftEyeMask = MySprite.CreateSprite("SquareSimple", llem_pos, llem_size);
                        logoLeftEyeMask.Color = Constants.COLOR_LOGO_PRIMARY;
                        logoLeftEyeMask.RotationOrScale = 0.785398F;

                        Vector2 lrem_pos = new Vector2(307, 245);
                        Vector2 lrem_size = new Vector2(40, 40);
                        var logoRightEyeMask = MySprite.CreateSprite("SquareSimple", lrem_pos, lrem_size);
                        logoRightEyeMask.Color = Constants.COLOR_LOGO_PRIMARY;
                        logoRightEyeMask.RotationOrScale = 0.785398F;

                        var logoUpperText = MySprite.CreateText("RAION", "Debug", Constants.COLOR_LOGO_PRIMARY, 1.5f, TextAlignment.CENTER);
                        logoUpperText.Position = new Vector2(256, 114);

                        var logoBottomText = MySprite.CreateText("TECHNOLOGIES", "Debug", Constants.COLOR_LOGO_PRIMARY, 1f, TextAlignment.CENTER);
                        logoBottomText.Position = new Vector2(256, 356);

                        spriteList.Add(background);
                        spriteList.Add(logoMane);
                        spriteList.Add(logoFace);
                        spriteList.Add(logoNose);
                        spriteList.Add(logoNoseMask);
                        spriteList.Add(logoMouthLine);
                        spriteList.Add(logoMouth);
                        spriteList.Add(logoMouthMask);
                        spriteList.Add(logoLeftEye);
                        spriteList.Add(logoRightEye);
                        spriteList.Add(logoLeftEyeMask);
                        spriteList.Add(logoRightEyeMask);
                        spriteList.Add(logoUpperText);
                        spriteList.Add(logoBottomText);

                        frame.AddRange(spriteList);

                    }
                }
            }

        }
    }
}

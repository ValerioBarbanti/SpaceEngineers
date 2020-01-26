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
        public class AirlockScreen {

            Program myProgram;

            
            ScreenManager ScreenManager;

            public AirlockScreen(ScreenManager sManager, Program program) {
                ScreenManager = sManager;
                
                myProgram = program;
            }

            public void GenerateScreen(Dictionary<string, Airlock> airlocks) {
                
                foreach (KeyValuePair<string, Airlock> _al in airlocks) {
                    Airlock airlock = _al.Value;

                    foreach (IMyTextPanel panel in airlock.Panels) {
                        using (var frame = panel.DrawFrame()) {

                            List<MySprite> backgroundSpriteList = new List<MySprite>();
                            List<MySprite> pressureInfoSpriteList = new List<MySprite>();
                            List<MySprite> oxygenInfoSpriteList = new List<MySprite>();
                            List<MySprite> pressureButtonSpriteList = new List<MySprite>();
                            List<MySprite> doorButtonSpriteList = new List<MySprite>();
                            List<MySprite> footerSpriteList = new List<MySprite>();


                            DrawBackground(airlock, backgroundSpriteList);

                            DrawPressureInfo(airlock, pressureInfoSpriteList);

                            DrawPressureButton(airlock, pressureButtonSpriteList);

                            DrawOxygenInfo(airlock, oxygenInfoSpriteList);

                            DrawDoorButtons(airlock, doorButtonSpriteList);

                            DrawFooter(airlock, footerSpriteList);

                            // SPRITES TO FRAME

                            frame.AddRange(backgroundSpriteList);
                            frame.AddRange(pressureInfoSpriteList);
                            frame.AddRange(pressureButtonSpriteList);
                            frame.AddRange(oxygenInfoSpriteList);
                            frame.AddRange(doorButtonSpriteList);
                            frame.AddRange(footerSpriteList);
                        }
                    }
                }
            }

            private void DrawBackground(Airlock airlock, List<MySprite> backgroundSpriteList) {
                Vector2 b_pos = new Vector2(256, 256);
                Vector2 b_size = new Vector2(512, 512);
                var background = MySprite.CreateSprite("SquareSimple", b_pos, b_size);
                background.Color = Constants.COLOR_BACKGROUND_MASK;

                Vector2 bc_pos = new Vector2(256, 150);
                Vector2 bc_size = new Vector2(280, 280);
                var backgroundCircle = MySprite.CreateSprite("Circle", bc_pos, bc_size);
                backgroundCircle.Color = Constants.COLOR_BACKGROUND;

                Vector2 bcr_pos = new Vector2(256, 150);
                Vector2 bcr_size = new Vector2(512, 170);
                var backgroundCircleRect = MySprite.CreateSprite("SquareSimple", bcr_pos, bcr_size);
                backgroundCircleRect.Color = Constants.COLOR_BACKGROUND;

                backgroundSpriteList.Add(background);
                backgroundSpriteList.Add(backgroundCircle);
                backgroundSpriteList.Add(backgroundCircleRect);

            }

            private void DrawPressureInfo(Airlock airlock, List<MySprite> pressureInfoSpriteList) {
                Color contrastColor = new Color();
                if (airlock.RoomPressure.Equals("High Pressure")) {
                    contrastColor = Constants.COLOR_GREEN;
                } else if (airlock.RoomPressure.Equals("Low Pressure")) {
                    contrastColor = Constants.COLOR_YELLOW;
                } else {
                    contrastColor = Constants.COLOR_RED;
                }

                Vector2 oc_pos = new Vector2(256, 150);
                Vector2 oc_size = new Vector2(240, 240);
                var outerCircle = MySprite.CreateSprite("Circle", oc_pos, oc_size);
                outerCircle.Color = contrastColor;

                Vector2 ocm_pos = new Vector2(256, 150);
                Vector2 ocm_size = new Vector2(230, 230);
                var outerCircleMask = MySprite.CreateSprite("Circle", ocm_pos, ocm_size);
                outerCircleMask.Color = Constants.COLOR_BACKGROUND_MASK;

                var pressureText = MySprite.CreateText(airlock.OxygenLevel, "Debug", Constants.COLOR_WHITE, 4f, TextAlignment.CENTER);
                pressureText.Position = new Vector2(256, 90);

                var barText = MySprite.CreateText("BAR", "Debug", contrastColor, 1f, TextAlignment.CENTER);
                barText.Position = new Vector2(256, 200);

                pressureInfoSpriteList.Add(outerCircle);
                pressureInfoSpriteList.Add(outerCircleMask);
                pressureInfoSpriteList.Add(pressureText);
                pressureInfoSpriteList.Add(barText);
            }

            private void DrawPressureButton(Airlock airlock, List<MySprite> pressureButtonSpriteList) {
                Vector2 buttonSize = new Vector2(130, 50); //136 48
                Vector2 frameSize = new Vector2(130, 50); //136 48
                Vector2 frameMaskSize = new Vector2(126, 46); //132 44
                Vector2 frameMaskSizeH = new Vector2(130, 30); // 136 28
                Vector2 frameMaskSizeV = new Vector2(110, 50); // 116 48

                // 130*50

                Vector2 bbl_pos = new Vector2(75, 330);
                var buttonBgLeft = MySprite.CreateSprite("SquareSimple", bbl_pos, buttonSize);
                buttonBgLeft.Color = Constants.COLOR_NAVY_BLUE;

                Vector2 bbc_pos = new Vector2(256, 330);
                var buttonBgCenter = MySprite.CreateSprite("SquareSimple", bbc_pos, buttonSize);
                buttonBgCenter.Color = Constants.COLOR_NAVY_BLUE;

                Vector2 bbr_pos = new Vector2(437, 330);
                var buttonBgRight = MySprite.CreateSprite("SquareSimple", bbr_pos, buttonSize);
                buttonBgRight.Color = Constants.COLOR_NAVY_BLUE;

                var frameButton = MySprite.CreateSprite("SquareSimple", bbc_pos, frameSize);
                var frameButtonMask = MySprite.CreateSprite("SquareSimple", bbc_pos, frameMaskSize);
                frameButtonMask.Color = Constants.COLOR_NAVY_BLUE;
                var frameButtonMaskH = MySprite.CreateSprite("SquareSimple", bbc_pos, frameMaskSizeH);
                frameButtonMaskH.Color = Constants.COLOR_NAVY_BLUE;
                var frameButtonMaskV = MySprite.CreateSprite("SquareSimple", bbc_pos, frameMaskSizeV);
                frameButtonMaskV.Color = Constants.COLOR_NAVY_BLUE;
                if (airlock.PublicStatus.Equals(Constants.AP_PRESSURIZED)) {
                    frameButton.Color = Constants.COLOR_GREEN;
                    frameButtonMask.Color = Constants.COLOR_GREEN_DARK;
                    frameButtonMaskH.Color = Constants.COLOR_GREEN_DARK;
                    frameButtonMaskV.Color = Constants.COLOR_GREEN_DARK;
                    frameButton.Position = bbl_pos;
                    frameButtonMask.Position = bbl_pos;
                    frameButtonMaskH.Position = bbl_pos;
                    frameButtonMaskV.Position = bbl_pos;
                } else if (airlock.PublicStatus.Equals(Constants.AP_CYCLE)) {
                    frameButton.Color = Constants.COLOR_YELLOW;
                    frameButtonMask.Color = Constants.COLOR_YELLOW_DARK;
                    frameButtonMaskH.Color = Constants.COLOR_YELLOW_DARK;
                    frameButtonMaskV.Color = Constants.COLOR_YELLOW_DARK;
                    frameButton.Position = bbc_pos;
                    frameButtonMask.Position = bbc_pos;
                    frameButtonMaskH.Position = bbc_pos;
                    frameButtonMaskV.Position = bbc_pos;
                } else if (airlock.PublicStatus.Equals(Constants.AP_DEPRESSURIZED)) {
                    frameButton.Color = Constants.COLOR_RED;
                    frameButtonMask.Color = Constants.COLOR_RED_DARK;
                    frameButtonMaskH.Color = Constants.COLOR_RED_DARK;
                    frameButtonMaskV.Color = Constants.COLOR_RED_DARK;
                    frameButton.Position = bbr_pos;
                    frameButtonMask.Position = bbr_pos;
                    frameButtonMaskH.Position = bbr_pos;
                    frameButtonMaskV.Position = bbr_pos;
                }

                var bblText = MySprite.CreateText("PRESSURIZED", "Debug", Constants.COLOR_WHITE, 0.6f, TextAlignment.CENTER);
                bblText.Position = new Vector2(75, 320);

                var bbcText = MySprite.CreateText("CYCLING", "Debug", Constants.COLOR_WHITE, 0.6f, TextAlignment.CENTER);
                bbcText.Position = new Vector2(256, 320);

                var bbrText = MySprite.CreateText("DEPRESSURIZED", "Debug", Constants.COLOR_WHITE, 0.6f, TextAlignment.CENTER);
                bbrText.Position = new Vector2(437, 320);

                pressureButtonSpriteList.Add(buttonBgCenter);
                pressureButtonSpriteList.Add(buttonBgLeft);
                pressureButtonSpriteList.Add(buttonBgRight);
                pressureButtonSpriteList.Add(frameButton);
                pressureButtonSpriteList.Add(frameButtonMask);
                pressureButtonSpriteList.Add(frameButtonMaskH);
                pressureButtonSpriteList.Add(frameButtonMaskV);
                pressureButtonSpriteList.Add(bbcText);
                pressureButtonSpriteList.Add(bblText);
                pressureButtonSpriteList.Add(bbrText);
            }

            private void DrawOxygenInfo(Airlock airlock, List<MySprite> oxygenInfoSpriteList) {
                Vector2 orl_pos = new Vector2(112.5f, 390);
                Vector2 orl_size = new Vector2(205, 30);
                var outerRectLeft = MySprite.CreateSprite("SquareSimple", orl_pos, orl_size);
                outerRectLeft.Color = Constants.COLOR_WHITE;

                Vector2 orlm_pos = new Vector2(112.5f, 390);
                Vector2 orlm_size = new Vector2(201, 26);
                var outerRectLeftMask = MySprite.CreateSprite("SquareSimple", orlm_pos, orlm_size);
                outerRectLeftMask.Color = Constants.COLOR_BACKGROUND;

                Vector2 bfl_pos = new Vector2(112.5f, 390);
                Vector2 bfl_size = new Vector2((197 * float.Parse(airlock.OxygenLevelDecimals)), 22);
                var barFillLeft = MySprite.CreateSprite("SquareSimple", bfl_pos, bfl_size);
                barFillLeft.Color = Constants.COLOR_WHITE;

                var oxigenText = MySprite.CreateText("AIRLOCK OXYGEN", "Debug", Constants.COLOR_WHITE, 0.5f, TextAlignment.CENTER);
                oxigenText.Position = new Vector2(112.5f, 410);

                Vector2 orr_pos = new Vector2(399.5f, 390);
                Vector2 orr_size = new Vector2(205, 30);
                var outerRectRight = MySprite.CreateSprite("SquareSimple", orr_pos, orr_size);
                outerRectRight.Color = Constants.COLOR_WHITE;

                Vector2 orrm_pos = new Vector2(399.5f, 390);
                Vector2 orrm_size = new Vector2(201, 26);
                var outerRectRightMask = MySprite.CreateSprite("SquareSimple", orrm_pos, orrm_size);
                outerRectRightMask.Color = Constants.COLOR_BACKGROUND;

                Vector2 bfr_pos = new Vector2(399.5f, 390);
                Vector2 bfr_size = new Vector2((197 * float.Parse(airlock.OxygenTankFillDecimals)), 22);
                var barFillRight = MySprite.CreateSprite("SquareSimple", bfr_pos, bfr_size);
                barFillRight.Color = Constants.COLOR_WHITE;

                var tankText = MySprite.CreateText("02 TANKS STORAGE", "Debug", Constants.COLOR_WHITE, 0.5f, TextAlignment.CENTER);
                tankText.Position = new Vector2(399.5f, 410);

                oxygenInfoSpriteList.Add(outerRectLeft);
                oxygenInfoSpriteList.Add(outerRectLeftMask);
                oxygenInfoSpriteList.Add(barFillLeft);
                oxygenInfoSpriteList.Add(oxigenText);

                oxygenInfoSpriteList.Add(outerRectRight);
                oxygenInfoSpriteList.Add(outerRectRightMask);
                oxygenInfoSpriteList.Add(barFillRight);
                oxygenInfoSpriteList.Add(tankText);
            }

            private void DrawDoorButtons(Airlock airlock, List<MySprite> doorButtonSpriteList) {

                Color internalColor = new Color(255, 255, 255);
                Color externalColor = new Color(255, 255, 255);

                if (airlock.OpenDoors.Equals("Both")) {
                    internalColor = Constants.COLOR_GREEN;
                    externalColor = Constants.COLOR_GREEN;
                } else if (airlock.OpenDoors.Equals("Internal")) {
                    internalColor = Constants.COLOR_GREEN;
                    externalColor = Constants.COLOR_RED;
                } else if (airlock.OpenDoors.Equals("External")) {
                    internalColor = Constants.COLOR_RED;
                    externalColor = Constants.COLOR_GREEN;
                } else {
                    internalColor = Constants.COLOR_RED;
                    externalColor = Constants.COLOR_RED;
                }

                Vector2 ldr_pos = new Vector2(70, 150);
                Vector2 ldr_size = new Vector2(60, 60);
                var leftDoorRect = MySprite.CreateSprite("SquareSimple", ldr_pos, ldr_size);
                leftDoorRect.Color = Constants.COLOR_WHITE;

                Vector2 ldrm_pos = new Vector2(70, 150);
                Vector2 ldrm_size = new Vector2(56, 56);
                var leftDoorRectMask = MySprite.CreateSprite("SquareSimple", ldrm_pos, ldrm_size);
                leftDoorRectMask.Color = Constants.COLOR_BACKGROUND;

                Vector2 lds_pos = new Vector2(70, 187);
                Vector2 lds_size = new Vector2(60, 10);
                var leftDoorSignal = MySprite.CreateSprite("SquareSimple", lds_pos, lds_size);
                leftDoorSignal.Color = internalColor;

                var innerDoorText = MySprite.CreateText("INNER\nDOOR", "Debug", internalColor, 0.6f, TextAlignment.CENTER);
                innerDoorText.Position = new Vector2(70, 133);

                Vector2 rdr_pos = new Vector2(437, 150);
                Vector2 rdr_size = new Vector2(60, 60);
                var rightDoorRect = MySprite.CreateSprite("SquareSimple", rdr_pos, rdr_size);
                rightDoorRect.Color = Constants.COLOR_WHITE;

                Vector2 rdrm_pos = new Vector2(437, 150);
                Vector2 rdrm_size = new Vector2(56, 56);
                var rightDoorRectMask = MySprite.CreateSprite("SquareSimple", rdrm_pos, rdrm_size);
                rightDoorRectMask.Color = Constants.COLOR_BACKGROUND;

                Vector2 rds_pos = new Vector2(437, 187);
                Vector2 rds_size = new Vector2(60, 10);
                var rightDoorSignal = MySprite.CreateSprite("SquareSimple", rds_pos, rds_size);
                rightDoorSignal.Color = externalColor;

                var outerDoorText = MySprite.CreateText("OUTER\nDOOR", "Debug", externalColor, 0.6f, TextAlignment.CENTER);
                outerDoorText.Position = new Vector2(437, 133);

                doorButtonSpriteList.Add(leftDoorRect);
                doorButtonSpriteList.Add(leftDoorRectMask);
                doorButtonSpriteList.Add(leftDoorSignal);
                doorButtonSpriteList.Add(innerDoorText);
                doorButtonSpriteList.Add(rightDoorRect);
                doorButtonSpriteList.Add(rightDoorRectMask);
                doorButtonSpriteList.Add(rightDoorSignal);
                doorButtonSpriteList.Add(outerDoorText);

            }

            private void DrawFooter(Airlock airlock, List<MySprite> footerSpriteList) {

                Vector2 fb_pos = new Vector2(256, 457.5f);
                Vector2 fb_size = new Vector2(492, 5);
                var footerBar = MySprite.CreateSprite("SquareSimple", fb_pos, fb_size);
                footerBar.Color = Constants.COLOR_ORANGE;

                var airlockNameText = MySprite.CreateText(airlock.Name, "Debug", Constants.COLOR_ORANGE, 1.5f, TextAlignment.CENTER);
                airlockNameText.Position = new Vector2(256, 462f);

                footerSpriteList.Add(footerBar);
                footerSpriteList.Add(airlockNameText);

            }




        }
    }
}

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
        public class ScreenManager {

            Program myProgram;

            public string Status { get; set; }

            private List<IMyTextPanel> GlobalPanels { get; set; }
            private List<IMyTextPanel> Panels { get; set; }

            private int tick = 0;

            private float rotationRadiants = 0;

            public ScreenManager(Program program) {
                myProgram = program;
                Init();
            }

            public void Init() {
                Status = Constants.S_STATUS_INIT;
                GlobalPanels = new List<IMyTextPanel>();
                myProgram.GridTerminalSystem.GetBlocksOfType(GlobalPanels);
                Panels = new List<IMyTextPanel>();
                foreach (IMyTextPanel panel in GlobalPanels) {
                    if (panel.CustomData.Contains(Constants.T_LSM_AIRVENT_SCREEN)) {
                        Panels.Add(panel);
                        panel.ContentType = ContentType.SCRIPT;
                    }
                }
                foreach (KeyValuePair<string, Airlock> _al in myProgram.airlockController.Airlocks) {
                    Airlock airlock = _al.Value;
                    foreach (IMyTextPanel panel in airlock.Panels) {
                        panel.ContentType = ContentType.SCRIPT;
                        panel.CustomData = Constants.T_LSM_AIRLOCK_SCREEN;
                        Panels.Add(panel);
                    }
                }
            }

            public void ScreenRuntime() {
                tick++;
                switch (Status) {
                    case Constants.S_STATUS_INIT:
                        ShowSplashScreen();
                        break;
                    case Constants.S_STATUS_RUNNING:
                        ShowScreenInfos();
                        break;
                }
            }

            private void ShowSplashScreen() {

                if (tick > 10) {
                    Status = Constants.S_STATUS_RUNNING;
                }

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

            private void ShowScreenInfos() {
                foreach (IMyTextPanel panel in Panels) {
                    if (panel.CustomData.Contains(Constants.T_LSM_AIRLOCK_SCREEN)) {
                        UpdateAirlockScreen(panel);
                    }

                    if (panel.CustomData.Contains(Constants.T_LSM_AIRVENT_SCREEN)) {
                        int page = 1;
                        if (panel.CustomData.Contains("page")) {
                            int sIndex = panel.CustomData.IndexOf("=");
                            string pString = panel.CustomData.Substring(sIndex+1);
                            page = Int32.Parse(pString);
                        }
                        UpdateLeakScreen(panel, page);
                    }
                }
            }

            private void UpdateAirlockScreen(IMyTextPanel panel) {
                foreach (KeyValuePair<string, Airlock> _al in myProgram.airlockController.Airlocks) {
                    Airlock airlock = _al.Value;
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

            private static void DrawBackground(Airlock airlock, List<MySprite> backgroundSpriteList) {
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

            private static void DrawPressureInfo(Airlock airlock, List<MySprite> pressureInfoSpriteList) {
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

            private void UpdateLeakScreen(IMyTextPanel panel, int page) {
                using (var frame = panel.DrawFrame()) {
                    List<MySprite> backgroundSpriteList = new List<MySprite>();
                    List<List<MySprite>> airventRowList = new List<List<MySprite>>();
                    List<MySprite> footerSpriteList = new List<MySprite>();

                    DrawBackground(backgroundSpriteList);

                    DrawAirvents(myProgram.leakController.Airvents, airventRowList, page);

                    DrawFooter(myProgram.leakController.Airvents.Count, footerSpriteList, page);

                    frame.AddRange(backgroundSpriteList);
                    foreach(List<MySprite> airventRowSprites in airventRowList) {
                        frame.AddRange(airventRowSprites);
                    }
                    frame.AddRange(footerSpriteList);
                    
                }
            }

            

            private void DrawBackground(List<MySprite> backgroundSpriteList) {
                Vector2 b_pos = new Vector2(256, 256);
                Vector2 b_size = new Vector2(512, 512);
                var background = MySprite.CreateSprite("SquareSimple", b_pos, b_size);
                background.Color = Constants.COLOR_BACKGROUND;

                backgroundSpriteList.Add(background);
            }

            private void DrawAirvents(List<IMyAirVent> airvents, List<List<MySprite>> airventRowList, int page) {
                float leftColumnX = 130.5f;
                float rightColumnX = 380.5f;
                float startColumnY = 43;
                float columnYincrement = 76;

                
                int maxPageIndex = page * 10;
                int minPageIndex = maxPageIndex - 10;
                int pageIndex = 0;



                int index = 0;
                int halfIndex = 0;

                foreach (IMyAirVent airvent in airvents) {
                    if (pageIndex >= minPageIndex && pageIndex < maxPageIndex) {
                        Color stateColor = new Color(256, 256, 256);
                        bool isWorking = true;
                        if (airvent.CanPressurize && airvent.IsWorking && airvent.IsFunctional) {
                            stateColor = Constants.COLOR_WHITE;
                            isWorking = true;
                        } else {
                            stateColor = Constants.COLOR_RED;
                            isWorking = false;
                        }

                        List<MySprite> airventRow = new List<MySprite>();

                        float x;
                        float y;
                    
                        if (index%2 == 0) {
                            x = leftColumnX;
                            y = startColumnY + (columnYincrement * halfIndex);
                        } else {
                            x = rightColumnX;
                            y = startColumnY + (columnYincrement * halfIndex);
                        }

                        MySprite sectionBackground;
                        DrawSectionBackground(x, y, out sectionBackground);

                        MySprite iconBorder, iconBackground, fanH, fanV, fanCenterBackground, fanCenter;
                        DrawAirventIcon(x-101.5f, y-14, stateColor, isWorking, out iconBorder, out iconBackground, out fanH, out fanV, out fanCenterBackground, out fanCenter);

                        MySprite airventText, divider, statusTitle, statusInfo, oxygenTitle, oxygenInfo, pressurizeTitle, pressurizeInfo;
                        DrawAirventInfos(x, y, stateColor, isWorking, airvent, out airventText, out divider, out statusTitle, out statusInfo, out oxygenTitle, out oxygenInfo, out pressurizeTitle, out pressurizeInfo);

                        airventRow.Add(sectionBackground);

                        airventRow.Add(iconBorder);
                        airventRow.Add(iconBackground);
                        airventRow.Add(fanH);
                        airventRow.Add(fanV);
                        airventRow.Add(fanCenterBackground);
                        airventRow.Add(fanCenter);

                        airventRow.Add(airventText);
                        airventRow.Add(divider);
                        airventRow.Add(statusTitle);
                        airventRow.Add(statusInfo);
                        airventRow.Add(oxygenTitle);
                        airventRow.Add(oxygenInfo);
                        airventRow.Add(pressurizeTitle);
                        airventRow.Add(pressurizeInfo);

                        airventRowList.Add(airventRow);

                        if (index%2 == 1) {
                            halfIndex++;
                        }
                        index++;
                        pageIndex++;
                    } else {
                        pageIndex++;
                        index++;
                    }
                }


            }

            private void DrawSectionBackground(float posX, float posY, out MySprite sectionBackground) {
                Vector2 ib_pos = new Vector2(posX, posY);
                Vector2 ib_size = new Vector2(241, 66);
                sectionBackground = MySprite.CreateSprite("SquareSimple", ib_pos, ib_size);
                sectionBackground.Color = Constants.COLOR_BACKGROUND_MASK;
            }

            private void DrawAirventIcon(float posX, float posY, Color stateColor, bool isWorking, out MySprite iconBorder, out MySprite iconBackground, out MySprite fanH, out MySprite fanV, out MySprite fanCenterBackground, out MySprite fanCenter) {
                Vector2 ib_pos = new Vector2(posX, posY);
                Vector2 ib_size = new Vector2(30, 30);
                iconBorder = MySprite.CreateSprite("SquareSimple", ib_pos, ib_size);
                iconBorder.Color = stateColor;

                Vector2 ibg_pos = new Vector2(posX, posY);
                Vector2 ibg_size = new Vector2(26, 26);
                iconBackground = MySprite.CreateSprite("Circle", ibg_pos, ibg_size);
                iconBackground.Color = Constants.COLOR_BACKGROUND;

                Vector2 fh_pos = new Vector2(posX, posY);
                Vector2 fh_size = new Vector2(20, 4);
                fanH = MySprite.CreateSprite("SquareSimple", fh_pos, fh_size);
                fanH.Color = stateColor;
                if (isWorking)
                    fanH.RotationOrScale = rotationRadiants;

                Vector2 fv_pos = new Vector2(posX, posY);
                Vector2 fv_size = new Vector2(4, 20);
                fanV = MySprite.CreateSprite("SquareSimple", fv_pos, fv_size);
                fanV.Color = stateColor;
                if (isWorking)
                    fanV.RotationOrScale = rotationRadiants;

                rotationRadiants += 0.05f;

                Vector2 fcb_pos = new Vector2(posX, posY);
                Vector2 fcb_size = new Vector2(10, 10);
                fanCenterBackground = MySprite.CreateSprite("Circle", fcb_pos, fcb_size);
                fanCenterBackground.Color = Constants.COLOR_BACKGROUND;

                Vector2 fc_pos = new Vector2(posX, posY);
                Vector2 fc_size = new Vector2(6, 6);
                fanCenter = MySprite.CreateSprite("Circle", fc_pos, fc_size);
                fanCenter.Color = stateColor;
            }

            private void DrawAirventInfos(float posX, float posY, Color stateColor, bool isWorking, IMyAirVent airvent, out MySprite airventText, out MySprite divider, out MySprite statusTitle, out MySprite statusInfo, out MySprite oxygenTitle, out MySprite oxygenInfo, out MySprite pressurizeTitle, out MySprite pressurizeInfo) {
                string name = airvent.CustomName;
                if (name.Length > 17) {
                    name = name.Substring(0, 14);
                    name += "...";
                }

                airventText = MySprite.CreateText(name, "Debug", Constants.COLOR_WHITE, 0.8f, TextAlignment.LEFT);
                airventText.Position = new Vector2(posX-83, posY-25);

                Vector2 d_pos = new Vector2(posX+17, posY);
                Vector2 d_size = new Vector2(199, 2);
                divider = MySprite.CreateSprite("SquareSimple", d_pos, d_size);
                divider.Color = Constants.COLOR_GREEN;

                statusTitle = MySprite.CreateText("Status", "Debug", Constants.COLOR_WHITE, 0.5f, TextAlignment.LEFT);
                statusTitle.Position = new Vector2(posX - 116, posY + 2);

                string airventStatus = "";
                Color textColor = Constants.COLOR_WHITE;
                if (!airvent.IsFunctional) {
                    airventStatus = "BROKEN";
                    textColor = Constants.COLOR_RED;
                } else if (!airvent.IsWorking) {
                    airventStatus = "NOT WORKING";
                    textColor = Constants.COLOR_RED;
                } else if (!airvent.CanPressurize) {
                    airventStatus = "LEAK";
                    textColor = Constants.COLOR_RED;
                } else {
                    airventStatus = "OPTIMAL";
                }

                statusInfo = MySprite.CreateText(airventStatus, "Debug", textColor, 0.5f, TextAlignment.LEFT);
                statusInfo.Position = new Vector2(posX - 116, posY + 16);

                oxygenTitle = MySprite.CreateText("Oxygen", "Debug", Constants.COLOR_WHITE, 0.5f, TextAlignment.CENTER);
                oxygenTitle.Position = new Vector2(posX, posY + 2);

                oxygenInfo = MySprite.CreateText((airvent.GetOxygenLevel()*100).ToString("0.0"), "Debug", Constants.COLOR_WHITE, 0.5f, TextAlignment.CENTER);
                oxygenInfo.Position = new Vector2(posX, posY + 16);

                pressurizeTitle = MySprite.CreateText("Action", "Debug", Constants.COLOR_WHITE, 0.5f, TextAlignment.RIGHT);
                pressurizeTitle.Position = new Vector2(posX + 116, posY + 2);

                string action = "";
                if (airvent.Depressurize) {
                    action = "Depressurizing";
                } else {
                    action = "Pressurizing";
                }
                pressurizeInfo = MySprite.CreateText(action, "Debug", Constants.COLOR_WHITE, 0.5f, TextAlignment.RIGHT);
                pressurizeInfo.Position = new Vector2(posX + 116, posY + 16);

            }

            private void DrawFooter(int count, List<MySprite> footerSpriteList, int page) {
                Vector2 pf_pos = new Vector2(256, 405);
                Vector2 pf_size = new Vector2(492, 30);
                MySprite pageFrame = MySprite.CreateSprite("SquareSimple", pf_pos, pf_size);
                pageFrame.Color = Constants.COLOR_BACKGROUND_MASK;

                int numPages = 1;
                if (count % 10 == 0) {
                    numPages = count / 10;
                } else {
                    numPages = count / 10 + 1;
                }
                MySprite pageNumber = MySprite.CreateText($"Page {page} of {numPages}", "Debug", Constants.COLOR_WHITE, 0.8f, TextAlignment.CENTER);
                pageNumber.Position = new Vector2(256, 392);

                Vector2 ff_pos = new Vector2(256, 466);
                Vector2 ff_size = new Vector2(492, 72);
                MySprite footerFrame = MySprite.CreateSprite("SquareSimple", ff_pos, ff_size);
                if (myProgram.leakController.Status.Equals(Constants.P_ON)) {
                    footerFrame.Color = Constants.COLOR_GREEN;
                } else {
                    footerFrame.Color = Constants.COLOR_RED;
                }

                MySprite leakStatus = MySprite.CreateText($"Leak Prevention: {myProgram.leakController.Status.ToUpper()}", "Debug", Constants.COLOR_WHITE, 1.5f, TextAlignment.CENTER);
                leakStatus.Position = new Vector2(256, 442);

                footerSpriteList.Add(pageFrame);
                footerSpriteList.Add(pageNumber);
                footerSpriteList.Add(footerFrame);
                footerSpriteList.Add(leakStatus);
            }

        }
    }
}

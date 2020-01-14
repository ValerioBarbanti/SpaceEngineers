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
        public class ProductionScreen {

            Program myProgram;

            List<IMyTextPanel> Panels;
            List<IMyTextPanel> DoublePanels;
            ScreenManager ScreenManager;

            public ProductionScreen(Program program, ScreenManager sManager, List<IMyTextPanel> panels, List<IMyTextPanel> doublePanels) {
                myProgram = program;
                ScreenManager = sManager;
                Panels = panels;
                DoublePanels = doublePanels;
            }

            public void GenerateScreen() {
                foreach (IMyTextPanel panel in Panels) {
                    using (var frame = panel.DrawFrame()) {

                        AddElementsToPanels(frame, false);

                    }
                }
                foreach (IMyTextPanel panel in DoublePanels) {
                    using (var frame = panel.DrawFrame()) {
                        myProgram.screenManager.leakScreen.AddElementsToPanel(1, frame);
                        AddElementsToPanels(frame, true);

                    }
                }
            }

            private void AddElementsToPanels(MySpriteDrawFrame frame, bool isDoublePanel) {
                List<MySprite> backgroundSpriteList = new List<MySprite>();
                List<MySprite> headerSpriteList = new List<MySprite>();
                List<MySprite> productionSpriteList = new List<MySprite>();
                List<MySprite> gasTanksSpriteList = new List<MySprite>();
                List<MySprite> airlocksSpriteList = new List<MySprite>();

                float screenOffset = 0;
                if (isDoublePanel) {
                    screenOffset = 512;
                }

                DrawBackground(backgroundSpriteList, screenOffset);
                DrawHeader(headerSpriteList, screenOffset);
                DrawProduction(productionSpriteList, screenOffset);
                DrawGasTanks(gasTanksSpriteList, screenOffset);
                DrawAirlocks(airlocksSpriteList, screenOffset);

                frame.AddRange(backgroundSpriteList);
                frame.AddRange(headerSpriteList);
                frame.AddRange(productionSpriteList);
                frame.AddRange(gasTanksSpriteList);
                frame.AddRange(airlocksSpriteList);
            }

            private void DrawBackground(List<MySprite> backgroundSpriteList, float screenOffset) {
                Vector2 b_pos = new Vector2(256 + screenOffset, 256);
                Vector2 b_size = new Vector2(512, 512);
                var background = MySprite.CreateSprite("SquareSimple", b_pos, b_size);
                background.Color = Constants.COLOR_BACKGROUND;

                backgroundSpriteList.Add(background);
            }

            private void DrawHeader(List<MySprite> headerSpriteList, float screenOffset) {
                Color color = (myProgram.productionController.AutomaticLifeSupport) ? Constants.COLOR_GREEN : Constants.COLOR_RED;

                Vector2 b_pos = new Vector2(256 + screenOffset, 34);
                Vector2 b_size = new Vector2(492, 48);
                var background = MySprite.CreateSprite("SquareSimple", b_pos, b_size);
                background.Color = color;

                string text = (myProgram.productionController.AutomaticLifeSupport) ? "ON" : "OFF";
                var headerText = MySprite.CreateText($"AUTO O2 PRODUCTION: {text}", "Debug", Constants.COLOR_WHITE, 1.5f, TextAlignment.CENTER);
                headerText.Position = new Vector2(256 + screenOffset, 10);


                headerSpriteList.Add(background);
                headerSpriteList.Add(headerText);
            }

            private void DrawProduction(List<MySprite> productionSpriteList, float screenOffset) {
                Vector2 b_pos = new Vector2(130.5f + screenOffset, 130);
                Vector2 b_size = new Vector2(241, 124);
                var background = MySprite.CreateSprite("SquareSimple", b_pos, b_size);
                background.Color = Constants.COLOR_BACKGROUND_MASK;

                Vector2 h_pos = new Vector2(130.5f + screenOffset, 78);
                Vector2 h_size = new Vector2(241, 20);
                var header = MySprite.CreateSprite("SquareSimple", h_pos, h_size);
                header.Color = Constants.COLOR_GREEN;

                var headerText = MySprite.CreateText("PRODUCTION INFO", "Debug", Constants.COLOR_WHITE, 0.6f, TextAlignment.LEFT);
                headerText.Position = new Vector2(20 + screenOffset, 69);

                Vector2 gb_pos = new Vector2(130.5f + screenOffset, 116.5f);
                Vector2 gb_size = new Vector2(221, 37);
                var generatorsBackground = MySprite.CreateSprite("SquareSimple", gb_pos, gb_size);
                generatorsBackground.Color = Constants.COLOR_BACKGROUND_LIGHT;

                Vector2 ofb_pos = new Vector2(130.5f + screenOffset, 163.5f);
                Vector2 ofb_size = new Vector2(221, 37);
                var oxygenFarmBackground = MySprite.CreateSprite("SquareSimple", ofb_pos, ofb_size);
                oxygenFarmBackground.Color = Constants.COLOR_BACKGROUND_LIGHT;

                Vector2 gs_pos = new Vector2(30 + screenOffset, 116.5f);
                Vector2 gs_size = new Vector2(20, 37);
                var generatorsSwitch = MySprite.CreateSprite("SquareSimple", gs_pos, gs_size);
                generatorsSwitch.Color = myProgram.productionController.LifeSupportInfo.IsGeneratorsWorking ? Constants.COLOR_GREEN : Constants.COLOR_RED;

                Vector2 ofs_pos = new Vector2(30 + screenOffset, 163.5f);
                Vector2 ofs_size = new Vector2(20, 37);
                var oxygenFarmSwitch = MySprite.CreateSprite("SquareSimple", ofs_pos, ofs_size);
                oxygenFarmSwitch.Color = myProgram.productionController.LifeSupportInfo.IsOxygenFarmWorking ? Constants.COLOR_GREEN : Constants.COLOR_RED;

                var generatorsSwitchText = MySprite.CreateText($"H2/O2 GENERATORS\n{myProgram.productionController.WorkingGenerators}", "Debug", Constants.COLOR_WHITE, 0.6f, TextAlignment.LEFT);
                generatorsSwitchText.Position = new Vector2(47 + screenOffset, 98.5f);

                var oxygenFarmSwitchText = MySprite.CreateText($"OXYGEN FARMS\n{myProgram.productionController.WorkingOxygenFarms}", "Debug", Constants.COLOR_WHITE, 0.6f, TextAlignment.LEFT);
                oxygenFarmSwitchText.Position = new Vector2(47 + screenOffset, 145.5f);

                productionSpriteList.Add(background);
                productionSpriteList.Add(header);
                productionSpriteList.Add(headerText);
                productionSpriteList.Add(generatorsBackground);
                productionSpriteList.Add(oxygenFarmBackground);
                productionSpriteList.Add(generatorsSwitch);
                productionSpriteList.Add(oxygenFarmSwitch);
                productionSpriteList.Add(generatorsSwitchText);
                productionSpriteList.Add(oxygenFarmSwitchText);
            }

            private void DrawGasTanks(List<MySprite> gasTanksSpriteList, float screenOffset) {
                Vector2 b_pos = new Vector2(130.5f + screenOffset, 352);
                Vector2 b_size = new Vector2(241, 300);
                var background = MySprite.CreateSprite("SquareSimple", b_pos, b_size);
                background.Color = Constants.COLOR_BACKGROUND_MASK;

                Vector2 h_pos = new Vector2(130.5f + screenOffset, 212);
                Vector2 h_size = new Vector2(241, 20);
                var header = MySprite.CreateSprite("SquareSimple", h_pos, h_size);
                header.Color = Constants.COLOR_GREEN;

                var headerText = MySprite.CreateText("GAS TANKS", "Debug", Constants.COLOR_WHITE, 0.6f, TextAlignment.LEFT);
                headerText.Position = new Vector2(20 + screenOffset, 203);

                // OXYGEN TANK
                Color oxygenColor = Constants.COLOR_GREEN;
                if (myProgram.productionController.LifeSupportInfo.TotalOxygenInTanks <= 70) {
                    oxygenColor = Constants.COLOR_YELLOW;
                } else if (myProgram.productionController.LifeSupportInfo.TotalOxygenInTanks <= 30) {
                    oxygenColor = Constants.COLOR_RED;
                }

                Vector2 of_pos = new Vector2(47 + screenOffset, 362);
                Vector2 of_size = new Vector2(54, 260);
                var oxygenFrame = MySprite.CreateSprite("SquareSimple", of_pos, of_size);
                oxygenFrame.Color = Constants.COLOR_WHITE;

                Vector2 ofm_pos = new Vector2(47 + screenOffset, 362);
                Vector2 ofm_size = new Vector2(46, 252);
                var oxygenFrameMask = MySprite.CreateSprite("SquareSimple", ofm_pos, ofm_size);
                oxygenFrameMask.Color = Constants.COLOR_BACKGROUND_MASK;

                float oxygenBarPosY = (float)(374 + 110-((110 * myProgram.productionController.LifeSupportInfo.TotalOxygenInTanks) / 100));
                float oxygenBarHeight = (float)((220 * myProgram.productionController.LifeSupportInfo.TotalOxygenInTanks) / 100);

                Vector2 ofb_pos = new Vector2(47 + screenOffset, oxygenBarPosY);
                Vector2 ofb_size = new Vector2(38, oxygenBarHeight);
                var oxygenFrameBar = MySprite.CreateSprite("SquareSimple", ofb_pos, ofb_size);
                oxygenFrameBar.Color = oxygenColor;

                var oxygenBarText = MySprite.CreateText("100%", "Debug", Constants.COLOR_WHITE, 0.5f, TextAlignment.CENTER);
                oxygenBarText.Position = new Vector2(47 + screenOffset, oxygenBarPosY - (oxygenBarHeight / 2) - 15);

                var oxygenBarTitle = MySprite.CreateText("O\nX\nY\nG\nE\nN\n\nT\nA\nN\nK\nS", "Debug", Constants.COLOR_WHITE, 0.5f, TextAlignment.CENTER);
                oxygenBarTitle.Position = new Vector2(82 + screenOffset, 230);

                // HYDROGEN TANK
                Color hydrogenColor = Constants.COLOR_GREEN;
                if (myProgram.productionController.LifeSupportInfo.TotalHydrogenInTanks <= 70 && myProgram.productionController.LifeSupportInfo.TotalHydrogenInTanks > 30) {
                    hydrogenColor = Constants.COLOR_YELLOW;
                } else if (myProgram.productionController.LifeSupportInfo.TotalHydrogenInTanks <= 30) {
                    hydrogenColor = Constants.COLOR_RED;
                }

                Vector2 hf_pos = new Vector2(157 + screenOffset, 362);
                Vector2 hf_size = new Vector2(54, 260);
                var hydrogenFrame = MySprite.CreateSprite("SquareSimple", hf_pos, hf_size);
                hydrogenFrame.Color = Constants.COLOR_WHITE;

                Vector2 hfm_pos = new Vector2(157 + screenOffset, 362);
                Vector2 hfm_size = new Vector2(46, 252);
                var hydrogenFrameMask = MySprite.CreateSprite("SquareSimple", hfm_pos, hfm_size);
                hydrogenFrameMask.Color = Constants.COLOR_BACKGROUND_MASK;

                float hydrogenBarPosY = (float)(374 + 110 - ((110 * myProgram.productionController.LifeSupportInfo.TotalHydrogenInTanks) / 100));
                float hydrogenBarHeight = (float)((220 * myProgram.productionController.LifeSupportInfo.TotalHydrogenInTanks) / 100);

                Vector2 hfb_pos = new Vector2(157 + screenOffset, hydrogenBarPosY);
                Vector2 hfb_size = new Vector2(38, hydrogenBarHeight);
                var hydrogenFrameBar = MySprite.CreateSprite("SquareSimple", hfb_pos, hfb_size);
                hydrogenFrameBar.Color = hydrogenColor;

                var hydrogenBarText = MySprite.CreateText($"{myProgram.productionController.LifeSupportInfo.ReadableHydrogenInTanks}", "Debug", Constants.COLOR_WHITE, 0.5f, TextAlignment.CENTER);
                hydrogenBarText.Position = new Vector2(157 + screenOffset, hydrogenBarPosY - (hydrogenBarHeight / 2) - 15);

                var hydrogenBarTitle = MySprite.CreateText("H\nY\nD\nR\nO\nG\nE\nN\n\nT\nA\nN\nK\nS", "Debug", Constants.COLOR_WHITE, 0.5f, TextAlignment.CENTER);
                hydrogenBarTitle.Position = new Vector2(192 + screenOffset, 230);

                gasTanksSpriteList.Add(background);
                gasTanksSpriteList.Add(header);
                gasTanksSpriteList.Add(headerText);
                gasTanksSpriteList.Add(oxygenFrame);
                gasTanksSpriteList.Add(oxygenFrameMask);
                gasTanksSpriteList.Add(oxygenFrameBar);
                gasTanksSpriteList.Add(oxygenBarText);
                gasTanksSpriteList.Add(oxygenBarTitle);
                gasTanksSpriteList.Add(hydrogenFrame);
                gasTanksSpriteList.Add(hydrogenFrameMask);
                gasTanksSpriteList.Add(hydrogenFrameBar);
                gasTanksSpriteList.Add(hydrogenBarText);
                gasTanksSpriteList.Add(hydrogenBarTitle);
            }

            private void DrawAirlocks(List<MySprite> airlocksSpriteList, float screenOffset) {
                Vector2 b_pos = new Vector2(381.5f + screenOffset, 285);
                Vector2 b_size = new Vector2(241, 434);
                var background = MySprite.CreateSprite("SquareSimple", b_pos, b_size);
                background.Color = Constants.COLOR_BACKGROUND_MASK;

                Vector2 h_pos = new Vector2(381.5f + screenOffset, 78);
                Vector2 h_size = new Vector2(241, 20);
                var header = MySprite.CreateSprite("SquareSimple", h_pos, h_size);
                header.Color = Constants.COLOR_GREEN;

                var headerText = MySprite.CreateText("AIRLOCKS STATUS", "Debug", Constants.COLOR_WHITE, 0.6f, TextAlignment.LEFT);
                headerText.Position = new Vector2(271 + screenOffset, 69);

                airlocksSpriteList.Add(background);
                airlocksSpriteList.Add(header);
                airlocksSpriteList.Add(headerText);

                float startY = 133; // 213
                float offset = 80;
                int counter = 0;
                foreach (KeyValuePair<string, Airlock> _al in myProgram.airlockController.Airlocks) {

                    Airlock airlock = _al.Value;
                    myProgram.Echo($"Airlock: {airlock.Name}");

                    Color airlockStatusColor = new Color(256, 256, 256);
                    if (airlock.PublicStatus.Equals(Constants.AP_IDLE) || airlock.PublicStatus.Equals(Constants.AP_CYCLE)) {
                        airlockStatusColor = Constants.COLOR_YELLOW;
                    } else if (airlock.PublicStatus.Equals(Constants.AP_PRESSURIZED)) {
                        airlockStatusColor = Constants.COLOR_GREEN;
                    } else if (airlock.PublicStatus.Equals(Constants.AP_DEPRESSURIZED)) {
                        airlockStatusColor = Constants.COLOR_RED;
                    }

                    Vector2 ab_pos = new Vector2(381.5f + screenOffset, startY + (offset * counter));
                    Vector2 ab_size = new Vector2(221, 70);
                    var airlockBackground = MySprite.CreateSprite("SquareSimple", ab_pos, ab_size);
                    airlockBackground.Color = Constants.COLOR_BACKGROUND_LIGHT;

                    Vector2 asi_pos = new Vector2(276f + screenOffset, startY + (offset * counter));
                    Vector2 asi_size = new Vector2(10, 70);
                    var airlockStatusIndicator = MySprite.CreateSprite("SquareSimple", asi_pos, asi_size);
                    airlockStatusIndicator.Color = airlockStatusColor;

                    var airlockName = MySprite.CreateText(airlock.Name, "Debug", Constants.COLOR_WHITE, 1f, TextAlignment.LEFT);
                    airlockName.Position = new Vector2(288f + screenOffset, 100 + (offset * counter));

                    var airlockStatusTitle = MySprite.CreateText("STATUS", "Debug", Constants.COLOR_WHITE, 0.6f, TextAlignment.LEFT);
                    airlockStatusTitle.Position = new Vector2(288f + screenOffset, 128 + (offset * counter));

                    var airlockStatus = MySprite.CreateText(airlock.PublicStatus, "Debug", Constants.COLOR_WHITE, 0.6f, TextAlignment.RIGHT);
                    airlockStatus.Position = new Vector2(480f + screenOffset, 128 + (offset * counter));

                    var airlockDoorStatusTitle = MySprite.CreateText("DOORS", "Debug", Constants.COLOR_WHITE, 0.6f, TextAlignment.LEFT);
                    airlockDoorStatusTitle.Position = new Vector2(288.5f + screenOffset, 145 + (offset * counter));

                    var airlockDoorStatus = MySprite.CreateText(airlock.OpenDoors, "Debug", Constants.COLOR_WHITE, 0.6f, TextAlignment.RIGHT);
                    airlockDoorStatus.Position = new Vector2(480f + screenOffset, 145 + (offset * counter));

                    airlocksSpriteList.Add(airlockBackground);
                    airlocksSpriteList.Add(airlockStatusIndicator);
                    airlocksSpriteList.Add(airlockName);
                    airlocksSpriteList.Add(airlockStatusTitle);
                    airlocksSpriteList.Add(airlockStatus);
                    airlocksSpriteList.Add(airlockDoorStatusTitle);
                    airlocksSpriteList.Add(airlockDoorStatus);

                    counter++;

                }


                
            }
        }
    }
}

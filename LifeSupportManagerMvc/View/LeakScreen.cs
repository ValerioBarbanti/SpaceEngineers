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
        public class LeakScreen {

            Program myProgram;

            List<IMyTextPanel> Panels;
            List<IMyAirVent> Airvents;

            ScreenManager ScreenManager;

            private float rotationRadiants = 0;

            public LeakScreen(Program program, ScreenManager sManager, List<IMyTextPanel> panels, List<IMyTextPanel> doublePanels, List<IMyAirVent> airvents) {
                myProgram = program;
                ScreenManager = sManager;
                Panels = panels;
                Panels.AddList(doublePanels);
                Airvents = airvents;
            }

            public void GenerateScreen() {
                foreach (IMyTextPanel panel in Panels) {

                    int page = 1;
                    if (panel.CustomData.Contains("page")) {
                        int sIndex = panel.CustomData.IndexOf("=");
                        string pString = panel.CustomData.Substring(sIndex + 1);
                        page = Int32.Parse(pString);
                    }

                    using (var frame = panel.DrawFrame()) {
                        AddElementsToPanel(page, frame);

                    }
                }
            }

            public void AddElementsToPanel(int page, MySpriteDrawFrame frame) {
                List<MySprite> backgroundSpriteList = new List<MySprite>();
                List<List<MySprite>> airventRowList = new List<List<MySprite>>();
                List<MySprite> footerSpriteList = new List<MySprite>();

                DrawBackground(backgroundSpriteList);

                DrawAirvents(Airvents, airventRowList, page);

                DrawFooter(Airvents.Count, footerSpriteList, page);

                frame.AddRange(backgroundSpriteList);
                foreach (List<MySprite> airventRowSprites in airventRowList) {
                    frame.AddRange(airventRowSprites);
                }
                frame.AddRange(footerSpriteList);
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

                        if (index % 2 == 0) {
                            x = leftColumnX;
                            y = startColumnY + (columnYincrement * halfIndex);
                        } else {
                            x = rightColumnX;
                            y = startColumnY + (columnYincrement * halfIndex);
                        }

                        MySprite sectionBackground;
                        DrawSectionBackground(x, y, out sectionBackground);

                        MySprite iconBorder, iconBackground, fanH, fanV, fanCenterBackground, fanCenter;
                        DrawAirventIcon(x - 101.5f, y - 14, stateColor, isWorking, out iconBorder, out iconBackground, out fanH, out fanV, out fanCenterBackground, out fanCenter);

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

                        if (index % 2 == 1) {
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
                airventText.Position = new Vector2(posX - 83, posY - 25);

                Vector2 d_pos = new Vector2(posX + 17, posY);
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

                oxygenInfo = MySprite.CreateText((airvent.GetOxygenLevel() * 100).ToString("0.0"), "Debug", Constants.COLOR_WHITE, 0.5f, TextAlignment.CENTER);
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
                string statusText = "";
                if (myProgram.isLeakManagementOn) {
                    footerFrame.Color = Constants.COLOR_GREEN;
                    statusText = "ON";
                } else {
                    footerFrame.Color = Constants.COLOR_RED;
                    statusText = "OFF";
                }

                MySprite leakStatus = MySprite.CreateText($"Leak Prevention: {statusText}", "Debug", Constants.COLOR_WHITE, 1.5f, TextAlignment.CENTER);
                leakStatus.Position = new Vector2(256, 442);

                footerSpriteList.Add(pageFrame);
                footerSpriteList.Add(pageNumber);
                footerSpriteList.Add(footerFrame);
                footerSpriteList.Add(leakStatus);
            }



        }
    }
}

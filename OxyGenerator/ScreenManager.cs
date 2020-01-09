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

            public enum Elements { Background, Header, Footer, OxygenBar, OxygenInfo};

            private static float screenX;
            private static float screenY;
            private static float halfScreenX;
            private static float halfScreenY;

            private static float spacing;
            private static float headerFooterHeight;



            private static void DefineCoordinates(IMyTextPanel screen) {
                screenX = screen.SurfaceSize.X;
                screenY = screen.SurfaceSize.Y;
                halfScreenX = screenX / 2;
                halfScreenY = screenY / 2;

                spacing = 10;
                headerFooterHeight = 50;
            }

            public static List<MySprite> CreateBackground(IMyTextPanel screen) {
                DefineCoordinates(screen);
                List<MySprite> spriteList = new List<MySprite>();

                Vector2 b_pos = new Vector2(halfScreenX, halfScreenY);
                Vector2 b_size = new Vector2(screenX, screenY);
                var background = MySprite.CreateSprite("SquareSimple", b_pos, b_size);
                background.Color = Constants.C_DARK_BLUE;

                spriteList.Add(background);

                return spriteList;
            }

            public static List<MySprite> CreateHeader(IMyTextPanel screen, string text, float textSize) {
                DefineCoordinates(screen);
                List<MySprite> spriteList = new List<MySprite>();

                // HEADER
                Vector2 h_pos = new Vector2(halfScreenX, spacing + 25);
                Vector2 h_size = new Vector2(screenX - (spacing * 2), headerFooterHeight);
                var header = MySprite.CreateSprite("SquareSimple", h_pos, h_size);
                header.Color = Constants.C_CYAN;
                spriteList.Add(header);

                // HEADER MASK
                Vector2 hm_pos = new Vector2(halfScreenX, spacing + 24);
                Vector2 hm_size = new Vector2(screenX - (spacing * 2), headerFooterHeight - 2);
                var headerMask = MySprite.CreateSprite("SquareSimple", hm_pos, hm_size);
                headerMask.Color = Constants.C_DARK_BLUE;
                spriteList.Add(headerMask);

                // HEADER TITLE
                var headerText = MySprite.CreateText(text, "Debug", Constants.C_LIGHT_GREY, textSize, TextAlignment.CENTER);
                headerText.Position = new Vector2(halfScreenX, 5);
                spriteList.Add(headerText);

                return spriteList;
            }

            public static List<MySprite> CreateFooter(IMyTextPanel screen, string text, float textSize) {
                DefineCoordinates(screen);
                List<MySprite> spriteList = new List<MySprite>();

                // FOOTER
                Vector2 f_pos = new Vector2(halfScreenX, 512 - spacing - 25);
                Vector2 f_size = new Vector2(screenX - (spacing * 2), headerFooterHeight);
                var footer = MySprite.CreateSprite("SquareSimple", f_pos, f_size);
                footer.Color = Constants.C_CYAN;
                spriteList.Add(footer);

                // FOOTER MASK
                Vector2 fm_pos = new Vector2(halfScreenX, 512 - spacing - 24);
                Vector2 fm_size = new Vector2(screenX - (spacing * 2), headerFooterHeight - 2);
                var footerMask = MySprite.CreateSprite("SquareSimple", fm_pos, fm_size);
                footerMask.Color = Constants.C_DARK_BLUE;
                spriteList.Add(footerMask);

                // FOOTER TITLE
                var footerText = MySprite.CreateText(text, "Debug", Constants.C_LIGHT_GREY, textSize, TextAlignment.CENTER);
                footerText.Position = new Vector2(halfScreenX, 457);
                spriteList.Add(footerText);

                return spriteList;
            }

            public static List<MySprite> CreateOxygenBar(IMyTextPanel screen, List<IMyAirVent> airvents) {
                DefineCoordinates(screen);
                List<MySprite> spriteList = new List<MySprite>();

                float obc_syzeY = 512 - (headerFooterHeight * 2) - (spacing * 4);

                Vector2 obc_pos = new Vector2(spacing + 50, halfScreenY);
                Vector2 obc_size = new Vector2(100, obc_syzeY);
                var oxygenBarContainer = MySprite.CreateSprite("SquareSimple", obc_pos, obc_size);
                oxygenBarContainer.Color = Constants.C_CYAN;
                spriteList.Add(oxygenBarContainer);

                // OXYGEN BAR CONTAINER MASK
                Vector2 obcm_pos = new Vector2(60, 256);
                Vector2 obcm_size = new Vector2(96, obc_syzeY - 4);
                var oxygenBarContainerMask = MySprite.CreateSprite("SquareSimple", obcm_pos, obcm_size);
                oxygenBarContainerMask.Color = Constants.C_DARK_CYAN;
                spriteList.Add(oxygenBarContainerMask);

                int oxLevel = 0;
                foreach (IMyAirVent airvent in airvents) {
                    oxLevel = (int)(airvent.GetOxygenLevel() * 10);
                }
                
                float posY = 94;
                for (int i = 0; i < 10; i++) {
                    if ((10 - oxLevel) <= i) {
                        Vector2 ob_size = new Vector2(84, 30);
                        Vector2 ob_pos = new Vector2(60, posY + (36 * i));
                        var oxygenBar = MySprite.CreateSprite("SquareSimple", ob_pos, ob_size);
                        oxygenBar.Color = Constants.C_LIGHT_GREY;
                        spriteList.Add(oxygenBar);
                    }
                }

                return spriteList;
            }

            public static List<MySprite> CreateInfoPanel(IMyTextPanel screen, string leftColumnText, string rightColumnText) {
                DefineCoordinates(screen);
                List<MySprite> spriteList = new List<MySprite>();

                // INFO PANEL CONTAINER
                float ipc_syzeY = 512 - (headerFooterHeight * 2) - (spacing * 4);
                Vector2 ipc_size = new Vector2(382, ipc_syzeY);
                Vector2 ipc_pos = new Vector2(310, 256);
                var infoPanelContainer = MySprite.CreateSprite("SquareSimple", ipc_pos, ipc_size);
                infoPanelContainer.Color = Constants.C_CYAN;
                spriteList.Add(infoPanelContainer);

                // INFO PANEL CONTAINER MASK
                Vector2 ipcm_size = new Vector2(378, ipc_syzeY - 4);
                Vector2 ipcm_pos = new Vector2(310, 256);
                var infoPanelContainerMask = MySprite.CreateSprite("SquareSimple", ipcm_pos, ipcm_size);
                infoPanelContainerMask.Color = Constants.C_DARK_CYAN;
                spriteList.Add(infoPanelContainerMask);

                // INFO PANEL TEXT LEFT
                var infoPanelTextLeft = MySprite.CreateText(leftColumnText, "Debug", Constants.C_LIGHT_GREY, 0.8f, TextAlignment.LEFT);
                infoPanelTextLeft.Position = new Vector2(130, 80);
                spriteList.Add(infoPanelTextLeft);

                // INFO PANEL TEXT RIGHT
                var infoPanelTextRight = MySprite.CreateText(rightColumnText, "Debug", Constants.C_LIGHT_GREY, 0.8f, TextAlignment.RIGHT);
                infoPanelTextRight.Position = new Vector2(490, 80);
                spriteList.Add(infoPanelTextRight);

                return spriteList;
            }

            public static List<MySprite> CreateInfoPanelList(IMyTextPanel screen, Dictionary<string, Airlock> airlocks) {
                DefineCoordinates(screen);
                List<MySprite> spriteList = new List<MySprite>();

                // INFO PANEL CONTAINER
                float ipc_syzeY = 512 - (headerFooterHeight * 2) - (spacing * 4);
                Vector2 ipc_size = new Vector2(screenX - (spacing * 2), ipc_syzeY);
                Vector2 ipc_pos = new Vector2(halfScreenX, halfScreenY);
                var infoPanelContainer = MySprite.CreateSprite("SquareSimple", ipc_pos, ipc_size);
                infoPanelContainer.Color = Constants.C_CYAN;
                spriteList.Add(infoPanelContainer);

                // INFO PANEL CONTAINER MASK
                Vector2 ipcm_size = new Vector2(screenX - (spacing * 2) - 4, ipc_syzeY - 4);
                Vector2 ipcm_pos = new Vector2(halfScreenX, halfScreenY);
                var infoPanelContainerMask = MySprite.CreateSprite("SquareSimple", ipcm_pos, ipcm_size);
                infoPanelContainerMask.Color = Constants.C_DARK_CYAN;
                spriteList.Add(infoPanelContainerMask);

                // INFO PANEL TEXT LEFT
                float pos = 80;
                int index = 0;

                if (airlocks.Count != 0) {
                    foreach (KeyValuePair<string, Airlock> airlock in airlocks) {
                        string text = airlock.Value.Name + ": " + airlock.Value.PublicStatus + "\nO2: "+airlock.Value.OxygenLevel + " - Tank: "+airlock.Value.OxygenTankFill;
                        Color color = Constants.C_LIGHT_GREY;
                        if (!airlock.Value.Issues.Equals("None") || airlock.Value.OxygenLevel.Equals("None")) {
                            color = Color.IndianRed;
                        }
                        var infoPanelTextLeft = MySprite.CreateText(text, "Monospace", color, 0.5f, TextAlignment.LEFT);
                        infoPanelTextLeft.Position = new Vector2(22, pos);
                        spriteList.Add(infoPanelTextLeft);

                        if (index < airlocks.Count-1) {
                            Vector2 divLinePos = new Vector2(256, pos + 37);
                            Vector2 divLineSize = new Vector2(screenX - (spacing * 2) - 24, 2);
                            var divisoryLine = MySprite.CreateSprite("SquareSimple", divLinePos, divLineSize);
                            divisoryLine.Color = Constants.C_CYAN;
                            spriteList.Add(divisoryLine);
                        }

                        index++;
                        pos = pos + 41;
                    }
                } else {
                    string text = "No airlocks found";
                    Color color = Color.IndianRed;
                    var infoPanelTextLeft = MySprite.CreateText(text, "Monospace", color, 0.5f, TextAlignment.LEFT);
                    infoPanelTextLeft.Position = new Vector2(22, pos);
                }


                

                

                return spriteList;
            }

        }
    }
}

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

        float maxSensorRange = 400;

        List<IMySensorBlock> sensors = new List<IMySensorBlock>();
        List<IMyLargeTurretBase> turrets = new List<IMyLargeTurretBase>();
        List<IMyTextPanel> screens = new List<IMyTextPanel>();

        IMySensorBlock sensor;
        List<IMyTextPanel> usedScreens = new List<IMyTextPanel>();

        List<MyDetectedEntityInfo> entities = new List<MyDetectedEntityInfo>();
        List<Target> targets = new List<Target>();

        // "[COS-T] Targets"

        public Program() {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;

            GridTerminalSystem.GetBlocksOfType(sensors);
            GridTerminalSystem.GetBlocksOfType(turrets);
            GridTerminalSystem.GetBlocksOfType(screens);
            
            InitBlocks();
        }

        private void InitBlocks() {
            // init sensor
            InitSensor();

            // init turrets
            InitTurrets();

            // init screens
            InitScreens();
        }

        private void InitSensor() {
            foreach (IMySensorBlock mySensor in sensors) {
                if (mySensor.CustomData.Equals("[COS-T]")) {
                    sensor = mySensor;
                }
            }
            sensor.LeftExtend = maxSensorRange;
            sensor.RightExtend = maxSensorRange;
            sensor.TopExtend = maxSensorRange;
            sensor.BottomExtend = maxSensorRange;
            sensor.FrontExtend = maxSensorRange;
            sensor.BackExtend = maxSensorRange;
        }

        private void InitTurrets() {
            foreach (IMyLargeTurretBase turret in turrets) {
                //turret
            }
        }

        private void InitScreens() {
            foreach (IMyTextPanel screen in screens) {
                if (screen.CustomData.Contains("[COS-T]")) {
                    usedScreens.Add(screen);
                    screen.ContentType = ContentType.SCRIPT;
                }
            }
        }

        public void Save() {
            
        }

        public void Main(string argument, UpdateType updateSource) {
            FindTargetsList();
        }

        private void FindTargetsList() {
            Echo("Sensor Range: " + sensor.MaxRange);
            //Echo("Running");
            sensor.DetectedEntities(entities);

            foreach (MyDetectedEntityInfo entity in entities) {
                Target t = new Target(entity, Me.GetPosition());
                targets.Add(t);
            }

            Echo("Entities: "+entities.Count);
            Echo("Screens: " + screens.Count);
            Echo("Used Screens: " + usedScreens.Count);

            targets.Sort((x, y) => x.Distance.CompareTo(y.Distance));

            foreach (IMyTextPanel screen in usedScreens) {
                using (var frame = screen.DrawFrame()) {
                    float point = 10;
                    foreach (Target target in targets) {
                        Echo("start");
                        Echo("sensor: " + sensor.ToString());

                        Vector2 pos2 = new Vector2(256, 256);
                        Vector2 size = new Vector2(512, 512);
                        var background = MySprite.CreateSprite("SquareSimple", pos2, size);
                        background.Color = Color.Black;
                        frame.Add(background);

                        if (point == 10) {
                            target.Selected = true;
                        }

                        StringBuilder textEnt = new StringBuilder();
                        if (target.Selected) {
                            textEnt.Append("*| ");
                        } else {
                            textEnt.Append(" | ");
                        }
                        textEnt.Append(target.Info.Name + " " + target.Distance.ToString("0.00") + "m");
                        
                        //string textEnt = target.Info.Name + ": " + target.Distance.ToString("0.00");
                        Echo("textent: " + textEnt);
                        Vector2 pos = new Vector2(10, point);
                        var text = MySprite.CreateText(textEnt.ToString(), "Monospace", Color.White, 0.8f, TextAlignment.LEFT);

                        if (target.Info.Relationship.Equals(MyRelationsBetweenPlayerAndBlock.Enemies)) {
                            text.Color = Color.Red;
                        }

                        text.Position = pos;
                        frame.Add(text);
                        point += 25;
                    }
                }
            }

            targets.Clear();
        }
    }
}

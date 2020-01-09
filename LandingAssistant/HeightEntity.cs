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
        public class HeightEntity {

            public const string FRONT_LEFT = "[LAS] front left";
            public const string FRONT_RIGHT = "[LAS] front right";
            public const string BACK_LEFT = "[LAS] back left";
            public const string BACK_RIGHT = "[LAS] back right";

            public string Name { get; set; }

            /*private double _distance;
            private string _distanceToString;*/
            public IMyCameraBlock Camera { get; set; }
            public MyDetectedEntityInfo Info { get; set; }

            public HeightEntity() {

            }

            public HeightEntity(string name, IMyCameraBlock camera) {
                Name = name;
                this.Camera = camera;
            }

            public double Distance {
                get {
                    if (!Info.IsEmpty()) {
                        return Vector3D.Distance(Camera.GetPosition(), Info.HitPosition.Value);
                    } else {
                        return Program.SCAN_DISTANCE + 1;
                    }
                    
                }
            }

            public string DistanceToString {
                get {
                    if (Distance < Program.SCAN_DISTANCE + 1) {
                        return Distance.ToString("0.00");
                    } else {
                        return ">" + Program.SCAN_DISTANCE.ToString();
                    }
                    
                }
            }

        }
    }
}

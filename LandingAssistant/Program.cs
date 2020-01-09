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

        public const double SCAN_DISTANCE = 100.0;

        MyCommandLine _commandLine = new MyCommandLine();

        List<IMyCameraBlock> cameras = new List<IMyCameraBlock>();
        List<MyDetectedEntityInfo> infos = new List<MyDetectedEntityInfo>();

        HeightEntity heFrontLeft;
        HeightEntity heFrontRight;
        HeightEntity heBackLeft;
        HeightEntity heBackRight;
        List<HeightEntity> heightEntities = new List<HeightEntity>();

        List<IMyTextPanel> screens = new List<IMyTextPanel>();

        List<IMyGyro> gyros = new List<IMyGyro>();

        string Status;

        public Program() {
            Runtime.UpdateFrequency = UpdateFrequency.Update10;

            GridTerminalSystem.GetBlocksOfType(cameras);
            GridTerminalSystem.GetBlocksOfType(screens);
            GridTerminalSystem.GetBlocksOfType(gyros);

            Status = "IDLE";

            InitCameras();
            InitSurfaces();
        }

        private void InitCameras() {
            bool checkFL = false;
            bool checkFR = false;
            bool checkBL = false;
            bool checkBR = false;
            foreach (IMyCameraBlock camera in cameras) {



                
                if (!String.IsNullOrEmpty(camera.CustomData)) {
                    if (camera.CustomData.Equals(HeightEntity.FRONT_LEFT)) {
                        heightEntities.Add(heFrontLeft = new HeightEntity(HeightEntity.FRONT_LEFT, camera));
                        checkFL = true;
                    } else if (camera.CustomData.Equals(HeightEntity.FRONT_RIGHT)) {
                        heightEntities.Add(heFrontRight = new HeightEntity(HeightEntity.FRONT_RIGHT, camera));
                        checkFR = true;
                    } else if (camera.CustomData.Equals(HeightEntity.BACK_LEFT)) {
                        heightEntities.Add(heBackLeft = new HeightEntity(HeightEntity.BACK_LEFT, camera));
                        checkBL = true;
                    } else if (camera.CustomData.Equals(HeightEntity.BACK_RIGHT)) {
                        heightEntities.Add(heBackRight = new HeightEntity(HeightEntity.BACK_RIGHT, camera));
                        checkBR = true;
                    } 
                }
            }
            if (!checkFL) {
                throw new Exception("ERROR: Front Left camera missing");
            }
            if (!checkFR) {
                throw new Exception("ERROR: Front Right camera missing");
            }
            if (!checkBL) {
                throw new Exception("ERROR: Back Left camera missing");
            }
            if (!checkBR) {
                throw new Exception("ERROR: Back Right camera missing");
            }
        }

        

        private void InitSurfaces() {
            foreach (IMyTextPanel screen in screens) {
                screen.ContentType = ContentType.TEXT_AND_IMAGE;
            }
        }

        public void Save() {
            
        }

        public void Main(string argument, UpdateType updateSource) {
            if (_commandLine.TryParse(argument)) {
                string command = _commandLine.Argument(0);
                if (command == null) {
                    Echo("No command specified");
                } else if (!command.Equals("land") && !command.Equals("scan")) {
                    Echo("Wrong command issued, should be 'scan' or 'land'");
                } else if (command.Equals("scan")){
                    ExecuteScan();
                } else if (command.Equals("land")) {
                    ExecuteLanding();
                }
            }

            Echo("Status: " + Status);
            Scan();
            Land();
            
        }

        private void Scan() {

            foreach(IMyCameraBlock camera in cameras) {
                Echo("Camera " + camera.CustomName + ": " + camera.Position.ToString());
            }

            if (Status.Equals("SCAN")) {
                ActivateRaycasts();
                CheckLandability();
            }
        }

        private void ActivateRaycasts() {
            if (!heFrontLeft.Camera.EnableRaycast) {
                heFrontLeft.Camera.EnableRaycast = true;
            }
            if (!heFrontRight.Camera.EnableRaycast) {
                heFrontRight.Camera.EnableRaycast = true;
            }
            if (!heBackLeft.Camera.EnableRaycast) {
                heBackLeft.Camera.EnableRaycast = true;
            }
            if (!heBackRight.Camera.EnableRaycast) {
                heBackRight.Camera.EnableRaycast = true;
            }
        }

        private void CheckLandability() {
            heFrontLeft.Info = heFrontLeft.Camera.Raycast(SCAN_DISTANCE);
            heFrontRight.Info = heFrontLeft.Camera.Raycast(SCAN_DISTANCE);
            heBackLeft.Info = heFrontLeft.Camera.Raycast(SCAN_DISTANCE);
            heBackRight.Info = heFrontLeft.Camera.Raycast(SCAN_DISTANCE);
            double rollSpread = (heFrontLeft.Distance + heBackLeft.Distance) - (heFrontRight.Distance + heBackRight.Distance);
            double pitchSpread = (heFrontLeft.Distance + heFrontRight.Distance) - (heBackLeft.Distance + heBackRight.Distance);

            StringBuilder sb = new StringBuilder();
            sb.Append("F-L Distance: " + heFrontLeft.DistanceToString+"\n");
            sb.Append("F-R Distance: " + heFrontRight.DistanceToString + "\n");
            sb.Append("B-L Distance: " + heBackLeft.DistanceToString + "\n");
            sb.Append("B-R Distance: " + heBackRight.DistanceToString + "\n");
            sb.Append("Roll: " + rollSpread.ToString("0.00") + "\nPitch: " + pitchSpread.ToString("0.00"));
            WriteOnScreens(sb.ToString());
        }

        private void Land() {
            
            if (Status.Equals("LAND")) {
                int counter = 0;
                string distance = "";
                foreach (IMyCameraBlock camera in cameras) {

                    infos[counter] = camera.Raycast(100.0);
                    distance += "Position "+(counter+1)+": "+Vector3D.Distance(camera.GetPosition(), infos[counter].HitPosition.Value).ToString("0.00")+"\n";

                    Echo(distance);

                    counter++;
                }
                foreach (IMyTextPanel screen in screens) {
                    //screen.WriteText(distance, false);
                }

                foreach(IMyGyro gyro in gyros) {
                    if (!gyro.GyroOverride) {
                        gyro.GyroOverride = true;
                    }
                    //tries to pitch
                    foreach (IMyCameraBlock camera in cameras) {
                        
                    }
                }
            }
        }

        private void ExecuteScan() {
            Status = "SCAN";
        }

        private void ExecuteLanding() {
            Status = "LAND";
        }

        private void ScanCameraHeight() {

        }

        private void WriteOnScreens(String text) {
            foreach (IMyTextPanel screen in screens) {
                screen.WriteText(text, false);
            }
        }
    }
}

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

        private const double SCAN_DISTANCE_SR = 100.0;
        private const double SCAN_DISTANCE_MR = 1000.0;
        private const double SCAN_DISTANCE_LR = 10000.0;

        private const float MIN_CAMERA_PITCH = -64;
        private const float MAX_CAMERA_PITCH = 64;
        private const float MIN_CAMERA_YAW = -64;
        private const float MAX_CAMERA_YAW = 64;

        // DO NOT MODIFY BELOW HERE

        MyCommandLine _commandLine = new MyCommandLine();
        private double scanDistance;
        private string status;
        private int precision;

        private const string STATUS_SR = "SHORT_RANGE";
        private const string STATUS_MR = "MEDIUM_RANGE";
        private const string STATUS_LR = "LONG_RANGE";

        private const int PRECISION_SR = 2;
        private const int PRECISION_MR = 4;
        private const int PRECISION_LR = 16;

        private float cameraPitch;
        private float cameraYaw;

        // options
        

        // full lists for blocks
        List<IMyCameraBlock> cameras = new List<IMyCameraBlock>();
        List<IMyTextPanel> screens = new List<IMyTextPanel>();

        //lists of used blocks
        List<IMyCameraBlock> usedCameras = new List<IMyCameraBlock>();
        List<IMyTextPanel> usedScreens = new List<IMyTextPanel>();

        // helper lists
        List<MyDetectedEntityInfo> infos = new List<MyDetectedEntityInfo>();
        List<MySprite> drawPoints = new List<MySprite>();

        public Program() {
            Runtime.UpdateFrequency = UpdateFrequency.Update100;
            InitScanner();
        }

        private void InitScanner() {
            cameraPitch = MIN_CAMERA_PITCH;
            cameraYaw = MIN_CAMERA_YAW;
            precision = PRECISION_SR;
            status = STATUS_SR;
            scanDistance = SCAN_DISTANCE_SR;
            GridTerminalSystem.GetBlocksOfType(cameras);
            GridTerminalSystem.GetBlocksOfType(screens);
            foreach(IMyCameraBlock camera in cameras) {
                if (camera.CustomData.Equals("[SS]")) {
                    camera.EnableRaycast = true;
                    usedCameras.Add(camera);
                }
            }
            foreach (IMyTextPanel screen in screens) {
                if (screen.CustomData.Equals("[SS]")) {
                    screen.ContentType = ContentType.SCRIPT;
                    usedScreens.Add(screen);
                }
            }
        }

        public void Save() {
            
        }

        public void Main(string argument, UpdateType updateSource) {
            if (_commandLine.TryParse(argument)) {
                string command = _commandLine.Argument(0);
                if (command == null || command.Equals("short range")) {
                    scanDistance = SCAN_DISTANCE_SR;
                    status = STATUS_SR;
                    precision = PRECISION_SR;
                    Echo("Short Range Scan applied");
                } else if (command.Equals("medium range")) {
                    scanDistance = SCAN_DISTANCE_MR;
                    status = STATUS_MR;
                    precision = PRECISION_MR;
                    Echo("Medium Range Scan applied");
                } else if (command.Equals("long range")) {
                    scanDistance = SCAN_DISTANCE_LR;
                    status = STATUS_LR;
                    precision = PRECISION_LR;
                    Echo("Long Range Scan applied");
                } else {
                    scanDistance = SCAN_DISTANCE_SR;
                    status = STATUS_SR;
                    precision = PRECISION_SR;
                    Echo("Short Range Scan applied");
                }
            }

            Echo("Status: "+status);

            ExecuteScan();
        }

        public void ExecuteScan() {
            foreach (IMyCameraBlock camera in usedCameras) {
                if (camera.CanScan(scanDistance)) {
                    Echo("Pitch: " + cameraPitch + " - Yaw: " + cameraYaw);
                    Echo("Can Raycast: " + camera.CanScan(scanDistance));
                    MyDetectedEntityInfo info = camera.Raycast(scanDistance, cameraPitch, cameraYaw);
                    
                    foreach (IMyTextPanel screen in usedScreens) {
                        using (var frame = screen.DrawFrame()) {
                            UpdateScreen(frame, camera, info);
                        }
                    }
                }
            }

            UpdateCameraScanPosition();
        }

        private void UpdateScreen(MySpriteDrawFrame frame, IMyCameraBlock camera, MyDetectedEntityInfo info) {

            Vector2 size = new Vector2();
            if (status.Equals(STATUS_SR)) {
                size = new Vector2(8, 8);
            } else if (status.Equals(STATUS_MR)) {
                size = new Vector2(32, 32);
            } else if (status.Equals(STATUS_LR)) {
                size = new Vector2(128, 128);
            }


            float xPos = (cameraYaw + 64)*8;
            float yPos = 512 - (cameraPitch + 64)*8;
            Vector2 pos = new Vector2(xPos, yPos);

            Echo("Position: " + xPos + "," + yPos);

            var dot = MySprite.CreateSprite("SquareSimple", pos, size);
            if (info.IsEmpty()) {
                Echo("Info Empty");
                dot.Color = Color.Black;
            } else {
                Echo("Info: " + Vector3D.Distance(camera.GetPosition(), info.HitPosition.Value).ToString("0.00"));
                int color = (int)Vector3D.Distance(camera.GetPosition(), info.HitPosition.Value);
                color = 255 - (color * 3);
                Echo("color: " + color);
                dot.Color = new Color(color, color, color);
            }

            drawPoints.Add(dot);
            frame.AddRange(drawPoints);
        }

        private void UpdateCameraScanPosition() {
            if (cameraPitch <= MAX_CAMERA_PITCH && cameraYaw <= MAX_CAMERA_YAW) {
                cameraPitch = cameraPitch + precision;
            } else if (cameraPitch > MAX_CAMERA_PITCH && cameraYaw <= MAX_CAMERA_YAW) {
                cameraYaw = cameraYaw + precision;
                cameraPitch = MIN_CAMERA_PITCH;
            } else if (cameraYaw > MAX_CAMERA_YAW) {
                cameraPitch = MIN_CAMERA_PITCH;
                cameraYaw = MIN_CAMERA_YAW;
            }
        }
    }
}

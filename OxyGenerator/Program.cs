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

        MyCommandLine _commandLine = new MyCommandLine();
        Dictionary<string, Action> _commands = new Dictionary<string, Action>(StringComparer.OrdinalIgnoreCase);
        Dictionary<string, string[]> commandLines = new Dictionary<string, string[]>();

        List<IMyBlockGroup> blockGroups = new List<IMyBlockGroup>();
        Dictionary<string, Airlock> _airlocks = new Dictionary<string, Airlock>();

        List<IMyTextPanel> lcdScreens = new List<IMyTextPanel>();

        public Program() {
            InitAirlocks();
        }

        private void InitAirlocks() {
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            GridTerminalSystem.GetBlockGroups(blockGroups, group => group.Name.Contains("Airlock"));
            GridTerminalSystem.GetBlocksOfType(lcdScreens);
            if (blockGroups.Count == 0) {
                Echo("Warning, there are no valid airlocks on your ship / station");
            } else {
                foreach (IMyBlockGroup blockGroup in blockGroups) {
                    Airlock airlock = new Airlock(blockGroup);
                    _airlocks.Add(blockGroup.Name, airlock);
                }
            }
        }

        public void Save() {

        }

        public void Main(string argument, UpdateType updateSource) {
            CheckForAction(argument);
            CheckIfCycle();
            UpdateAirlockScreens();
            UpdateLcdScreens();
        }

        private void CheckForAction(string argument) {
            if (_commandLine.TryParse(argument)) {
                string command = _commandLine.Argument(0);
                string airlockName = _commandLine.Argument(1);
                string action = _commandLine.Argument(2);

                bool error = ValidateCommand(command, airlockName, action);

                if (error) {
                    Echo("Cannot cycle airlock.");
                } else {
                    AddCommandToCommandList();
                }

            }
        }

        private bool ValidateCommand(string command, string airlockName, string action) {
            bool error = false;

            if (command == null) {
                Echo("No command specified");
                error = true;
            } else if (!command.Equals("cycle")) {
                Echo("Wrong command issued, should be 'cycle'");
                error = true;
            }

            if (airlockName == null) {
                Echo("No airlock specified");
                error = true;
            } else if (!_airlocks.ContainsKey(airlockName)) {
                Echo("No airlocks with name: " + airlockName);
                error = true;
            }

            if (action == null) {
                Echo("No action specified");
                error = true;
            } else if (!action.Equals("depressurize") && !action.Equals("pressurize")) {
                Echo("Wrong action issued, should be 'depressurize' or 'pressurize'");
                error = true;
            }

            return error;
        }

        private void AddCommandToCommandList() {
            string[] commandLineArray = new string[3];
            commandLineArray[0] = _commandLine.Argument(0);
            commandLineArray[1] = _commandLine.Argument(1);
            commandLineArray[2] = _commandLine.Argument(2);
            commandLines[commandLineArray[1]] = commandLineArray;
        }


        private void CheckIfCycle() {

            List<string> commandsToRemove = new List<string>();

            foreach (KeyValuePair<string, string[]> cm in commandLines) {
                Airlock airlock = _airlocks[cm.Key];
                airlock.Execute(cm.Value[2]);
                if (airlock.Status.Equals(Constants.END_CYCLE)) {
                    commandsToRemove.Add(cm.Key);
                    airlock.Status = Constants.IDLE_STATUS;
                    //airlock.PublicStatus = Constants.PS_IDLE;
                }
            }

            foreach (string cmtr in commandsToRemove) {
                commandLines.Remove(cmtr);
            }

        }

        private void UpdateAirlockScreens() {
            foreach (KeyValuePair<string, Airlock> airlock in _airlocks) {
                airlock.Value.UpdateStatusScreen();
            }
        }

        private void UpdateLcdScreens() {
            foreach (IMyTextPanel screen in lcdScreens) {
                if (screen.CustomData.Equals("airlock infopanel")) {
                    screen.ContentType = ContentType.SCRIPT;
                    using (var frame = screen.DrawFrame()) {
                        frame.AddRange(ScreenManager.CreateBackground(screen)); // Background
                        frame.AddRange(ScreenManager.CreateHeader(screen, "Life Support", 1.5f)); // Header
                        frame.AddRange(ScreenManager.CreateFooter(screen, "CheeriOS v0.1\nLife Support Module", 0.75f)); // Footer
                        frame.AddRange(ScreenManager.CreateInfoPanelList(screen, _airlocks)); // Footer
                    }
                }
            }
        }


    }
}

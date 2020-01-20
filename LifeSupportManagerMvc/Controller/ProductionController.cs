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
        public class ProductionController {

            Program myProgram;

            public LifeSupportInfo LifeSupportInfo { get; set; }

            List<IMyOxygenFarm> oxygenFarmList = new List<IMyOxygenFarm>();
            List<IMyGasGenerator> gasGeneratorList = new List<IMyGasGenerator>();

            public string WorkingOxygenFarms { get; set; }
            public string WorkingGenerators { get; set; }

            List<IMyGasTank> gasTankListTemp = new List<IMyGasTank>();
            List<IMyGasTank> gasTankList = new List<IMyGasTank>();

            public ProductionController(Program program) {
                myProgram = program;
                Init();
            }

            private void Init() {
                LifeSupportInfo = new LifeSupportInfo();
                myProgram.GridTerminalSystem.GetBlocksOfType(oxygenFarmList);
                myProgram.GridTerminalSystem.GetBlocksOfType(gasGeneratorList);
                myProgram.GridTerminalSystem.GetBlocksOfType(gasTankListTemp);
                foreach(IMyGasTank gasTank in gasTankListTemp) {
                    bool isAirlockTank = false;
                    foreach (KeyValuePair<string, Airlock> airlock in myProgram.airlockController.Airlocks) {
                        foreach (IMyGasTank airlockGasTank in airlock.Value.Tanks) {
                            if (gasTank.EntityId == airlockGasTank.EntityId) {
                                isAirlockTank = true;
                            }
                        }
                    }
                    if (!isAirlockTank) {
                        gasTankList.Add(gasTank);
                    }
                }
            }

            public void AddCommandToStack(MyCommandLine _commandLine) {
                if (null != _commandLine.Argument(1)) {
                    if (_commandLine.Argument(1).Equals(Constants.P_ON)) {
                        myProgram.isProductionOn = true;
                    } else if (_commandLine.Argument(1).Equals(Constants.P_OFF)) {
                        myProgram.isProductionOn = false;
                    } else {
                        myProgram.Echo("No valid command\n");
                    }
                }
            }

            public void OxygenRuntime() {

                ManageGasProduction();

                LifeSupportInfo.IsGeneratorsWorking = IsGeneratorsWorking();
                LifeSupportInfo.IsOxygenFarmWorking = IsOxygenFarmWorking();

                LifeSupportInfo.TotalOxygenInTanks = GetOxygenInTanks();
                LifeSupportInfo.ReadableOxygenInTanks = (LifeSupportInfo.TotalOxygenInTanks).ToString("0.0") + "%";
                LifeSupportInfo.TotalHydrogenInTanks = GetHydrogenInTanks();
                LifeSupportInfo.ReadableHydrogenInTanks = (LifeSupportInfo.TotalHydrogenInTanks).ToString("0.0") + "%";

            }

            private void ManageGasProduction() {

                if (myProgram.isProductionOn) {
                    bool oxygenProduction = true;
                    if (LifeSupportInfo.TotalOxygenInTanks > myProgram.maximumOxygenInTanks) {
                        oxygenProduction = false;
                    }
                    if (LifeSupportInfo.TotalOxygenInTanks < myProgram.minimumOxygenInTanks) {
                        oxygenProduction = true;
                    }

                    bool hydrogenProduction = true;
                    if (LifeSupportInfo.TotalHydrogenInTanks > myProgram.maximumHydrogenInTanks) {
                        hydrogenProduction = false;
                    }
                    if (LifeSupportInfo.TotalOxygenInTanks < myProgram.minimumHydrogenInTanks) {
                        hydrogenProduction = true;
                    }

                    if (oxygenProduction && hydrogenProduction) {
                        ModifyProduction(true, true);
                    } else if (!oxygenProduction && !hydrogenProduction) {
                        ModifyProduction(false, false);
                    } else if (oxygenProduction && !hydrogenProduction) {
                        ModifyProduction(true, true);
                    } else if (!oxygenProduction && hydrogenProduction) {
                        ModifyProduction(false, true);
                    }

                }
            }

            private void ModifyProduction(bool farmEnabled, bool generatorEnabled) {
                foreach (IMyOxygenFarm farm in oxygenFarmList) {
                    IMyFunctionalBlock farmTest = farm as IMyFunctionalBlock;
                    farmTest.Enabled = farmEnabled;
                }
                foreach (IMyGasGenerator generator in gasGeneratorList) {
                    generator.Enabled = generatorEnabled;
                }
            }

            private bool IsGeneratorsWorking() {
                bool generatorsWorking = false;
                int workingGenerators = 0;
                foreach (IMyGasGenerator gen in gasGeneratorList) {
                    if (gen.IsFunctional && gen.IsWorking) {
                        workingGenerators++;
                        generatorsWorking = true;
                    }
                }
                WorkingGenerators = $"({workingGenerators} working out of {gasGeneratorList.Count} total)";
                return generatorsWorking;
            }

            private bool IsOxygenFarmWorking() {
                bool farmsWorking = false;
                int workingFarms = 0;
                foreach (IMyOxygenFarm farm in oxygenFarmList) {
                    if (farm.IsFunctional && farm.IsWorking) {
                        workingFarms++;
                        farmsWorking = true;
                    }
                }
                WorkingOxygenFarms = $"({workingFarms} working out of {oxygenFarmList.Count} total)";
                return farmsWorking;
            }

            private double GetOxygenInTanks() {
                double oxygenFillPercentage = 0;
                int tankCount = 0;
                foreach (IMyGasTank tank in gasTankList) {
                    if (tank.DefinitionDisplayNameText.ToUpper().Contains("OXYGEN")) {
                        oxygenFillPercentage += tank.FilledRatio;
                        tankCount++;
                    }
                }
                oxygenFillPercentage = (oxygenFillPercentage / tankCount) * 100;
                myProgram.Echo($"Oxygen: {oxygenFillPercentage.ToString("0.0")}%");
                return oxygenFillPercentage;
            }

            private double GetHydrogenInTanks() {
                double hydrogenFillPercentage = 0;
                int tankCount = 0;
                foreach (IMyGasTank tank in gasTankList) {
                    if (tank.DefinitionDisplayNameText.ToUpper().Contains("HYDROGEN")) {
                        hydrogenFillPercentage += tank.FilledRatio;
                        tankCount++;
                    }
                }
                hydrogenFillPercentage = (hydrogenFillPercentage / tankCount) * 100;
                myProgram.Echo($"Hydrogen: {hydrogenFillPercentage.ToString("0.0")}%");
                return hydrogenFillPercentage;
            }

        }
    }
}

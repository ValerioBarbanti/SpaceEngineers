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

            public double GasTankPercentage { get; set; }

            List<IMyOxygenFarm> oxygenFarmList = new List<IMyOxygenFarm>();
            List<IMyGasGenerator> gasGeneratorList = new List<IMyGasGenerator>();

            List<IMyGasTank> gasTankListTemp = new List<IMyGasTank>();
            List<IMyGasTank> gasTankList = new List<IMyGasTank>();

            public ProductionController(Program program) {
                myProgram = program;
                Init();
            }

            private void Init() {
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

            public void OxygenRuntime() {
                
                GasTankPercentage = CalculateGasTanksFillPercentage();

                CalculateGasGeneratorProduction();
                ManageGasProduction();

                

            }

            private double CalculateGasTanksFillPercentage() {
                double gasTankPercentage = 0;
                foreach (IMyGasTank gasTank in gasTankList) {
                    gasTankPercentage += gasTank.FilledRatio;
                    myProgram.Echo($"Gas tank: {gasTank.Capacity}");
                }
                gasTankPercentage = gasTankPercentage / gasTankList.Count;

                return gasTankPercentage;
            }

            private void CalculateGasGeneratorProduction() {

            }

            private void ManageGasProduction() {
                if (GasTankPercentage > 0.5) {
                    foreach (IMyOxygenFarm farm in oxygenFarmList) {
                        IMyFunctionalBlock farmTest = farm as IMyFunctionalBlock;
                        farmTest.Enabled = false;
                    }
                    foreach (IMyGasGenerator generator in gasGeneratorList) {
                        generator.Enabled = false;
                    }
                } else {
                    foreach (IMyOxygenFarm farm in oxygenFarmList) {
                        IMyFunctionalBlock farmTest = farm as IMyFunctionalBlock;
                        farmTest.Enabled = true;
                    }
                    foreach (IMyGasGenerator generator in gasGeneratorList) {
                        generator.Enabled = true;
                    }
                }
            }

        }
    }
}

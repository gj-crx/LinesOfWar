using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Economics
{
    [System.Serializable]
    public class StartingProvinceBuilder
    {
        public float StartingMaxPopulation = 1000;

        public void CreateStartingAssetsInProvince(Province ProvinceToBuild)
        {
            ProvinceToBuild.Population = Random.Range(100, StartingMaxPopulation);

        }

        private void CreateStartingProductionUnits(Province ProvinceToBuild)
        {
            float MaxChance = 0;
            ProductionUnit MostPossibleProductionUnit = null;
            foreach (var PossibleProductionUnit in GameManager.types.ProductionUnitTypes)
            {
                for (byte i = 0; i < PossibleProductionUnit.PossibleLandTypesToGenerate.Count; i++)
                {
                    if (ProvinceToBuild.WaypathOfProvince.LandTypeOfPath == PossibleProductionUnit.PossibleLandTypesToGenerate[i])
                    { //if this production units is aviable in this type of land
                        //rolling the chance to have this production unit in here
                        if (Random.Range(0.0f, 1.0f) < PossibleProductionUnit.ChancesToGenerateInLandType[i])
                        { 
                            ProvinceToBuild.productionUnits.Add(PossibleProductionUnit);
                        }
                        //calculating production unit with highest chance in case we didn't get any by a random
                        else if (PossibleProductionUnit.ChancesToGenerateInLandType[i] > MaxChance)
                        { 
                            MaxChance = PossibleProductionUnit.ChancesToGenerateInLandType[(byte)ProvinceToBuild.WaypathOfProvince.LandTypeOfPath];
                            MostPossibleProductionUnit = PossibleProductionUnit;
                        }
                    }
                }
            }
            //if we actually didn't get any production units by a random we just add most possible one
            if (ProvinceToBuild.productionUnits.Count == 0) ProvinceToBuild.productionUnits.Add(MostPossibleProductionUnit);
        }
    }
}

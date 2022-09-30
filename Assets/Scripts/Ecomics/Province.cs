using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using States;
using System;

namespace Economics {
    public class Province
    {
        public Vector3Int Position;
        public float Population;
        public State state;
        public Waypath WaypathOfProvince;
        public List<ProductionUnit> productionUnits = new List<ProductionUnit>();
        public List<ConsumptionNeed> populationNeeds = new List<ConsumptionNeed>();

        public float[] ResourcesStored;
        public float[] TotalProduced;
        public float[] TotalConsumed;
        public float[] ProductionToConsumptionRate;

        public Province(Vector3Int Position, State state, Waypath WaypathOfProvince)
        {
            this.Position = Position;
            this.state = state;
            this.WaypathOfProvince = WaypathOfProvince;


            ResourcesStored = new float[GameInfo.Singleton.ResourcesInGame.Count];
            TotalProduced = new float[GameInfo.Singleton.ResourcesInGame.Count];
            TotalConsumed = new float[GameInfo.Singleton.ResourcesInGame.Count];
            ProductionToConsumptionRate = new float[GameInfo.Singleton.ResourcesInGame.Count];
        }

        public void InnerProductionCycle()
        {
            Produce();
            Consume();
            for (int i = 0; i < ProductionToConsumptionRate.Length; i++) ProductionToConsumptionRate[i] = TotalProduced[i] / TotalConsumed[i];
            Transport();
        }
        private void Produce()
        {
            foreach (ProductionUnit productionUnit in productionUnits)
            {
                TotalProduced[productionUnit.ProducedResource.ID] += productionUnit.ProductivityModifier * productionUnit.GetConsumptionNeedsModifier() * productionUnit.PopulationNeedToFunction;
            }
        }
        private void Consume()
        {
            //Calculating needs fulfillment from the last turn
            //ñalculating needs of production units
            foreach (ProductionUnit productionUnit in productionUnits)
            {
                foreach (ConsumptionNeed PUNeed in productionUnit.ConsumptionNeedsForProduction)
                {
                    float MinimumMeet = 1;
                    foreach (var ResourcesNeeded in PUNeed.ResourcesToFulfillNeeded)
                    {
                        if (ProductionToConsumptionRate[ResourcesNeeded.Item1.ID] < MinimumMeet) MinimumMeet = ProductionToConsumptionRate[ResourcesNeeded.Item1.ID];
                    }
                    PUNeed.LastProductionMeet = MinimumMeet;
                }
            }
            //calculation needs of population
            foreach (ConsumptionNeed PopNeed in populationNeeds)
            {
                float MinimumMeet = 1;
                foreach (var ResourcesNeeded in PopNeed.ResourcesToFulfillNeeded)
                {
                    if (ProductionToConsumptionRate[ResourcesNeeded.Item1.ID] < MinimumMeet) MinimumMeet = ProductionToConsumptionRate[ResourcesNeeded.Item1.ID];
                }
                PopNeed.LastProductionMeet = MinimumMeet;
            }

            //calculating actual resources needs of this turn
            foreach (ProductionUnit productionUnit in productionUnits)
            {
                foreach (ConsumptionNeed PUNeed in productionUnit.ConsumptionNeedsForProduction)
                {
                    foreach (var ResourcesNeeded in PUNeed.ResourcesToFulfillNeeded)
                    {
                        TotalConsumed[ResourcesNeeded.Item1.ID] -= ResourcesNeeded.Item2 * productionUnit.PopulationNeedToFunction / productionUnit.ConsumptionNeedsForProduction.Count;
                    }
                }
            }
        }
        private void Transport()
        { //bases on local production balance, tries to exports every resource with positive balance to nearest provinces

        }
    }
}

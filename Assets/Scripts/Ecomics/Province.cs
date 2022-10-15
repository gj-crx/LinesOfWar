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

        public float PassageCost = 1;

        public List<Building> Buildings = new List<Building>();

        public List<ProductionUnit> productionUnits = new List<ProductionUnit>();

        public List<ConsumptionNeed> PopulationNeeds = new List<ConsumptionNeed>();
        public List<ConsumptionNeed> InfrastructureNeeds = new List<ConsumptionNeed>();
        public List<ConsumptionNeed> ArmyNeeds = new List<ConsumptionNeed>();


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

        public void Produce()
        {
            TotalProduced = new float[GameInfo.Singleton.ResourcesInGame.Count];
            foreach (ProductionUnit productionUnit in productionUnits)
            {
                TotalProduced[productionUnit.ProducedResource.ID] += productionUnit.ProductivityModifier * productionUnit.ConsumptionNeedsForProduction.LastProductionMeet * productionUnit.JobsCount;
            }
        }
        public void CalculateNeedsAndProductionRate()
        {
            TotalConsumed = new float[GameInfo.Singleton.ResourcesInGame.Count];
            //consumed by production units
            foreach (ProductionUnit productionUnit in productionUnits)
            {
                foreach (var ResourcesNeeded in productionUnit.ConsumptionNeedsForProduction.ResourcesToFulfillNeeded)
                {
                    TotalConsumed[ResourcesNeeded.Item1.ID] -= ResourcesNeeded.Item2;
                }
            }
            //consumed by population
            TotalConsumed = ConsumptionNeed.CalculateConsumptionAmount(PopulationNeeds, Population, TotalConsumed);
            //consumed by Infrastructure
            TotalConsumed = ConsumptionNeed.CalculateConsumptionAmount(InfrastructureNeeds, 1, TotalConsumed);
            //consumed by army
            TotalConsumed = ConsumptionNeed.CalculateConsumptionAmount(PopulationNeeds, 1, TotalConsumed);

            ProductionToConsumptionRate = new float[TotalConsumed.Length];
            for (int i = 0; i < ProductionToConsumptionRate.Length; i++) ProductionToConsumptionRate[i] = TotalProduced[i] / TotalConsumed[i];
        }
        public void Consume()
        {
            //to be worked on

            //------- Production
            foreach (ProductionUnit productionUnit in productionUnits)
            { //getting average fulfillment of required resources
                float TotalResourcesNeed = 0;
                foreach (var ResourcesNeeded in productionUnit.ConsumptionNeedsForProduction.ResourcesToFulfillNeeded)
                { 
                    TotalResourcesNeed = ProductionToConsumptionRate[ResourcesNeeded.Item1.ID];
                }
                productionUnit.ConsumptionNeedsForProduction.LastProductionMeet = TotalResourcesNeed / productionUnit.ConsumptionNeedsForProduction.ResourcesToFulfillNeeded.Count;
            }
            //-------


            //------- Population
            foreach (ConsumptionNeed Need in PopulationNeeds)
            { //getting average fulfillment of required resources
                float TotalResourcesNeed = 0;
                foreach (var ResourcesNeeded in Need.ResourcesToFulfillNeeded)
                {
                    TotalResourcesNeed = ProductionToConsumptionRate[ResourcesNeeded.Item1.ID];
                }
                Need.LastProductionMeet = TotalResourcesNeed / Need.ResourcesToFulfillNeeded.Count;
            }
            //-------


            //------- Infrastructure
            foreach (ConsumptionNeed Need in InfrastructureNeeds)
            { //getting average fulfillment of required resources
                float TotalResourcesNeed = 0;
                foreach (var ResourcesNeeded in Need.ResourcesToFulfillNeeded)
                {
                    TotalResourcesNeed = ProductionToConsumptionRate[ResourcesNeeded.Item1.ID];
                }
                Need.LastProductionMeet = TotalResourcesNeed / Need.ResourcesToFulfillNeeded.Count;
            }
            //-------


            //------- Army
            foreach (ConsumptionNeed Need in ArmyNeeds)
            { //getting average fulfillment of required resources
                float TotalResourcesNeed = 0;
                foreach (var ResourcesNeeded in Need.ResourcesToFulfillNeeded)
                {
                    TotalResourcesNeed = ProductionToConsumptionRate[ResourcesNeeded.Item1.ID];
                }
                Need.LastProductionMeet = TotalResourcesNeed / Need.ResourcesToFulfillNeeded.Count;
            }
        }
        public void Transport()
        { //bases on local production balance, tries to exports every resource with positive balance to nearest provinces
            
        }

        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using States;
using System;

namespace Economics {
    [System.Serializable]
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

            GameManager.dataBase.ShowProvinceInInspector(this);
        }

        public void Produce()
        {
            TotalProduced = new float[GameInfo.Singleton.ResourcesInGame.Count];
            foreach (ProductionUnit productionUnit in productionUnits)
            {
                //in order to work for 100% power production unit needed to have maximum power 
                float ProductionPowerModifier = Mathf.Min(productionUnit.ConsumptionNeedForProduction.LastProductionMeet, productionUnit.CurrentWorkers / productionUnit.MaximumWorkers);
                TotalProduced[productionUnit.ProducedResource.ID] += productionUnit.ProductivityModifier * ProductionPowerModifier;
            }
        }
        public void CalculateNeedsAndProductionRate()
        {
            TotalConsumed = new float[GameInfo.Singleton.ResourcesInGame.Count];
            //consumed by production units
            foreach (ProductionUnit productionUnit in productionUnits)
            {
                foreach (var ResourcesNeeded in productionUnit.ConsumptionNeedForProduction.ResourcesToFulfillNeeded)
                {
                    TotalConsumed[ResourcesNeeded.Item1.ID] -= ResourcesNeeded.Item2 * productionUnit.CurrentWorkers;
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
            //------- Production
            foreach (ProductionUnit productionUnit in productionUnits)
            { //getting average fulfillment of required resources
                float TotalResourcesNeed = 0;
                foreach (var resource in productionUnit.ConsumptionNeedForProduction.ResourcesToFulfillNeeded)
                { 
                    TotalResourcesNeed = ProductionToConsumptionRate[resource.Item1.ID];
                }
                productionUnit.ConsumptionNeedForProduction.LastProductionMeet = TotalResourcesNeed / productionUnit.ConsumptionNeedForProduction.ResourcesToFulfillNeeded.Count;
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
            for (int i = 0; i < TotalProduced.Length; i++)
            {
                float ExcessAmount = TotalProduced[i] - TotalConsumed[i];
                if (ExcessAmount > 0)
                { //excess amounts of resources should be transfered where it's actually needed
                    int CurrentTransportationID = WaypathOfProvince.AviableTradingProvinces.Count - 1;
                    while (ExcessAmount > 0)
                    {
                        Province CurrentTargetToTransport = WaypathOfProvince.AviableTradingProvinces[CurrentTransportationID].Item1.province;
                        if (CurrentTargetToTransport.ProductionToConsumptionRate[i] < 1)
                        { //shortage detected
                            float ShortageAmount = CurrentTargetToTransport.TotalConsumed[i] - CurrentTargetToTransport.TotalProduced[i];
                            if (ShortageAmount > ExcessAmount)
                            { //shortage partially fulfilled, transportation of this resource ends here
                                CurrentTargetToTransport.TotalProduced[i] += ExcessAmount;
                                ExcessAmount = 0;
                            }
                            else
                            { //shortage fulfilled
                                CurrentTargetToTransport.TotalProduced[i] += ShortageAmount;
                                ExcessAmount -= ShortageAmount;
                            }
                            
                        }
                    }
                }
            }
        }

        
    }
}

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


        public ResourcesStorage ResourcesStored = new ResourcesStorage();
        public ResourcesStorage TotalProduced;
        public ResourcesStorage TotalConsumed;
        public ResourcesStorage ProductionToConsumptionRate;

        public Province(Vector3Int Position, State state, Waypath WaypathOfProvince)
        {
            this.Position = Position;
            this.state = state;
            this.WaypathOfProvince = WaypathOfProvince;

            GameManager.dataBase.ShowProvinceInInspector(this);
        }

        public void Produce()
        {
            TotalProduced = new ResourcesStorage();
            foreach (ProductionUnit productionUnit in productionUnits)
            {   //in order to work for 100% power production unit needed to have maximum power 
                float ProductionPowerModifier = Mathf.Min(productionUnit.ConsumptionNeedForProduction.LastProductionMeet, productionUnit.CurrentWorkers / productionUnit.MaximumWorkers);
                TotalProduced.Add(productionUnit.ProducedResource, productionUnit.ProductivityModifier * ProductionPowerModifier);
            }
        }
        public void CalculateNeedsAndProductionRate()
        {
            TotalConsumed = new ResourcesStorage();
            //consumed by production units
            foreach (ProductionUnit productionUnit in productionUnits)
            {
                foreach (var resourcesNeeded in productionUnit.ConsumptionNeedForProduction.ResourcesToFulfillNeeded)
                {
                    TotalConsumed.Add(resourcesNeeded.NeededResource, resourcesNeeded.Amount * productionUnit.CurrentWorkers);
                }
            }
            //consumed by population
            ConsumptionNeed.SubstractListOfNeeds(PopulationNeeds, Population, TotalConsumed);
            //consumed by Infrastructure
            ConsumptionNeed.SubstractListOfNeeds(InfrastructureNeeds, 1, TotalConsumed);
            //consumed by army
            ConsumptionNeed.SubstractListOfNeeds(PopulationNeeds, 1, TotalConsumed);
            //finalizing with production rate to be used in transportation
            ProductionToConsumptionRate = TotalProduced.GetProductionRate(TotalConsumed);
        }
        public void Consume()
        {
            //------- Production consumption
            foreach (ProductionUnit productionUnit in productionUnits)
            { //getting average fulfillment of required resources
                float TotalResourcesFulfillmentRate = 0;
                foreach (var resource in productionUnit.ConsumptionNeedForProduction.ResourcesToFulfillNeeded)
                { 
                    TotalResourcesFulfillmentRate += ProductionToConsumptionRate.FindStoredResource(resource.NeededResource).Amount;
                }
                productionUnit.ConsumptionNeedForProduction.LastProductionMeet = TotalResourcesFulfillmentRate / productionUnit.ConsumptionNeedForProduction.ResourcesToFulfillNeeded.Count;
            }
            //-------


            //------- Population consumption
            foreach (ConsumptionNeed Need in PopulationNeeds)
            { //getting average fulfillment of required resources
                float TotalResourcesFulfillmentRate = 0;
                foreach (var resource in Need.ResourcesToFulfillNeeded)
                {
                    TotalResourcesFulfillmentRate += ProductionToConsumptionRate.FindStoredResource(resource.NeededResource).Amount;
                }
                Need.LastProductionMeet = TotalResourcesFulfillmentRate / Need.ResourcesToFulfillNeeded.Count;
            }
            //-------


            //------- Infrastructure consumption
            foreach (ConsumptionNeed Need in InfrastructureNeeds)
            { //getting average fulfillment of required resources
                float TotalResourcesFulfillmentRate = 0;
                foreach (var resource in Need.ResourcesToFulfillNeeded)
                {
                    TotalResourcesFulfillmentRate += ProductionToConsumptionRate.FindStoredResource(resource.NeededResource).Amount;
                }
                Need.LastProductionMeet = TotalResourcesFulfillmentRate / Need.ResourcesToFulfillNeeded.Count;
            }
            //-------


            //------- Army
            foreach (ConsumptionNeed Need in ArmyNeeds)
            { //getting average fulfillment of required resources
                float TotalResourcesFulfillmentRate = 0;
                foreach (var resource in Need.ResourcesToFulfillNeeded)
                {
                    TotalResourcesFulfillmentRate += ProductionToConsumptionRate.FindStoredResource(resource.NeededResource).Amount;
                }
                Need.LastProductionMeet = TotalResourcesFulfillmentRate / Need.ResourcesToFulfillNeeded.Count;
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

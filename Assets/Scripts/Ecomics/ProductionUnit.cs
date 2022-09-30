using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Economics
{
    public class ProductionUnit
    {
        public string ProductionUnitName = "Farmlands";
        public float PopulationNeedToFunction = 1000;
        public float ProductivityModifier = 1;

        public Resource ProducedResource = null;
        public List<ConsumptionNeed> ConsumptionNeedsForProduction = null;
        

        public float GetConsumptionNeedsModifier()
        {
            float Minimum = 1;
            foreach (var Need in ConsumptionNeedsForProduction)
            {
                if (Need.LastProductionMeet < Minimum) Minimum = Need.LastProductionMeet;
            }
            return Minimum;
        }
    }
}
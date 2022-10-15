using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Economics
{
    public class ProductionUnit
    {
        public string ProductionUnitName = "Farmlands";
        public float JobsCount = 1000;
        public float ProductivityModifier = 1;

        public Resource ProducedResource = null;
        public ConsumptionNeed ConsumptionNeedsForProduction = null;
    }
}
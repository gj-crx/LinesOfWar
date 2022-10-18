using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Economics
{
    [System.Serializable]
    public class ProductionUnit
    {
        public string ProductionUnitName = "Farmlands";
        public float JobsCount = 1000;
        public float ProductivityModifier = 1;

        [HideInInspector]
        public Resource ProducedResource = null;
        public ConsumptionNeed ConsumptionNeedForProduction = null;

        [Header("--- Inspector only construction fields")]
        [SerializeField]
        private string producedResourceName = null;

        public List<Map.LandType> PossibleLandTypesToGenerate = new List<Map.LandType>();
        public float[] ChancesToGenerateInLandType = new float[6];

        public ProductionUnit()
        {
            ProducedResource = GameManager.types.GetResourceTypeByName(producedResourceName);
        }
    }
}
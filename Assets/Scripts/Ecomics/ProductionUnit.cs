using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Economics
{
    [CreateAssetMenu(fileName = "Production unit", menuName = "ScriptableObjects/Production unit", order = 3)]
    [System.Serializable]
    public class ProductionUnit : ScriptableObject
    {
        public string ProductionUnitName = "Farmlands";
        public float MaximumWorkers = 1000;
        public float CurrentWorkers = 0;
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
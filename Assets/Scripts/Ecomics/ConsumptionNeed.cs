using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Economics
{
    /// <summary>
    /// Represent a need of certain resources for population / industry / logistics / army
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "Consumption need", menuName = "ScriptableObjects/Consumption need", order = 4)]
    public class ConsumptionNeed : ScriptableObject
    { 
        public string NeedName = "Food";

        /// <summary>
        /// Indicates how much of this need was meet from 0.0 to 1.0
        /// </summary>
        [HideInInspector]
        public float LastProductionMeet = 1;

        public List<ResourceAmount> ResourcesToFulfillNeeded = new List<ResourceAmount>();



        public static void SubstractListOfNeeds(List<ConsumptionNeed> needsList, float consumersAmount, ResourcesStorage storageToSubstract)
        {
            foreach (var Need in needsList)
            {
                foreach (var substracted in Need.ResourcesToFulfillNeeded)
                {
                    storageToSubstract.Substract(substracted.NeededResource, substracted.Amount * consumersAmount);
                }
            }
        }

        public enum CategoryOfNeed : byte
        {
            PopulationNeed = 0,
            ProductionNeed = 1,
            InfrastructureNeed = 2,
            ArmyNeed = 3
        }
    }
}

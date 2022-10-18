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
    public class ConsumptionNeed
    { 
        public string NeedName = "Food";

        /// <summary>
        /// Indicates how much of this need was meet from 0.0 to 1.0
        /// </summary>
        public float LastProductionMeet = 1;

        public List<Tuple<Resource, float>> ResourcesToFulfillNeeded = new List<Tuple<Resource, float>>();

        [SerializeField]
        private List<string> resourcesToFulfillNeededInspectorBuilder = new List<string>();
        [SerializeField]
        private List<float> resourcesToFulfillAmountInspectorBuilder = new List<float>();


        public ConsumptionNeed()
        {
            for (int i = 0; i < resourcesToFulfillNeededInspectorBuilder.Count; i++)
            {
                ResourcesToFulfillNeeded.Add(new Tuple<Resource, float>(GameManager.types.GetResourceTypeByName(resourcesToFulfillNeededInspectorBuilder[i]), resourcesToFulfillAmountInspectorBuilder[i]));
            }
        }

        public static float[] CalculateConsumptionAmount(List<ConsumptionNeed> NeedsList, float UsersAmount, float[] ResourcesToDistract)
        {
            foreach (var Need in NeedsList)
            {
                foreach (var Resource in Need.ResourcesToFulfillNeeded)
                {
                    ResourcesToDistract[Resource.Item1.ID] -= Resource.Item2 * UsersAmount;
                }
            }
            return ResourcesToDistract;
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

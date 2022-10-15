using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Economics
{
    /// <summary>
    /// Represent a need of certain resources for population / industry / logistics / army
    /// </summary>
    public class ConsumptionNeed
    { 
        public string NeedName = "Food";

        /// <summary>
        /// Indicates how much of this need was meet from 0.0 to 1.0
        /// </summary>
        public float LastProductionMeet = 1;

        public List<Tuple<Resource, float>> ResourcesToFulfillNeeded = new List<Tuple<Resource, float>>();


        public static float[] CalculateConsumptionAmount(List<ConsumptionNeed> NeedsList, float UsersAmount, float[] ResourcesToDistract)
        {
            foreach (var Need in NeedsList)
            {
                foreach (var resource in Need.ResourcesToFulfillNeeded)
                {
                    ResourcesToDistract[resource.Item1.ID] -= resource.Item2 * UsersAmount;
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

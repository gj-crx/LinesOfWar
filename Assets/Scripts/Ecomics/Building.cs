using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Economics{
    [System.Serializable]
    [CreateAssetMenu(fileName = "Building", menuName = "ScriptableObjects/Building", order = 2)]
    public class Building : ScriptableObject
    {
        public string BuildingName = "Paved road";
        public ConsumptionNeed MaintainNeed = null;
        public List<Tuple<Resource, float>> ResourcesNeededToBuild = new List<Tuple<Resource, float>>();
    }
}

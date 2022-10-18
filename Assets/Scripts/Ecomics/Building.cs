using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Economics{
    [System.Serializable]
    public class Building
    {
        public string BuildingName = "Paved road";
        public ConsumptionNeed MaintainNeed = null;
        public List<Tuple<Resource, float>> ResourcesNeededToBuild = new List<Tuple<Resource, float>>();
    }
}

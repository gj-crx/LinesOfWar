using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Economics
{
    [Serializable]
    public class Resource
    {
        public int ID = 0;
        public string ResourceName = "Grain";
        public float BasicProductionModifier = 1;

        public List<Tuple<Resource, float>> ResourcesCostsForProduction = new List<Tuple<Resource, float>>();

    }
}

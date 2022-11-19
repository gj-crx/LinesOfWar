using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Economics
{
    [System.Serializable]
    public class ResourceAmount
    {
        public Resource NeededResource;
        public float Amount;

        public int ResourceID
        {
            get
            {
                return NeededResource.ID;
            }
        }

        public ResourceAmount(Resource resource, float storedAmount = 0)
        {
            NeededResource = resource;
            Amount = storedAmount;
        }
    }
}

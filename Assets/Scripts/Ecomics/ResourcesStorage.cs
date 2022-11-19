using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Economics
{
    /// <summary>
    /// Stores all kinds of resources
    /// </summary>
    public class ResourcesStorage
    {
        private List<ResourceAmount> storedResources = new List<ResourceAmount>();

        public List<ResourceAmount> StoredResources
        {
            get { return storedResources; }
        }


        public void Add(Resource resourceToAdd, float resourceAmount)
        {
            var alreadyStored = FindStoredResource(resourceToAdd);
            if (alreadyStored == null) storedResources.Add(new ResourceAmount(resourceToAdd, resourceAmount));
            else alreadyStored.Amount += resourceAmount;
        }
        /// <summary>
        /// return true if there is enough resources to substract
        /// </summary>
        public bool Substract(Resource resourceToSubstract, float resourceAmount, bool allowNegative = true)
        {
            var alreadyStored = FindStoredResource(resourceToSubstract);
            if (alreadyStored == null) 
            { //resource is not recorded yet
                if (allowNegative) storedResources.Add(new ResourceAmount(resourceToSubstract, -resourceAmount));
                else return false;
            }
            else
            { //
                if (alreadyStored.Amount >= resourceAmount) alreadyStored.Amount -= resourceAmount; 
                else
                { //we have a record of this resource but do not have enough amount of it
                    if (allowNegative == false) alreadyStored.Amount = 0; //set 0 instead of negative numbers
                    else alreadyStored.Amount -= resourceAmount;
                    return false;
                }
            }
            return true;
        }
        public ResourcesStorage GetProductionRate(ResourcesStorage compareTo)
        {
            ResourcesStorage productionRateStorage = new ResourcesStorage();
            foreach (var resource in StoredResources)
            {
                if (compareTo.ResourceStored(resource.NeededResource) == false && resource.Amount > 0)
                {
                    productionRateStorage.Add(resource.NeededResource, 1.0f);
                }
                else
                {
                    productionRateStorage.Add(resource.NeededResource, resource.Amount / compareTo.FindStoredResource(resource.NeededResource).Amount));
                }
            }
            //check for resources that does not stored in referred storage
            foreach (var resource in compareTo.StoredResources)
            {
                if (ResourceStored(resource.NeededResource)) productionRateStorage.Add(resource.NeededResource, 0);
            }

            return productionRateStorage;
        }
        public bool ResourceStored(Resource checkedResource)
        {
            foreach (var resource in StoredResources)
            {
                if (resource.NeededResource == checkedResource) return true;
            }
            return false;
        }

        public ResourceAmount FindStoredResource(Resource resourceToFind)
        {
            foreach (var stored in storedResources)
            {
                if (stored.NeededResource == resourceToFind) return stored;
            }
            return null;
        }

    }
}

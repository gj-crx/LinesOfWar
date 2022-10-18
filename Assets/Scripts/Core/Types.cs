using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Economics;

[System.Serializable]
public class Types 
{
    public List<Resource> ResourceTypes = new List<Resource>();
    public List<Building> BuildingTypes = new List<Building>();
    public List<ProductionUnit> ProductionUnitTypes = new List<ProductionUnit>();



    public Resource GetResourceTypeByName(string ResourceName)
    {
        foreach (Resource resource in ResourceTypes)
        {
            if (resource.ResourceName == ResourceName) return resource;
        }
        return null;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointPathfinding
{
    public int MaxSearchDistance = 50;
    public List<Map.LandType> CurrentEnabledLandTypesToTravel = new List<Map.LandType>();

    private Map map;
    private Stack<Waypath>[] NextPathsGroups = new Stack<Waypath>[2];
    private byte CurrentGroup = 0;
    private float CurrentDistance = 0;

    private Waypath CurrentFrom;

    private byte NextGroup
    { get
        {
            if (CurrentGroup == 0) return 1;
            else return 0;
        }
    }
    public WaypointPathfinding(Map map)
    {
        this.map = map;
        CurrentEnabledLandTypesToTravel.Add(Map.LandType.Plains);
    }

    public Tuple<List<Waypath>, float> CalculateWay(Waypath TargetPath, Waypath From)
    {
        if (CurrentEnabledLandTypesToTravel.Contains(TargetPath.LandTypeOfPath) == false || CurrentEnabledLandTypesToTravel.Contains(From.LandTypeOfPath) == false)
        {
            Debug.Log("bad province");
            return null;
        }
        map.ClearWaypointsDistances();
        bool FoundTarget = false;
        CurrentFrom = From;
        CurrentDistance = 0;
        NextPathsGroups = new Stack<Waypath>[2];
        NextPathsGroups[0] = new Stack<Waypath>();
        NextPathsGroups[1] = new Stack<Waypath>();

        NextPathsGroups[CurrentGroup].Push(From);
        while (CurrentDistance < MaxSearchDistance)
        {
            foreach (var path in NextPathsGroups[CurrentGroup])
            {
              //  Debug.Log(path.province.Position + " is " + CurrentDistance);
                path.CurrentDistance = CurrentDistance;
                path.Marked = true;
                foreach (var NeibPath in path.NeigbourPaths)
                {
                    if (NeibPath != null && NeibPath.Item1 != null && ValidWaypoint(NeibPath.Item1))
                    {
                        if (NeibPath.Item1 == TargetPath) FoundTarget = true;
                        else NextPathsGroups[NextGroup].Push(NeibPath.Item1);
                    }
                    if (FoundTarget) break;
                }
                if (FoundTarget) break;
            }
            CurrentDistance++;
            if (FoundTarget) break;
            NextPathsGroups[CurrentGroup] = new Stack<Waypath>();
            CurrentGroup = NextGroup;
        }
        if (FoundTarget)
        {
            Debug.Log("Found " + CurrentDistance);
            return new Tuple<List<Waypath>, float>(WayReconstruction(TargetPath, From), CurrentDistance);
        }
        Debug.Log("Not Found " + CurrentDistance);
        return new Tuple<List<Waypath>, float>(null, -1);
    }
    private bool ValidWaypoint(Waypath ReferencePath)
    {
        return ReferencePath.Marked == false && NextPathsGroups[NextGroup].Contains(ReferencePath) == false && NextPathsGroups[CurrentGroup].Contains(ReferencePath) == false
            && CurrentEnabledLandTypesToTravel.Contains(ReferencePath.LandTypeOfPath);
            //&& ReferencePath.province.state.TradingEnabledWith.Contains(CurrentFrom.province.state.ID);
    }
    private List<Waypath> WayReconstruction(Waypath TargetPath, Waypath From)
    {
        List<Waypath> Way = new List<Waypath>();
        Waypath CurrentPath = TargetPath;
        int Limiter = 0;
        while (CurrentPath != From && Limiter < 150)
        {
            Limiter++;
            Waypath MinimalDistancePath = null;
            float CurrentMinDistance = 10000;
            foreach (var CurrentNeib in CurrentPath.NeigbourPaths)
            {
                if (CurrentNeib != null && CurrentNeib.Item1.Marked && CurrentNeib.Item1.CurrentDistance < CurrentMinDistance)
                {
                    CurrentMinDistance = CurrentNeib.Item1.CurrentDistance;
                    MinimalDistancePath = CurrentNeib.Item1;
                }
            }
            Way.Add(CurrentPath);
            if (MinimalDistancePath == null)
            {
                Debug.LogError("Failed to find returning way in " + From.LandTypeOfPath);
                return null;
            }
            CurrentPath = MinimalDistancePath;
        }
        return Way;
    }
    
}

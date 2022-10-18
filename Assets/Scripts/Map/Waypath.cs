using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Waypath
{

    public Map.LandType LandTypeOfPath = Map.LandType.Plains;
    public readonly Economics.Province province;
    /// <summary>
    /// Contains information about neigbhour provinces and the travel cost to them
    /// </summary>
    public List<Waypath> NeigbourPaths = new List<Waypath>();
    public List<Tuple<Waypath, float>> AviableTradingProvinces = new List<Tuple<Waypath, float>>();

    public float CurrentDistance = 0;
    public bool Marked = false;

    public Waypath(Map.LandType landType, Vector3Int Position, States.State state)
    {
        LandTypeOfPath = landType;
        province = new Economics.Province(Position, state, this);
    }
    public void ChangePassingCost(float NewValue)
    {
        foreach (var Neib in NeigbourPaths)
        {
            foreach (var NeibOfNeib in Neib.NeigbourPaths)
            {
                if (NeibOfNeib == this)
                {
                    Neib.NeigbourPaths[Neib.NeigbourPaths.IndexOf(NeibOfNeib)] = NeibOfNeib;
                }
                return;
            }
        }
    }

    public void CalculateNeigbhours(DataBase.ProvincesMapHolder provincesMapHolder)
    {
        NeigbourPaths = new List<Waypath>();

        if (provincesMapHolder[province.Position.x + 0, province.Position.y + 1] != null)
            NeigbourPaths.Add(provincesMapHolder[province.Position.x + 0, province.Position.y + 1]);

        if ((float)province.Position.y / 2 == province.Position.y / 2)
        {
            if (provincesMapHolder[province.Position.x - 1, province.Position.y + 1] != null)
                NeigbourPaths.Add(provincesMapHolder[province.Position.x - 1, province.Position.y + 1]);
        }
        else
        {
            if (provincesMapHolder[province.Position.x + 1, province.Position.y + 1] != null)
                NeigbourPaths.Add(provincesMapHolder[province.Position.x + 1, province.Position.y + 1]);
        }

        if (provincesMapHolder[province.Position.x + 1, province.Position.y + 0] != null)
            NeigbourPaths.Add(provincesMapHolder[province.Position.x + 1, province.Position.y + 0]);
        if (provincesMapHolder[province.Position.x - 1, province.Position.y + 0] != null)
            NeigbourPaths.Add(provincesMapHolder[province.Position.x - 1, province.Position.y + 0]);

        if (provincesMapHolder[province.Position.x + 0, province.Position.y - 1] != null)
            NeigbourPaths.Add(provincesMapHolder[province.Position.x + 0, province.Position.y - 1]);

        if ((float)province.Position.y / 2 == province.Position.y / 2)
        {
            if (provincesMapHolder[province.Position.x - 1, province.Position.y - 1] != null)
                NeigbourPaths.Add(provincesMapHolder[province.Position.x - 1, province.Position.y - 1]);
        }
        else
        {
            if (provincesMapHolder[province.Position.x + 1, province.Position.y - 1] != null)
                NeigbourPaths.Add(provincesMapHolder[province.Position.x + 1, province.Position.y - 1]);
        }
    }
    /// <summary>
    /// Only for pathfinding thread
    /// </summary>
    public void GetTradingRoutesSortedList()
    {
       // if (System.Threading.Thread.CurrentThread != GameManager.controller.PathfindingThread) { Debug.LogError("Function called from incorrect thread"); return; }
        GameManager.map.ClearWaypointsDistances();

        AviableTradingProvinces = new List<Tuple<Waypath, float>>();
        Queue<Waypath> CheckedList = new Queue<Waypath>();
        Stack<Waypath> MarkedList = new Stack<Waypath>();
        CheckedList.Enqueue(this);
        MarkedList.Push(this);
        this.CurrentDistance = 0;
        while (CheckedList.Count > 0)
        {
            Waypath CurrentPath = CheckedList.Dequeue();
            foreach (var path in CurrentPath.NeigbourPaths)
            {
                if (MarkedList.Contains(path) == false && GameManager.Pathfinding.CurrentEnabledLandTypesToTravel.Contains(path.LandTypeOfPath)
                    && (province.state == null || province.state.TradingEnabledWith.Contains(path.province.state.ID)))
                {
                    path.CurrentDistance = CurrentPath.CurrentDistance + path.province.PassageCost;
                    AviableTradingProvinces.Add(new Tuple<Waypath, float>(path, CurrentPath.CurrentDistance + path.province.PassageCost));
                    CheckedList.Enqueue(path);
                    MarkedList.Push(path);
                }
            }
        }
        AviableTradingProvinces.Sort((x, y) => y.Item2.CompareTo(x.Item2));
    }
}

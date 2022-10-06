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
    public List<Tuple<Waypath, float>> NeigbourPaths = new List<Tuple<Waypath, float>>();
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
            foreach (var NeibOfNeib in Neib.Item1.NeigbourPaths)
            {
                if (NeibOfNeib.Item1 == this)
                {
                    Neib.Item1.NeigbourPaths[Neib.Item1.NeigbourPaths.IndexOf(NeibOfNeib)] = new Tuple<Waypath, float>(NeibOfNeib.Item1, NewValue);
                }
                return;
            }
        }
    }

    public void CalculateNeigbhours(DataBase.ProvincesMapHolder provincesMapHolder)
    {
        NeigbourPaths = new List<Tuple<Waypath, float>>();

        if (provincesMapHolder[province.Position.x + 0, province.Position.y + 1] != null)
            NeigbourPaths.Add(new Tuple<Waypath, float>(provincesMapHolder[province.Position.x + 0, province.Position.y + 1], 1));

        if ((float)province.Position.y / 2 == province.Position.y / 2)
        {
            if (provincesMapHolder[province.Position.x - 1, province.Position.y + 1] != null)
                NeigbourPaths.Add(new Tuple<Waypath, float>(provincesMapHolder[province.Position.x - 1, province.Position.y + 1], 1));
        }
        else
        {
            if (provincesMapHolder[province.Position.x + 1, province.Position.y + 1] != null)
                NeigbourPaths.Add(new Tuple<Waypath, float>(provincesMapHolder[province.Position.x + 1, province.Position.y + 1], 1));
        }

        if (provincesMapHolder[province.Position.x + 1, province.Position.y + 0] != null)
            NeigbourPaths.Add(new Tuple<Waypath, float>(provincesMapHolder[province.Position.x + 1, province.Position.y + 0], 1));
        if (provincesMapHolder[province.Position.x - 1, province.Position.y + 0] != null)
            NeigbourPaths.Add(new Tuple<Waypath, float>(provincesMapHolder[province.Position.x - 1, province.Position.y + 0], 1));

        if (provincesMapHolder[province.Position.x + 0, province.Position.y - 1] != null)
            NeigbourPaths.Add(new Tuple<Waypath, float>(provincesMapHolder[province.Position.x + 0, province.Position.y - 1], 1));

        if ((float)province.Position.y / 2 == province.Position.y / 2)
        {
            if (provincesMapHolder[province.Position.x - 1, province.Position.y - 1] != null)
                NeigbourPaths.Add(new Tuple<Waypath, float>(provincesMapHolder[province.Position.x - 1, province.Position.y - 1], 1));
        }
        else
        {
            if (provincesMapHolder[province.Position.x + 1, province.Position.y - 1] != null)
                NeigbourPaths.Add(new Tuple<Waypath, float>(provincesMapHolder[province.Position.x + 1, province.Position.y - 1], 1));
        }
    }
    public List<Waypath> GetTradingRoutesSortedList()
    {
        AviableTradingProvinces = new List<Tuple<Waypath, float>>();
        Queue<Waypath> CheckedList = new Queue<Waypath>();
        CheckedList.Enqueue(this);
        while (true)
        {
            Waypath CurrentPath = CheckedList.Dequeue();
            foreach (var path in CurrentPath.NeigbourPaths)
            {
                if (AviableTradingProvinces.Contains(path) == false && (province.state == null || province.state.TradingEnabledWith.Contains(path.Item1.province.state.ID)))
                    AviableTradingProvinces.Add(path);
            }
        }
    }
}

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
    public Tuple<Waypath, float>[] NeigbourPaths = new Tuple<Waypath, float>[6];

    public float CurrentDistance = 0;
    public bool Marked = false;

    public Waypath(Map.LandType landType, Vector3Int Position, States.State state)
    {
        LandTypeOfPath = landType;
        province = new Economics.Province(Position, state, this);
    }

    public void CalculateNeigbhours(DataBase.ProvincesMapHolder provincesMapHolder)
    {
        NeigbourPaths = new Tuple<Waypath, float>[6];

        if (provincesMapHolder[province.Position.x + 0, province.Position.y + 1] != null)
            NeigbourPaths[0] = new Tuple<Waypath, float>(provincesMapHolder[province.Position.x + 0, province.Position.y + 1], 1);

        if ((float)province.Position.y / 2 == province.Position.y / 2)
        {
            if (provincesMapHolder[province.Position.x - 1, province.Position.y + 1] != null)
                NeigbourPaths[1] = new Tuple<Waypath, float>(provincesMapHolder[province.Position.x - 1, province.Position.y + 1], 1);

        }
        else
        {
            if (provincesMapHolder[province.Position.x + 1, province.Position.y + 1] != null)
                NeigbourPaths[1] = new Tuple<Waypath, float>(provincesMapHolder[province.Position.x + 1, province.Position.y + 1], 1);
        }

        if (provincesMapHolder[province.Position.x + 1, province.Position.y + 0] != null)
            NeigbourPaths[2] = new Tuple<Waypath, float>(provincesMapHolder[province.Position.x + 1, province.Position.y + 0], 1);
        if (provincesMapHolder[province.Position.x - 1, province.Position.y + 0] != null)
            NeigbourPaths[3] = new Tuple<Waypath, float>(provincesMapHolder[province.Position.x - 1, province.Position.y + 0], 1);

        if (provincesMapHolder[province.Position.x + 0, province.Position.y - 1] != null)
            NeigbourPaths[4] = new Tuple<Waypath, float>(provincesMapHolder[province.Position.x + 0, province.Position.y - 1], 1);

        if ((float)province.Position.y / 2 == province.Position.y / 2)
        {
            if (provincesMapHolder[province.Position.x - 1, province.Position.y - 1] != null)
                NeigbourPaths[5] = new Tuple<Waypath, float>(provincesMapHolder[province.Position.x - 1, province.Position.y - 1], 1);
        }
        else
        {
            if (provincesMapHolder[province.Position.x + 1, province.Position.y - 1] != null)
                NeigbourPaths[5] = new Tuple<Waypath, float>(provincesMapHolder[province.Position.x + 1, province.Position.y - 1], 1);
        }
    }
}

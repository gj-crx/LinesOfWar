using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map
{
    public Tilemap tileMap;
    public Stack<Waypath> AllWaypaths = new Stack<Waypath>();

    [SerializeField]
    private Vector3Int tileMapBounds1 = new Vector3Int(200, 200, 0);
    [SerializeField]
    private Vector3Int tileMapBounds2 = new Vector3Int(-200, -200, 0);
    public Map(Tilemap SourceTilemap)
    {
        tileMap = SourceTilemap;
    }

    public void ClearWaypointsDistances()
    {
        foreach (var waypath in AllWaypaths)
        {
            waypath.CurrentDistance = 0;
            waypath.Marked = false;
        }
    }

    public enum LandType : byte
    {
        Plains = 0,
        Impassable = 1,
        Forest = 2,
        Mountains = 3,
        Water = 4

    }
}
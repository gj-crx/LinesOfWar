using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator
{
    public int XSize = 25;
    public int YSize = 25;

    private Map map;
    private Tilemap tilemap;

    public MapGenerator(Map map, Tilemap tilemap)
    {
        this.map = map;
        this.tilemap = tilemap;

        GenerateMap();
    }

    public void GenerateMap()
    {
        for (int y = 0; y < YSize; y++)
        {
            for (int x = 0; x < XSize; x++)
            {
                if (Random.Range(0, 2) == 0)
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), GameInfo.Singleton.TilesOfLandTypes[0]);
                    GameManager.dataBase.ProvincesMap[x, y] = new Waypath(Map.LandType.Plains, new Vector3Int(x, y, 0), null);
                    GameManager.dataBase.AllProvinces.Push(GameManager.dataBase.ProvincesMap[x, y]);
                }
                else
                {
                    tilemap.SetTile(new Vector3Int(x, y, 0), GameInfo.Singleton.TilesOfLandTypes[1]);
                    GameManager.dataBase.ProvincesMap[x, y] = new Waypath(Map.LandType.Water, new Vector3Int(x, y, 0), null);
                    GameManager.dataBase.AllProvinces.Push(GameManager.dataBase.ProvincesMap[x, y]);
                }
            }
        }
        PrecalculateWaypaths();
    }
    private void PrecalculateWaypaths()
    {
        foreach (var Waypath in GameManager.dataBase.AllProvinces)
        {
            Waypath.CalculateNeigbhours(GameManager.dataBase.ProvincesMap);
        }
    }
}

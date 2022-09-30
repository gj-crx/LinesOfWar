using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Economics;

public class GameInfo : MonoBehaviour
{
    [SerializeField]
    private int Framelimit = 60;
    [HideInInspector]
    public static GameInfo Singleton;
    public List<Resources> ResourcesInGame = new List<Resources>();
    public Tile[] TilesOfLandTypes = new Tile[6];


    public Tilemap tilemap;



    private void Awake()
    {
        Singleton = this;
        Application.targetFrameRate = Framelimit;
        GameInit();
    }


    private static void GameInit()
    {
        GameManager.map = new Map(GameInfo.Singleton.tilemap);
        GameManager.Pathfinding = new WaypointPathfinding(GameManager.map);
        GameManager.mapGenerator = new MapGenerator(GameManager.map, GameInfo.Singleton.tilemap);
    }

}

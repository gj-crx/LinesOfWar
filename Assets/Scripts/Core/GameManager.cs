using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;
using Economics;

public static class GameManager
{
    public static DataBase dataBase;
    public static GameController controller;
    public static WaypointPathfinding Pathfinding;
    public static Map map;
    public static MapGenerator mapGenerator;


    private static bool IsAlreadyInited = false;



    public static void GameInit()
    {
        if (IsAlreadyInited) return;
        else IsAlreadyInited = true;

        dataBase = new DataBase();
        map = new Map(GameInfo.Singleton.tilemap);
        Pathfinding = new WaypointPathfinding(map);
        mapGenerator = new MapGenerator(map, GameInfo.Singleton.tilemap);

    }
}

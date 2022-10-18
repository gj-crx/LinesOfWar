using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore;
using Economics;

public static class GameManager
{
    public static bool GameStillRunning = true;
    public static bool GameIsPaused = false;

    public static DataBase dataBase;
    public static Types types;
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
        types = new Types();
        map = new Map(GameInfo.Singleton.tilemap);
        Pathfinding = new WaypointPathfinding(map);
        mapGenerator = new MapGenerator(map, GameInfo.Singleton.tilemap);
        controller = new GameController();


        GameInfo.Singleton.dataBaseToTest = dataBase;
        GameInfo.Singleton.GameTypesPreset = types;
        controller.StartGameControlling();

    }
}

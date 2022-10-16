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
    public List<Resource> ResourcesInGame = new List<Resource>();
    public Tile[] TilesOfLandTypes = new Tile[6];

    public DataBase dataBaseToTest;


    public Tilemap tilemap;



    private void Awake()
    {
        Singleton = this;
        Application.targetFrameRate = Framelimit;
        GameManager.GameInit();
    }


    

}

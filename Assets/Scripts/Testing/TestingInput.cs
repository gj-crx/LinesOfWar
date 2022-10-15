using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingInput : MonoBehaviour
{
    public GameObject Marker;

    public Waypath lastpath;
    public Waypath target;
    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector3Int TargetPos = new Vector3Int((int)Camera.main.ScreenToWorldPoint(Input.mousePosition).x, (int)Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
            Instantiate(Marker, TargetPos, Quaternion.identity);
            TargetPos = GameInfo.Singleton.tilemap.WorldToCell(TargetPos);
            lastpath = GameManager.dataBase.ProvincesMap[TargetPos.x, TargetPos.y];
            UI.UIManager.Singleton.provinceReviewer.ShowProvinceInfo(GameManager.dataBase.ProvincesMap[TargetPos.x, TargetPos.y].province);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            Vector3Int TargetPos = new Vector3Int((int)Camera.main.ScreenToWorldPoint(Input.mousePosition).x, (int)Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
            Instantiate(Marker, TargetPos, Quaternion.identity);
            TargetPos = GameInfo.Singleton.tilemap.WorldToCell(TargetPos);
            target = GameManager.dataBase.ProvincesMap[TargetPos.x, TargetPos.y];

            GameManager.Pathfinding.CalculateWay(target, lastpath);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3Int TargetPos = new Vector3Int((int)Camera.main.ScreenToWorldPoint(Input.mousePosition).x, (int)Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
            Instantiate(Marker, TargetPos, Quaternion.identity);
            TargetPos = GameInfo.Singleton.tilemap.WorldToCell(TargetPos);
            lastpath = GameManager.dataBase.ProvincesMap[TargetPos.x, TargetPos.y];
            foreach (var v in lastpath.NeigbourPaths)
            {
                Debug.Log("Selected prov " + v.province.Position + GameManager.dataBase.ProvincesMap[TargetPos.x, TargetPos.y].LandTypeOfPath);
            }
        }
    }
}

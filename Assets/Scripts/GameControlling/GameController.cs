using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class GameController
{


    public void RecalculateAllTradeRoutes()
    {
        foreach (var province in GameManager.dataBase.AllProvinces)
        {
            province.GetTradingRoutesSortedList();
        }
    }
}

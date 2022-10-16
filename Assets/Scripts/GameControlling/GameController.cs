using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

namespace GameCore
{
    public class GameController
    {
        public Thread MainThread;
        public Thread PathfindingThread;
        public Thread EconomyThread;

        public void StartGameControlling()
        {
            RecalculateAllTradeRoutes();
        }


        public void RecalculateAllTradeRoutes()
        {
            foreach (var province in GameManager.dataBase.AllProvinces)
            {
                province.GetTradingRoutesSortedList();
            }
            Debug.Log("recalculation of trading ways complete");
        }

        public void ControlEconomy()
        {
            //at first, every province performs a production based on needs met of last turn
            //if this is very first turn so every need initially sets to 100%
            //then required resources to fulfill need are being calculated and the ratio of production to consumption aswell
            //goods are began to be transported based on this ratio 
            //if province has atleast 1 goods type being excessively produced then the trading search is begun
            //province checks other nearest province with shortage of atleast 1 goods type that being exported and it's simply being transfered
            //transportation process of province ends when all excess goods were transported or when list of possible provinces to travel ends
            //if not all goods were transported then they can be stored to the future transportation or consumption, but currently they just disappear
            foreach (var province in GameManager.dataBase.AllProvinces) province.province.Produce();
            foreach (var province in GameManager.dataBase.AllProvinces) province.province.Consume();
            foreach (var province in GameManager.dataBase.AllProvinces) province.province.Transport();
        }

    }
}

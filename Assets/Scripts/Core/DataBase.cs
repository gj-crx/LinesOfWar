using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Contains all dynamic game data
/// </summary>
public class DataBase
{
    public ProvincesMapHolder ProvincesMap = new ProvincesMapHolder();









    public class ProvincesMapHolder
    {
        Dictionary<Tuple<int, int>, Waypath> DistanceMapDictionary = new Dictionary<Tuple<int, int>, Waypath>();


        public Waypath this[int x, int y]
        {
            get
            {
                var t = Tuple.Create(x, y);
                if (DistanceMapDictionary.ContainsKey(t)) return DistanceMapDictionary[t];
                return null;
            }
            set
            {
                var t = Tuple.Create(x, y);
                DistanceMapDictionary[t] = value;
            }
        }
    }
}

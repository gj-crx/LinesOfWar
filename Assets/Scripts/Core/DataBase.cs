using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Economics;


/// <summary>
/// Contains all dynamic game data
/// </summary>
[Serializable]
public class DataBase
{
    public ProvincesMapHolder ProvincesMap = new ProvincesMapHolder();
    public Stack<Waypath> AllPaths = new Stack<Waypath>();

    [SerializeField]
    private List<Province> AllProvinces = new List<Province>();






    public void ShowProvinceInInspector(Province province)
    {
        AllProvinces.Add(province);
    }



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

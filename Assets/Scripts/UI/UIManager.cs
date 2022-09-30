using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [HideInInspector]
        public static UIManager Singleton;
        public UIPrefabs uiPrefabs;
        public ProvinceReviewer provinceReviewer;

        private void Awake()
        {
            Singleton = this;
        }


        public static string GetBasePartOfText(string SourceText)
        {
            return SourceText.Substring(0, SourceText.IndexOf(":") + 2);
        }
    }

    [System.Serializable]
    public class UIPrefabs
    {
        public Sprite[] LandTypesIcons = new Sprite[6];

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Economics;

namespace UI
{
    [System.Serializable]
    public class ProvinceReviewer
    {
        public Image LandTypeIconOfProvince;
        public Text text_LandTypeName;
        public Text text_StateOwnerOfProvince;
        public Text text_Population;
        public Text text_ArmyStrength;



        public void ShowProvinceInfo(Province ProvinceToShow)
        {
            text_LandTypeName.text = UIManager.GetBasePartOfText(text_LandTypeName.text) + Localization.Singleton.vocabulary.LandTypesNames[(int)ProvinceToShow.WaypathOfProvince.LandTypeOfPath];
            LandTypeIconOfProvince.sprite = UIManager.Singleton.uiPrefabs.LandTypesIcons[(int)ProvinceToShow.WaypathOfProvince.LandTypeOfPath];

            if (ProvinceToShow.state != null)
                text_StateOwnerOfProvince.text = UIManager.GetBasePartOfText(text_StateOwnerOfProvince.text) + ProvinceToShow.state.StateName;
            else text_StateOwnerOfProvince.text = UIManager.GetBasePartOfText(text_StateOwnerOfProvince.text) + "no state";

            text_Population.text = UIManager.GetBasePartOfText(text_Population.text) + ProvinceToShow.Population.ToString();
            text_ArmyStrength.text = UIManager.GetBasePartOfText(text_ArmyStrength.text) + "?";
        }
    }
}

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

        public Image[] NeedsFillers = new Image[4];



        public void ShowProvinceInfo(Province ProvinceToShow)
        {
            text_LandTypeName.text = UIManager.GetBasePartOfText(text_LandTypeName.text) + Localization.Singleton.vocabulary.LandTypesNames[(int)ProvinceToShow.WaypathOfProvince.LandTypeOfPath];
            LandTypeIconOfProvince.sprite = UIManager.Singleton.uiPrefabs.LandTypesIcons[(int)ProvinceToShow.WaypathOfProvince.LandTypeOfPath];

            if (ProvinceToShow.state != null)
                text_StateOwnerOfProvince.text = UIManager.GetBasePartOfText(text_StateOwnerOfProvince.text) + ProvinceToShow.state.StateName;
            else text_StateOwnerOfProvince.text = UIManager.GetBasePartOfText(text_StateOwnerOfProvince.text) + "no state";

            text_Population.text = UIManager.GetBasePartOfText(text_Population.text) + ProvinceToShow.Population.ToString();
            text_ArmyStrength.text = UIManager.GetBasePartOfText(text_ArmyStrength.text) + "?";

            ShowEveryNeed(ProvinceToShow);
        }


        private void ShowEveryNeed(Province ProvinceToShow)
        {
            //population needs
            float CurrentNeedsSum = 0;
            int CurrentNeedsCount = 0;
            foreach (var Need in ProvinceToShow.PopulationNeeds)
            {
                CurrentNeedsCount++;
                CurrentNeedsSum += Need.LastProductionMeet;
            }
            NeedsFillers[0].fillAmount = CurrentNeedsSum / CurrentNeedsCount;

            //production needs
            CurrentNeedsSum = 0;
            CurrentNeedsCount = 0;
            foreach (var ProductionUnit in ProvinceToShow.productionUnits)
            {
                CurrentNeedsCount++;
                CurrentNeedsSum += ProductionUnit.ConsumptionNeedForProduction.LastProductionMeet;
            }
            NeedsFillers[1].fillAmount = CurrentNeedsSum / CurrentNeedsCount;

            //infrastructure needs
            CurrentNeedsSum = 0;
            CurrentNeedsCount = 0;
            foreach (var building in ProvinceToShow.Buildings)
            {
                CurrentNeedsCount++;
                CurrentNeedsSum += building.MaintainNeed.LastProductionMeet;
            }
            NeedsFillers[2].fillAmount = CurrentNeedsSum / CurrentNeedsCount;

            //army needs
            NeedsFillers[3].fillAmount = 1;
        }
    }
}

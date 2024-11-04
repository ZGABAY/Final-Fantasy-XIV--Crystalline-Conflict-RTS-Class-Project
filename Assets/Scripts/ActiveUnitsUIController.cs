using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveUnitsUIController : MonoBehaviour
{
    public RectTransform parentPanelTransform;
    public GameObject unitButtonPrefab;

    public Dictionary<InfantryType, GameObject> activeUnits = new Dictionary<InfantryType, GameObject>();

    private void OnEnable()
    {
        UnitButtonController.OnCreateUnit += UnitButtonController_OnCreateUnit;

        Structure.OnUpdateUnitsProgress += Structure_OnUpdateUnitsProgress;
    }

    private void OnDisable()
    {
        UnitButtonController.OnCreateUnit -= UnitButtonController_OnCreateUnit;

        Structure.OnUpdateUnitsProgress -= Structure_OnUpdateUnitsProgress;
    }

    private void UnitButtonController_OnCreateUnit(ScriptableObject unit)
    {
        // check scriptable object
        UnitScriptableObject uso = unit as UnitScriptableObject;

        // check dictionary to see if there is a key already in there
        if (!activeUnits.ContainsKey(uso.data.infantryType))
        {
            var goUI = GameObject.Instantiate(unitButtonPrefab, parentPanelTransform);
            var unitButtonController = goUI.GetComponent<UnitButtonController>();
            unitButtonController.tmpUnitCaption.text = uso.name;
            unitButtonController.tmpUnitCount.text = 1.ToString();
            unitButtonController.tmpUnitProductionProgress.text = String.Format("{0:F2}", "");

            // if no key, make one
            activeUnits.Add(uso.data.infantryType, goUI);
        }
        else
        {
            var goUI = (GameObject)activeUnits[uso.data.infantryType];

            goUI.GetComponent<UnitButtonController>().tmpUnitCount.text = $"{Convert.ToInt32(goUI.GetComponent<UnitButtonController>().tmpUnitCount.text) + 1}";
        }
    }

    private void Structure_OnUpdateUnitsProgress(UnitScriptableObject units, float progress)
    {
        // check dictionary to see if there is a key already in there
        if (activeUnits.ContainsKey(units.data.infantryType))
        {
            var goUI = (GameObject)activeUnits[units.data.infantryType];

            var ubc = goUI.GetComponent<UnitButtonController>();
            ubc.tmpUnitProductionProgress.text = String.Format("{0:F2}", progress);

            ubc.imgProgress.fillAmount = progress / 100f;

            if (progress >= 100)
            {
                ubc.tmpUnitProductionProgress.text = "";
                ubc.imgProgress.fillAmount = 0;
            }
        }
    }

}
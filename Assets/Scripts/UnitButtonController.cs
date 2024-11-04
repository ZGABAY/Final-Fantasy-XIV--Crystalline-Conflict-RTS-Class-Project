using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitButtonController : MonoBehaviour
{
    public delegate void CreateUnit(ScriptableObject unit);
    public static event CreateUnit OnCreateUnit;

    public Button buttonComponent;
    public TMP_Text tmpUnitCaption;
    public TMP_Text tmpUnitCount;
    public TMP_Text tmpUnitProductionProgress;
    public Image imgProgress;

    public ScriptableObject unitData;

    // Start is called before the first frame update
    void Start()
    {
        if (buttonComponent != null)
        {
            buttonComponent.onClick.AddListener(() =>
            {
                //Debug.Log($"You clicked: {tmpUnitCaption.text}");
                OnCreateUnit?.Invoke(unitData);
            });
        }
    }
}
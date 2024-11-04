using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class UnitUIController : MonoBehaviour
{
   public RectTransform parentPanelTransform;
   public GameObject unitButtonPrefab;
   public List<ScriptableObject> unitData = new List<ScriptableObject>();
 
   private void OnEnable()
   {
      Structure.OnUpdateUnitsView += Structure_OnUpdateUnitsView;
   }
 
 
   private void OnDisable()
   {
      Structure.OnUpdateUnitsView -= Structure_OnUpdateUnitsView;
   }
 
 
   private void Structure_OnUpdateUnitsView(StructureScriptableObject structure)
   {
      ClearUnitView();
 
      foreach (UnitScriptableObject u in structure.structureUnits)
      {
         var goUI = GameObject.Instantiate(unitButtonPrefab, parentPanelTransform);
 
         var unitButtonController = goUI.GetComponent<UnitButtonController>();
         unitButtonController.tmpUnitCaption.text = u.UnitButtonCaption;
         unitButtonController.unitData = u;
 
         Debug.Log($"{u.name}");
      }
   }
 
   private void ClearUnitView()
   {
      for(int i=0; i<parentPanelTransform.childCount; i++)
         Destroy(parentPanelTransform.GetChild(i).gameObject);
   }
}
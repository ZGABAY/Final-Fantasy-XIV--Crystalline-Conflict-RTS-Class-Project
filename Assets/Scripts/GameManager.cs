using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : RTSSingleton<GameManager>
{
   public GameObject soldierPrefab;
   public GameObject marinePrefab;
   public GameObject sniperPrefab;
   public GameObject shockTrooperPrefab;
   public Transform unitSpawnPoint;
   int unitCount = 0;
   public static int gil = 1000;
   public static int crystals = 10;
   public static int ore = 0;
   public static int wood = 0;
   public static int food = 10;


   private void OnEnable()
   {
      UIManager.OnCreateUnit += UIManager_OnCreateUnit;
      UIManager.OnCreateUnitSO += UIManager_OnCreateUnitSO;
   }

   private void OnDisable()
   {
      UIManager.OnCreateUnit -= UIManager_OnCreateUnit;
      UIManager.OnCreateUnitSO -= UIManager_OnCreateUnitSO;
   }

   private void UIManager_OnCreateUnit(Unit unit)
   {
      Debug.Log(unit.Type + " " + unit.Name);
      GameObject unitPrefab = null;

      if (unit.Type == UnitType.Infantry)
      {
         var infantryUnit = unit as Infantry;
         switch (infantryUnit.InfantryType)
         {
            case InfantryType.Soldier:
               unitPrefab = soldierPrefab;
               break;
            case InfantryType.Marine:
               unitPrefab = marinePrefab;
               break;
            case InfantryType.Sniper:
               unitPrefab = sniperPrefab;
               break;
            case InfantryType.ShockTrooper:
               unitPrefab = shockTrooperPrefab;
               break;
         }
         if (unitPrefab != null)
         {
            // instantiate the unit here ...
            var newUnit = GameObject.Instantiate(unitPrefab, unitSpawnPoint.position, Quaternion.identity);
            newUnit.name = $"newUnit{unitCount}";
            unitCount++;

            // need to re-calculate the nav mesh
            var newUnitAgent = newUnit.transform.GetComponent<NavMeshAgent>();
            newUnitAgent.enabled = false;

            NavMeshHit meshHit;
            bool hit = NavMesh.SamplePosition(unitSpawnPoint.position, out meshHit, 100, 1);
            if (hit)
            {
               newUnitAgent.Warp(meshHit.position);
               newUnitAgent.enabled = true;
            }

            // Deselect the unit after creating it
            var newUnitController = newUnit.transform.GetComponent<UnitController>();
            newUnitController.IsSelected = false;
         }
      }
   }
   private void UIManager_OnCreateUnitSO(UnitScriptableObject unitSO)
   {
      throw new System.NotImplementedException();
   }
   public void UpdateGil(int amount)
   {
      gil += amount;
   }

   public void UpdateCrystals(int amount)
   {
      crystals += amount;
   }

   public void UpdateWood(int amount)
   {
      wood += amount;
   }

   public void UpdateOre(int amount)
   {
      ore += amount;
   }

   public void UpdateFood(int amount)
   {
      food += amount;
   }

   // Start is called before the first frame update
   void Start()
   {


   }

   // Update is called once per frame
   void Update()
   {

   }
}
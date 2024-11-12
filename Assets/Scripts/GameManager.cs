using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : RTSSingleton<GameManager>
{
   public static event System.Action<int> OnUpdateEnemyCount; // Define update enemy count event
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

        switch (unit.Type)
        {
            case UnitType.Paladin:
                unitPrefab = paladinPrefab;
                break;
            case UnitType.BlackMage:
                unitPrefab = blackMagePrefab;
                break;
            case UnitType.Scholar:
                unitPrefab = scholarPrefab;
                break;
            case UnitType.Dancer:
                unitPrefab = dancerPrefab;
                break;
            case UnitType.Culinarian:
                unitPrefab = culinarianPrefab;
                break;
            case UnitType.Carpenter:
                unitPrefab = carpenterPrefab;
                break;
            case UnitType.Botanist:
                unitPrefab = botanistPrefab;
                break;
            case UnitType.Miner:
                unitPrefab = minerPrefab;
                break;
            case UnitType.Monk:
                unitPrefab = monkPrefab;
                break;
            case UnitType.Soldier:
                unitPrefab = soldierPrefab;
                break;
            case UnitType.Alchemist:
                unitPrefab = alchemistPrefab;
                break;
            case UnitType.Blacksmith:
                unitPrefab = blacksmithPrefab;
                break;
        }

        if (unitPrefab != null)
        {
            // instantiate the unit here ...
            var newUnit = GameObject.Instantiate(unitPrefab, unitSpawnPoint.position, Quaternion.identity);
            newUnit.name = $"newUnit{unitCount}";
            unitCount++;

            // Trigger the OnUpdateEnemyCount event
            OnUpdateEnemyCount?.Invoke(unitCount);

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
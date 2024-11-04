using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
   public delegate void CreateUnit(Unit unit);
   private int unitSubType;
   public static event CreateUnit OnCreateUnit;
   public Button CreateSoldierButton;
   public Button CreateMarineButton;
   public Button CreateSniperButton;
   public Button CreateShockTrooperButton;

   public delegate void CreateUnitSO(UnitScriptableObject unit);
   public static event CreateUnitSO OnCreateUnitSO;
   public Button CreateUnitSOButton;

   public Button GilButton;
   public Button CrystalButton;
   public Button OreButton;
   public Button WoodButton;
   public Button FoodButton;

   public Button Structure1Button;
   public Button Structure2Button;
   public Button Structure3Button;
   public Button Structure4Button;
   public Button Structure5Button;
   public Button Structure6Button;

   public Button ExitButton;

   // Start is called before the first frame update
   void Start()
   {
      CreateSoldierButton.onClick.AddListener(() => CreateInfantry(0));
      CreateMarineButton.onClick.AddListener(() => CreateInfantry(1));
      CreateSniperButton.onClick.AddListener(() => CreateInfantry(2));
      CreateShockTrooperButton.onClick.AddListener(() => CreateInfantry(3));
      CreateUnitSOButton.onClick.AddListener(CreateUnitScriptableObject);
      ExitButton.onClick.AddListener(ExitGame);
      Structure1Button.onClick.AddListener(() => CreateStructureClicked(Structure1Button));
      Structure2Button.onClick.AddListener(() => CreateStructureClicked(Structure2Button));
      Structure3Button.onClick.AddListener(() => CreateStructureClicked(Structure3Button));
      Structure4Button.onClick.AddListener(() => CreateStructureClicked(Structure4Button));
      Structure5Button.onClick.AddListener(() => CreateStructureClicked(Structure5Button));
      Structure6Button.onClick.AddListener(() => CreateStructureClicked(Structure6Button));
   }

   // Update is called once per frame
   void Update()
   {
      // Update the UI with the current resource amounts
      GilButton.GetComponentInChildren<Text>().text = GameManager.gil.ToString();
      CrystalButton.GetComponentInChildren<Text>().text = GameManager.crystals.ToString();
      OreButton.GetComponentInChildren<Text>().text = GameManager.ore.ToString();
      WoodButton.GetComponentInChildren<Text>().text = GameManager.wood.ToString();
      FoodButton.GetComponentInChildren<Text>().text = GameManager.food.ToString();
   }

   public void ExitGame()
   {
      // might want to perform some clean-up before exiting
      Application.Quit();
   }

   public void CreateInfantry(int unitSubType)
   {
      GameObject unitGameObject = new GameObject("Unit");
      Unit unit = unitGameObject.AddComponent<Infantry>();
      Destroy(unitGameObject);
      unit.Type = UnitType.Infantry;
      switch(unitSubType)
      {
         case 0:
            ((Infantry)unit).InfantryType = InfantryType.Soldier;
            break;
         case 1:
            ((Infantry)unit).InfantryType = InfantryType.Marine;
            break;
         case 2:
            ((Infantry)unit).InfantryType = InfantryType.Sniper;
            break;
         case 3:
            ((Infantry)unit).InfantryType = InfantryType.ShockTrooper;
            break;
      }
      OnCreateUnit?.Invoke(unit);
   }
      public void CreateVehicle()
   {
      throw new System.NotImplementedException();
   }
      public void CreateAircraft()
   {
      throw new System.NotImplementedException();
   }
      public void CreateSpecial()
   {
      throw new System.NotImplementedException();
   }

   public void CreateUnitScriptableObject()
   {
      throw new System.NotImplementedException();
   }

   public void CreateStructureClicked(Button button)
   {
      Debug.Log("Creating structure: " + button.name);
   }
}
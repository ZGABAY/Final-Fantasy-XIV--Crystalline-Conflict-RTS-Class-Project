using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
   public TMP_Text tmpPlayerBullets;
   public TMP_Text tmpPlayerResources;

   public TMP_Text tmpNumberOfEnemyUnits;
   public TMP_Text tmpEnemyResources;

   private void OnEnable()
   {
      PlayerAttackController.OnUpdatePlayerAmo += PlayerAttackController_OnUpdatePlayerAmo;
      StorageController.OnUpdatePlayerResources += StorageController_OnUpdatePlayerResources;

      GameManager.OnUpdateEnemyCount += GameManager_OnUpdateEnemyCount;
      StorageController.OnUpdateEnemyResources += StorageController_OnUpdateEnemyResources;
   }

   private void OnDisable()
   {
      PlayerAttackController.OnUpdatePlayerAmo -= PlayerAttackController_OnUpdatePlayerAmo;
      StorageController.OnUpdatePlayerResources -= StorageController_OnUpdatePlayerResources;

      GameManager.OnUpdateEnemyCount -= GameManager_OnUpdateEnemyCount;
      StorageController.OnUpdateEnemyResources -= StorageController_OnUpdateEnemyResources;
   }

   private void PlayerAttackController_OnUpdatePlayerAmo(int value)
   {
      tmpPlayerBullets.text = value.ToString();
   }

   private void StorageController_OnUpdatePlayerResources(int value)
   {
      tmpPlayerResources.text = value.ToString();
   }

   private void GameManager_OnUpdateEnemyCount(int value)
   {
      tmpNumberOfEnemyUnits.text = value.ToString();
   }

   private void StorageController_OnUpdateEnemyResources(int value)
   {
      tmpEnemyResources.text = value.ToString();
   }


   // Start is called before the first frame update
   void Start()
   {
      tmpPlayerResources.text = "0";
      tmpEnemyResources.text = "0";
   }

   // Update is called once per frame
   void Update()
   {

   }
}

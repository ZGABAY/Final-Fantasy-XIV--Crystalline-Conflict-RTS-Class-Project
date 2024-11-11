using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class demoGameManager : MonoBehaviour
{
   public delegate void UpdateEnemyCount(int value);
   public static event UpdateEnemyCount OnUpdateEnemyCount;

   public int numberOfEnemies;

   private void OnEnable()
   {
      EnemyController.OnEnemyDied += EnemyController_OnEnemyDied;
   }

   private void OnDisable()
   {
      EnemyController.OnEnemyDied -= EnemyController_OnEnemyDied;
   }

   private void EnemyController_OnEnemyDied(Transform enemyT, int id)
   {
      numberOfEnemies -= 1;
      if(numberOfEnemies>=0)
      {
         // update the UI 
         OnUpdateEnemyCount?.Invoke(numberOfEnemies);

         // destroy the enemy object
         Destroy(enemyT.gameObject);
      }
   }


   // Start is called before the first frame update
   void Start()
   {
      var eObj = GameObject.FindGameObjectsWithTag("Enemy").ToList();
      numberOfEnemies = eObj.Count;

      OnUpdateEnemyCount?.Invoke(numberOfEnemies);
   }

   // Update is called once per frame
   void Update()
   {

   }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySenseController : MonoBehaviour
{
   public EnemyController myController;

   // Start is called before the first frame update
   void Start()
   {

   }

   // Update is called once per frame
   void Update()
   {

   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.transform.tag.Equals("Player"))
      {
         Debug.LogWarning($"I {myController.id} have detected player {other.name} located at {other.transform.position}");

         myController.playerObject = other.gameObject;
         myController.playerInSight = true;
      }
   }
}

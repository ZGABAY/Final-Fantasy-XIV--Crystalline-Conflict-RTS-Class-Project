using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
   public RoomController roomController;

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
         // do something ...
         roomController.OpenDoor(true);
      }
   }

   private void OnTriggerExit(Collider other)
   {
      if (other.transform.tag.Equals("Player"))
      {
         // do something ...
         roomController.OpenDoor(false);
      }
   }
}

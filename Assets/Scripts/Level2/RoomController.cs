using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
   public int id;
   public bool doorOpen = false;

   public Transform roomDoor;
   public Transform roomLight;

   public Color roomColor;

   public Animator roomAnimator;

   // Start is called before the first frame update
   void Start()
   {
      roomAnimator = GetComponent<Animator>();

      roomLight.GetComponent<Light>().color = roomColor;
   }

   // Update is called once per frame
   void Update()
   {

   }

   public void OpenDoor(bool value)
   {
      doorOpen = value;
      roomAnimator.SetBool("open", doorOpen);
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.transform.tag.Equals("Player"))
      {
         if(other.transform.GetComponent<PlayerRoomController>().roomId == id)
         {
            // open the door
            OpenDoor(true);

            // update internal data
            other.transform.GetComponent<PlayerRoomController>().rooms[id].visited = true;
         }
      }
   }

   private void OnTriggerExit(Collider other)
   {
      if (other.transform.tag.Equals("Player"))
      {
         // do something ...
         OpenDoor(false);
      }
   }
}

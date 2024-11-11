using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSelectorController : MonoBehaviour
{
   public List<Transform> nodeList = new List<Transform>();

   // Start is called before the first frame update
   void Start()
   {

   }

   // Update is called once per frame
   void Update()
   {
      transform.Rotate(Vector3.up, 1);
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.transform.tag.Equals("Player"))
      {
         var playerRoom = other.transform.GetComponent<PlayerRoomController>();

         // assign a new objective
         int randomRoom = Random.Range(0, 3);
         
         playerRoom.roomId = randomRoom;

         Color32 color;
         switch (randomRoom)
         {
            case 0:
               color = new Color(1, 0, 0);
               break;
            case 1:
               color = new Color(0, 1, 0);
               break;
            case 2:
               color = new Color(0, 0, 1);
               break;
            default:
               color = new Color(1, 1, 1);
               break;
         }

         foreach (var n in nodeList)
         {
            n.GetComponent<Renderer>().material.color = color;
         }
      }
   }
}

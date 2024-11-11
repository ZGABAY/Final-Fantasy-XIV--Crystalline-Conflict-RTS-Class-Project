using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoomController : MonoBehaviour
{
   [Serializable]
   public class RoomData
   {
      public int id;
      public bool visited;
   }

   public int roomId;
   public List<RoomData> rooms = new List<RoomData>();

   public float remainingTime = 33f;

   public bool keepPlaying=true;

   // Start is called before the first frame update
   void Start()
   {
   }

   // Update is called once per frame
   void Update()
   {
      if(keepPlaying)
      {
         if (remainingTime > 0)
         {
            remainingTime -= Time.deltaTime;
         }
         else
         {
            remainingTime = 0;

            keepPlaying = false;
         }

         // display in minutes, seconds, milliseconds
         DisplayTime(remainingTime);
      }
   }

   void CheckObjective()
   {
      int count = 0;
      foreach (var room in rooms)
      {
         if(room.visited)
            count++;
      }

      if (count == 3)
      {
         Debug.Log($"You Made It!");
         keepPlaying = false;

         Time.timeScale = 0;
      }
      else if (remainingTime==0)
      {
         Debug.Log($"Sorry you lost!");

         Time.timeScale = 0;
      }
   }

   void DisplayTime(float timeToDisplay)
   {
      float minutes = Mathf.FloorToInt(timeToDisplay / 60);
      float seconds = Mathf.FloorToInt(timeToDisplay % 60);
      float milliSeconds = (timeToDisplay % 1) * 1000;

      Debug.Log(string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliSeconds));

      CheckObjective();
   }
}

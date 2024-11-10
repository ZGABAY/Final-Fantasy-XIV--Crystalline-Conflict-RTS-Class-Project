using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
   public float playerSpeed = 1.5f;

   public bool isEnemy;

   Transform playerTransform;

   // Start is called before the first frame update
   void Start()
   {

   }

   // Update is called once per frame
   void Update()
   {
      if(!isEnemy)
      {
         if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.forward * Time.deltaTime * playerSpeed);
         if (Input.GetKey(KeyCode.S))
            transform.Translate(-Vector3.forward * Time.deltaTime * playerSpeed);

         if (Input.GetKey(KeyCode.D))
            transform.Rotate(Vector3.up, 1);
         if (Input.GetKey(KeyCode.A))
            transform.Rotate(Vector3.up, -1);
      }
      else
      {
         // perform basic navigation logic for the enemy npc
      }

   }
}

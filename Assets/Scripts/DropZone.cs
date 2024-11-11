using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropZone : MonoBehaviour
{
   public Transform indicatorTransform;

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
         var playerBag = other.transform.GetComponent<PlayerBag>();

         if (playerBag.bag.Count > 3)
         {
            Vector3 center = transform.position;
            
            int index = 1;
            foreach (var item in playerBag.bag)
            {
               Vector3 pos = CirclePath(center, 1.0f, index);
               index++;
               Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - pos);

               GameObject tmp = GameObject.CreatePrimitive(PrimitiveType.Cube);
               tmp.transform.position = pos;
               tmp.transform.rotation = rot;
               
               tmp.transform.localScale = new Vector3(item.size, item.size, item.size);
               tmp.GetComponent<Renderer>().material = item.material;
            }

            indicatorTransform.GetComponent<Renderer>().material.color = Color.green;
         }
         else
         {
            Debug.Log("Player does not have all items!");
         }
      }
   }

   Vector3 CirclePath(Vector3 center, float radius, int id)
   {
      float ang = 90 * id;
      Vector3 pos;
      pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
      pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
      pos.y = center.y + 1;
      return pos;
   }
}






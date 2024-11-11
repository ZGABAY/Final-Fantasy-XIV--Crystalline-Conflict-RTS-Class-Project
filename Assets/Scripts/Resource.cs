using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
   public int id;
   public float size = 1;

   public Material material;

   // Start is called before the first frame update
   void Start()
   {
      transform.localScale = new Vector3 (size, size, size);
   }

   // Update is called once per frame
   void Update()
   {

   }

   private void OnTriggerEnter(Collider other)
   {
      if(other.transform.tag.Equals("Player"))
      {
         Debug.Log($"Picking up resource {id}");
         
         // player picked-up the item
         other.gameObject.GetComponent<PlayerBag>().AddItem(this);

         Destroy(transform.gameObject, 0.1f);
      }
   }
}

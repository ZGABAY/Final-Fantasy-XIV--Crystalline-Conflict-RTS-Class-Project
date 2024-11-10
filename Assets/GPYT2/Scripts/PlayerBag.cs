using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBag : MonoBehaviour
{
   public Stack<Resource> bag;

   // Start is called before the first frame update
   void Start()
   {
      bag = new Stack<Resource>();
      bag.Clear();
   }

   // Update is called once per frame
   void Update()
   {
      if (Input.GetKeyDown(KeyCode.B))
      {
         Debug.Log($"There are {bag.Count} items in the bag!");
      }
   }

   public void AddItem(Resource item)
   {
      bag.Push(item);
   }

   public Resource RemoveItem()
   {
      if (bag.Count > 0)
      {
         var item = bag.Pop();
         return item;
      }

      return null;
   }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageResourceController : MonoBehaviour
{
   public List<Transform> resourceVisualTransforms = new List<Transform>();

   public int resourceIndex;

   // Start is called before the first frame update
   void Start()
   {
      foreach (Transform t in resourceVisualTransforms)
         t.gameObject.SetActive(false);
   }

   public void UpdateStorageResources(bool storing=true)
   {
      if(storing)
      {
         if (resourceIndex < resourceVisualTransforms.Count)
         {
            resourceVisualTransforms[resourceIndex].gameObject.SetActive(true);
            resourceIndex++;
         }
      }
      else
      {
         if(resourceIndex>0)
         {
            resourceIndex--;
            
            if (resourceIndex <= 0)
               resourceIndex = 0;

            resourceVisualTransforms[resourceIndex].gameObject.SetActive(false);
         }
      }
   }
}

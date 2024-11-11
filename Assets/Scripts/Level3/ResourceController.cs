using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
   #region CUSTOM EVENTS
   public delegate void ResourceDepleted(Transform location, int resourceId, int storageId);
   public static event ResourceDepleted OnResourceDepleted;

   public delegate void ResourceFound(Transform location, int resourceId, int storageId);
   public static event ResourceFound OnResourceFound;

   public delegate void EnemyResourceFound(Transform location, int resourceId, int storageId);
   public static event EnemyResourceFound OnEnemyResourceFound;
   #endregion

   bool IsCollecting = false;

   public bool resourceFound = false;
   public bool isDepleted = false;

   public Transform collectionPoint;

   public int id;
   public int StorageId;

   public bool PlayerResource;

   public int resourceIndex;
   public List<Transform> resourceVisualTransforms = new List<Transform>();

   private void OnEnable()
   {
      CollectorController.OnFinishedCollecting += CollectorController_OnFinishedCollecting;
   }

   private void OnDisable()
   {
      CollectorController.OnFinishedCollecting -= CollectorController_OnFinishedCollecting;
   }

   private void CollectorController_OnFinishedCollecting(int resourceId, int value)
   {
      if (id.Equals(resourceId))
      {
         if (resourceIndex > 0)
         {
            resourceVisualTransforms[resourceVisualTransforms.Count-resourceIndex].gameObject.SetActive(false);

            resourceIndex--;
            if (resourceIndex <= 0)
            {
               resourceIndex = 0;
               isDepleted = true;

               OnResourceDepleted?.Invoke(transform, id, StorageId);
            }
         }
      }
   }


   // Start is called before the first frame update
   void Start()
   {
      resourceIndex = resourceVisualTransforms.Count;
   }

   private void OnTriggerEnter(Collider other)
   {
      if (PlayerResource)
      {
         if (other.transform.tag.Equals("Player"))
         {
            if (!resourceFound)
            {
               resourceFound = true;

               OnResourceFound?.Invoke(collectionPoint, id, StorageId);
            }
         }

         if (other.transform.tag.Equals("Collector"))
         {
            var cc = other.transform.GetComponent<CollectorController>();
            Debug.Log("Controller in Resource Controller Trigger!!!");
            if (!IsCollecting && !isDepleted)
               cc.StartCollecting();
         }
      }
      else
      {
         if (other.transform.tag.Equals("Enemy"))
         {
            if (!resourceFound)
            {
               resourceFound = true;
               OnEnemyResourceFound?.Invoke(collectionPoint, id, StorageId);
            }
         }

         if (other.transform.tag.Equals("EnemyCollector"))
         {
            Debug.Log("Enemy Controller in Resource Controller Trigger!!!");
            var cc = other.transform.GetComponent<CollectorController>();
            if (!IsCollecting && !isDepleted)
               cc.StartCollecting();
         }
      }
   }

   private void OnTriggerExit(Collider other)
   {
      if (PlayerResource)
      {
         if (other.transform.tag.Equals("Collector"))
         {
            Debug.Log("Controller exiting Resource Controller Trigger!!!");
            IsCollecting = false;

            other.GetComponent<CollectorController>().ResourcesCollectedQuantity += 3;
         }
      }
      else
      {
         if (other.transform.tag.Equals("EnemyCollector"))
         {
            Debug.Log("Controller exiting Resource Controller Trigger!!!");
            IsCollecting = false;

            other.GetComponent<CollectorController>().ResourcesCollectedQuantity += 3;
         }
      }
   }
}

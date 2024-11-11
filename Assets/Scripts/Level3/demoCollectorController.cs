using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static CollectorController;

public class CollectorController : MonoBehaviour
{

   #region CUSTOM EVENTS
   public delegate void FinishedCollecting(int resourceId, int value);
   public static event FinishedCollecting OnFinishedCollecting;

   public delegate void FinishedStoring(int storageId, int value);
   public static event FinishedStoring OnFinishedStoring;
   #endregion

   public int id;
   public int ResourceId;
   public int StorageId;

   public Transform storageTransform;
   public Transform resourceTransform;
   public Transform queueTransform;
   public Transform currentDestination;

   public bool isCollecting=false;
   public bool PlayerCollector = true;

   public int ResourcesCollectedQuantity = 0;

   public NavMeshAgent agent;

   private void OnEnable()
   {
      ResourceController.OnResourceDepleted += ResourceController_OnResourceDepleted;
   }

   private void OnDisable()
   {
      ResourceController.OnResourceDepleted -= ResourceController_OnResourceDepleted;
   }

   private void ResourceController_OnResourceDepleted(Transform location, int resourceId, int storageId)
   {
      // in the future we might do something here ...
      Debug.Log($"Resource {transform.name} deploted! ResourceId:{resourceId} Belongs to StorageId:{storageId}");
   }

   public void StartCollecting()
   {
      StartCoroutine(CollectingResource(5));
   }

   IEnumerator CollectingResource(float collectionTime)
   {
      isCollecting = true;
      yield return new WaitForSeconds(collectionTime);
      OnFinishedCollecting?.Invoke(ResourceId, 3);
      GotoStoragePoint();
   }

   public void GotoStoragePoint()
   {
      isCollecting=false;
      GotoLocation(storageTransform);
   }

   public void StartStoring()
   {
      StartCoroutine(StoringResource(5));
   }

   IEnumerator StoringResource(float depositTime)
   {
      isCollecting = false;
      yield return new WaitForSeconds(depositTime);
      OnFinishedStoring?.Invoke(StorageId, 3);
      GotoCollectionPoint();
   }

   public void GotoCollectionPoint()
   {
      isCollecting = true;
      GotoLocation(resourceTransform);
   }

   void GotoLocation(Transform location)
   {
      currentDestination = location;

      agent.SetDestination(location.position);
   }
}

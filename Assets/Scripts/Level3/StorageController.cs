using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StorageController : MonoBehaviour
{

   public delegate void UpdatePlayerResources(int value);
   public static event UpdatePlayerResources OnUpdatePlayerResources;

   public delegate void UpdateEnemyResources(int value);
   public static event UpdateEnemyResources OnUpdateEnemyResources;

   public int id;
   public bool doorOpen = false;

   public Animator roomAnimator;

   public GameObject collectorPrefab;
   public Transform collectorSpawnPoint;
   public Transform collectorDepositPoint;
   public Transform queueTransform;

   public bool PlayerStorage;

   public int storedResources = 0;
   public bool DistributingResources = false;
   public StorageResourceController storageResourceController;

   public List<CollectorController> collectorQueue = new List<CollectorController>();

   private void OnEnable()
   {
      ResourceController.OnResourceFound += ResourceController_OnResourceFound;
      ResourceController.OnEnemyResourceFound += ResourceController_OnEnemyResourceFound;

      CollectorController.OnFinishedStoring += CollectorController_OnFinishedStoring;
   }

   private void OnDisable()
   {
      ResourceController.OnResourceFound -= ResourceController_OnResourceFound;
      ResourceController.OnEnemyResourceFound -= ResourceController_OnEnemyResourceFound;

      CollectorController.OnFinishedStoring -= CollectorController_OnFinishedStoring;
   }


   // Start is called before the first frame update
   void Start()
   {
      OnUpdatePlayerResources?.Invoke(storedResources);
      roomAnimator = GetComponent<Animator>();
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

   //bool IsStoring;

   private void OnTriggerEnter(Collider other)
   {
      if (PlayerStorage)
      {
         if (other.transform.tag.Equals("Player") || other.transform.tag.Equals("Collector"))
         {
            // open the door
            OpenDoor(true);
         }

         if (other.transform.tag.Equals("Collector"))
         {
            var cc = other.transform.GetComponent<CollectorController>();

            if (!cc.isCollecting)
               cc.StartStoring();
         }
      }
      else
      {
         if (other.transform.tag.Equals("Enemy") || other.transform.tag.Equals("EnemyCollector"))
         {
            // open the door
            OpenDoor(true);
         }

         if (other.transform.tag.Equals("EnemyCollector"))
         {
            var cc = other.transform.GetComponent<CollectorController>();

            if (cc.isCollecting)
               return;

            if (!cc.isCollecting)
               cc.StartStoring();
         }
      }
   }

   private void OnTriggerStay(Collider other)
   {
      if (PlayerStorage)
      {
         if (other.transform.tag.Equals("Player") || other.transform.tag.Equals("Collector"))
         {
            // open the door
            OpenDoor(true);
         }

         if(other.transform.tag.Equals("Player"))
         {
            // get some resources from the storage ...
            if (!DistributingResources)
            {
               // only provide resources to the requester if we have enough resources
               if (storedResources - 3 >= 0)
                  StartCoroutine(GetResources(2, other));
            }
         }
      }
      else
      {
         if (other.transform.tag.Equals("Enemy") || other.transform.tag.Equals("EnemyCollector"))
         {
            // open the door
            OpenDoor(true);
         }
      }
   }

   IEnumerator GetResources(float depositTime, Collider other)
   {
      DistributingResources = true;
      yield return new WaitForSeconds(depositTime);
      other.gameObject.GetComponent<PlayerAttackController>().UpdateAmo(3);

      storageResourceController.UpdateStorageResources(false);
      storedResources -= 3;

      DistributingResources = false;

      if (PlayerStorage)
         OnUpdatePlayerResources?.Invoke(storedResources);
      else
         OnUpdateEnemyResources?.Invoke(storedResources);
   }

   private void CollectorController_OnFinishedStoring(int storageId, int value)
   {
      if(id.Equals(storageId))
      {
         storedResources += value;
         storageResourceController.UpdateStorageResources();

         if(PlayerStorage)
            OnUpdatePlayerResources?.Invoke(storedResources);
         else
            OnUpdateEnemyResources?.Invoke(storedResources);
      }
   }


   private void OnTriggerExit(Collider other)
   {
      if (PlayerStorage)
      {
         if (other.transform.tag.Equals("Player") || other.transform.tag.Equals("Collector"))
         {
            // do something ...
            OpenDoor(false);
         }

         if (other.transform.tag.Equals("Collector"))
         {
            Debug.Log("Controller exiting Storage Controller Trigger!!!");

            //storedResources += other.GetComponent<CollectorController>().ResourcesCollectedQuantity;
            other.GetComponent<CollectorController>().ResourcesCollectedQuantity = 0;
         }
      }
      else
      {

         if (other.transform.tag.Equals("Enemy") || other.transform.tag.Equals("EnemyCollector"))
         {
            // do something ...
            OpenDoor(false);
         }

         if (other.transform.tag.Equals("EnemyCollector"))
         {
            Debug.Log("Controller exiting Storage Controller Trigger!!!");

            //storedResources += other.GetComponent<CollectorController>().ResourcesCollectedQuantity;
            other.GetComponent<CollectorController>().ResourcesCollectedQuantity = 0;
         }
      }
   }

   private void ResourceController_OnResourceFound(Transform location, int resourceId, int storageId)
   {
      if(id==storageId)
      {
         // create a collector unit and set it's destination ...
         var collector = GameObject.Instantiate(collectorPrefab, collectorSpawnPoint);

         collector.transform.parent = null;

         var cc = collector.GetComponent<CollectorController>();
         cc.resourceTransform = location;
         cc.storageTransform = collectorDepositPoint;
         cc.StorageId = id;
         cc.ResourceId = resourceId;

         cc.queueTransform = queueTransform;

         // need to re-calculate the nav mesh
         var newUnitAgent = collector.transform.GetComponent<NavMeshAgent>();
         newUnitAgent.enabled = false;

         NavMeshHit meshHit;
         bool hit = NavMesh.SamplePosition(collector.transform.position, out meshHit, 100, 1);
         if (hit)
         {
            newUnitAgent.Warp(meshHit.position);
            newUnitAgent.enabled = true;
         }

         cc.GotoCollectionPoint();
      }
   }

   private void ResourceController_OnEnemyResourceFound(Transform location, int resourceId, int storageId)
   {
      if (id == storageId && !PlayerStorage)
      {
         // create a collector unit and set it's destination ...
         var collector = GameObject.Instantiate(collectorPrefab, collectorSpawnPoint);
         collector.transform.tag = "EnemyCollector";

         collector.transform.parent = null;

         var cc = collector.GetComponent<CollectorController>();
         cc.resourceTransform = location;
         cc.storageTransform = collectorDepositPoint;
         cc.StorageId = id;
         cc.ResourceId = resourceId;

         cc.PlayerCollector = false;

         cc.queueTransform = queueTransform;

         // need to re-calculate the nav mesh
         var newUnitAgent = collector.transform.GetComponent<NavMeshAgent>();
         newUnitAgent.enabled = false;

         NavMeshHit meshHit;
         bool hit = NavMesh.SamplePosition(collector.transform.position, out meshHit, 100, 1);
         if (hit)
         {
            newUnitAgent.Warp(meshHit.position);
            newUnitAgent.enabled = true;
         }

         cc.GotoCollectionPoint();
      }
   }

}

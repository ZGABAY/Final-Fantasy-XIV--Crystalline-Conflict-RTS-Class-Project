using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
   public delegate void EnemyDied(Transform enemyT, int id);
   public static event EnemyDied OnEnemyDied;

   public int id;

   public float scoutingRadius = 10f;  // Radius around the NPC for random scouting points
   public float minDistance = 2f;      // Minimum distance from the NPC to a scouting point
   public int maxAttempts = 30;        // Maximum attempts to find a valid point on the NavMesh

   public NavMeshAgent agent;

   // initially the enemy agent will need to scout the surrounding areas
   public bool Scouting = true;
   public int scoutingIndex = 0;

   public List<Vector3> scoutingTargets = new List<Vector3>();

   public int scoutingPoints = 10;

   public GameObject playerObject;
   public bool playerInSight;

   public bool IsDead = false;

   public EnemyAttackController myAttackController;
   public EnemyHealth myHealth;

   // Start is called before the first frame update
   void Start()
   {
      if(Scouting)
      {
         // randomize a few positions on the current scene
         for(int i=0;i< scoutingPoints; i++)
         {
            var p = ScoutRandomPosition();

            scoutingTargets.Add(p);
         }
      }
   }

   public Vector3 ScoutRandomPosition()
   {
      Vector3 randomPoint = RandomNavMeshPoint(transform.position, scoutingRadius);

      return randomPoint;
   }

   // Method to find a random point within the radius and check if it's on the NavMesh
   Vector3 RandomNavMeshPoint(Vector3 center, float radius)
   {
      for (int i = 0; i < maxAttempts; i++)
      {
         Vector3 randomDirection = Random.insideUnitSphere * radius;
         randomDirection += center;

         // Sample the NavMesh at that point
         if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas))
         {
            float distance = Vector3.Distance(transform.position, hit.position);
            if (distance >= minDistance)
            {
               return hit.position;
            }
         }
      }

      // Return Vector3.zero if no valid point is found after max attempts
      return Vector3.zero;
   }

   // Update is called once per frame
   void Update()
   {
      if (IsDead)
      {
         // send message to everyone that I am dead!!!
         OnEnemyDied?.Invoke(transform, id);
         return;
      }

      if(myHealth.health<=0)
      {
         IsDead = true;
         myAttackController.animator.SetBool("dead", IsDead);
      }

      if (Scouting) 
      {
         var d = Vector3.Distance(transform.position, scoutingTargets[scoutingIndex]);
         //Debug.Log(d);

         if (d < 1.5f)
         {
            GotoNextScoutingPoint();
         }
         else
         {
            agent.SetDestination(scoutingTargets[scoutingIndex]);

            // check to see if destination is reachable
            Invoke(nameof(ValidatePath), 0.1f);
         }
      }

      if(playerInSight)
      {
         // stop scouting
         Scouting = false;

         var d = Vector3.Distance(transform.position, playerObject.transform.position);
         if(d<20)
         {
            agent.SetDestination(playerObject.transform.position);
         }
         else
         {
            playerInSight = false;
            Scouting = true;
         }

         myAttackController.Attack(playerInSight);
      }
   }

   void GotoNextScoutingPoint()
   {
      scoutingIndex += 1;

      if (scoutingIndex >= scoutingTargets.Count)
      { scoutingIndex = 0; }
   }

   void ValidatePath()
   {
      if (agent.pathStatus == NavMeshPathStatus.PathComplete)
      {
         //Debug.Log("Destination is reachable.");
      }
      else if (agent.pathStatus == NavMeshPathStatus.PathPartial)
      {
         Debug.LogWarning("Destination is partially reachable, but not fully.");
         GotoNextScoutingPoint();
      }
      else if (agent.pathStatus == NavMeshPathStatus.PathInvalid)
      {
         Debug.LogWarning("Destination is not reachable.");
         GotoNextScoutingPoint();
      }
   }
}

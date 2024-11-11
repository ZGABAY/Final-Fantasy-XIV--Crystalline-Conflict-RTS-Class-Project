using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
   public Animator animator;

   public int numberOfBullets;

   public GameObject bulletPrefab;
   public Transform bulletSpawnPoint;
   public Transform canonHandle;

   public bool IsAttacking = false;
   public bool ReadyToShoot = true;
   public float accelerationForce = 100;

   // Start is called before the first frame update
   void Start()
   {
      animator.SetBool("attack",IsAttacking);
   }

   public void Attack(bool value)
   {
      IsAttacking = value;
      animator.SetBool("attack", IsAttacking);
      
      if(ReadyToShoot)
         StartCoroutine(SpawnBullet(2));
   }

   IEnumerator SpawnBullet(float loadTime)
   {
      ReadyToShoot = false;
      yield return new WaitForSeconds(loadTime);

      var bullet = GameObject.Instantiate(bulletPrefab, bulletSpawnPoint);
      bullet.transform.SetParent(null);

      bullet.GetComponent<Rigidbody>().AddForce(bulletSpawnPoint.forward * accelerationForce, ForceMode.Acceleration);

      Destroy(bullet,5);

      ReadyToShoot = true;
   }
}

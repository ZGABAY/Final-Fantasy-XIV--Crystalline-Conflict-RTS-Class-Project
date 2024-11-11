using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
   public delegate void UpdatePlayerAmo(int value);
   public static event UpdatePlayerAmo OnUpdatePlayerAmo;

   public Animator animator;

   public int numberOfBullets;

   public GameObject bulletPrefab;
   public Transform bulletSpawnPoint;

   public bool IsAttacking = false;
   public bool ReadyToShoot = true;
   public float accelerationForce = 100;

   public float ReloadTime = 0.5f;

   // Start is called before the first frame update
   void Start()
   {
      OnUpdatePlayerAmo?.Invoke(numberOfBullets);
   }

   // Update is called once per frame
   void Update()
   {
      if (Input.GetKeyDown(KeyCode.T))
      {
         Attack();
      }

      if(Input.GetKeyDown(KeyCode.Space))
      {
         if(IsAttacking)
            Shoot();
      }
   }

   void Attack()
   {
      IsAttacking = !IsAttacking;
      animator.SetBool("attack", IsAttacking);
   }

   void Shoot()
   {
      if (ReadyToShoot && numberOfBullets>0)
         StartCoroutine(SpawnBullet(ReloadTime));
   }

   public void UpdateAmo(int value)
   {
      numberOfBullets += value;
      OnUpdatePlayerAmo.Invoke(numberOfBullets);
   }

   IEnumerator SpawnBullet(float loadTime)
   {
      ReadyToShoot = false;

      var bullet = GameObject.Instantiate(bulletPrefab, bulletSpawnPoint);
      bullet.transform.SetParent(null);

      bullet.GetComponent<Rigidbody>().AddForce(bulletSpawnPoint.forward * accelerationForce, ForceMode.Acceleration);

      numberOfBullets--;
      OnUpdatePlayerAmo?.Invoke(numberOfBullets);

      Destroy(bullet, 5);

      yield return new WaitForSeconds(loadTime);
      ReadyToShoot = true;
   }
}

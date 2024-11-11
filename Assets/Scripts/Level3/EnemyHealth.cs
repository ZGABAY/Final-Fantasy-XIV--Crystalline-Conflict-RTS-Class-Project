using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
   public Image enemyHealthBar;


   public int health = 100;

   private void Start()
   {
      enemyHealthBar.fillAmount = health / 100f;
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.transform.tag.Equals("Bullet"))
      {
         health -= 10;
         enemyHealthBar.fillAmount = health / 100f;
         
         Debug.Log($"Enemy {transform.name} was hit by {other.transform.name} health is now {health}");
      }
   }
}

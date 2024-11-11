using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

   public Image playerHealthBar;


   public int health = 100;

   private void Start()
   {
      playerHealthBar.fillAmount = health/100f;
   }


   private void OnTriggerEnter(Collider other)
   {
      if (other.transform.tag.Equals("Bullet"))
      {
         health -= 10;
         
         playerHealthBar.fillAmount = health / 100f;

         Debug.Log($"Player was hit by {other.transform.name} health is now {health}");
      }
   }
}

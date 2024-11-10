using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
 
public class CdUnitProduction : MonoBehaviour
{
   public bool IsSelected = true;
   public bool IsProducing = false;
 
   // using a list of floats that will be generated at runtime
   // the float value will indicate the production time
   public List<float> productionList = new List<float>();
 
   public float angleStep = 25;
   public float currentAngle = 0;
   public float radius = 5;
 
   public int unitCount = 0;
   float updateInterval = 0.1f;

   public TMP_Text tmpProgress;  // used to update numerical prograss
   public Image progressImage;   // used to visualize progress bar
 
   // Start is called before the first frame update
   void Start()
   {
      progressImage.fillAmount = 0;
 
      for (int i = 0; i < 3; i++)
      {
         productionList.Add(Random.Range(0.5f, 2.5f));
      }
   }
 
   // Update is called once per frame
   void Update()
   {
      if (Input.GetKeyDown(KeyCode.P))
      {
         StartProduction();
      }
   }
 
   private void StartProduction()
   {
      if (!IsSelected)
         return;
 
      if (!IsProducing)
      {
         // we currently are not taking FIFO into consideration
         StartCoroutine(ProduceUnit(productionList[0]));
      }
   }
 
   private IEnumerator ProduceUnit(float productionTime)
   {
 
      float elapsedTime = 0f;
 
      IsProducing = true;
 
      // Continue updating progress until the total production time is reached
      while (elapsedTime < productionTime)
      {
         // Increment the time that has passed by the update interval
         elapsedTime += updateInterval;
 
         // Clamp elapsedTime to ensure it does not exceed productionTime
         elapsedTime = Mathf.Clamp(elapsedTime, 0f, productionTime);
 
         // Calculate the production progress as a percentage
         float progress = (elapsedTime / productionTime);
         
         progressImage.fillAmount = progress;
         tmpProgress.text = string.Format("{0:f2}", progress * 100f);
 
         // Display progress or show time left
         Debug.Log($"production progress: {progress:F2}% ({productionTime - elapsedTime:F2} seconds remaining)");
 
         // Wait for the update interval before the next progress update
         yield return new WaitForSeconds(updateInterval);
      }
 
      // Once the time has passed, the unit is produced
      // Calculate position on the circle
      float angleInRadians = currentAngle * Mathf.Deg2Rad;
      float x = transform.position.x + Mathf.Cos(angleInRadians) * radius;
      float z = transform.position.z + Mathf.Sin(angleInRadians) * radius;
 
      Vector3 spawnPosition = new Vector3(x, transform.position.y, z);
 
      currentAngle += angleStep;
 
      var newUnit = GameObject.CreatePrimitive(PrimitiveType.Sphere);
 
      newUnit.transform.position = spawnPosition;
      newUnit.transform.rotation = Quaternion.identity;
 
      newUnit.name = $"newUnit{unitCount}";
      unitCount++;
 
      IsProducing = false;
 
      if (productionList.Count > 1)
      {
         productionList.RemoveAt(0);
         StartCoroutine(ProduceUnit(productionList[0]));
      }
      else
      {
         // make sure everything is cleared
         productionList.Clear();
 
         tmpProgress.text = $"TUP: {unitCount}";
      }
   }
}
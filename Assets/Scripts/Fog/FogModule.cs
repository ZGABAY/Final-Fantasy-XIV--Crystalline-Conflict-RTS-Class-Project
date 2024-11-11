using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FogModule : MonoBehaviour
{
   [SerializeField]
   List<MeshRenderer> renderers;

   // Start is called before the first frame update
   void Start()
   {
      renderers = transform.GetComponentsInChildren<MeshRenderer>().ToList();
   }

   // Update is called once per frame
   void Update()
   {

   }

   public void ShowMeshRenderer(bool show)
   {
      foreach (var renderer in renderers)
      {
         renderer.enabled = show;
      }
   }
}

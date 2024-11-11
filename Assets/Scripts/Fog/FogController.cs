using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogController : MonoBehaviour
{
   public GameObject player;

   public GameObject fogPlane;
   //public List<GameObject> selectedUnits;

   public List<GameObject> structures;
   public List<GameObject> removeStructures = new List<GameObject>();

   public List<GameObject> hidables;
   public List<GameObject> removeHidables = new List<GameObject>();

   public LayerMask fogLayer;
   public float fogRadius = 10f;

   float FogArea { get { return fogRadius * fogRadius; } }

   Mesh mesh;
   [SerializeField]
   Vector3[] vertices;
   [SerializeField]
   Color[] colors;

   // Start is called before the first frame update
   void Start()
   {
      mesh = fogPlane.GetComponent<MeshFilter>().mesh;
      vertices = mesh.vertices;
      colors = new Color[vertices.Length];
      for (int i = 0; i < vertices.Length; i++)
      {
         colors[i] = Color.black;
      }
      UpdateColors();
   }

   // Update is called once per frame
   void Update()
   {
      //selectedUnits = UnitSelectionManager.Instance.unitsSelected;
      //selectedUnits = SelectionManager.Instance.currentSelected; 

      //foreach (var unit in selectedUnits)
      {
         
         //Ray r = new Ray(transform.position, unit.transform.position - transform.position);

         //Debug.Log(r);

         //RaycastHit hit;
         //if(Physics.Raycast(r, out hit, 1000, fogLayer, QueryTriggerInteraction.Collide))
         {
            //Debug.Log("I AM IN THE RAYCASE HIT IF CONDITION");

            for (int i = 0; i < vertices.Length; i++)
            {
               Vector3 v = fogPlane.transform.TransformPoint(vertices[i]);

               //float distance = Vector3.SqrMagnitude(v - (new Vector3(unit.transform.position.x, v.y, unit.transform.position.z)));
               float distance = Vector3.SqrMagnitude(v - (new Vector3(player.transform.position.x, v.y, player.transform.position.z)));

               //Debug.LogWarning($"v:{v} - distance:{distance} - FogArea:{FogArea}");

               if (distance < FogArea)
               {
                  float alpha = Mathf.Min(colors[i].a, distance / FogArea);
                  colors[i].a = alpha;

                  if (structures.Count > 0)
                  {
                     foreach (var s in structures)
                     {
                        float visible = Vector3.SqrMagnitude(v - (new Vector3(s.transform.position.x, v.y, s.transform.position.z)));
                        if (visible < FogArea)
                        {
                           s.transform.GetComponent<FogModule>().ShowMeshRenderer(true);
                           // we would probably want to remove from the list to reduce computation
                           removeStructures.Add(s);
                        }
                     }
                  }

                  if (hidables.Count > 0)
                  {
                     foreach (var h in hidables)
                     {
                        float visible = Vector3.SqrMagnitude(v - (new Vector3(h.transform.position.x, v.y, h.transform.position.z)));
                        if (visible < FogArea)
                        {
                           h.transform.GetComponent<FogModule>().ShowMeshRenderer(true);
                           // we would probably want to remove from the list to reduce computation
                           removeHidables.Add(h);
                        }
                     }
                  }

                  if (removeStructures.Count > 0)
                  {
                     foreach (var s in removeStructures)
                        structures.Remove(s);
                     removeStructures.Clear();
                  }

                  if (removeHidables.Count > 0)
                  {
                     foreach (var h in removeHidables)
                        hidables.Remove(h);
                     removeHidables.Clear();
                  }
               }
            }
         }

         UpdateColors();
      }
   }

   void UpdateColors()
   {
      mesh.colors = colors;
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.tag.Equals("structure"))
      {
         if (!structures.Contains(other.gameObject))
         {
            structures.Add(other.gameObject);
            other.transform.GetComponent<FogModule>().ShowMeshRenderer(false);
         }
      }

      if (other.tag.Equals("hidable"))
      {
         if (!hidables.Contains(other.gameObject))
         {
            hidables.Add(other.gameObject);
            other.transform.GetComponent<FogModule>().ShowMeshRenderer(false);
         }
      }
   }

}

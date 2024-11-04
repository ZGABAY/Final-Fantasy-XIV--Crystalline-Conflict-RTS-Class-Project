using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class RTSSingleton<T> : MonoBehaviour where T : Component
{
   private static T instance;
 
   public static T Instance
   {
      get
      {
         if (instance == null)
         {
            instance = FindObjectOfType<T>();
            if (instance == null)
            {
               GameObject obj = new GameObject();
               obj.name = string.Format("--{0}", typeof(T).Name);
               instance = obj.AddComponent<T>();
            }
            Debug.Log("ing_ instance created :");
         }
         return instance;
      }
   }
 
   public virtual void Awake()
   {
      if (instance == null)
      {
         instance = this as T;
 
         DontDestroyOnLoad(gameObject);
      }
      else
      {
         Destroy(gameObject);
      }
   }
}
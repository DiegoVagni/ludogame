using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  abstract class SingletonScriptableObject<T> : ScriptableObject where T:ScriptableObject
{

    private static T _instance = null;
    [SerializeField]
    private T serializedInstance = null;
   

    public static T Instance() {
        if (_instance == null) {
            //Resources.FindObjectsOfTypeAll<T>();
            T[] results = Resources.LoadAll<T>("/Resources");


            if (results.Length == 0)
            {
                Debug.LogError("Scriptable singleton -> instances length for resource " + typeof(T).ToString() + " is 0.");
                return null;
            }
            else if (results.Length > 1) {
                Debug.LogError("Scriptable singleton -> instances length for resource " + typeof(T).ToString() + " is > 1.");
                return null;
            }

            _instance = results[0];
        }
       
        return _instance;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiate : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    public void Awake() {
        NetworkInstancer.NetworkInstantiate(prefab, transform.position, transform.rotation);
        Debug.Log("instantiated over the network " + prefab.name);
    } 
}

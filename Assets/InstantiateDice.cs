using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateDice : MonoBehaviour
{
    [SerializeField]
    private GameObject dice;
    // Start is called before the first frame update
    public void Awake()
    {
        Instantiate(dice, transform);
     
    }

}

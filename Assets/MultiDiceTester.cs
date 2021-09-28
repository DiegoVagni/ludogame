using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiDiceTester : MonoBehaviour
{
    [SerializeField] private GameObject playGroundPrefab;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                //GameObject.Instantiate(playGroundPrefab);
                if (i == 0 && j == 0) continue;
                GameObject a = GameObject.Instantiate(playGroundPrefab);
                a.transform.position = new Vector3(35 * i, 0, 35*j);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

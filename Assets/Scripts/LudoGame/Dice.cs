using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Dice : MonoBehaviour
{
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        System.Random random = new System.Random(Guid.NewGuid().GetHashCode());
        rb = this.GetComponent<Rigidbody>();
        rb.AddTorque(new Vector3(random.Next(-1000,1000), random.Next(-1000, 1000), random.Next(-1000, 1000)), ForceMode.Force);
    }
    int result = 0;
    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude == 0 && result == 0) {
            if (Vector3.Angle(transform.forward, Vector3.up) == 0)
            {
                result = 3;
            }
            else if (Vector3.Angle(-transform.forward, Vector3.up) == 0)
            {
                result = 4;
            }
            else if (Vector3.Angle(transform.up, Vector3.up) == 0)
            {
                result = 2;
            }
            else if (Vector3.Angle(-transform.up, Vector3.up) == 0)
            {
                result = 5;
            }
            else if (Vector3.Angle(transform.right, Vector3.up) == 0)
            {
                result = 6;
            }
            else if (Vector3.Angle(-transform.right, Vector3.up) == 0)
            {
                result = 1;
            }
        }
        Debug.Log(result);
    }
}

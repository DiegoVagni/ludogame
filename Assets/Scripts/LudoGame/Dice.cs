using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Dice : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private int torqueMin;
    [SerializeField] private int torqueMax;
    [SerializeField] private int forceMin;
    [SerializeField] private int forceMax;

    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private Vector3 startingScale;

    FileLogger fileLogger = LoggerManager.GetInstance();


    private System.Random random;
    public static bool isRolling = false;
    [SerializeField] private int tries = 1;


    private int currentThrow = 0;
    private int result = 0;
    public static int[] throws;
    public static int finished;
    private float prevVelocity = -1;
    private float prevPrevVelocity = -1;
    // Start is called before the first frame update
    //sposta sto codice in un unittest
    static Dice()
    {
        throws = new int[] { 0, 0, 0, 0, 0, 0, 0 };
        finished = 0;
    }

    void Start()
    {
        startingPosition = transform.position;
        startingRotation = transform.rotation;
        startingScale = transform.localScale;
        random = new System.Random(Guid.NewGuid().GetHashCode());
       // RollDice();
    }
    // Update is called once per frame
    void Update()
    {

        if (isRolling && rb.velocity.magnitude == 0 && prevPrevVelocity == 0 &&prevVelocity == 0)
        {
            currentThrow++;
            isRolling = false;
            Debug.Log("set to false");

            //ordered: 0->1 , 1->2 , ... , 5->6
            List<float> angles = new List<float>() {
                Vector3.Angle(-transform.right, Vector3.up),
                Vector3.Angle(transform.up, Vector3.up),
                Vector3.Angle(transform.forward, Vector3.up),
                Vector3.Angle(-transform.forward, Vector3.up),
                Vector3.Angle(-transform.up, Vector3.up),
                Vector3.Angle(transform.right, Vector3.up)
            };

            float min = float.MaxValue;

            for (int i = 0; i < angles.Count; i++)
            {
                if (angles[i] == 0)
                {
                    result = i + 1;
                    break;
                }
                else if (Math.Abs(angles[i]) < min)
                {
                    min = Math.Abs(angles[i]);
                    result = i + 1;
                }

            }

            /*if (Vector3.Angle(transform.forward, Vector3.up) == 0)
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
            }*/
            throws[result]++;
            //Debug.Log("Try no. " + currentThrow + " : " + result);

         /*   if (currentThrow < tries)
            {
                result = 0;
                RollDice();
            }
            else
            {
                finished++;
                Debug.Log(finished);
            }*/
        }
        // questo � meglio farlo in altra maniera
        /*else if (rb.velocity.magnitude != 0)
        {
            isRolling = true;
        }*/
        prevPrevVelocity = prevVelocity;
        prevVelocity = rb.velocity.magnitude;
    }
    public bool IsRolling() {
        return isRolling;
    }
    
    public int GetResult()
    {
        return result;
    }
    public void RollDice()
    {
        //pi� pulito, lo setti a true solo una volta, non possono esserci concurrency
        isRolling = true;
        Debug.Log("Set to true");
        transform.position = startingPosition;
        transform.rotation = startingRotation;
        transform.localScale = startingScale;
        rb = this.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * random.Next(forceMin, forceMax));
        rb.AddTorque(new Vector3(random.Next(torqueMin, torqueMax), random.Next(torqueMin, torqueMax), random.Next(torqueMin, torqueMax)), ForceMode.Force);
    }


}

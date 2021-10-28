using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;

public class Dice : MonoBehaviour
{
    public delegate void RollAction(int result);
    public static event RollAction diceRolled;

    private Rigidbody rb;
    [SerializeField] private int torqueMin;
    [SerializeField] private int torqueMax;
    [SerializeField] private int forceMin;
    [SerializeField] private int forceMax;

    private Vector3 startingPosition;
    private Quaternion startingRotation;
    private Vector3 startingScale;

    private float lastDiceUpdateTime = 0.25f;

    //FileLogger fileLogger = LoggerManager.GetInstance();


    private System.Random random;
    public bool isRolling = false;

    private int currentThrow = 0;
    private int result = 0;
    public static int[] throws;
    public static int finished;
    private float prevVelocity = -1;
    private float prevPrevVelocity = -1;
    public float gameSpeed;
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
        rb = this.GetComponent<Rigidbody>();
        Time.timeScale = gameSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        if (isRolling && (Time.time > lastDiceUpdateTime + 0.1) && PhotonNetwork.IsMasterClient)
        {
            lastDiceUpdateTime = Time.time;
            PhotonView pw = PhotonView.Get(this);
            pw.RPC("AdjustDice", RpcTarget.Others, transform.position, transform.rotation.eulerAngles, rb.velocity, rb.angularVelocity);
        }
        if (isRolling && rb.velocity.magnitude == 0 && prevPrevVelocity == 0 && prevVelocity == 0)
        {
            currentThrow++;
            isRolling = false;

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

            throws[result]++;
            diceRolled(result);
        }
        // questo è meglio farlo in altra maniera
        else if (rb.velocity.magnitude != 0)
        {
            isRolling = true;
        }
        prevPrevVelocity = prevVelocity;
        prevVelocity = rb.velocity.magnitude;
    }
    [PunRPC]
    public void AdjustDice(Vector3 position, Vector3 rotationEuler, Vector3 velocity, Vector3 angVelocity)
    {
        transform.position = position;
        transform.rotation = Quaternion.Euler(rotationEuler);
        rb.velocity = velocity;
        rb.angularVelocity = angVelocity;
    }

    public bool IsRolling()
    {
        return isRolling;
    }

    public int GetResult()
    {
        return result;
    }
    public void RollDice()
    {

        if (PhotonNetwork.IsMasterClient)
        {

            Vector3 upForce = Vector3.up * random.Next(forceMin, forceMax);
            Vector3 torqueForce = new Vector3(random.Next(torqueMin, torqueMax), random.Next(torqueMin, torqueMax), random.Next(torqueMin, torqueMax));


            PhotonView pw = PhotonView.Get(this);
            pw.RPC("RollDiceOnNetwork", RpcTarget.All, new object[] { upForce, torqueForce });
        }

    }

    [PunRPC]
    //0 upForce, 1 torqueForce;
    private void RollDiceOnNetwork(object[] param)
    {
        transform.position = startingPosition;
        transform.rotation = startingRotation;
        transform.localScale = startingScale;
        Debug.Log("rolling dice");
        Debug.Log((Vector3)param[0]);
        Debug.Log((Vector3)param[1]);
        rb.AddForce((Vector3)param[0], ForceMode.Impulse);
        rb.AddTorque((Vector3)param[1], ForceMode.Impulse);

    }
}

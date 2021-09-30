using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class GM : MonoBehaviour
{
    private void OnEnable()
    {
        Dice.diceRolled += rollEnd;
    }

    private void OnDisable()
    {
        Dice.diceRolled -= rollEnd;
    }

    private void rollEnd()
    {
        if (!gameFinished)
        {
            int result = dice.GetResult();
            //chose move for player.
            Debug.Log("player " + currentPlayer.GetPlayerNumber() + "rolled a " + result);
            if (result == 6)
            {

            }
            currentPlayer = players[currentPlayer.GetPlayerNumber() % 4];

            dice.RollDice();
        }
    }





    [SerializeField]
    private List<Material> playerMaterials;
    private Dice dice;
    private Player currentPlayer;
    private List<Player> players;
    private bool gameFinished = false;
    //private bool whyAreYouRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        dice = GameObject.FindGameObjectWithTag("Dice").GetComponent<Dice>();

        players = new List<Player>();
        List<Photon.Realtime.Player> punplayers = new List<Photon.Realtime.Player>(PhotonNetwork.PlayerList);
        int photonIndex = 0;
        foreach (Material m in playerMaterials)
        {
            if (photonIndex < punplayers.Count)
            {
                players.Add(new Player(m, punplayers[photonIndex]));
                photonIndex++;
            }
            else
            {
                players.Add(new Player(m));
            }
        }
        currentPlayer = players[0];
        dice.RollDice();
    }
    //private void StartTurn()
    //{
    //whyAreYouRunning = true;


    //yield return new WaitForSeconds(1f);
    //yield return new WaitUntil(() => !dice.IsRolling());
    /*while (dice.IsRolling())
    {
        yield return new WaitForSeconds(0.5f);
    }*/

    //currentPlayer = players[currentPlayer.GetPlayerNumber() % 4];
    //whyAreYouRunning = false;
    //StartCoroutine("Turn");
    //}
    /*public IEnumerator Turn()
    {
        lock (dice)
        {

            dice.RollDice();
            yield return new WaitForSeconds(1f);
            yield return new WaitUntil(() => !dice.IsRolling());
            *//*while (dice.IsRolling())
            {
                yield return new WaitForSeconds(0.5f);
            }*//*
            int result = dice.GetResult();
            //chose move for player.
            //Debug.Log("player " + currentPlayer.GetPlayerNumber() + "rolled a " + result);
            if (result == 6)
            {

            }
            currentPlayer = players[currentPlayer.GetPlayerNumber() % 4];
            whyAreYouRunning = false;
        }

    }*/
    // Update is called once per frame
    void Update()
    {
        if (/*Time.frameCount >10000*/Time.realtimeSinceStartup > 100)
        {
            gameFinished = true;
        }
        /*if (!gameFinished && *//*!whyAreYouRunning*//*diceRolled)
        {
            
        }*/
    }

    public List<Player> GetPlayers()
    {
        return players;
    }

}

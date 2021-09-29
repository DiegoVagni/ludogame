using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class GM : MonoBehaviour
{
    [SerializeField]
    private List<Material> playerMaterials;
    private Player currentPlayer;
    private List<Player> players;
    private bool gameFinished = false;
    private bool whyAreYouRunning = false;
    private int playerIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        players = new List<Player>();
        List<Photon.Realtime.Player> punplayers = new List<Photon.Realtime.Player>(PhotonNetwork.PlayerList);
        int photonIndex = 0;
        foreach (Material m in playerMaterials) {
            if (photonIndex < punplayers.Count)
            {
                players.Add(new Player(m, punplayers[photonIndex]));
                photonIndex++;
            }
            else {
                players.Add(new Player(m));
            }
        }
        currentPlayer = players[0];
        StartTurn();
    }
    private void StartTurn() {
        whyAreYouRunning = true;
        StartCoroutine("Turn");
    }
    public IEnumerator Turn() {
        int diceResult=6; //= dice.throw.something
        currentPlayer.MakeMove(diceResult);
        yield return new WaitForSeconds(3);
      
        currentPlayer = players[currentPlayer.GetPlayerNumber()%4];
        whyAreYouRunning = false;
       
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount > 5000) {
            gameFinished = true;
        }
        if (!gameFinished && !whyAreYouRunning)
        {
            StartTurn();
        }
    }
    public List<Player> GetPlayers() {
        return players;
    }

}

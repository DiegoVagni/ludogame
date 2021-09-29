using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class GM : MonoBehaviour
{
    [SerializeField]
    private List<Material> playerMaterials;

    private List<Player> players;
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
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.frameCount % 3600 == 0) {
            foreach (Player p in players) {
              //  p.StampPlayer();
            }
        } 
    }
    public List<Player> GetPlayers() {
        return players;
    }
}

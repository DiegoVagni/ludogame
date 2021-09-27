using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    [SerializeField]
    private List<Material> playerMaterials;
    private List<Player> players;
    // Start is called before the first frame update
    void Start()
    {
        players = new List<Player>();
        foreach (Material m in playerMaterials) {
            players.Add(new Player(m));        
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public List<Player> GetPlayers() {
        return players;
    }
}

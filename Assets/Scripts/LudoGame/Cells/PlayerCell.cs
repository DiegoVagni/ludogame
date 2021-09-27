using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCell : Cell
{
    private Player player;
    public void SetPlayer(Player p) {
        player = p;
    }
    public Player GetPlayer()
    {
        return player;
    }
}

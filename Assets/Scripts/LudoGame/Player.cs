using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player 
{
    private static int players = 1;
    private int playerNumber;
    private Material mat;
    public Player(Material mat)
	{
        this.mat = mat;
        playerNumber = players;
        players++;
	}
    public Material GetMaterial() {
        return mat;
    }
    public int GetPlayerNumber() {
        return playerNumber;
    }
    // Destructor
    ~Player()
    {
        players--;
    }
}

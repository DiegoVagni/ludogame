using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
public class Player 
{
    private static int players = 1;
    private int playerNumber;
    private Material mat;
    private Photon.Realtime.Player photonPlayer;
    public Player(Material mat,Photon.Realtime.Player photonPlayer=null)
	{
        this.mat = mat;
        playerNumber = players;
        this.photonPlayer = photonPlayer;
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

    public void StampPlayer() {
        Debug.Log("Player " + playerNumber + " is known as " + photonPlayer.NickName);
    }
}

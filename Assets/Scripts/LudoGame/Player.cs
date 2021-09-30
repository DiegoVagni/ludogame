using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using System;

public class Player 
{
    private static int players = 1;
    private int playerNumber;
    private Material mat;
    private List<Pawn> pawns;
    private Photon.Realtime.Player photonPlayer;
    private HomeCell home;
 
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
    public void SetHome(HomeCell home)
	{

        this.home = home;
	}
    public HomeCell GetHome() {
        return home;
    }
    public PlayerCell GetStart() {
        return (PlayerCell)home.GetConnectedCells()[CellIntersections.Next];
    }
    // Destructor
    ~Player()
    {
        players--;
    }
    public void SetPawns(List<Pawn> pawns) {
        this.pawns = pawns;
 
    }
    public void StampPlayer() {
        Debug.Log("Player " + playerNumber + " is known as " + photonPlayer.NickName);
    }

	
}

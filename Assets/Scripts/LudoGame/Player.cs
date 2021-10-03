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
    public Pawn GetPawnInHome() {
        foreach (Pawn p in pawns) {
            if (p.GetCurrentCell().GetCellType() == CellType.Home) {
                
                return p;
            }
        }
        return null;
    }
    public Pawn GetPawnOutOfHome() {
        foreach (Pawn p in pawns)
        {
            
            
            if (p.GetCurrentCell().GetCellType() != CellType.Home && p.GetCurrentCell().GetCellType() != CellType.Finish)
            {
                return p;
            }
        }
        return null;
    }
    public void ChooseMove(int diceNumber) {
        if (diceNumber == 6)
        {
            foreach (Pawn p in pawns)
            {
              
                if (p.GetCurrentCell().GetCellType() == CellType.Start)
                {
                    p.Move(diceNumber);
                    return;
                }
            }
            Pawn pawnToMove = GetPawnInHome();
            if (pawnToMove != null)
            {
                home.ExitPawnToHome(pawnToMove);
                return;
            }
            else
            {
                pawnToMove = GetPawnOutOfHome();
                pawnToMove.Move(diceNumber);
                return;
            }

        }
        else {
            foreach (Pawn p in pawns)
            {

                if (p.GetCurrentCell().GetCellType() == CellType.Start)
                {
                    p.Move(diceNumber);
                    return;
                }
            }
            if (GetPawnOutOfHome() != null) {
                GetPawnOutOfHome().Move(diceNumber);
                return;
            }
        }
    }

	
}

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

    public Player(Material mat, Photon.Realtime.Player photonPlayer = null)
    {
        this.mat = mat;
        playerNumber = players;
        this.photonPlayer = photonPlayer;
        players++;
    }
    public Material GetMaterial()
    {
        return mat;
    }
    public int GetPlayerNumber()
    {
        return playerNumber;
    }
    public void SetHome(HomeCell home)
    {

        this.home = home;
    }
    public HomeCell GetHome()
    {
        return home;
    }
    public PlayerCell GetStart()
    {
        return (PlayerCell)home.GetConnectedCells()[CellIntersections.Next];
    }
    // Destructor
    ~Player()
    {
        players--;
    }
    public void SetPawns(List<Pawn> pawns)
    {
        this.pawns = pawns;

    }
    public void StampPlayer()
    {
        //Debug.Log("Player " + playerNumber + " is known as " + photonPlayer.NickName);
    }
    public Pawn GetPawnInHome()
    {
        foreach (Pawn p in pawns)
        {
            if (p.GetCurrentCell().GetCellType() == CellType.Home)
            {

                return p;
            }
        }
        return null;
    }
    public Pawn GetPawnOutOfHome()
    {
        foreach (Pawn p in pawns)
        {


            if (p.GetCurrentCell().GetCellType() != CellType.Home && p.GetCurrentCell().GetCellType() != CellType.Finish)
            {
                return p;
            }
        }
        return null;
    }

    public List<Pawn> GetPawnsOutOfHome()
    {
        List<Pawn> outOfHomePawns = new List<Pawn>();
        pawns.ForEach((p) =>
        {
            if (p.GetCurrentCell().GetCellType() != CellType.Home && p.GetCurrentCell().GetCellType() != CellType.Finish)
            {
                outOfHomePawns.Add(p);
            }
        });
        return outOfHomePawns;
    }
    public List<Pawn> GetPawnsInHome()
    {
        List<Pawn> inHomePawns = new List<Pawn>();
        pawns.ForEach((p) =>
        {
            if (p.GetCurrentCell().GetCellType() == CellType.Home)
            {
                inHomePawns.Add(p);
            }
        });
        return inHomePawns;
    }

    private List<Move> GetMoves(int diceNumber)
    {
        List<Move> moves = new List<Move>();
        foreach (Pawn p in pawns)
        {
            if (p.GetCurrentCell().GetCellType() == CellType.Home)
            {
                if (diceNumber == 6)
                {
                    Move move = new Move(1, p.getEndCell(1), p);
                    moves.Add(move);
                }
            }
            else if (p.GetCurrentCell().GetCellType() != CellType.Finish)
            {
                Move move = new Move(diceNumber, p.getEndCell(diceNumber), p);
                moves.Add(move);
            }
        }

        return moves;
    }

    public bool AssignMoves(int diceNumber)
    {
        List<Move> moves = GetMoves(diceNumber);
        foreach (Move move in moves)
        {
            move.GetPawn().GetComponent<PawnMouseInteractions>().assignDestinationCell(move.GetFinishCell());
        }
        return moves.Count > 0;
    }

    public void ChooseMove(int diceNumber)
    {
        //StartCoroutine(ChooseMoveRoutine(diceNumber));


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
                home.ExitPawnFromHome(pawnToMove);
                return;
            }
            else
            {
                pawnToMove = GetPawnOutOfHome();
                pawnToMove.Move(diceNumber);
                return;
            }

        }
        else
        {
            foreach (Pawn p in pawns)
            {

                if (p.GetCurrentCell().GetCellType() == CellType.Start)
                {
                    p.Move(diceNumber);
                    return;
                }
            }
            if (GetPawnOutOfHome() != null)
            {
                GetPawnOutOfHome().Move(diceNumber);
                return;
            }
        }
    }

    public void clearPawnSuggestions()
    {
        foreach (Pawn p in pawns)
        {
            p.GetComponent<PawnMouseInteractions>().clearCell();
        }
    }

    public List<Pawn> GetPawns()
    {
        return pawns;
    }
}

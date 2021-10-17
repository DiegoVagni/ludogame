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
	private System.Random r;

	public string GetPhotonNickName() {
		if (photonPlayer != null)
		{
			return photonPlayer.NickName;
		}
		else {
			return playerNumber.ToString();
		}
	}
	//initialization
	public Player(Material mat, Photon.Realtime.Player photonPlayer = null)
	{
		r = new System.Random();
		this.mat = mat;
		playerNumber = players;
		this.photonPlayer = photonPlayer;
		players++;
	}
	// Destructor
	~Player()
	{
		players--;
	}
	//getter
	public Material GetMaterial()
	{
		return mat;
	}
	public int GetPlayerNumber()
	{
		return playerNumber;
	}
	public HomeCell GetHome()
	{
		return home;
	}
	public PlayerCell GetStart()
	{
		return (PlayerCell)home.GetConnectedCells()[CellIntersections.Next];
	}
	//setter
	public void SetHome(HomeCell home)
	{
		this.home = home;
	}
	public void SetPawns(List<Pawn> pawns)
	{
		this.pawns = pawns;

	}
	public void AddPawn(Pawn pawn)
	{
		if (pawns == null)
		{
			pawns = new List<Pawn>();
		}
		else if (pawns.Count > 4) {
			Debug.LogError("ci son troppy pawns per il player " + playerNumber + " ne ha ben " + pawns.Count);		
		}
		pawns.Add(pawn);

	}
	public List<List<Pawn>> GetFusedPawns()
	{
		List<List<Pawn>> result = null;
		foreach (Pawn p in pawns)
		{
			if (p.GetFused())
			{
				if (result == null)
				{
					result = new List<List<Pawn>>();
				}
				List<Pawn> couple = p.GetCurrentCell().GetPawnsInCell();
				Debug.Log(couple[0].GetPawnName() + " is married with " + couple[1].GetPawnName());
				result.Add(couple);
			}
		}
		return result;
	}

	//interaction
	public void StampPlayer()
	{
		//Debug.Log("Player " + playerNumber + " is known as " + photonPlayer.NickName);
	}

	public List<Move> GetMoves(int diceNumber)
	{
		List<Move> possibleMoves = new List<Move>();
		Debug.Log(diceNumber);
		if (diceNumber % 2 == 0)
		{
			Debug.Log("i throw a multiple of two");
			List<List<Pawn>> fusedPawns = GetFusedPawns();
			if (fusedPawns != null)
			{
				foreach (List<Pawn> couple in fusedPawns)
				{
					Move checkPossibleMove = couple[0].MoveCouple(diceNumber);
					if (checkPossibleMove != null)
					{
						possibleMoves.Add(checkPossibleMove);
					}
				}
			}
			else
			{
				Debug.Log("no fused pawns");
			}
		}
		foreach (Pawn p in pawns)
		{
			if (!p.GetFused())
			{
				Move possibleMove;
				switch (p.GetCurrentCell().GetCellType())
				{
					case CellType.Finish: break;
					case CellType.Home:
						if (diceNumber == 6 && GetStart().GetNumberOfPawns() < 2)
						{
							possibleMoves.Add(new Move(GetStart(), p));
						}
						break;
					default:
						possibleMove = p.Move(diceNumber);
						if (possibleMove != null)
						{

							possibleMoves.Add(possibleMove);
						}
						break;
				}
			}
		}
		return possibleMoves;
	}

	//logic
	public void ChooseMove(Move m)
	{
		
			foreach (Pawn p in m.GetPawn())
			{
				Debug.Log("choosen move: " + p.GetPawnName() + " wants to move to " + m.GetFinishCell() + " from " + p.GetCurrentCell().name);
				if (m.GetFinishCell().GetCellType() == CellType.Start && p.GetCurrentCell().GetCellType() == CellType.Home && m.GetFinishCell().GetNumberOfPawns() < 2)
				{
					home.ExitPawnFromHome(p);
				}
				else
				{
					p.MoveCoroutine(m.GetFinishCell());
				}
			}
	}
	
	public bool AssignMoves(int diceNumber)
	{
		List<Move> moves = GetMoves(diceNumber);
		foreach (Move move in moves)
		{
			if (move.GetFinishCell() != null)
			{
				move.GetPawn()[0].GetComponent<PawnMouseInteractions>().assignPossibleMove(move);
			}
		}
		return moves.Count > 0;
	}

	public Pawn GetPawn(string name) {
		Debug.LogError(name);
		foreach (Pawn p in pawns) {
			if (p.GetPawnName() == name) {
				return p;
			}
		}
		return null;
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

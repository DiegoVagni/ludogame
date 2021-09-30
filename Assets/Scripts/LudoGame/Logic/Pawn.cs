using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn
{
    private GameObject pawnGameObject;
    private Player player;
    private Cell currentCell;

	public Pawn(GameObject pawnGameObject, Player player, Cell currentCell)
	{
		this.pawnGameObject = pawnGameObject;
		this.player = player;
		this.currentCell = currentCell;
	}

	public GameObject GetPawn() {
		return pawnGameObject;
	}

	public Player GetPlayer()
	{
		return player;
	}
	public Cell GetCurrentCell() {
		return currentCell;
	}
}

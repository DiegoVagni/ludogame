using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
   
	private Cell finishCell;
	private List<Pawn> pawns;
	private bool alternateMove;
	public Move(Cell finishCell, Pawn pawn, Pawn secondPawn = null, bool alternateMove = false)
	{
		this.alternateMove = alternateMove;
		this.finishCell = finishCell;
		pawns = new List<Pawn>();
		pawns.Add(pawn);
		if (secondPawn != null) {
			pawns.Add(secondPawn);
		}
	}
	
	public Cell GetFinishCell() {
		return finishCell;
	}
	public void SetAlternateMoveToTrue() {
		alternateMove = true;
	}

	public List<Pawn> GetPawn() {
		return pawns;
	}
	public bool IsAlternateMove() {
		return alternateMove;
	}
	
}

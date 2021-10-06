using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
   
	private Cell finishCell;
	private List<Pawn> pawns;

	public Move(Cell finishCell, Pawn pawn, Pawn secondPawn = null)
	{
	
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

	public List<Pawn> GetPawn() {
		return pawns;
	}

	
}

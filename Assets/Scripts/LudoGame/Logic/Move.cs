using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    private int steps;
	private Cell finishCell;
	private Pawn pawn;

	public Move(int steps, Cell finishCell, Pawn pawn)
	{
		this.steps = steps;
		this.finishCell = finishCell;
		this.pawn = pawn;
	}

	public int GetSteps() {
		return steps;
	}

	public Cell GetFinishCell() {
		return finishCell;
	}

	public Pawn GetPawn() {
		return pawn;
	}

	
}

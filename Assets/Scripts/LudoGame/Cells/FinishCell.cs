using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishCell : PlayerCell
{
	private List<bool> occupaedSpawnPoints;
	public void Start() {
		occupaedSpawnPoints = new List<bool>();
	
		for (int i = 0; i < pawnPosition.Count; i++) {
			occupaedSpawnPoints.Add(false);
		}
	}
	public void EndPawn(Pawn pawn)
	{
		
		for (int i = 0; i < pawnPosition.Count; i++)
		{
			if (!occupaedSpawnPoints[i])
			{
				pawn.transform.position = pawnPosition[i].position;
				occupaedSpawnPoints[i] = true;
				pawn.GetCurrentCell().ExitPawn(pawn, this);
				break;
			}
		}
		if (CheckEndGame()){
			Debug.Log(pawn.GetPlayer().GetPlayerNumber() + " Ha vinto!");
			GM.EndGame();
		}
	}
	public bool CheckEndGame() {
		foreach (bool b in occupaedSpawnPoints) {
			if (!b)
			{
				return false;
			}
		}
		return true;
	}
}

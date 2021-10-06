using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeCell : PlayerCell {
	[SerializeField]
	private List<Pawn> pawnPrefab;
	

	public void SpawnPawns(Player player) {
		List<Pawn> pawns = new List<Pawn>();
		int index = 0;
		foreach (Transform s in pawnPosition) {
			Pawn pawn = Instantiate(pawnPrefab[player.GetPlayerNumber()-1], s.position, Quaternion.identity);
			//unico punto dove non son riuscito a far convergere le informazioni. ma � in inizializzazione quindi va bene
			pawn.Initialize(player, this, player.GetPlayerNumber() + "_"+index+"Pawn",index);
			pawn.GetPawn().AddComponent<PawnMouseInteractions>();
			pawnInCell.Add(pawn);

			
			pawn.transform.rotation = Quaternion.Euler(-90, 0, 0);
			
			pawns.Add(pawn);
			index++;
		}
		player.SetPawns(pawns);
	}


	public void SendPawnToHome(Pawn pawn) {
		pawn.transform.position = pawnPosition[pawn.GetPawnNumber()].position;
	}
	public void ExitPawnFromHome(Pawn pawn)
	{
		Debug.Log("exiting to home " + pawn.GetPawnName());
		pawn.MoveCoroutine(intersections[CellIntersections.Next]);
	}
}

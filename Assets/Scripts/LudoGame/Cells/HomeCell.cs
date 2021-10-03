using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeCell : PlayerCell {
	[SerializeField]
	private List<Pawn> pawnPrefab;
	
	private List<bool> occupaedSpawnPoints;
	public void SpawnPawns(Player player) {
		List<Pawn> pawns = new List<Pawn>();
		occupaedSpawnPoints = new List<bool>();
		foreach (Transform s in pawnPosition) {
			Pawn pawn = Instantiate(pawnPrefab[player.GetPlayerNumber()-1], s.position, Quaternion.identity);
			//unico punto dove non son riuscito a far convergere le informazioni. ma è in inizializzazione quindi va bene
			pawn.Initialize(player, this);
			pawn.GetPawn().AddComponent<PawnMouseInteractions>();
			pawnInCell.Add(pawn);

			
			pawn.transform.rotation = Quaternion.Euler(-90, 0, 0);
			
			pawns.Add(pawn);
			occupaedSpawnPoints.Add(true);
		}
		player.SetPawns(pawns);
	}
	
	public void SendPawnToHome(Pawn pawn) {

		for (int i = 0; i < pawnPosition.Count; i++) {
			if (!occupaedSpawnPoints[i]) { 
			pawn.transform.position = pawnPosition[i].position;
				occupaedSpawnPoints[i] = true;
				pawn.ReturnToHome(this);
			}		
		} 
	}
	public void ExitPawnToHome(Pawn pawn)
	{
		for (int i = 0; i < pawnPosition.Count; i++) {
			if (pawn == pawnInCell[i]) {
				pawn.Move(1);
				occupaedSpawnPoints[i] = false;
				break;
			}
		}
	}
}

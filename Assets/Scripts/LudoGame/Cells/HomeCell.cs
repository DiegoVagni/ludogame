using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeCell : PlayerCell {
	[SerializeField]
	private List<Pawn> pawnPrefab;
	[SerializeField]
	private List<GameObject> spawnPoints;
	private List<bool> occupaedSpawnPoints;
	public void SpawnPawns(Player player) {
		List<Pawn> pawns = new List<Pawn>();
		occupaedSpawnPoints = new List<bool>();
		foreach (GameObject s in spawnPoints) {
			Pawn pawn = Instantiate(pawnPrefab[player.GetPlayerNumber()-1], s.transform.position, Quaternion.identity);
			pawn.Initialize(player, this);
			pawn.transform.rotation = Quaternion.Euler(-90, 0, 0);
			pawns.Add(pawn);
			occupaedSpawnPoints.Add(true);
		}
		player.SetPawns(pawns);
	}

	public void SendPawnToHome(Pawn pawn) {

		for (int i = 0; i < spawnPoints.Count; i++) {
			if (!occupaedSpawnPoints[i]) { 
			pawn.GetPawn().transform.position = spawnPoints[i].transform.position;
				occupaedSpawnPoints[i] = true;
				pawn.SetCurrentCell(this);
			}		
		} 
	}
	public void ExitPawnToHome(Pawn pawn)
	{
		for (int i = 0; i < spawnPoints.Count; i++) {
			if (occupaedSpawnPoints[i]) {
				pawn.Move(1);
				occupaedSpawnPoints[i] = false;
				break;
			}
		}
	}
}

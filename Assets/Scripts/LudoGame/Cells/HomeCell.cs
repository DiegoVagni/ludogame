using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeCell : PlayerCell {
	[SerializeField]
	private List<GameObject> pawnPrefab;
	[SerializeField]
	private List<GameObject> spawnPoints;
	private List<bool> occupaedSpawnPoints;
	public void SpawnPawns(Player player) {
		List<Pawn> pawns = new List<Pawn>();
		occupaedSpawnPoints = new List<bool>();
		foreach (GameObject s in spawnPoints) {
			GameObject pawn = Instantiate(pawnPrefab[player.GetPlayerNumber()-1], s.transform.position, Quaternion.identity);
			pawn.transform.rotation = Quaternion.Euler(-90, 0, 0);
			pawns.Add(new Pawn(pawn,player,this));
			occupaedSpawnPoints.Add(true);
		}
		player.SetPawns(pawns);
	}

	public void SendPawnToHome(Pawn pawn) {

		for (int i = 0; i < spawnPoints.Count; i++) {
			if (!occupaedSpawnPoints[i]) { 
			pawn.GetPawn().transform.position = spawnPoints[i].transform.position;
				occupaedSpawnPoints[i] = true;
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

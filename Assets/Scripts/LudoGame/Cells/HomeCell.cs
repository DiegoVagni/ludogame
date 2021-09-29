using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeCell : PlayerCell {
	[SerializeField]
	private List<GameObject> playerPrefab;
	[SerializeField]
	private List<GameObject> spawnPoints;

	public void SpawnPawns(Player player) {
		foreach (GameObject s in spawnPoints) {
			GameObject pawn = Instantiate(playerPrefab[player.GetPlayerNumber()-1], s.transform.position, Quaternion.identity);
			pawn.transform.rotation = Quaternion.Euler(-90, 0, 0);
		}
	}
}

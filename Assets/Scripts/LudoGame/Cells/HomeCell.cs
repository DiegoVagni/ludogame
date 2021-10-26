using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeCell : PlayerCell{
	[SerializeField]
	private List<Pawn> pawnPrefab;


	public void SpawnPawns(Player player)
	{
		List<Pawn> pawns = new List<Pawn>();
		int index = 0;
		foreach (Transform s in pawnPosition)
		{
			Pawn pawn;
			if (PhotonNetwork.IsMasterClient)
			{


				pawn = PhotonNetwork.Instantiate(pawnPrefab[player.GetPlayerNumber() - 1].name, s.position, Quaternion.identity, 0, new object[] { player.GetPlayerNumber() , index }).GetComponent<Pawn>();
				//unico punto dove non son riuscito a far convergere le informazioni. ma � in inizializzazione quindi va bene

				pawn.Initialize(player, this, player.GetPlayerNumber() + "_" + index + "Pawn", index);
				pawn.GetPawn().AddComponent<PawnMouseInteractions>();
				pawnInCell.Add(pawn);

				pawns.Add(pawn);
				index++;
			}
		}
		if (PhotonNetwork.IsMasterClient)
		{
			player.SetPawns(pawns);

		}
	}
	public void AddPawn(Pawn p) {
		pawnInCell.Add(p);
		
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

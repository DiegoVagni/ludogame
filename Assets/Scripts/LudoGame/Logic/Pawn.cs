using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{

	private Rigidbody pawnRigidBody;
    private Player player;
    private Cell currentCell;


	public void Initialize(Player player, Cell currentCell) {
		pawnRigidBody = GetComponent<Rigidbody>();
		this.player = player;
		this.currentCell = currentCell;
	}
	public GameObject GetPawn() {
		return gameObject;
	}

	public Player GetPlayer()
	{
		return player;
	}
	public Cell GetCurrentCell() {
		return currentCell;
	}
	public void SetCurrentCell(Cell cell) {
		currentCell = cell;
	}
	public IEnumerator MovePawn(int steps) {
		Cell endCell = currentCell;

		for (int i = 0; i < steps; i++)
		{
			endCell = endCell.GetNextCell();
			
		}
		yield return new WaitForEndOfFrame();
		RaycastHit hit;
		if (Physics.Raycast(endCell.transform.position, Vector3.up, out hit, 90))
		{ 
				
			if (hit.collider.gameObject.tag == "Pawns") {
				Pawn other = hit.collider.gameObject.GetComponent<Pawn>();
				if (other.GetPlayer().GetPlayerNumber() != player.GetPlayerNumber())
				{
					Debug.Log("player " + player.GetPlayerNumber() + " has eaten a pawn of " + other.GetPlayer().GetPlayerNumber());
					other.GetPlayer().GetHome().SendPawnToHome(other);
				}
				else {
					Debug.Log("fu-sio-ne!");
					Debug.Log("forse(?)");
					Debug.Log("ok è in wip");
				}
				Debug.Log(hit.collider.gameObject.GetComponent<Pawn>().GetPlayer().GetPlayerNumber());
			}
		}
		yield return new WaitForEndOfFrame();

			pawnRigidBody.MovePosition(endCell.transform.position + new Vector3(0, 1, 0));
			currentCell = endCell;
		
	}
	
	public void Move(int steps) {
		StartCoroutine(MovePawn(steps));
		
	}
}

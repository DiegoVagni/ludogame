using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{

	private Rigidbody pawnRigidBody;
	private Player player;
	private Cell currentCell;


	public void Initialize(Player player, Cell currentCell)
	{
		pawnRigidBody = GetComponent<Rigidbody>();
		this.player = player;
		this.currentCell = currentCell;
	}
	public GameObject GetPawn()
	{
		return gameObject;
	}

	public Player GetPlayer()
	{
		return player;
	}
	public Cell GetCurrentCell()
	{
		return currentCell;
	}
	public void SetCurrentCell(Cell cell)
	{
		currentCell = cell;
	}

	public IEnumerator MovePawn(int steps)
	{
		Cell endCell = currentCell;
		for (int i = 0; i < steps; i++)
		{
			if (endCell.GetCellType() == CellType.Junction && ((PlayerCell)endCell).GetPlayer().GetPlayerNumber() == player.GetPlayerNumber())
			{
				Debug.Log("entrando nella junction con " + player.GetPlayerNumber());
				endCell = endCell.GetConnectedCells()[CellIntersections.Color];
				Debug.Log(endCell.GetCellType());
			}
			else
			{
				if (endCell.GetCellType() == CellType.Finish) {
					break;
				}
				endCell = endCell.GetNextCell();
			}

		}
		if (endCell.GetCellType() == CellType.Finish)
		{
			((FinishCell)endCell).EndPawn(this);
		}
		else
		{
			Pawn pawnInCell = endCell.PawnInCell();
			if (pawnInCell != null)
			{
				Player otherPawnPlayer = pawnInCell.GetPlayer();
				if (otherPawnPlayer.GetPlayerNumber() != GetPlayer().GetPlayerNumber())
				{
					if (pawnInCell.GetCurrentCell().GetCellType() == CellType.Safe && ((PlayerCell)pawnInCell.GetCurrentCell()).GetPlayer().GetPlayerNumber() == GetPlayer().GetPlayerNumber())
					{
						Debug.Log(GetPlayer().GetPlayerNumber() + " ha provato a Gnammete " + otherPawnPlayer.GetPlayerNumber() + " ma ha fallito perchè è nella safeZone quindi perde il turno");
					}
					else
					{

						Debug.Log(GetPlayer().GetPlayerNumber() + " Gnammete " + otherPawnPlayer.GetPlayerNumber());

						otherPawnPlayer.GetHome().SendPawnToHome(pawnInCell);
						pawnRigidBody.MovePosition(endCell.GetPawnPositions()[0].position);
					}

				}
				else
				{

					Debug.Log(GetPlayer().GetPlayerNumber() + " FUSIONE!");
					pawnInCell.GetComponent<Rigidbody>().MovePosition(endCell.GetPawnPositions()[1].position);
					pawnRigidBody.MovePosition(endCell.GetPawnPositions()[2].position);

					currentCell.ExitPawn(this, endCell);
				}
			}
			else
			{

				pawnRigidBody.MovePosition(endCell.GetPawnPositions()[0].position);
				currentCell.ExitPawn(this, endCell);
			}
			/*yield return new WaitForEndOfFrame();
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
			}*/
		}
		yield return new WaitForEndOfFrame();


	}
	public void ReturnToHome(Cell home)
	{
		currentCell.ExitPawn(this, home);
	}
	public void Move(int steps)
	{
		StartCoroutine(MovePawn(steps));

	}
}

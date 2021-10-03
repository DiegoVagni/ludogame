using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Field : MonoBehaviour
{
	[SerializeField]
	private Cell cellPrefab;
	[SerializeField]
	private PlayerCell playerCellPrefab;
	[SerializeField]
	private PlayerCell finishCell;
	[SerializeField]
	private HomeCell homeCell;
	[SerializeField]
	private float cellDistance;

	[SerializeField]
	private List<ScriptableSegment> segmentInstructions;
	[SerializeField]
	private GM gameManager;
	[SerializeField]
	private List<ScriptableSpecCell> specialCells;
	[SerializeField]
	private List<ScriptableSegment> middleSegment;
	private List<Cell> path;
	private List<PlayerCell> start;
	private List<PlayerCell> safes;

	//needs heavy refactory when i want
	// Start is called before the first frame update
	void Start()
	{
		BuildPath();
	}


	private void BuildPath()
	{
		start = new List<PlayerCell>();
		path = new List<Cell>();
		safes = new List<PlayerCell>();
		List<PlayerCell> junctions = new List<PlayerCell>();
		Player currentPlayer = gameManager.GetPlayers()[0];
		ScriptableSpecCell nextCell = specialCells[0];
		
		int specIndex = 1;
		int pc = path.Count;
		int lastIndex = pc - 1;
		foreach ((ScriptableSegment value, int index) in segmentInstructions.Select((item, index) => (item, index)))
		{
			Vector3 pos;
			if (pc > 0)
			{
				if (value.GetMoveDiagonal())
				{
					pos = path[lastIndex].GetPosition() + value.GetAxis() + segmentInstructions[index - 1].GetAxis() + (value.GetAxis() + segmentInstructions[index - 1].GetAxis()) * cellDistance;
				}
				else
				{
					pos = path[lastIndex].GetPosition() + value.GetAxis() + value.GetAxis() * cellDistance;
				}
			}
			else
			{
				pos = value.GetCellIndex();
			}
			for (int i = 0; i < value.GetNumberOfCells(); i++)
			{
				Cell instantiatedCell = null;
				if (path.Count == nextCell.GetPosition())
				{
					PlayerCell c;
					c = Instantiate(playerCellPrefab, pos, Quaternion.identity);
					c.SetPlayer(currentPlayer);
					switch (nextCell.GetCellType())
					{
						case CellType.Junction:
							junctions.Add(c);
							c.SetType(CellType.Junction);
							break;
						case CellType.Start:
							c.GetComponent<MeshRenderer>().material = currentPlayer.GetMaterial();
							start.Add(c);
							c.SetType(CellType.Start);
							break;
						case CellType.Safe:
							c.GetComponent<MeshRenderer>().material = currentPlayer.GetMaterial();
							currentPlayer = gameManager.GetPlayers()[(gameManager.GetPlayers().IndexOf(currentPlayer) + 1) % gameManager.GetPlayers().Count];

							c.SetType(CellType.Safe);
							safes.Add(c);
							break;
						default:
							Debug.LogError("default case in field.cs");
							break;
					}
					if (specialCells.Count > specIndex)
					{
						nextCell = specialCells[specIndex];
						specIndex++;
					}
					instantiatedCell = c;
				}
				if (instantiatedCell == null)
				{
					instantiatedCell = Instantiate(cellPrefab, pos, Quaternion.identity);
				}
				instantiatedCell.transform.SetParent(transform);

				if (path.Count > 0)
				{
					Cell lastCell = path[lastIndex];
					lastCell.AddIntersection(CellIntersections.Next, instantiatedCell);
					instantiatedCell.AddIntersection(CellIntersections.Prev, lastCell);
				}
				path.Add(instantiatedCell);
				pc++;
				lastIndex++;

				pos += value.GetAxis() + value.GetAxis() * cellDistance;
			}

		}

		path[lastIndex].AddIntersection(CellIntersections.Next, path[0]);
		path[0].AddIntersection(CellIntersections.Prev, path[lastIndex]);
		int playerIndex = 0;
		foreach (PlayerCell j in junctions)
		{
			currentPlayer = gameManager.GetPlayers()[playerIndex];
			ScriptableSegment ms = middleSegment[junctions.IndexOf(j)];
			Vector3 pos = j.GetPosition() + ms.GetAxis() + ms.GetAxis() * cellDistance;
			for (int i = 0; i < ms.GetNumberOfCells(); i++)
			{
				PlayerCell c = Instantiate(playerCellPrefab, pos, Quaternion.identity);
				c.transform.SetParent(transform);
				c.GetComponent<MeshRenderer>().material = currentPlayer.GetMaterial();
				path.Add(c);
				if (i == 0)
				{
					j.AddIntersection(CellIntersections.Color, c);
					c.AddIntersection(CellIntersections.Prev, j);
				}
				else
				{
					c.AddIntersection(CellIntersections.Prev, path[path.Count - 1]);
					path[path.Count - 1].AddIntersection(CellIntersections.Next, c);
				}
				pos = path[path.Count - 1].GetPosition() + ms.GetAxis() + ms.GetAxis() * cellDistance;
			}
			bool settedHome = false;
			foreach (PlayerCell s in start)
			{
				foreach (PlayerCell saf in safes)
				{
					if (currentPlayer.GetPlayerNumber() == s.GetPlayer().GetPlayerNumber() && s.GetPlayer().GetPlayerNumber() == saf.GetPlayer().GetPlayerNumber())
						if (currentPlayer.GetPlayerNumber() == s.GetPlayer().GetPlayerNumber())
						{
							switch (playerIndex % 4) {
								case 0: 
									pos = new Vector3(s.transform.position.x+4, 0,  saf.transform.position.z+4);
									break;
								case 1:
									pos = new Vector3( s.transform.position.x-1.5f, 0, saf.transform.position.z+0.4f);
									break;
								case 2:
									pos = new Vector3( s.transform.position.x-4, 0, saf.transform.position.z-4);
									break;
								case 3:
									pos = new Vector3( s.transform.position.x+1.5f, 0,  saf.transform.position.z-0.4f);
									break;
								default:
									Debug.Log("will come...");
									break;
							}
							
							
							settedHome = true;
							HomeCell home = Instantiate(homeCell, pos, Quaternion.identity);
							home.SpawnPawns(currentPlayer);
							home.GetComponent<MeshRenderer>().material = currentPlayer.GetMaterial();
							home.AddIntersection(CellIntersections.Next, s);
							s.AddIntersection(CellIntersections.Prev, home);
							currentPlayer.SetHome(home);
							break;
						}
				}
				if (settedHome) {
					break;
				}
			}
			//adding player cell
			//lo schifo ma è semplice così
			pos = path[path.Count - 1].GetPosition() + ms.GetAxis() + ms.GetAxis() * (cellDistance + 0.15f);
			Quaternion rotation = Quaternion.Euler(90, 90 * ((playerIndex + 2) % 4), 0);
			PlayerCell finish = Instantiate(finishCell, pos, rotation);
			finish.transform.SetParent(transform);

			finish.GetComponent<MeshRenderer>().material = currentPlayer.GetMaterial();
			finish.AddIntersection(CellIntersections.Prev, path[path.Count - 1]);
			path[path.Count - 1].AddIntersection(CellIntersections.Next, finish);
			path.Add(finish);

			playerIndex++;

		}

	}

	// Update is called once per frame
	
}

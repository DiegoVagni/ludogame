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
					switch (nextCell.GetCellType()) {
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
							break;
						default: Debug.LogError("default case in field.cs");
							break;
					}
					if (specialCells.Count > specIndex)
					{
						nextCell = specialCells[specIndex];
						specIndex++;
					}
					instantiatedCell = c;
				}
				if (instantiatedCell == null) { 
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
		foreach (PlayerCell j in junctions) {
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
				else {
					c.AddIntersection(CellIntersections.Prev, path[path.Count - 1]);
					path[path.Count-1].AddIntersection(CellIntersections.Next,c);
				}
				pos = path[path.Count - 1].GetPosition() + ms.GetAxis() + ms.GetAxis() * cellDistance;
			}
			//adding player cell
			PlayerCell home = Instantiate(playerCellPrefab, pos, Quaternion.identity);
			home.transform.SetParent(transform);
			home.GetComponent<MeshRenderer>().material = currentPlayer.GetMaterial();
			home.AddIntersection(CellIntersections.Prev, path[path.Count - 1]);
			path[path.Count - 1].AddIntersection(CellIntersections.Next, home);
			path.Add(home);
			playerIndex++;

		}

	}

	// Update is called once per frame
	void Update()
	{
	
		
	}
}

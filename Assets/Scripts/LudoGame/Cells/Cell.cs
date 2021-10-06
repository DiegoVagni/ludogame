using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField]
    private CellType type = CellType.Normal;
    [SerializeField]
    protected List<Transform> pawnPosition;
    //le celle hanno una referenza al pedone per facilitare il posizionamento.
    protected List<Pawn> pawnInCell;
    protected Dictionary<CellIntersections, Cell> intersections;

    public void Awake() {
        intersections =  new Dictionary<CellIntersections, Cell>();
        pawnInCell = new List<Pawn>();
      
    }
    public void AddIntersection(CellIntersections key, Cell intersection) {
        intersections[key] = intersection;
    }

    public Dictionary<CellIntersections, Cell> GetConnectedCells() {
        return intersections;
    }

    public Vector3 GetPosition() {
        return transform.position;
    }
    public Transform GetSinglePawnPosition() {
        return pawnPosition[0];
    }
    public void ExitPawn(Pawn p, Cell finalCell = null) {
        pawnInCell.Remove(p);
     
        PositionPawnsInCell();

        if (finalCell == null)
        {
            GetNextCell().EnterPawn(p);
        }
        else {
            finalCell.EnterPawn(p);
        }
    }
    public int GetNumberOfPawns() {
        return pawnInCell.Count;
    }
    public List<Transform> GetPawnPositions() {
        return pawnPosition;
    }

    private void EnterPawn(Pawn p) {
        p.SetCurrentCell(this);
        pawnInCell.Add(p);
        PositionPawnsInCell();
    }
    protected void PositionPawnsInCell() {
        if(type != CellType.Home) { 
        if (pawnInCell.Count == 1)
        {
            pawnInCell[0].GetRigidbody().MovePosition(pawnPosition[0].position);
        }
        else if(pawnInCell.Count == 2)
        {
            Pawn currentPawn;
            //trustami, è settato così da editor
            for (int i = 0; i < 2; i++)
            {
                currentPawn = pawnInCell[i];
                currentPawn.GetRigidbody().MovePosition(pawnPosition[i + 1].position);
            }
        }
        }
    }
    public CellType GetCellType() {
        return type;
    }
    public void SetType(CellType t) {
        type = t;
    }
    public List<Pawn> GetPawnsInCell() {
        return pawnInCell;
    }
	internal Cell GetNextCell()
	{
        return intersections[CellIntersections.Next];
	}
}

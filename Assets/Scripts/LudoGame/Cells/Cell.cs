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
    //se sono 2 pawn nella stessa cella son per forza dello stesso giocatore quindi per ora basta tornarne uno
    public Pawn PawnInCell() {
        if (pawnInCell.Count == 0) {
            return null;
        }
        return pawnInCell[0];
    }
    private void EnterPawn(Pawn p) {
        p.SetCurrentCell(this);
        pawnInCell.Add(p);
    }
    public CellType GetCellType() {
        return type;
    }
    public void SetType(CellType t) {
        type = t;
    }

	internal Cell GetNextCell()
	{
        return intersections[CellIntersections.Next];
	}
}

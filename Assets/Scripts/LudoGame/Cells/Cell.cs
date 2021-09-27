using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private CellType type = CellType.Normal;

    Dictionary<CellIntersections, Cell> intersections;
    public void Awake() {
        intersections =  new Dictionary<CellIntersections, Cell>();
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

    public CellType GetCellType() {
        return type;
    }
    public void SetType(CellType t) {
        type = t;
    }
}

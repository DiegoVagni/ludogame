using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Segment", menuName = "ScriptableObject/Segment", order = 1)]
public class ScriptableSegment : ScriptableObject
{
    [SerializeField]
    private Vector3 cellIndex;
    [SerializeField]
    private int numberOfCells;
    [SerializeField]
    private Vector3 axis;
    [SerializeField]
    private bool moveDiagonal;
    public Vector3 GetCellIndex() {
        return cellIndex;
    }

    public Vector3 GetAxis()
    {
        return axis;
    }

    public int GetNumberOfCells() {
        return numberOfCells;
    }

    public bool GetMoveDiagonal() {
        return moveDiagonal;
    }
}

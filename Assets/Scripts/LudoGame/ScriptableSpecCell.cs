using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableSpecCell", menuName = "ScriptableObject/ScriptableSpecCell", order = 2)]
public class ScriptableSpecCell : ScriptableObject
{
    [SerializeField]
    private int position;
    [SerializeField]
    private CellType type;

    public int GetPosition()
    {
        return position;
    }


    public CellType GetCellType()
    {
        return type;
    }

}
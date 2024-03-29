using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{

    private Rigidbody pawnRigidBody;
    private Player player;
    private Cell currentCell;
    private bool fused = false;
    private string pawnName;
    private int pawnNumber;
    //initialization
    public void Initialize(Player player, Cell currentCell, string pawnName, int pawnNumber)
    {
        pawnRigidBody = GetComponent<Rigidbody>();
        this.player = player;
        this.currentCell = currentCell;
        this.pawnName = pawnName;
        this.pawnNumber = pawnNumber;
    }
    //getter
    public int GetPawnNumber()
    {
        return pawnNumber;

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
    public bool GetFused()
    {
        return fused;
    }
    public string GetPawnName()
    {
        return pawnName;
    }

    public Rigidbody GetRigidbody()
    {
        return pawnRigidBody;
    }

    public int GetPlayerNumber()
    {
        return player.GetPlayerNumber();
    }
    public Cell GetEndCell(int steps)
    {
        Cell nextCell = currentCell;
        for (int i = 0; i < steps; i++)
        {
            if (!fused)
            {
                if (nextCell.GetNumberOfPawns() == 2)
                {
                    if (nextCell.GetPawnsInCell()[0].GetPlayerNumber() != GetPlayerNumber() || steps - 1 == i)
                    {
                        return null;
                    }
                }
                if (nextCell.GetNumberOfPawns() != 0 && nextCell.GetCellType() == CellType.Safe && ((PlayerCell)nextCell).GetPlayer().GetPlayerNumber() != GetPlayerNumber())
                {
                    return null;
                }
            }
            else
            {
                if (steps == i - 1)
                {
                    if ((nextCell.GetNumberOfPawns() > 0 && (nextCell.GetPawnsInCell()[0].GetPlayerNumber() == GetPlayerNumber()) || (nextCell.GetCellType() == CellType.Safe && ((PlayerCell)nextCell).GetPlayer().GetPlayerNumber() != GetPlayerNumber())))
                    {
                        return null;
                    }
                }

            }
            if (nextCell.GetCellType() == CellType.Junction && ((PlayerCell)nextCell).GetPlayer().GetPlayerNumber() == player.GetPlayerNumber())
            {
                Debug.Log("entrando nella junction con " + pawnName);
                nextCell = nextCell.GetConnectedCells()[CellIntersections.Color];
                Debug.Log(nextCell.GetCellType());
            }
            else
            {
                if (nextCell.GetCellType() == CellType.Finish)
                {
                    return nextCell;
                }
                nextCell = nextCell.GetConnectedCells()[CellIntersections.Next];
            }
        }
        return nextCell;
    }

    //setter
    public void SetCurrentCell(Cell cell)
    {
        currentCell = cell;
    }
    public void SetFused(bool fused)
    {
        this.fused = fused;
    }

    //logic
    public Move Move(int steps)
    {
        Cell endCell = GetEndCell(steps);
        if (endCell == null) return null;
        return new Move(endCell, this);
    }

    public Move MoveCouple(int steps)
    {
        steps = Mathf.CeilToInt((steps / 2));
        Cell endCell = GetEndCell(steps);
        if (endCell == null) return null;
        List<Pawn> thisCouple = currentCell.GetPawnsInCell();
        return new Move(endCell, thisCouple[0], thisCouple[1]);
    }

    public void SendHome()
    {
        player.GetHome().SendPawnToHome(this);
        ReturnToHome();
    }
    public void ReturnToHome()
    {
        fused = false;
        currentCell.ExitPawn(this, player.GetHome());
    }
    //coroutine
    public IEnumerator MovePawn(Cell endCell)
    {

        if (endCell.GetCellType() == CellType.Finish)
        {
            ((FinishCell)endCell).EndPawn(this);
        }
        else
        {
            List<Pawn> pawnsInCell = endCell.GetPawnsInCell();
            if (pawnsInCell.Count > 0)
            {
                //bug, magari il pawn � in posizione 1...
                Player otherPawnPlayer = pawnsInCell[0].GetPlayer();
                bool somethingToEat = false;
                foreach (Pawn p in pawnsInCell)
                {
                    if (p.GetPlayer() != GetPlayer())
                    {
                        somethingToEat = true;
                        break;
                    }
                }
                if (somethingToEat)
                {

                    //cos� non si lamenta che modifico na collezione nel foreach
                    List<Pawn> pawnsInCellCopy = new List<Pawn>(pawnsInCell);
                    foreach (Pawn p in pawnsInCellCopy)
                    {
                        if (p.GetPlayerNumber() != GetPlayerNumber())
                        {
                            Debug.Log(pawnName + " Gnammete " + p.GetPawnName());
                            p.SendHome();
                        }
                    }
                    pawnRigidBody.MovePosition(endCell.GetPawnPositions()[0].position);
                }
                else
                {

                    Pawn pawnInCell = pawnsInCell[0];
                    pawnInCell.GetComponent<Rigidbody>().MovePosition(endCell.GetPawnPositions()[1].position);
                    pawnRigidBody.MovePosition(endCell.GetPawnPositions()[2].position);
                    currentCell.ExitPawn(this, endCell);
                    if (endCell.GetCellType() != CellType.Home)
                    {
                        Debug.Log(pawnName + " FUSIONE!");
                        SetFused(true);
                        pawnInCell.SetFused(true);
                    }

                }

            }
            else
            {
                currentCell.ExitPawn(this, endCell);
            }
        }
        yield return new WaitForEndOfFrame();
    }


    public void MoveCoroutine(Cell finishCell)
    {
        StartCoroutine(MovePawn(finishCell));

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BGEditor.NodeSystem;

public class MovementController : MonoBehaviour {

    public Tester Tester;
    public int PointsOfMovement = 6;

    /// <summary>
    /// Calcola le celle in cui è possibile muoversi
    /// </summary>
    /// <returns>Le celle raggiungibili dalla pedina</returns>
    public List<Cell> EvaluateMovementPown()
    {
        List<Cell> possibleMovement = new List<Cell>();
        List<INode> tempList = new List<INode>();

        Cell startCell = Tester.WorldToGridPosition(transform.position);
        if (PointsOfMovement >= 0)
        {
            possibleMovement.Add(startCell);
        }
        else
            return null;

        for (int i = 0; i < PointsOfMovement; i++)
        {
            for (int j = 0; j < possibleMovement.Count; j++)
            {
                tempList.AddRange(possibleMovement[j].GetNeighbourgs());
            }

            for (int k = 0; k < tempList.Count; k++)
            {
                if(!possibleMovement.Contains(tempList[k] as Cell))
                {
                    possibleMovement.Add(tempList[k] as Cell);
                }
            }
        }

        return possibleMovement;
    }
}

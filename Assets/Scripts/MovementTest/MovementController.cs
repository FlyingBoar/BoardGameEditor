using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid;

public class MovementController : MonoBehaviour {

    public int PointsOfMovement = 6;
    public GridController TESTController;
    /// <summary>
    /// Calcola le celle in cui è possibile muoversi
    /// </summary>
    /// <returns>Le celle raggiungibili dalla pedina</returns>
    public List<Vector3Int> EvaluateMovementPown()
    {
        List<Vector3Int> possibleMovement = new List<Vector3Int>();
        List<Vector3Int> tempList = new List<Vector3Int>();

        //TODO: check con Luca/Fulvio
        //Vector3Int startCell = MasterGrid.GetCoordinatesByPosition(transform.position);
        //if (PointsOfMovement >= 0)
        //{
        //    possibleMovement.Add(startCell);
        //}
        //else
        //    return null;

        Debug.LogError("Funzionalità rimossa durante il refactoring del sistema a Layer e la rimozione della Matrice di celle");

        //for (int i = 0; i < PointsOfMovement; i++)
        //{
        //    for (int j = 0; j < possibleMovement.Count; j++)
        //    {
        //        List<Vector3Int> cells = new List<Vector3Int>();
        //        foreach (var item in MasterGrid.GetNeighbours()
        //        {
        //            cells.Add(MasterGrid.GetCellByCoordinates(item));
        //        }
        //        tempList.AddRange(cells);
        //    }

        //    for (int k = 0; k < tempList.Count; k++)
        //    {
        //        if(!possibleMovement.Contains(tempList[k] as Cell))
        //        {
        //            possibleMovement.Add(tempList[k] as Cell);
        //        }
        //    }
        //}

        return possibleMovement;
    }

    public void Drag(Vector3 _position)
    {
        transform.position = _position;
    }

    public void Drop(Vector3 _position)
    {
        transform.position = _position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BGEditor.NodeSystem;

public class Tester : MonoBehaviour {

    
    public NodeNetworkData NetworkData;

    List<Cell> cells = new List<Cell>();

	// Use this for initialization
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
            PopulateGrid();
        if (Input.GetKeyDown(KeyCode.A))
            ClearGrid();

    }


    public void PopulateGrid()
    {
        for (int i = 0; i < Mathf.Sqrt(NetworkData.Cells.Count); i++)
        {
            for (int j = 0; j < Mathf.Sqrt(NetworkData.Cells.Count); j++)
            {
                Cell tempCell = new Cell(NetworkData.Cells[i + j]);
                tempCell.SetPosition(new Vector3(i * tempCell.GetRadius(), 0, j * tempCell.GetRadius()));
                cells.Add(tempCell);
            }
        }
    }

    public void ClearGrid()
    {
        cells.Clear();
    }

    private void OnDrawGizmos()
    {
        if (cells.Count <= 0)
            return;

        Gizmos.color = Color.cyan;
        foreach (Cell item in cells)
        {
            Gizmos.DrawWireCube(item.GetCenter(), Vector3.one * item.GetRadius());
        }
    }
}

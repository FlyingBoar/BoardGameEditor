using System.Collections;
using Grid;
using UnityEngine;


[ExecuteInEditMode]
public class Tester : MonoBehaviour
{
    public GridController GridCtrl;

	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GridCtrl.CreateNewGrid();
        }
        if (Input.GetKeyDown(KeyCode.D))
            GridCtrl.ClearGrid();
        if (Input.GetKeyDown(KeyCode.S))
            GridCtrl.SaveCurrent();
        if (Input.GetKeyDown(KeyCode.L))
            GridCtrl.Load();
        if (Input.GetKeyDown(KeyCode.T))
        {
            GridScanner scaner = new GridScanner(GridCtrl.GetListOfCells(), GridCtrl.SectorData);
            scaner.ScanGrid();
        }
    }
}

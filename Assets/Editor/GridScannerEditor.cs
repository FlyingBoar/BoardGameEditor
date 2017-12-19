using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Grid
{
    [CustomEditor(typeof(GridScanner)), CanEditMultipleObjects]
    public class GridScannerEditor : Editor
    {
        GridScanner scanner;

        private void OnEnable()
        {
            scanner = (GridScanner)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Scan Grid"))
                ScanGrid();
        }

        void ScanGrid()
        {
            GridController gridCtrl = scanner.GetComponent<GridController>();
            if (gridCtrl != null)
                scanner.ScanGrid(gridCtrl.GetListOfCells(), gridCtrl.SectorData);
        }
    }
}


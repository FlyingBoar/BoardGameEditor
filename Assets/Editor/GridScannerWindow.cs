using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Grid
{
    public class GridScannerWindow
    {
        GridScanner scanner;
        GridController gridCtrl;

        public GridScannerWindow(GridScanner _scanner, GridController _gridCtrl)
        {
            scanner = _scanner;
            gridCtrl = _gridCtrl;
        }

        public void Show()
        {
            EditorGUILayout.BeginVertical("Box");

            if (GUILayout.Button("Scan Grid"))
                scanner.ScanGrid(gridCtrl);

            EditorGUILayout.EndVertical();
        }
    }
}


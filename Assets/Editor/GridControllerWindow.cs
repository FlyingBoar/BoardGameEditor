using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Grid
{
    public class GridControllerWindow
    {
        GridController gridCtrl;
        TextAsset fileToLoad;

        private Vector3Int _selectedCoordinates;
        public Vector3Int SelectedCoordinates
        {
            get { return _selectedCoordinates; }
            private set
            {
                _selectedCoordinates = value;
                MasterGridWindow.GridVisualizer.SelectedCell = _selectedCoordinates;
            }
        }

        public MouseActions CurrentMouseAction { get; private set; }

        [SerializeField]
        Vector2 scrollPosition;

        public GridControllerWindow(GridController _gridCtrl)
        {
            gridCtrl = _gridCtrl;
        }

        public void Show()
        {
            EditorGUILayout.BeginVertical("Box");
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            EditorGUILayout.BeginVertical("Box");
            GUILayout.Label("Sector Data", EditorStyles.boldLabel);
            EditorGUI.indentLevel = 1;   
            gridCtrl.GridData.SectorData.Shape = (CellData.AreaShape)EditorGUILayout.EnumPopup("Area Shape", gridCtrl.SectorData.Shape);
            gridCtrl.GridData.SectorData.Radius = EditorGUILayout.Vector3Field("Radius", gridCtrl.SectorData.Radius);
            EditorGUI.indentLevel = 0;
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("Box");
            GUILayout.Label("Grid Data", EditorStyles.boldLabel);
            EditorGUI.indentLevel = 1;
            gridCtrl.Size = EditorGUILayout.Vector3IntField("Size", gridCtrl.Size);
            gridCtrl.Origin = EditorGUILayout.Vector3Field("Origin", gridCtrl.Origin);
            gridCtrl.ResolutionCorrection = EditorGUILayout.Vector3Field("Resolution Correction", gridCtrl.ResolutionCorrection);
            EditorGUI.indentLevel = 0;
            EditorGUILayout.EndVertical();

            GUILayout.Space(5);

            EditorGUILayout.BeginHorizontal("Box");
            if (GUILayout.Button("Make Grid"))
            {
                gridCtrl.CreateNewGrid();
            }

            GUILayout.Space(5);

            if (GUILayout.Button("Clear Grid"))
            {
                gridCtrl.ClearGrid();
            }

            GUILayout.Space(5);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        public void SelectCell()
        {
            SelectedCoordinates = MasterGrid.GetCoordinatesByPosition(GridInput.PointerPosition);
        }

        public void DeselectCell()
        {
            Debug.LogError("Attenzione! Workaround");
            SelectedCoordinates = new Vector3Int(1000,1000,1000);
        }

        public void StartMouseAction(MouseActions _mouseActions)
        {
            CurrentMouseAction = _mouseActions;
            MasterGridWindow.GridVisualizer.ShowMouseAction = true;
        }

        public void EndMouseAction()
        {
            CurrentMouseAction = MouseActions.None;
            MasterGridWindow.GridVisualizer.ShowMouseAction = false;
        }

        /// <summary>
        /// Chiama la funzione link della cella salvata in precedenza
        /// </summary>
        public void LinkSelectedCell(bool _mutualLink = false)
        {
            Vector3Int cellToLink = MasterGrid.GetCoordinatesByPosition(GridInput.PointerPosition);
            if (SelectedCoordinates != null && cellToLink != null)
            {
                Debug.LogError("Attenzione! Funzionalità rimossa durante la rimozione del sistema a celle");
                //gridCtrl.LayerCtrl.LinkCells(SelectedCoordinates, cellToLink, gridCtrl.LayerCtrl.GetLayerAtIndex(gridCtrl.LayerCtrl.SelectedLayer), _mutualLink);
                EndMouseAction();
                DeselectCell();
            }
        }

        public void UnlinkSelectedCell()
        {
            Debug.LogError("Attenzione: funzionalità rimossa durane la rimozione del sistema a celle");
            //Cell cellToUnlink = MasterGrid.GetCellFromPosition(GridInput.PointerPosition);
            if (SelectedCoordinates != null /*&& cellToUnlink != null*/)
            {
                //gridCtrl.LayerCtrl.UnlinkCells(SelectedCoordinates, cellToUnlink);
                EndMouseAction();
                DeselectCell();
            }
        }

        public enum MouseActions
        {
            None, Link, LinkMutual, Unlink
        }
    }
}


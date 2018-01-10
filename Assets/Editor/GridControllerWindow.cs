using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Grid
{
    public class GridControllerWindow
    {
        GridControllerVisualizer gridVisualizer;
        GridData gridData;

        private Cell _selectedCell;
        public Cell SelectedCell
        {
            get { return _selectedCell; }
            private set
            {
                _selectedCell = value;
                gridVisualizer.SelectedCell = _selectedCell;
            }
        }

        public MouseActions CurrentMouseAction { get; private set; }

        [SerializeField]
        Vector2 scrollPosition;

        [SerializeField]
        string newDataName = "NewGridData";

        public GridControllerWindow(GridControllerVisualizer _gridVisualizer)
        {
            gridVisualizer = _gridVisualizer;
        }

        public void Show()
        {
            EditorGUILayout.BeginVertical("Box");
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            EditorGUILayout.BeginVertical("Box");
            GUILayout.Label("Sector Data", EditorStyles.boldLabel);
            EditorGUI.indentLevel = 1;
            gridVisualizer.GridCtrl.SectorData.Shape = (CellData.AreaShape)EditorGUILayout.EnumPopup("Area Shape", gridVisualizer.GridCtrl.SectorData.Shape);
            gridVisualizer.GridCtrl.SectorData.Radius = EditorGUILayout.Vector3Field("Radius", gridVisualizer.GridCtrl.SectorData.Radius);
            EditorGUI.indentLevel = 0;
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("Box");
            GUILayout.Label("Grid Data", EditorStyles.boldLabel);
            EditorGUI.indentLevel = 1;
            gridVisualizer.GridCtrl.Size = EditorGUILayout.Vector3IntField("Size", gridVisualizer.GridCtrl.Size);
            gridVisualizer.GridCtrl.Origin = EditorGUILayout.Vector3Field("Origin", gridVisualizer.GridCtrl.Origin);
            gridVisualizer.GridCtrl.ResolutionCorrection = EditorGUILayout.Vector3Field("Resolution Correction", gridVisualizer.GridCtrl.ResolutionCorrection);
            EditorGUI.indentLevel = 0;
            EditorGUILayout.EndVertical();

            GUILayout.Space(5);

            if (GUILayout.Button("Make Grid"))
            {
                gridVisualizer.GridCtrl.CreateNewGrid();
                //SaveCornersPosition();
            }

            GUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();

            if(!gridVisualizer.GridCtrl.DoesGridExist())
                GUI.enabled = false;

            if (GUILayout.Button("Save Grid", GUILayout.Height(40)))
            {
                gridVisualizer.GridCtrl.Save(newDataName);
            }

            if (!gridVisualizer.GridCtrl.DoesGridExist())
                GUI.enabled = true;

            EditorGUILayout.BeginVertical();
            GUILayout.Label("Asset Name", EditorStyles.boldLabel);
            newDataName = GUILayout.TextField(newDataName);

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Load Grid"))
            {
                gridVisualizer.GridCtrl.Load(gridData);
                MasterGrid.LayerCtrl.LoadFromData(gridData);
            }

            gridData = (GridData)EditorGUILayout.ObjectField(gridData, typeof(GridData), false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        public void SelectCell()
        {
            SelectedCell = gridVisualizer.GridCtrl.GetCellFromPosition(GridInput.PointerPosition);
            Debug.Log(SelectedCell.GridCoordinates);
        }

        public void DeselectCell()
        {
            SelectedCell = null;
        }

        public void StartMouseAction(MouseActions _mouseActions)
        {
            CurrentMouseAction = _mouseActions;
            MasterGrid.GridVisualizer.ShowMouseAction = true;
        }

        public void EndMouseAction()
        {
            CurrentMouseAction = MouseActions.None;
            MasterGrid.GridVisualizer.ShowMouseAction = false;
        }

        /// <summary>
        /// Chiama la funzione link della cella salvata in precedenza
        /// </summary>
        public void LinkSelectedCell(bool _mutualLink = false)
        {
            Cell cellToLink = gridVisualizer.GridCtrl.GetCellFromPosition(GridInput.PointerPosition);
            if (SelectedCell != null && cellToLink != null)
            {
                gridVisualizer.GridCtrl.LinkCells(SelectedCell, cellToLink, _mutualLink);
                EndMouseAction();
                DeselectCell();
            }
        }

        public void UnlinkSelectedCell()
        {
            Cell cellToUnlink = gridVisualizer.GridCtrl.GetCellFromPosition(GridInput.PointerPosition);
            if (SelectedCell != null && cellToUnlink != null)
            {
                gridVisualizer.GridCtrl.UnlinkCells(SelectedCell, cellToUnlink);
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


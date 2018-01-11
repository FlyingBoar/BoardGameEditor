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

        private Cell _selectedCell;
        public Cell SelectedCell
        {
            get { return _selectedCell; }
            private set
            {
                _selectedCell = value;
                gridCtrl.GridVisualizer.SelectedCell = _selectedCell;
            }
        }

        public MouseActions CurrentMouseAction { get; private set; }

        [SerializeField]
        Vector2 scrollPosition;

        [SerializeField]
        string newDataName = "NewGridData";

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

            if (GUILayout.Button("Make Grid"))
            {
                gridCtrl.CreateNewGrid();
            }

            GUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();

            if(!gridCtrl.DoesGridExist())
                GUI.enabled = false;

            if (GUILayout.Button("Save Grid", GUILayout.Height(40)))
            {
                gridCtrl.Save(newDataName);
            }

            if (!gridCtrl.DoesGridExist())
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
                gridCtrl.Load(AssetDatabase.GetAssetPath(fileToLoad));
                MasterGrid.LayerCtrl.LoadFromData(gridCtrl.GridData);
            }

            fileToLoad = (TextAsset)EditorGUILayout.ObjectField(fileToLoad, typeof(TextAsset), false);
            if(fileToLoad != null)
            {
                string loadFilePath = AssetDatabase.GetAssetPath(fileToLoad);
                if (!loadFilePath.Contains(".txt"))
                {
                    fileToLoad = null;
                    Debug.LogWarning("File format not supported !");
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        public void SelectCell()
        {
            SelectedCell = gridCtrl.GetCellFromPosition(GridInput.PointerPosition);
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
            Cell cellToLink = gridCtrl.GetCellFromPosition(GridInput.PointerPosition);
            if (SelectedCell != null && cellToLink != null)
            {
                gridCtrl.LayerCtrl.LinkCells(SelectedCell, cellToLink, gridCtrl.LayerCtrl.GetLayerAtIndex(gridCtrl.LayerCtrl.SelectedLayer), _mutualLink);
                EndMouseAction();
                DeselectCell();
            }
        }

        public void UnlinkSelectedCell()
        {
            Cell cellToUnlink = gridCtrl.GetCellFromPosition(GridInput.PointerPosition);
            if (SelectedCell != null && cellToUnlink != null)
            {
                gridCtrl.LayerCtrl.UnlinkCells(SelectedCell, cellToUnlink);
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


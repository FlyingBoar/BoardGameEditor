using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Grid
{
    public class GridControllerWindow
    {
        GridController gridCtrl;
        GridData gridData;

        List<Vector3> corners = new List<Vector3>();
        List<Vector3> handles = new List<Vector3>();

        private Cell _selectedCell;
        public Cell SelectedCell
        {
            get { return _selectedCell; }
            private set
            {
                _selectedCell = value;
                MasterGrid.GridVisualizer.SelectedCell = _selectedCell;
            }
        }

        [SerializeField]
        Vector2 scrollPosition;

        [SerializeField]
        string newDataName = "NewGridData";

        public GridControllerWindow(GridController _gridCtrl)
        {
            gridCtrl = _gridCtrl;
        }

        //private void OnSceneGUI()
        //{
        //    if (Event.current.type == EventType.MouseDown)
        //    {
        //        if (Event.current.button == 1)
        //        {
        //            GenericMenu menu = new GenericMenu();
        //            menu.AddItem(new GUIContent("Select Cell"), false, SelectCell);
        //            if (SelectedCell != null)
        //                menu.AddItem(new GUIContent("Link Cell"), false, LinkSelectedCell);
        //            menu.AddItem(new GUIContent("Deselect Cell"), false, DeselectCell);
        //            menu.ShowAsContext();
        //        }
        //    }


        //    if (corners.Count > 0)
        //    {
        //        EditorGUI.BeginChangeCheck();

        //        for (int i = 0; i < corners.Count; i++)
        //        {
        //            handles[i] = Handles.PositionHandle(corners[i], Quaternion.identity);
        //        }

        //        if (EditorGUI.EndChangeCheck())
        //        {
        //            Undo.RecordObject(maker, "Corner Changed");
        //            for (int i = 0; i < corners.Count; i++)
        //            {
        //                maker.CornersPos[i] = handles[i];
        //            }
        //        }
        //    }
        //}

        public void Show()
        {
            EditorGUILayout.BeginVertical("Box");
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            EditorGUILayout.BeginVertical("Box");
            GUILayout.Label("Sector Data", EditorStyles.boldLabel);
            EditorGUI.indentLevel = 1;
            gridCtrl.SectorData.Shape = (CellData.AreaShape)EditorGUILayout.EnumPopup("Area Shape", gridCtrl.SectorData.Shape);
            gridCtrl.SectorData.Radius = EditorGUILayout.Vector3Field("Radius", gridCtrl.SectorData.Radius);
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
                //SaveCornersPosition();
                SceneView.RepaintAll();
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
                gridCtrl.Load(gridData);
            }

            gridData = (GridData)EditorGUILayout.ObjectField(gridData, typeof(GridData), false);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        void SelectCell()
        {
            Debug.Log("Select Cell");
            SelectedCell = gridCtrl.GetCellFromPosition(GridInput.PointerPosition);
        }

        void DeselectCell()
        {
            Debug.Log("Deselect Cell");
            SelectedCell = null;
        }
        /// <summary>
        /// Chiama la funzione link della cella salvata in precedenza
        /// </summary>
        void LinkSelectedCell()
        {
            gridCtrl.LinkCells(SelectedCell, gridCtrl.GetCellFromPosition(GridInput.PointerPosition));
        }

        void SaveCornersPosition()
        {
            foreach (Cell cell in gridCtrl.GetGridCorners())
            {
                corners.Add(cell.GetPosition());
            }
        }
    }
}


﻿using System.Collections;
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

        private Cell _savedCell;
        public Cell SavedCell
        {
            get { return _savedCell; }
            private set { _savedCell = value; }
        }

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

        public void SelectCell()
        {
            SavedCell = MasterGrid.GridVisualizer.SelectedCell;
            if (SavedCell == null)
                Debug.LogError("No Cell saved.");
        }

        public void DeselectCell()
        {
            SavedCell = null;
        }
        /// <summary>
        /// Chiama la funzione link della cella salvata in precedenza
        /// </summary>
        public void LinkSelectedCell()
        {
            if (MasterGrid.GridVisualizer.SelectedCell != null)
            {
                gridCtrl.LinkCells(SavedCell, MasterGrid.GridVisualizer.SelectedCell);
                DeselectCell(); 
            }
            else
            {
                DeselectCell();
                Debug.LogError("No Cell selected.");
            }
        }

        public void UnlinkSelectedCell()
        {
            if (MasterGrid.GridVisualizer.SelectedCell != null)
            {
                gridCtrl.UnlinkCells(SavedCell, MasterGrid.GridVisualizer.SelectedCell);
                DeselectCell();
            }
            else
            {
                DeselectCell();
                Debug.LogError("No Cell selected.");
            }
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


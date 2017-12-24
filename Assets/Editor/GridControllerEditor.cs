using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Grid;

[CustomEditor(typeof(GridController)), CanEditMultipleObjects]
public class GridControllerEditor : Editor
{
    GridController gridCtrl;
    Rect rect;

    List<Vector3> corners = new List<Vector3>();

    List<Vector3> handles = new List<Vector3>();
        
    private void OnEnable()
    {
        gridCtrl = (GridController)target;
        rect = new Rect(0, 0, 50, 50);
    }

    private void OnSceneGUI()
    {

        if(Event.current.type == EventType.MouseDown)
        {
            if(Event.current.button == 1)
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Select Cell"), false, SelectCell);
                if(gridCtrl.SelectedCell != null)
                    menu.AddItem(new GUIContent("Link Cell"), false, LinkSelectedCell);
                menu.AddItem(new GUIContent("Deselect Cell"), false, DeselectCell);
                menu.ShowAsContext();
            }
        }

     
        //if (corners.Count > 0)
        //{
        //    EditorGUI.BeginChangeCheck();

        //    for (int i = 0; i < corners.Count; i++)
        //    {
        //        handles[i] = Handles.PositionHandle(corners[i], Quaternion.identity);
        //    }
            
        //    if (EditorGUI.EndChangeCheck())
        //    {
        //        Undo.RecordObject(maker, "Corner Changed");
        //        for (int i = 0; i < corners.Count; i++)
        //        {
        //            maker.CornersPos[i] = handles[i];
        //        }
        //    }
        //}
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Make Grid"))
        {
            gridCtrl.CreateNewGrid();
            //SaveCornersPosition();
        }
        if (GUILayout.Button("Reset Grid"))
            gridCtrl.ClearGrid();

        GUILayout.Space(10);

        if (GUILayout.Button("Save Grid"))
            gridCtrl.SaveCurrent();
        if (GUILayout.Button("Load Grid"))
            gridCtrl.Load();

        base.OnInspectorGUI();
    }

    /// <summary>
    /// Chiama la funzione SelectCell in GridController
    /// </summary>
    void SelectCell()
    {
        Debug.Log("Select Cell");
        gridCtrl.SelectCell();
    }
    /// <summary>
    /// Chiama Deselect in GridController
    /// </summary>
    void DeselectCell()
    {
        Debug.Log("Deselect Cell");
        gridCtrl.DeselectCell();
    }
    /// <summary>
    /// Chiama la funzione link della cella salvata in precedenza
    /// </summary>
    void LinkSelectedCell()
    {
        gridCtrl.LinkSelectedCell();
    }

    void SaveCornersPosition()
    {
        foreach (Cell cell in gridCtrl.GetGridCorners())
        {
            corners.Add(cell.GetCenter());
        }
    }
}

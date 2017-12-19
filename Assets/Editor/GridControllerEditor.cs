using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Grid;

[CustomEditor(typeof(GridController)), CanEditMultipleObjects]
public class GridControllerEditor : Editor
{
    GridController maker;
    Rect rect;

    List<Vector3> corners = new List<Vector3>();

    List<Vector3> handles = new List<Vector3>();
        
    private void OnEnable()
    {
        maker = (GridController)target;
        rect = new Rect(0, 0, 50, 50);
    }

    private void OnSceneGUI()
    {
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
            maker.CreateNewGrid();
            //SaveCornersPosition();
        }
        if (GUILayout.Button("Reset Grid"))
            maker.ClearGrid();

        GUILayout.Space(10);

        if (GUILayout.Button("Save Grid"))
            maker.SaveCurrent();
        if (GUILayout.Button("Load Grid"))
            maker.Load();

        base.OnInspectorGUI();
    }

    void SaveCornersPosition()
    {
        foreach (Cell cell in maker.GetGridCorners())
        {
            corners.Add(cell.GetCenter());
        }
    }
}

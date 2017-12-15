using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BGEditor.NodeSystem;

[CustomEditor(typeof(GridController)), CanEditMultipleObjects]
public class GridControllerEditor : Editor {

    GridController maker;
    Rect rect;

    bool makeGrid;
    bool resetGrid;

    bool testBool = false;

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
        base.OnInspectorGUI();

        makeGrid = GUILayout.Button("Make Grid");
        resetGrid = GUILayout.Button("Reset Grid");

        if (makeGrid)
        {
            maker.CreateNewGrid();
            SaveCornersPosition();
            testBool = true;
        }
        if (resetGrid)
            maker.ClearGrid();
    }

    void SaveCornersPosition()
    {
        foreach (Cell cell in maker.GetGridCorners())
        {
            corners.Add(cell.GetCenter());
        }
    }
}

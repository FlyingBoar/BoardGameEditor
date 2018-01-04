using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class InputAdapter_Tester : MonoBehaviour{

    static Ray mouseProjection;
    static Plane gridLevel = new Plane(Vector3.up, 0);
    public static Vector3 PointerPosition { get { return Test_FindMousePositionOnGridPlane(); } }

    public static Vector3 Test_FindMousePositionOnGridPlane()
    {
        return Vector3.zero;

        Vector3 currentMousePosition = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(Input.mousePosition);

        mouseProjection = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (gridLevel.Raycast(mouseProjection, out distance))
            currentMousePosition = mouseProjection.GetPoint(distance);

        return currentMousePosition;
    }
}

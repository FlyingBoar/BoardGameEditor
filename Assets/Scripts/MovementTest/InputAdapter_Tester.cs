using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputAdapter_Tester : MonoBehaviour{

    Ray mouseProjection;
    Plane gridLevel = new Plane(Vector3.up, 0);
    public Vector3 PointerPosition { get { return Test_FindMousePositionOnGridPlane(); } }

    public Vector3 Test_FindMousePositionOnGridPlane()
    {
        Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mouseProjection = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;
        if (gridLevel.Raycast(mouseProjection, out distance))
            currentMousePosition = mouseProjection.GetPoint(distance);

        return currentMousePosition;
    }
}

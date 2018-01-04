using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Grid {
    public class GridInput
    {
        static Ray mouseProjection;
        static Plane gridLevel = new Plane(Vector3.up, 0); //TODO: da adattare alla griglia
        public static Vector3 PointerPosition { get { return MousePositionOnGridPlane(); } }

        public static Vector3 MousePositionOnGridPlane()
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
}

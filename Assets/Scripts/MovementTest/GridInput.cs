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
            Vector3 currentMousePosition = Event.current.mousePosition;

            mouseProjection = Camera.current.ScreenPointToRay(new Vector3(Event.current.mousePosition.x, SceneView.currentDrawingSceneView.camera.pixelHeight - Event.current.mousePosition.y));
            float distance;
            if (gridLevel.Raycast(mouseProjection, out distance))
                currentMousePosition = mouseProjection.GetPoint(distance);

            return currentMousePosition;
        }
    }
}

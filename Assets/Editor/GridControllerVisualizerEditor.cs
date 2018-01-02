using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Grid
{
    [CustomEditor(typeof(GridControllerVisualizer)), CanEditMultipleObjects]
    public class GridControllerVisualizerEditor : Editor
    {
        GridControllerVisualizer visualizer;

        private void OnEnable()
        {
            visualizer = (GridControllerVisualizer)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical();

            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((GridControllerVisualizer)target), typeof(GridControllerVisualizer), false);
            GUI.enabled = true;

            EditorGUILayout.BeginHorizontal();
            visualizer.ShowGrid = EditorGUILayout.Toggle("ShowGrid", visualizer.ShowGrid);
            if(visualizer.ShowGrid)
            {
                visualizer.GridGizmosColor = EditorGUILayout.ColorField(visualizer.GridGizmosColor);
            }
            EditorGUILayout.EndHorizontal();

            visualizer.ShowLayersLink = EditorGUILayout.BeginToggleGroup("ShowLayersLink :", visualizer.ShowLayersLink);
            if (visualizer.ShowLayersLink)
            {
                EditorGUI.indentLevel = 1;
                if(visualizer.LinkArray == null || visualizer.LinkArray.Length != visualizer.GridCtrl.LayerCtrl.GetNumberOfLayers())
                {
                    visualizer.LinkArray = new bool[visualizer.GridCtrl.LayerCtrl.GetNumberOfLayers()];
                }
                for (int i = 0; i < visualizer.GridCtrl.LayerCtrl.GetNumberOfLayers(); i++)
                {
                    visualizer.LinkArray[i] = EditorGUILayout.Toggle(visualizer.GridCtrl.LayerCtrl.GetLayerAtIndex(i).Name, visualizer.LinkArray[i]);
                }

                EditorGUI.indentLevel = 0;
            }
            EditorGUILayout.EndToggleGroup();

            visualizer.ShowMousePosition = EditorGUILayout.Toggle("ShowMousePosition", visualizer.ShowMousePosition);

            visualizer.ShowMouseCell = EditorGUILayout.Toggle("ShowMouseCell", visualizer.ShowMouseCell);
            EditorGUILayout.EndVertical();
        }
    }
}

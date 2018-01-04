using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Grid
{
    public class GridVisualizerWindow
    {
        LayerController layerCtrl;
        GridControllerVisualizer visualizer;

        public GridVisualizerWindow(GridControllerVisualizer _visualizer, LayerController _layerCtrl)
        {
            visualizer = _visualizer;
            layerCtrl = _layerCtrl;
        }

        public void Show()
        {
            EditorGUILayout.BeginVertical("Box");

            EditorGUILayout.BeginHorizontal();
            visualizer.ShowGrid = EditorGUILayout.Toggle("ShowGrid", visualizer.ShowGrid);
            if (visualizer.ShowGrid)
            {
                visualizer.GridGizmosColor = EditorGUILayout.ColorField(visualizer.GridGizmosColor);
                if (visualizer.GridGizmosColor.a == 0)
                    visualizer.GridGizmosColor.a = 100;
            }
            EditorGUILayout.EndHorizontal();

            visualizer.ShowLayersLink = EditorGUILayout.BeginToggleGroup("ShowLayersLink :", visualizer.ShowLayersLink);
            if (visualizer.ShowLayersLink)
            {
                EditorGUI.indentLevel = 1;
                if (visualizer.LinkArray == null || visualizer.LinkArray.Length != layerCtrl.GetNumberOfLayers())
                {
                    visualizer.LinkArray = new bool[layerCtrl.GetNumberOfLayers()];
                }
                for (int i = 0; i < layerCtrl.GetNumberOfLayers(); i++)
                {
                    visualizer.LinkArray[i] = EditorGUILayout.Toggle(layerCtrl.GetLayerAtIndex(i).Name, visualizer.LinkArray[i]);
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

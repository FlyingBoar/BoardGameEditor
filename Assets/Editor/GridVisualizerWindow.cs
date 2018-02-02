using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Grid
{
    public class GridVisualizerWindow
    {
        GridVisualizer visualizer;
        bool[] tempLink;

        public GridVisualizerWindow(GridVisualizer _visualizer)
        {
            visualizer = _visualizer;
        }

        public void Show()
        {
            EditorGUILayout.BeginVertical("Box");

            EditorGUILayout.BeginHorizontal();
            visualizer.ShowGrid = EditorGUILayout.Toggle("ShowGrid", visualizer.ShowGrid);
            if (visualizer.ShowGrid)
            {
                visualizer.GridHandlesColor = EditorGUILayout.ColorField(visualizer.GridHandlesColor);
                if (visualizer.GridHandlesColor.a == 0)
                    visualizer.GridHandlesColor.a = 100;
            }
            EditorGUILayout.EndHorizontal();

            visualizer.ShowLayersLink = EditorGUILayout.BeginToggleGroup("ShowLayersLink :", visualizer.ShowLayersLink);
            if (visualizer.ShowLayersLink)
            {
                EditorGUI.indentLevel = 1;
                if (tempLink == null || tempLink.Length != visualizer.GridCtrl.LayerCtrl.GetNumberOfLayers())
                {
                    tempLink = new bool[visualizer.GridCtrl.LayerCtrl.GetNumberOfLayers()];
                    tempLink[0] = true;
                    visualizer.GridCtrl.LayerCtrl.SelectedLayer = 0;
                }
                if (!tempLink.ToList().Contains(true))
                {
                    tempLink[0] = true;
                    visualizer.GridCtrl.LayerCtrl.SelectedLayer = 0;
                }
                for (int i = 0; i < tempLink.Length; i++)
                {
                    CheckSelectedLayer(i);
                }

                EditorGUI.indentLevel = 0;
            }
            else
            {
                visualizer.GridCtrl.LayerCtrl.SelectedLayer = -1;
            }
            EditorGUILayout.EndToggleGroup();

            visualizer.ShowMousePosition = EditorGUILayout.Toggle("ShowMousePosition", visualizer.ShowMousePosition);

            visualizer.ShowMouseCell = EditorGUILayout.Toggle("ShowMouseCell", visualizer.ShowMouseCell);
            EditorGUILayout.EndVertical();
        }

        void CheckSelectedLayer(int _i)
        {
            tempLink[_i] = EditorGUILayout.Toggle(visualizer.GridCtrl.LayerCtrl.GetLayerAtIndex(_i).Data.ID, tempLink[_i]);

            if (tempLink[_i])
            {
                visualizer.GridCtrl.LayerCtrl.SelectedLayer = _i;

                for (int i = 0; i < tempLink.Length; i++)
                {
                    if (i != _i)
                    {
                        tempLink[i] = false;
                    }
                }
                return;
            }

            if (tempLink.ToList().Contains(true))
            {
                return;
            }
            else
            {
                if(visualizer.ShowLayersLink)
                    visualizer.ShowLayersLink = false;
                visualizer.GridCtrl.LayerCtrl.SelectedLayer = -1;
            }
        }
    }
}

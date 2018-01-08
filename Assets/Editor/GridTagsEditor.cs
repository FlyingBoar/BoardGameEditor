using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Grid
{
    [CustomEditor(typeof(GridTags)), CanEditMultipleObjects]
    public class GridTagsEditor : Editor
    {
        GridTags collider;

        private void OnEnable()
        {
            collider = (GridTags)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Active Layers", EditorStyles.boldLabel);

            EditorGUI.indentLevel = 1;
            EditorGUILayout.BeginVertical();

            if (MasterGrid.LayerCtrl == null)
                return; //WORKAROUND !!
            if (collider.ScannerLayers == null || collider.ScannerLayers.Count != MasterGrid.LayerCtrl.GetNumberOfLayers())
            {
                collider.ScannerLayers = new List<ScannerLayer>();
                for (int i = 0; i < MasterGrid.LayerCtrl.GetNumberOfLayers(); i++)
                {
                    collider.ScannerLayers.Add(new ScannerLayer(MasterGrid.LayerCtrl.GetLayerAtIndex(i), false));
                }
            }
            for (int i = 0; i < collider.ScannerLayers.Count; i++)
            {
                collider.ScannerLayers[i].Active = EditorGUILayout.Toggle(collider.ScannerLayers[i].Layer.Name, collider.ScannerLayers[i].Active);
            }

            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel = 0;
            EditorGUILayout.EndVertical();
        }
    }
}
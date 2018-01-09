using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Grid
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GridTags))]
    public class GridTagsEditor : Editor
    {
        GridTags gridTags;

        private void OnEnable()
        {
            gridTags = (GridTags)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Active Layers", EditorStyles.boldLabel);

            EditorGUI.indentLevel = 1;
            EditorGUILayout.BeginVertical();

            if (MasterGrid.LayerCtrl == null)
                return; //WORKAROUND !!
            if (gridTags.ScannerLayers == null || gridTags.ScannerLayers.Count != MasterGrid.LayerCtrl.GetNumberOfLayers())
            {
                gridTags.ScannerLayers = new List<ScannerLayer>();
                for (int i = 0; i < MasterGrid.LayerCtrl.GetNumberOfLayers(); i++)
                {
                    gridTags.ScannerLayers.Add(new ScannerLayer(MasterGrid.LayerCtrl.GetLayerAtIndex(i), false));
                }
            }
            for (int i = 0; i < gridTags.ScannerLayers.Count; i++)
            {
                //if (targets.Length > 1)
                //{
                //    foreach (GridTags _target in targets)
                //    {

                //    } 
                //}

                EditorGUI.BeginChangeCheck();
                bool changedBool = gridTags.ScannerLayers[i].Active = EditorGUILayout.Toggle(gridTags.ScannerLayers[i].Layer.Name, gridTags.ScannerLayers[i].Active);

                if (EditorGUI.EndChangeCheck())
                {
                    if (targets.Length > 1)
                    {
                        foreach (GridTags script in targets)
                        {
                            script.ScannerLayers[i].Active = changedBool;
                        }
                    }
                }
            }

            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel = 0;
            EditorGUILayout.EndVertical();
        }
    }
}
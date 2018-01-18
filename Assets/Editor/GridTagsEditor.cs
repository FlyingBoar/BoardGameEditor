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
        bool isMixedValues;
        private void OnEnable()
        {
            gridTags = (GridTags)target;


            if (targets.Length > 1)
            {
                UpdateScannerLayer();

                for (int i = 0; i < MasterGrid.LayerCtrl.GetNumberOfLayers(); i++)
                {
                    bool tempBool = (targets[targets.Length - 1] as GridTags).ScannerLayers[i].Active;  // lo 0 è l'ultimo elemento aggiunto all'array, da rivedere

                    for (int j = i; j < targets.Length; j++)
                    {
                        if((targets[j] as GridTags).ScannerLayers[i].Active != tempBool)
                        {
                            isMixedValues = true;
                            break;
                        }
                    }
                }


                //for (int j = 1; j < targets.Length; j++)
                //{
                //    if ((targets[j] as GridTags).ScannerLayers[i].Active != tempBool)
                //        EditorGUI.showMixedValue = true;
                //}
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Active Layers", EditorStyles.boldLabel);

            EditorGUI.indentLevel = 1;
            EditorGUILayout.BeginVertical();

            if (MasterGrid.LayerCtrl == null)
                return; //WORKAROUND !!

            UpdateScannerLayer();

            for (int i = 0; i < gridTags.ScannerLayers.Count; i++)
            {
                bool changedBool = false;
                EditorGUI.BeginChangeCheck();
                if (isMixedValues)
                {
                    EditorGUI.showMixedValue = true;
                    changedBool = gridTags.ScannerLayers[i].Active = EditorGUILayout.Toggle(gridTags.ScannerLayers[i].Layer.Name, gridTags.ScannerLayers[i].Active);
                    EditorGUI.showMixedValue = false;

                    if (EditorGUI.EndChangeCheck())
                    {
                        isMixedValues = false;

                        if (targets.Length > 1)
                        {
                            foreach (GridTags script in targets)
                            {
                                script.ScannerLayers[i].Active = true;
                            }
                        }
                    }
                }
                else
                {
                    
                    changedBool = gridTags.ScannerLayers[i].Active = EditorGUILayout.Toggle(gridTags.ScannerLayers[i].Layer.Name, gridTags.ScannerLayers[i].Active);

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
            }

            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel = 0;
            EditorGUILayout.EndVertical();
        }

        void UpdateScannerLayer()
        {
            if (gridTags.ScannerLayers.Count == 0 || gridTags.ScannerLayers.Count != MasterGrid.LayerCtrl.GetNumberOfLayers())
            {
                List<ScannerLayer> oldLayerList = gridTags.ScannerLayers;
                gridTags.ScannerLayers = new List<ScannerLayer>();
                for (int i = 0; i < MasterGrid.LayerCtrl.GetNumberOfLayers(); i++)
                {
                    gridTags.ScannerLayers.Add(new ScannerLayer(MasterGrid.LayerCtrl.GetLayerAtIndex(i), false));
                }

                for (int i = 0; i < gridTags.ScannerLayers.Count; i++)
                {
                    for (int j = 0; j < oldLayerList.Count; j++)
                    {
                        if (gridTags.ScannerLayers[i].Layer.Name == oldLayerList[j].Layer.Name)
                        {
                            gridTags.ScannerLayers[i].Active = oldLayerList[j].Active;
                            break;
                        }
                    }
                }
            }
        }
    }
}
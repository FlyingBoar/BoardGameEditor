using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Grid
{
    [CustomEditor(typeof(LayerController)), CanEditMultipleObjects]
    public class LayerControllerEditor : Editor
    {
        LayerController layerCtrl;

        private void OnEnable()
        {
            layerCtrl = (LayerController)target;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Layer", EditorStyles.boldLabel);
            GUILayout.Space(10);
            GUILayout.Label("IsEditable", EditorStyles.boldLabel);
            GUILayout.Label("GizmosColor", EditorStyles.boldLabel);
            GUILayout.EndHorizontal();

            foreach (Layer layer in layerCtrl.Layers)
            {
                EditorGUILayout.BeginHorizontal();
                layer.Name = EditorGUILayout.TextField(layer.Name);
                layer.IsEditable = EditorGUILayout.Toggle(layer.IsEditable);
                layer.GizmosColor = EditorGUILayout.ColorField(layer.GizmosColor);
                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
        }
    }
}

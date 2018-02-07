using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Grid
{
    public class GridLayerControllerWindow
    {
        GridLayerController layerCtrl;
        Texture addLayerTexture;
        Texture removeLayerTexture;

        bool[] tempSelectedLayer;

        string newLayerName = string.Empty;
        Color newLayersColor;

        string newLinkNetworkName = string.Empty;
        Color newLinkNetworkColor;

        Vector2 scrollPosition;
        Vector2 scrollPositionLayer;      
        Vector2 scrollPositionLink;

        public GridLayerControllerWindow(GridLayerController _layerCtrl)
        {
            layerCtrl = _layerCtrl;
            addLayerTexture = (Texture)EditorGUIUtility.Load("Plus.png");
            removeLayerTexture = (Texture)EditorGUIUtility.Load("Minus.png");
        }

        public void Show()
        {
            if (GUILayout.Button("SaveProva"))
                layerCtrl.Save();

            GUILayout.BeginVertical();
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            ShowLayerBox();

            ShowLinkNetworkTypeBox();

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        void ShowLayerBox()
        {
            GUILayout.BeginVertical("Box");
            scrollPositionLayer = GUILayout.BeginScrollView(scrollPositionLayer);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Is Active", EditorStyles.boldLabel);
            GUILayout.Label("Layer", EditorStyles.boldLabel);
            GUILayout.Space(5);
            GUILayout.Label("Color", EditorStyles.boldLabel);
            GUILayout.EndHorizontal();

            GUILayout.Space(4);

            for (int i = 0; i < layerCtrl.GetNumberOfLayers(); i++)
            {
                if (tempSelectedLayer == null || tempSelectedLayer.Length != layerCtrl.GetNumberOfLayers())
                {
                    tempSelectedLayer = new bool[layerCtrl.GetNumberOfLayers()];
                    tempSelectedLayer[0] = true;
                    layerCtrl.SelectedLayer = 0;
                }
                if (!tempSelectedLayer.ToList().Contains(true))
                {
                    tempSelectedLayer[0] = true;
                    layerCtrl.SelectedLayer = 0;
                }

                EditorGUILayout.BeginHorizontal();

                UpdateSelectedLayer(i);
                layerCtrl.GetLayerAtIndex(i).Data.ID = EditorGUILayout.TextField(layerCtrl.GetLayerAtIndex(i).Data.ID);
                layerCtrl.GetLayerAtIndex(i).Data.Color = EditorGUILayout.ColorField(layerCtrl.GetLayerAtIndex(i).Data.Color);

                if (GUILayout.Button(removeLayerTexture, GUILayout.Height(18), GUILayout.Width(20)))
                {
                    layerCtrl.RemoveLayer(layerCtrl.GetLayerAtIndex(i));
                    continue;
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.Space(4);
            GUILayout.Label("New Layer", EditorStyles.boldLabel);
            GUILayout.Space(2);

            GUILayout.BeginHorizontal();
            newLayerName = EditorGUILayout.TextField(newLayerName);
            newLayersColor = EditorGUILayout.ColorField(newLayersColor);

            if (newLayersColor.a == 0)
                newLayersColor.a = 100;

            if (GUILayout.Button(addLayerTexture, GUILayout.Height(17), GUILayout.Width(20)))
            {
                if (newLayerName == string.Empty)
                {
                    Debug.LogWarning("Can't add a Layer without a name !");
                    return;
                }
                for (int i = 0; i < layerCtrl.GetNumberOfLayers(); i++)
                {
                    if (layerCtrl.GetNumberOfLayers() > 0 && layerCtrl.GetLayerAtIndex(i).Data.ID == newLayerName)
                    {
                        Debug.LogWarning("The name of the layer already exist !");
                        return;
                    }
                }
                layerCtrl.AddLayer(newLayerName, newLayersColor);
                newLayerName = string.Empty;
                newLayersColor = Color.black;
                newLayersColor.a = 100;
            }

            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        void ShowLinkNetworkTypeBox()
        {
            GUILayout.BeginVertical("Box");
            scrollPositionLink = GUILayout.BeginScrollView(scrollPositionLink);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Link Network Type", EditorStyles.boldLabel);
            GUILayout.Space(10);
            GUILayout.Label("Color", EditorStyles.boldLabel);
            GUILayout.EndHorizontal();

            GUILayout.Space(4);

            for (int i = 0; i < layerCtrl.GetNumberOfLinkNetworks(); i++)
            {
                EditorGUILayout.BeginHorizontal();
                layerCtrl.GetLinkNetworkAtIndex(i).ID = EditorGUILayout.TextField(layerCtrl.GetLinkNetworkAtIndex(i).ID);
                layerCtrl.GetLinkNetworkAtIndex(i).Color = EditorGUILayout.ColorField(layerCtrl.GetLinkNetworkAtIndex(i).Color);

                if (GUILayout.Button(removeLayerTexture, GUILayout.Height(18), GUILayout.Width(20)))
                {
                    layerCtrl.RemoveLinkNetwork(layerCtrl.GetLinkNetworkAtIndex(i));
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.Space(2);
            GUILayout.Label("New Link Network Type", EditorStyles.boldLabel);
            GUILayout.Space(2);

            GUILayout.BeginHorizontal();
            newLinkNetworkName = EditorGUILayout.TextField(newLinkNetworkName);
            newLinkNetworkColor = EditorGUILayout.ColorField(newLinkNetworkColor);

            if (newLinkNetworkColor.a == 0)
                newLinkNetworkColor.a = 100;

            if (GUILayout.Button(addLayerTexture, GUILayout.Height(17), GUILayout.Width(20)))
            {
                if (newLinkNetworkName == string.Empty)
                {
                    Debug.LogWarning("Can't add a Link Network Type without a name !");
                    return;
                }
                for (int i = 0; i < layerCtrl.GetNumberOfLayers(); i++)
                {
                    if (layerCtrl.GetNumberOfLinkNetworks() > 0 && layerCtrl.GetLinkNetworkAtIndex(i).ID == newLinkNetworkName)
                    {
                        Debug.LogWarning("The name of the Link Network Type already exist !");
                        return;
                    }
                }

                layerCtrl.AddLinkNetwork(newLinkNetworkName, newLinkNetworkColor);
                newLinkNetworkName = string.Empty;
                newLinkNetworkColor = Color.black;
                newLinkNetworkColor.a = 100;
            }

            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        void UpdateSelectedLayer(int _i)
        {
            tempSelectedLayer[_i] = EditorGUILayout.Toggle(tempSelectedLayer[_i]);

            if (tempSelectedLayer[_i])
            {
                layerCtrl.SelectedLayer = _i;

                for (int i = 0; i < tempSelectedLayer.Length; i++)
                {
                    if (i != _i)
                    {
                        tempSelectedLayer[i] = false;
                    }
                }
                return;
            }

            if (tempSelectedLayer.ToList().Contains(true))
            {
                return;
            }
            else
            {
                layerCtrl.SelectedLayer = 0;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Grid
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(LayerItem))]
    public class LayerItemEditor : Editor
    {
        LayerItem layerItem;

        #region Textures
        Texture verticalArrows;
        Texture verticalArrowsCross;
        Texture horizontalArrows;
        Texture horizontalArrowsCross;
        Texture diagonalArrows1;
        Texture diagonalArrowsCross1;
        Texture diagonalArrows2;
        Texture diagonalArrowsCross2;
        Texture central;
        Texture centralCross;

        Texture[,] texturesButtonMatrix = new Texture[3, 3];
        #endregion

        bool[,] logicButtonMatrix = new bool[3, 3]; //true link not available - false link available

        string[] networkTypes;
        int _selectedNetworkTypes;
        int selectedNetworkTypes
        {
            get { return _selectedNetworkTypes; }
            set
            {
                int oldValue = _selectedNetworkTypes;
                _selectedNetworkTypes = value;
                if (oldValue != _selectedNetworkTypes)
                {
                    UpdateAllButtonsLogic(layerItem.GetBlockedLinkNetworkByType(networkTypes[selectedNetworkTypes]).ID);
                }
            }
        }

        private void Awake()
        {
            verticalArrows = (Texture)EditorGUIUtility.Load("VerticalArrows.png");
            verticalArrowsCross = (Texture)EditorGUIUtility.Load("VerticalArrowsCross.png");
            horizontalArrows = (Texture)EditorGUIUtility.Load("HorizontalArrows.png");
            horizontalArrowsCross = (Texture)EditorGUIUtility.Load("HorizontalArrowsCross.png");
            diagonalArrows1 = (Texture)EditorGUIUtility.Load("DiagonalArrows1.png");
            diagonalArrowsCross1 = (Texture)EditorGUIUtility.Load("DiagonalArrowsCross1.png");
            diagonalArrows2 = (Texture)EditorGUIUtility.Load("DiagonalArrows2.png");
            diagonalArrowsCross2 = (Texture)EditorGUIUtility.Load("DiagonalArrowsCross2.png");
            central = (Texture)EditorGUIUtility.Load("Central.png");
            centralCross = (Texture)EditorGUIUtility.Load("CentralCross.png");
        }

        private void OnEnable()
        {
            layerItem = (LayerItem)target;

            UpdateNetworkTypeSelection();

            if (layerItem.GetBlockedLinkNetworksCount() == 0)
            {
                texturesButtonMatrix[2, 0] = diagonalArrows1;
                texturesButtonMatrix[2, 1] = verticalArrows;
                texturesButtonMatrix[2, 2] = diagonalArrows2;

                texturesButtonMatrix[1, 0] = horizontalArrows;
                texturesButtonMatrix[1, 1] = central;
                texturesButtonMatrix[1, 2] = horizontalArrows;

                texturesButtonMatrix[0, 0] = diagonalArrows2;
                texturesButtonMatrix[0, 1] = verticalArrows;
                texturesButtonMatrix[0, 2] = diagonalArrows1;
            }
            else
            {
                UpdateAllButtonsLogic(layerItem.GetBlockedLinkNetworkByType(networkTypes[selectedNetworkTypes]).ID);
            }
        }

        public override void OnInspectorGUI()
        {
            if(layerItem.GetBlockedLinkNetworksCount() == 0 && (MasterGrid.gridLayerCtrl == null || MasterGrid.gridLayerCtrl.GetNumberOfLinkNetworks() == 0))
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.HelpBox("Can't edit links because there are no Link Netwotk Type in Grid Layer Controller", MessageType.Warning);
                EditorGUILayout.EndVertical();
            }
            else
            {
                EditorGUILayout.BeginVertical("Box");

                EditorGUILayout.LabelField("Edit Links :", EditorStyles.boldLabel);
                GUILayout.Space(3);

                UpdateNetworkTypeSelection();
                selectedNetworkTypes = EditorGUILayout.Popup(selectedNetworkTypes, networkTypes);

                ShowForwardButtons();
                ShowCentralButtons();
                ShowBackwardButtons();

                EditorGUILayout.EndVertical();
            }

            base.OnInspectorGUI();
        }

        #region Show
        void ShowForwardButtons()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(texturesButtonMatrix[2,0], GUILayout.Height(30), GUILayout.Width(30)))
            {
                UpdateButtonLogic(2, 0);
            }
            else if (GUILayout.Button(texturesButtonMatrix[2, 1], GUILayout.Height(30), GUILayout.Width(30)))
            {
                
            }
            else if (GUILayout.Button(texturesButtonMatrix[2, 2], GUILayout.Height(30), GUILayout.Width(30)))
            {
                
            }
            EditorGUILayout.EndHorizontal();
        }

        void ShowCentralButtons()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(texturesButtonMatrix[1, 0], GUILayout.Height(30), GUILayout.Width(30)))
            {
                
            }
            else if (GUILayout.Button(texturesButtonMatrix[1, 1], GUILayout.Height(30), GUILayout.Width(30)))
            {
                
            }
            else if (GUILayout.Button(texturesButtonMatrix[1, 2], GUILayout.Height(30), GUILayout.Width(30)))
            {
                
            }
            EditorGUILayout.EndHorizontal();
        }

        void ShowBackwardButtons()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(texturesButtonMatrix[0, 0], GUILayout.Height(30), GUILayout.Width(30)))
            {
                
            }
            else if (GUILayout.Button(texturesButtonMatrix[0, 1], GUILayout.Height(30), GUILayout.Width(30)))
            {
                
            }
            else if (GUILayout.Button(texturesButtonMatrix[0, 2], GUILayout.Height(30), GUILayout.Width(30)))
            {
                
            }
            EditorGUILayout.EndHorizontal();
        }

        #endregion

        void UpdateNetworkTypeSelection()
        {
            //if ((networkTypes == null || layerItem.GetBlockedLinkNetworksCount() != networkTypes.Length) && MasterGrid.gridLayerCtrl == null)
            //{
            //    networkTypes = new string[layerItem.GetBlockedLinkNetworksCount()];
            //    for (int i = 0; i < layerItem.GetBlockedLinkNetworksCount(); i++)
            //    {
            //        networkTypes[i] = layerItem.GetBlockedLinkNetworkByType(sele).ID;
            //    }
            //}
            //else 
            if (networkTypes == null || MasterGrid.gridLayerCtrl.GetNumberOfLinkNetworks() != networkTypes.Length)
            {
                networkTypes = new string[MasterGrid.gridLayerCtrl.GetNumberOfLinkNetworks()];
                for (int i = 0; i < MasterGrid.gridLayerCtrl.GetNumberOfLinkNetworks(); i++)
                {
                    networkTypes[i] = MasterGrid.gridLayerCtrl.GetLinkNetworkAtIndex(i).ID;
                }
            }   
        }

        void AddBlockedDirection(Vector3Int _direction)
        {
            layerItem.AddBlockedLink(_direction, MasterGrid.gridLayerCtrl.GetLinkNetworkAtIndex(selectedNetworkTypes).ID);
        }

        void RemoveBlockedDirection(Vector3Int _direction)
        {
            layerItem.RemoveBlockedLink(_direction, MasterGrid.gridLayerCtrl.GetLinkNetworkAtIndex(selectedNetworkTypes));
        }

        void UpdateButtonTexture(int _i, int _j, bool _value)
        {
            if(_i - _j == 0)
            {
                if (_value)
                    texturesButtonMatrix[_i, _j] = diagonalArrowsCross1;
                else
                    texturesButtonMatrix[_i, _j] = diagonalArrows1;
            }
            else if (Mathf.Abs(_i - _j) == 2)
            {
                if (_value)
                    texturesButtonMatrix[_i, _j] = diagonalArrowsCross2;
                else
                    texturesButtonMatrix[_i, _j] = diagonalArrows2;
            }
            else if(_i == 1)
            {
                if (_value)
                    texturesButtonMatrix[_i, _j] = verticalArrowsCross;
                else
                    texturesButtonMatrix[_i, _j] = verticalArrows;
            }
            else if (_j == 1)
            {
                if (_value)
                    texturesButtonMatrix[_i, _j] = horizontalArrowsCross;
                else
                    texturesButtonMatrix[_i, _j] = horizontalArrows;
            }
        }

        void UpdateButtonLogic(int _i, int _j)
        {
            if(logicButtonMatrix[_i,_j] == true)
            {
                RemoveBlockedDirection(new Vector3Int(_i - 1, 0, _j - 1));
                logicButtonMatrix[_i, _j] = false;
                UpdateButtonTexture(_i, _j, false);
            }
            else
            {
                AddBlockedDirection(new Vector3Int(_i - 1, 0, _j - 1));
                logicButtonMatrix[_i, _j] = true;
                UpdateButtonTexture(_i, _j, true);
            }
        }

        void UpdateAllButtonsLogic(string _linkID)
        {
            List<Vector3Int> links = layerItem.GetBlockedLinkNetworkByType(_linkID).GetLinks();

            for (int i = 0; i < links.Count; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    for (int k = -1; k < 2; k++)
                    {
                        if (j == 0 && k == 0)
                            continue;
                        if(links[i] == new Vector3Int(j, 0, k))
                        {
                            logicButtonMatrix[j + 1, k + 1] = true;
                            UpdateButtonTexture(j + 1, k + 1, true);
                        }
                        else
                        {
                            logicButtonMatrix[j + 1, k + 1] = false;
                            UpdateButtonTexture(j + 1, k + 1, false);
                        }
                    }
                }
            }
        }
    }
}


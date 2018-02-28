using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

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
                int lastSelected = _selectedNetworkTypes;
                _selectedNetworkTypes = value;
                if (lastSelected != _selectedNetworkTypes)
                {
                    if (_selectedNetworkTypes > layerItem.GetBlockedLinkNetworksCount() - 1)
                    {
                        layerItem.AddLinkNetwork(networkTypes[selectedNetworkTypes]);
                    }

                    SetupAllButtonsLogic(layerItem.GetBlockedLinkNetworkByType(networkTypes[selectedNetworkTypes]).ID);
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
                SetupAllButtonsLogic(layerItem.GetBlockedLinkNetworkByType(networkTypes[selectedNetworkTypes]).ID);
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

        #region NetworkType Selection
        void UpdateNetworkTypeSelection()
        {
            if (MasterGrid.gridLayerCtrl != null && MasterGrid.gridLayerCtrl.GetNumberOfLinkNetworks() > 0 && MasterGrid.gridLayerCtrl.GetNumberOfLinkNetworks() != layerItem.GetBlockedLinkNetworksCount())
            {
                int lastSelected = selectedNetworkTypes;

                // TODO : vanno controllati anche per id, oltre che per numero
                networkTypes = new string[MasterGrid.gridLayerCtrl.GetNumberOfLinkNetworks()];
                for (int i = 0; i < MasterGrid.gridLayerCtrl.GetNumberOfLinkNetworks(); i++)
                {
                    networkTypes[i] = MasterGrid.gridLayerCtrl.GetLinkNetworkAtIndex(i).ID;
                }

                RestorePreviousSelection(lastSelected);
            }
            else if (networkTypes == null || layerItem.GetBlockedLinkNetworksCount() != networkTypes.Length)
            {
                int lastSelected = selectedNetworkTypes;

                networkTypes = new string[layerItem.GetBlockedLinkNetworksCount()];
                for (int i = 0; i < layerItem.GetBlockedLinkNetworksCount(); i++)
                {
                    networkTypes[i] = layerItem.GetBlockedLinkNetworkByIndex(i).ID;
                }

                RestorePreviousSelection(lastSelected);
            }
        }

        void UpdateNetworkTypes(string[] _newIDs)
        {
            List<string> tempTypes = networkTypes.ToList();

            for (int i = 0; i < tempTypes.Count; i++)
            {
                for (int j = 0; j < _newIDs.Length; j++)
                {
                    if (tempTypes[i] == _newIDs[j])
                        break;

                    tempTypes.Add(_newIDs[j]);
                }
            }

            networkTypes = tempTypes.ToArray();
        }

        void RestorePreviousSelection(int _lastSelected)
        {
            if (selectedNetworkTypes != _lastSelected)
            {
                if (_lastSelected > networkTypes.Length - 1)
                    selectedNetworkTypes = 0;
                else
                    selectedNetworkTypes = _lastSelected;
            }
        }
        #endregion

        #region Show Buttons
        void ShowForwardButtons()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(texturesButtonMatrix[2,0], GUILayout.Height(30), GUILayout.Width(30))) // forward left
            {
                UpdateButtonLogic(2, 0);
            }
            else if (GUILayout.Button(texturesButtonMatrix[2, 1], GUILayout.Height(30), GUILayout.Width(30))) // forward
            {
                UpdateButtonLogic(2, 1);
            }
            else if (GUILayout.Button(texturesButtonMatrix[2, 2], GUILayout.Height(30), GUILayout.Width(30))) // forward right
            {
                UpdateButtonLogic(2, 2);
            }
            EditorGUILayout.EndHorizontal();
        }

        void ShowCentralButtons()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(texturesButtonMatrix[1, 0], GUILayout.Height(30), GUILayout.Width(30))) // left
            {
                UpdateButtonLogic(1, 0);
            }
            else if (GUILayout.Button(texturesButtonMatrix[1, 1], GUILayout.Height(30), GUILayout.Width(30))) // central
            {
                UpdateCentralButtonLogic();
            }
            else if (GUILayout.Button(texturesButtonMatrix[1, 2], GUILayout.Height(30), GUILayout.Width(30))) // right
            {
                UpdateButtonLogic(1, 2);
            }
            EditorGUILayout.EndHorizontal();
        }

        void ShowBackwardButtons()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(texturesButtonMatrix[0, 0], GUILayout.Height(30), GUILayout.Width(30))) // backward left
            {
                UpdateButtonLogic(0, 0);
            }
            else if (GUILayout.Button(texturesButtonMatrix[0, 1], GUILayout.Height(30), GUILayout.Width(30))) // beackward
            {
                UpdateButtonLogic(0, 1);
            }
            else if (GUILayout.Button(texturesButtonMatrix[0, 2], GUILayout.Height(30), GUILayout.Width(30))) // backward right
            {
                UpdateButtonLogic(0, 2);
            }
            EditorGUILayout.EndHorizontal();
        }

        #endregion

        #region Update Blocked Directions
        void AddBlockedDirection(Vector3Int _direction)
        {
            if(MasterGrid.gridLayerCtrl != null)
                layerItem.AddBlockedLink(_direction, MasterGrid.gridLayerCtrl.GetLinkNetworkAtIndex(selectedNetworkTypes).ID);
            else
                layerItem.AddBlockedLink(_direction, layerItem.GetBlockedLinkNetworkByIndex(selectedNetworkTypes).ID);
        }

        void RemoveBlockedDirection(Vector3Int _direction)
        {
            if (MasterGrid.gridLayerCtrl != null)
                 layerItem.RemoveBlockedLink(_direction, MasterGrid.gridLayerCtrl.GetLinkNetworkAtIndex(selectedNetworkTypes).ID);
            else
                layerItem.RemoveBlockedLink(_direction, layerItem.GetBlockedLinkNetworkByIndex(selectedNetworkTypes).ID);
        }
        #endregion

        #region Button Logics Update
        void UpdateButtonLogic(int _i, int _j)
        {
            if (logicButtonMatrix[_i, _j])
            {
                UpdateButtonLogic(_i, _j, false);
            }
            else
            {
                UpdateButtonLogic(_i, _j, true);
            }
        }

        void UpdateButtonLogic(int _i, int _j, bool _value)
        {       
            if(_i == 1 && _j == 1)
                return;

            if (_value)
            {
                AddBlockedDirection(new Vector3Int(_i - 1, 0, _j - 1));
                logicButtonMatrix[_i, _j] = _value;
                UpdateButtonTexture(_i, _j, _value);
            }
            else
            {
                RemoveBlockedDirection(new Vector3Int(_i - 1, 0, _j - 1));
                logicButtonMatrix[_i, _j] = _value;
                UpdateButtonTexture(_i, _j, _value);
            }

            UpdateCentralButtonLogicForced();
        }

        void UpdateCentralButtonLogic()
        {
            bool valueToSet;
            if(logicButtonMatrix[1, 1])
            {
                logicButtonMatrix[1, 1] = false;
                UpdateButtonTexture(1, 1, false);
                valueToSet = false;
            }
            else
            {
                logicButtonMatrix[1, 1] = true;
                UpdateButtonTexture(1, 1, true);
                valueToSet = true;
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (i == 1 && j == 1)
                        continue;

                    if (logicButtonMatrix[i,j] != valueToSet)
                    {
                        UpdateButtonLogic(i, j, valueToSet);
                        UpdateButtonTexture(i, j, valueToSet);
                    }
                }
            }
        }

        void UpdateCentralButtonLogicForced()
        {
            if (layerItem.GetBlockedLinkNetworkByIndex(selectedNetworkTypes).GetLinks().Count == 8)
            {
                logicButtonMatrix[1, 1] = true;
                UpdateButtonTexture(1, 1, true);
            }
            else
            {
                logicButtonMatrix[1, 1] = false;
                UpdateButtonTexture(1, 1, false);
            }
        }

        void SetupAllButtonsLogic(string _linkID)
        {
            List<Vector3Int> blockedLinks = layerItem.GetBlockedLinkNetworkByType(_linkID).GetLinks();

            if(blockedLinks.Count == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (i == 1 && j == 1)
                            continue;

                        UpdateButtonLogic(i, j, false);
                        UpdateButtonTexture(i, j, false);
                    }
                }
            }
            else
            {
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        for (int k = 0; k < blockedLinks.Count; k++)
                        {
                            if (i == 0 && j == 0)
                                continue;

                            if (blockedLinks[k] == new Vector3Int(i, 0, j))
                            {
                                logicButtonMatrix[i + 1, j + 1] = true;
                                UpdateButtonTexture(i + 1, j + 1, true);
                                break;
                            }
                            else
                            {
                                logicButtonMatrix[i + 1, j + 1] = false;
                                UpdateButtonTexture(i + 1, j + 1, false);
                            }
                        }
                    }
                }
            }

            UpdateCentralButtonLogicForced();
        }
        #endregion

        void UpdateButtonTexture(int _i, int _j, bool _value)
        {
            if (_i == 1 && _j == 1)
            {
                if (_value)
                    texturesButtonMatrix[_i, _j] = centralCross;
                else
                    texturesButtonMatrix[_i, _j] = central;
            }
            else if (_i - _j == 0)
            {
                if (_value)
                    texturesButtonMatrix[_i, _j] = diagonalArrowsCross2;
                else
                    texturesButtonMatrix[_i, _j] = diagonalArrows2;
            }
            else if (Mathf.Abs(_i - _j) == 2)
            {
                if (_value)
                    texturesButtonMatrix[_i, _j] = diagonalArrowsCross1;
                else
                    texturesButtonMatrix[_i, _j] = diagonalArrows1;
            }
            else if (_i == 1)
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
    }
}


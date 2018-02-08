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

        Texture forwardLeftButton;
        Texture forwardButton;
        Texture forwardRightButton;

        Texture leftButton;
        Texture centralButton;
        Texture rightButton;

        Texture backwardLeftButton;
        Texture backwardButton;
        Texture backwardRightButton;
        #endregion

        string[] networkTypes;
        int selectedNetworkTypes;

        private void OnEnable()
        {
            layerItem = (LayerItem)target;

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

            forwardLeftButton = diagonalArrows1;
            forwardButton = verticalArrows;
            forwardRightButton = diagonalArrows2;

            leftButton = horizontalArrows;
            centralButton = central;
            rightButton = horizontalArrows;

            backwardLeftButton = diagonalArrows2;
            backwardButton = verticalArrows;
            backwardRightButton = diagonalArrows1;
        }

        public override void OnInspectorGUI()
        {
            if(MasterGrid.gridLayerCtrl != null || MasterGrid.gridLayerCtrl.GetNumberOfLinkNetworks() > 0)
            {
                EditorGUILayout.BeginVertical("Box");

                EditorGUILayout.LabelField("Edit Links :", EditorStyles.boldLabel);
                GUILayout.Space(3);

                ShowNetworkTypeSelection();

                ShowForwardButtons();
                ShowCentralButtons();
                ShowBackwardButtons();

                EditorGUILayout.EndVertical();
            }
            else
            {
                EditorGUILayout.HelpBox("Can't edit links because there are no Link Netwotk Type in Grid Layer Controller", MessageType.Warning);
            }

            base.OnInspectorGUI();
        }

        #region Show
        void ShowForwardButtons()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(forwardLeftButton, GUILayout.Height(30), GUILayout.Width(30)))
            {
                if (forwardLeftButton == diagonalArrows1)
                {
                    AddBlockedDirection(new Vector3Int(-1, 0, 1));
                    forwardLeftButton = diagonalArrowsCross1;
                }
                else
                {
                    RemoveBlockedDirection(new Vector3Int(-1, 0, 1));
                    forwardLeftButton = diagonalArrows1;
                }
            }
            else if (GUILayout.Button(forwardButton, GUILayout.Height(30), GUILayout.Width(30)))
            {
                if (forwardButton == verticalArrows)
                {
                    AddBlockedDirection(new Vector3Int(0, 0, 1));
                    forwardButton = verticalArrowsCross;
                }
                else
                {
                    RemoveBlockedDirection(new Vector3Int(0, 0, 1));
                    forwardButton = verticalArrows;
                }
            }
            else if (GUILayout.Button(forwardRightButton, GUILayout.Height(30), GUILayout.Width(30)))
            {
                if (forwardRightButton == diagonalArrows2)
                {
                    AddBlockedDirection(new Vector3Int(1, 0, 1));
                    forwardRightButton = diagonalArrowsCross2;
                }
                else
                {
                    RemoveBlockedDirection(new Vector3Int(1, 0, 1));
                    forwardRightButton = diagonalArrows2;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        void ShowCentralButtons()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(leftButton, GUILayout.Height(30), GUILayout.Width(30)))
            {
                if (leftButton == horizontalArrows)
                {
                    AddBlockedDirection(new Vector3Int(-1, 0, 0));
                    leftButton = horizontalArrowsCross;
                }
                else
                {
                    RemoveBlockedDirection(new Vector3Int(-1, 0, 0));
                    leftButton = horizontalArrows;
                }
            }
            else if (GUILayout.Button(centralButton, GUILayout.Height(30), GUILayout.Width(30)))
            {
                if (centralButton == central)
                {
                    AddBlockedDirection(new Vector3Int(-1, 0, 1));
                    forwardLeftButton = diagonalArrowsCross1;
                    AddBlockedDirection(new Vector3Int(0, 0, 1));
                    forwardButton = verticalArrowsCross;
                    AddBlockedDirection(new Vector3Int(1, 0, 1));
                    forwardRightButton = diagonalArrowsCross2;

                    AddBlockedDirection(new Vector3Int(-1, 0, 0));
                    leftButton = horizontalArrowsCross;
                    centralButton = centralCross;
                    AddBlockedDirection(new Vector3Int(1, 0, 0));
                    rightButton = horizontalArrowsCross;


                    AddBlockedDirection(new Vector3Int(-1, 0, -1));
                    backwardLeftButton = diagonalArrowsCross2;
                    AddBlockedDirection(new Vector3Int(0, 0, -1));
                    backwardButton = verticalArrowsCross;
                    AddBlockedDirection(new Vector3Int(1, 0, -1));
                    backwardRightButton = diagonalArrowsCross1;
                }
                else
                {
                    RemoveBlockedDirection(new Vector3Int(-1, 0, 1));
                    forwardLeftButton = diagonalArrows1;
                    RemoveBlockedDirection(new Vector3Int(0, 0, 1));                
                    forwardButton = verticalArrows;
                    RemoveBlockedDirection(new Vector3Int(1, 0, 1));
                    forwardRightButton = diagonalArrows2;

                    RemoveBlockedDirection(new Vector3Int(-1, 0, 0));
                    leftButton = horizontalArrows;
                    centralButton = central;
                    RemoveBlockedDirection(new Vector3Int(1, 0, 0));
                    rightButton = horizontalArrows;

                    RemoveBlockedDirection(new Vector3Int(-1, 0, -1));
                    backwardLeftButton = diagonalArrows2;
                    RemoveBlockedDirection(new Vector3Int(0, 0, -1));
                    backwardButton = verticalArrows;
                    RemoveBlockedDirection(new Vector3Int(1, 0, -1));
                    backwardRightButton = diagonalArrows1;
                }
            }
            else if (GUILayout.Button(rightButton, GUILayout.Height(30), GUILayout.Width(30)))
            {
                if (rightButton == horizontalArrows)
                {
                    AddBlockedDirection(new Vector3Int(1, 0, 0));
                    rightButton = horizontalArrowsCross;
                }
                else
                {
                    AddBlockedDirection(new Vector3Int(1, 0, 0));
                    rightButton = horizontalArrows;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        void ShowBackwardButtons()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(backwardLeftButton, GUILayout.Height(30), GUILayout.Width(30)))
            {
                if (backwardLeftButton == diagonalArrows2)
                {
                    AddBlockedDirection(new Vector3Int(-1, 0, -1));
                    backwardLeftButton = diagonalArrowsCross2;
                }
                else
                {
                    RemoveBlockedDirection(new Vector3Int(-1, 0, -1));
                    backwardLeftButton = diagonalArrows2;
                }
            }
            else if (GUILayout.Button(backwardButton, GUILayout.Height(30), GUILayout.Width(30)))
            {
                if (backwardButton == verticalArrows)
                {
                    AddBlockedDirection(new Vector3Int(0, 0, -1));
                    backwardButton = verticalArrowsCross;
                }
                else
                {
                    RemoveBlockedDirection(new Vector3Int(0, 0, -1));
                    backwardButton = verticalArrows;
                }
            }
            else if (GUILayout.Button(backwardRightButton, GUILayout.Height(30), GUILayout.Width(30)))
            {
                if (backwardRightButton == diagonalArrows1)
                {
                    AddBlockedDirection(new Vector3Int(1, 0, -1));
                    backwardRightButton = diagonalArrowsCross1;
                }
                else
                {
                    RemoveBlockedDirection(new Vector3Int(1, 0, -1));
                    backwardRightButton = diagonalArrows1;
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        void ShowNetworkTypeSelection()
        {
            if (networkTypes == null || MasterGrid.gridLayerCtrl.GetNumberOfLinkNetworks() != networkTypes.Length)
            {
                networkTypes = new string[MasterGrid.gridLayerCtrl.GetNumberOfLinkNetworks()];
                for (int i = 0; i < MasterGrid.gridLayerCtrl.GetNumberOfLinkNetworks(); i++)
                {
                    networkTypes[i] = MasterGrid.gridLayerCtrl.GetLinkNetworkAtIndex(i).ID;
                }
            }

            selectedNetworkTypes = EditorGUILayout.Popup(selectedNetworkTypes, networkTypes);
        }
        #endregion

        void AddBlockedDirection(Vector3Int _direction)
        {
            layerItem.AddBlockedLink(_direction, MasterGrid.gridLayerCtrl.GetLinkNetworkAtIndex(selectedNetworkTypes));
        }

        void RemoveBlockedDirection(Vector3Int _direction)
        {
            layerItem.RemoveBlockedLink(_direction, MasterGrid.gridLayerCtrl.GetLinkNetworkAtIndex(selectedNetworkTypes));
        }
    }
}


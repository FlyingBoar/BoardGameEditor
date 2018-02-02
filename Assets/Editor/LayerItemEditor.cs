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
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Edit Links :", EditorStyles.boldLabel);
            GUILayout.Space(3);

            ShowForwardButtons();
            ShowCentralButtons();
            ShowBackwardButtons();

            EditorGUILayout.EndVertical();
        }

        void ShowForwardButtons()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(forwardLeftButton, GUILayout.Height(30), GUILayout.Width(30)))
            {
                if (forwardLeftButton == diagonalArrows1)
                {
                    forwardLeftButton = diagonalArrowsCross1;
                }
                else
                {
                    forwardLeftButton = diagonalArrows1;
                }
            }
            else if (GUILayout.Button(forwardButton, GUILayout.Height(30), GUILayout.Width(30)))
            {
                if (forwardButton == verticalArrows)
                {
                    forwardButton = verticalArrowsCross;

                }
                else
                {

                    forwardButton = verticalArrows;
                }
            }
            else if (GUILayout.Button(forwardRightButton, GUILayout.Height(30), GUILayout.Width(30)))
            {
                if (forwardRightButton == diagonalArrows2)
                {

                    forwardRightButton = diagonalArrowsCross2;
                }
                else
                {

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

                    leftButton = horizontalArrowsCross;
                }
                else
                {

                    leftButton = horizontalArrows;
                }
            }
            else if (GUILayout.Button(centralButton, GUILayout.Height(30), GUILayout.Width(30)))
            {
                if (centralButton == central)
                {
                    forwardLeftButton = diagonalArrowsCross1;
                    forwardButton = verticalArrowsCross;
                    forwardRightButton = diagonalArrowsCross2;

                    rightButton = horizontalArrowsCross;
                    centralButton = centralCross;
                    leftButton = horizontalArrowsCross;

                    backwardLeftButton = diagonalArrowsCross2;
                    backwardButton = verticalArrowsCross;
                    backwardRightButton = diagonalArrowsCross1;
                }
                else
                {
                    forwardLeftButton = diagonalArrows1;
                    forwardButton = verticalArrows;
                    forwardRightButton = diagonalArrows2;

                    rightButton = horizontalArrows;
                    centralButton = central;
                    leftButton = horizontalArrows;

                    backwardLeftButton = diagonalArrows2;
                    backwardButton = verticalArrows;
                    backwardRightButton = diagonalArrows1;
                }
            }
            else if (GUILayout.Button(rightButton, GUILayout.Height(30), GUILayout.Width(30)))
            {
                if (rightButton == horizontalArrows)
                {

                    rightButton = horizontalArrowsCross;
                }
                else
                {
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
                    backwardLeftButton = diagonalArrowsCross2;

                }
                else
                {

                    backwardLeftButton = diagonalArrows2;
                }
            }
            else if (GUILayout.Button(backwardButton, GUILayout.Height(30), GUILayout.Width(30)))
            {
                if (backwardButton == verticalArrows)
                {

                    backwardButton = verticalArrowsCross;
                }
                else
                {

                    backwardButton = verticalArrows;
                }
            }
            else if (GUILayout.Button(backwardRightButton, GUILayout.Height(30), GUILayout.Width(30)))
            {
                if (backwardRightButton == diagonalArrows1)
                {

                    backwardRightButton = diagonalArrowsCross1;
                }
                else
                {

                    backwardRightButton = diagonalArrows1;
                }
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Grid
{
    public class GridControllerVisualizer
    {
        GridController gridCtrl;

        //public InputAdapter_Tester InputTester;                                                       // Resa statica per Consentire accesso dal grid Controller
        Vector3 MousePos { get { return GridInput.MousePositionOnGridPlane(); } }    // Resa statica per Consentire accesso dal grid Controller

        public bool ShowGrid = true;
        public Color GridHandlesColor = Color.cyan;
        public bool ShowLayersLink = true;
        public bool[] LinkArray;
        public bool ShowMousePosition = true;
        public bool ShowMouseCell = true;
        [HideInInspector]
        public Cell SelectedCell;

        public GridControllerVisualizer(GridController _gridCtrl)
        {
            gridCtrl = _gridCtrl;
        }

        public void DrawHandles()
        {
            List<Cell> cellList = gridCtrl.GetListOfCells();
            if (cellList.Count <= 0)
                return;

            foreach (Cell cell in cellList)
            {
                if (ShowGrid)
                    DisplayCell(cell, GridHandlesColor);
                if (ShowLayersLink)
                {
                    if (LinkArray == null)
                        break;
                    for (int i = 0; i < LinkArray.Length; i++)
                    {
                        if(LinkArray[i])
                            DisplayLayerLink(cell, gridCtrl.LayerCtrl.GetLayerAtIndex(i));
                    }
                }
            }

            if (ShowMousePosition)
                DisplayMousePosition(Color.red);
            if (ShowMouseCell)
                DisplayMouseCell(Color.red);
        }

        void DisplayCell(Cell _cell, Color color)
        {
            if (_cell == null)
                return;

            if (_cell == SelectedCell)
                Handles.color = Color.red;
            else
                Handles.color = color;

            Handles.DrawWireCube(_cell.GetPosition(), _cell.GetRadius() * 2);
            Handles.DrawWireCube(_cell.GetPosition(), (_cell.GetRadius() / 25f));
        }

        void DisplayLayerLink(Cell _cell, Layer _layer)
        {
            Handles.color = _layer.HandlesColor;
            foreach (Cell link in _cell.GetCellData().GetLayeredLink(_layer))
            {
                Vector3 line = link.GetPosition() - _cell.GetPosition();
                Handles.DrawLine(_cell.GetPosition() + line * 0.25f, _cell.GetPosition() + line * .75f);
            }
        }

        void DisplayMousePosition(Color _color)
        {
            Handles.color = _color;
            Handles.DrawWireDisc(MousePos, Vector3.up, .5f);
        }

        void DisplayMouseCell(Color _color)
        {
            Handles.color = _color;
            Cell _mouseCell = gridCtrl.GetCellFromPosition(MousePos);

            if(_mouseCell != null)
                Handles.DrawWireCube(_mouseCell.GetPosition(), _mouseCell.GetRadius() * 2.01f);
        }
    }
}


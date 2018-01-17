using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Grid
{
    public class GridVisualizer
    {
        public GridController GridCtrl;

        //public InputAdapter_Tester InputTester;                                                       // Resa statica per Consentire accesso dal grid Controller
        Vector3 MousePos { get { return GridInput.MousePositionOnGridPlane(); } }    // Resa statica per Consentire accesso dal grid Controller

        public bool ShowGrid = true;
        public Color GridHandlesColor = Color.cyan;
        public bool ShowLayersLink = true;
        public bool ShowMousePosition;
        public bool ShowMouseCell;
        [HideInInspector]
        public Cell SelectedCell;

        public bool ShowMouseAction;

        public GridVisualizer(GridController _gridCtrl)
        {
            GridCtrl = _gridCtrl;
        }

        public void DrawHandles()
        {
            List<Cell> cellList = GridCtrl.GetCellsList();
            if (cellList.Count <= 0)
                return;

            foreach (Cell cell in cellList)
            {
                if (ShowGrid)
                    DisplayCell(cell, GridHandlesColor);
                if (ShowLayersLink)
                {
                    if (GridCtrl.LayerCtrl.SelectedLayer >= 0)
                        DisplayLayerLink(cell, GridCtrl.LayerCtrl.GetLayerAtIndex(GridCtrl.LayerCtrl.SelectedLayer));
                }
            }

            if(ShowMouseAction)
                DisplayLinkPreview(Color.red);

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
            {
                Handles.color = Color.red;
                Handles.DrawWireCube(_cell.GetPosition(), _cell.GetRadius() * 1.8f);
            }

            Handles.color = color;
            Handles.DrawWireCube(_cell.GetPosition(), _cell.GetRadius() * 2);
            Handles.DrawWireCube(_cell.GetPosition(), (_cell.GetRadius() / 25f));
        }

        void DisplayLayerLink(Cell _cell, Layer _layer)
        {
            Handles.color = _layer.HandlesColor;
            foreach (Vector3Int link in _cell.GetCellData().GetLinkCoordinates(_layer))
            {
                Vector3 line = GridCtrl.GetPositionByCoordinates(link) - _cell.GetPosition();
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
            Cell _mouseCell = GridCtrl.GetCellFromPosition(MousePos);

            if(_mouseCell != null)
                Handles.DrawWireCube(_mouseCell.GetPosition(), _mouseCell.GetRadius() * 2.01f);
        }

        void DisplayLinkPreview(Color _color)
        {
            Handles.color = _color;
            Cell _mouseCell = GridCtrl.GetCellFromPosition(MousePos);

            if (_mouseCell != null)
                Handles.DrawLine(SelectedCell.GetPosition(), _mouseCell.GetPosition());
        }
    }
}


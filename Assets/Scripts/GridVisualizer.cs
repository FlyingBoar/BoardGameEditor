using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Grid
{
    public class GridVisualizer
    {
        public GridController GridCtrl;

        Vector3 MousePos { get { return GridInput.MousePositionOnGridPlane(); } }    

        public bool ShowGrid = true;
        public Color GridHandlesColor = Color.cyan;
        public bool ShowLayersLink = true;
        public bool ShowMousePosition;
        public bool ShowMouseCell;
        [HideInInspector]
        public Vector2Int SelectedCell;

        public bool ShowMouseAction;

        public GridVisualizer(GridController _gridCtrl)
        {
            GridCtrl = _gridCtrl;
        }

        public void DrawHandles()
        {

            DisplayMouseCell(MasterGrid.gridLayerCtrl.GetSelectedLayer().Data.Color);

            //if(SelectedCell != null)
            //{
            //    DisplayCell(SelectedCell, Color.red);
            //    DisplayLayerLink(SelectedCell, MasterGrid.gridLayerCtrl.GetSelectedLayer());
            //}

            //List<Cell> cellList = GridCtrl.GetCellsList();
            //if (cellList.Count <= 0)
            //    return;

            //foreach (Cell cell in cellList)
            //{
            //    if (ShowGrid)
            //        DisplayCell(cell, GridHandlesColor);
            //    if (ShowLayersLink)
            //    {
            //        if (GridCtrl.LayerCtrl.SelectedLayer >= 0)
            //            DisplayLayerLink(cell, GridCtrl.LayerCtrl.GetLayerAtIndex(GridCtrl.LayerCtrl.SelectedLayer));
            //    }
            //}

            //if(ShowMouseAction)
            //    DisplayLinkPreview(Color.red);

            //if (ShowMousePosition)
            //    DisplayMousePosition(Color.red);
            //if (ShowMouseCell)
            //    DisplayMouseCell(Color.red);
        }

        //TODO: check con Luca/Fulvio l'uso di queste due funzioni (se non chiamate da nessuno, togliamole)
        /// <summary>
        /// [Deprecato]
        /// </summary>
        //void DisplayLayerLink(Cell _cell, Layer _layer)
        //{
        //    Handles.color = _layer.Data.Color;
        //    foreach (Vector3Int link in _cell.GetCellData().GetLinkCoordinates(_layer))
        //    {
        //        Vector3 line = MasterGrid.GetPositionByCoordinates(link) - _cell.GetPosition();
        //        Handles.DrawLine(_cell.GetPosition() + line * 0.25f, _cell.GetPosition() + line * .75f);
        //    }
        //}

        void DisplayMouseCell(Color _color)
        {
            Handles.color = _color;
            Vector3 _mouseCell = MasterGrid.GetPositionByCoordinates(MasterGrid.GetCoordinatesByPosition(MousePos));

            if(_mouseCell != null)
            {
                Handles.DrawWireCube(_mouseCell, GridCtrl.SectorData.Radius * 2.01f);
                _color.a *= .1f;
                Handles.color = _color;
                List<Vector2Int> neighbours = MasterGrid.GetNeighbours(MasterGrid.GetCoordinatesByPosition(_mouseCell));
                foreach (Vector2Int _neighbour in neighbours)
                {
                    Vector3 neighbourPos = MasterGrid.GetPositionByCoordinates(_neighbour);
                    Handles.DrawWireCube(neighbourPos, GridCtrl.SectorData.Radius * 2.01f);
                }
                _color.a = 1;
                foreach (Vector2Int _neighbour in neighbours)
                {
                    Vector3 neighbourPos = MasterGrid.GetPositionByCoordinates(_neighbour);
                    ShowLink(_mouseCell, neighbourPos, _color);
                }
            }
        }

        void ShowLink(Vector3 _startLink, Vector3 _endLink, Color _color)
        {
            Handles.color = _color;
            Handles.DrawLine(_startLink, _endLink);
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace Grid
{
    public class GridVisualizer
    {
        public GridController GridCtrl;

        Vector3 MousePos {
            get { return GridInput.PointerPosition; }
        }    

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
            ShowGridHandles(MasterGrid.gridLayerCtrl.GetSelectedLayer().Data.Color);
        }

        void ShowGridHandles(Color _color)
        {
            Handles.color = _color;
            Vector3 _mouseCell = MasterGrid.GetPositionByCoordinates(MasterGrid.GetCoordinatesByPosition(MousePos));

            if(_mouseCell != null)
            {
                Handles.DrawWireCube(_mouseCell, GridCtrl.GridData.Radius * 2.01f);
                _color.a *= .1f;
                Handles.color = _color;
                List<Vector2Int> neighbours = MasterGrid.GetNeighbours(MasterGrid.GetCoordinatesByPosition(_mouseCell));
                foreach (Vector2Int _neighbour in neighbours)
                {
                    Vector3 neighbourPos = MasterGrid.GetPositionByCoordinates(_neighbour);
                    Handles.DrawWireCube(neighbourPos, GridCtrl.GridData.Radius * 2.01f);
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


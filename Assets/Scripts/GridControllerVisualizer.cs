using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [RequireComponent(typeof(GridController))]
    public class GridControllerVisualizer : MonoBehaviour
    {
        GridController _gridCtrl;
        GridController gridCtrl
        {
            get
            {
                if (!_gridCtrl)
                    _gridCtrl = GetComponent<GridController>();

                return _gridCtrl;
            }
        }
        //public InputAdapter_Tester InputTester;                                                       // Resa statica per Consentire accesso dal grid Controller
        Vector3 MousePos { get { return InputAdapter_Tester.Test_FindMousePositionOnGridPlane(); } }    // Resa statica per Consentire accesso dal grid Controller

        public bool ShowGrid;
        public bool ShowLink;
        public bool ShowMousePosition;
        public bool ShowMouseCell;

        private void OnDrawGizmos()
        {
            List<Cell> cellList = gridCtrl.GetListOfCells();
            if (cellList.Count <= 0)
                return;

            foreach (Cell cell in cellList)
            {
                if (ShowGrid)
                    DisplayCell(cell, Color.cyan);
                if (ShowLink)
                    DisplayLink(cell, Color.black);
            }

            if (ShowMousePosition)
                DisplayMousePosition(Color.red);
            if (ShowMouseCell)
                DisplayMouseCell(Color.red);
        }

        void DisplayCell(Cell _cell, Color color)
        {
            Gizmos.color = color;
            Gizmos.DrawWireCube(_cell.GetCenter(), _cell.GetRadius() * 2);
            Gizmos.DrawWireCube(_cell.GetCenter(), (_cell.GetRadius() / 25f));
        }

        void DisplayLink(Cell _cell, Color color)
        {
            Gizmos.color = color;
            foreach (ILink link in _cell.GetNeighbourgs(gridCtrl.LayerCtrl.Layers[0]))
            {
                Vector3 line = link.GetPosition() - _cell.GetCenter();
                Gizmos.DrawLine(_cell.GetCenter() + line * 0.25f, _cell.GetCenter() + line * .75f);
            }
        }

        void DisplayMousePosition(Color _color)
        {
            Gizmos.color = _color;
            Gizmos.DrawSphere(MousePos, .5f);
        }

        void DisplayMouseCell(Color _color)
        {
            Gizmos.color = _color;
            Cell _mouseCell = gridCtrl.GetCellFromPosition(MousePos);

            if(_mouseCell != null)
                Gizmos.DrawWireCube(_mouseCell.GetCenter(), _mouseCell.GetRadius() * 2.01f);
        }
    }
}


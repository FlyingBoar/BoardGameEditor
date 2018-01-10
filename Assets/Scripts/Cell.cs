using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Grid
{
    [System.Serializable]
    public class Cell
    {
        [SerializeField]
        CellData cellData;

        GridController gridCtrl;

        public Vector3Int GridCoordinates
        {
            get
            {
                Vector3Int coordinates = gridCtrl.GetCoordinatesByPosition(cellData.Position);
                return new Vector3Int(coordinates.x, coordinates.y, coordinates.z);
            }
        }
        Vector3 distance {
            get { return cellData.Sector.Diameter + gridCtrl.ResolutionCorrection; }
        }
        bool isGridEnbedded
        {
            get
            {
                if (gridCtrl != null)
                    return true;
                else
                    return false;
            }
        }

        public Cell(CellData _data, GridController _ctrl)
        {
            cellData = _data;
            gridCtrl = _ctrl;
        }

        public Cell(CellData _data)
        {
            cellData = _data;
        }

        #region API
        #region Getter and Setters
        public CellData GetCellData()
        {
            return cellData;
        }

        public void SetPosition(Vector3 _position)
        {
            cellData.Position = _position;
        }

        public Vector3 GetPosition()
        {
            if(!isGridEnbedded)
                return cellData.Position;
            else
            {
                Vector3 centerPos = gridCtrl.GetPositionByCoordinates(GridCoordinates);
                //centerPos = new Vector3(GridCoordinates.x * distance.x, GridCoordinates.y * distance.y, GridCoordinates.z * distance.z);
                //centerPos -= cellData.SectorData.Radius;
                //centerPos += gridCtrl.Origin;
                return centerPos;
            }
        }

        public Vector3 GetRadius()
        {
            return cellData.Sector.Radius;
        }
        #endregion

        public List<Vector3Int> GetNeighbourgs(Layer _layer)
        {
            return cellData.GetLinkCoordinates(_layer);
        }

        public void Link(Vector3Int _node, Layer _layer)
        {
            if(_node != GridCoordinates)
                cellData.AddLink(_node, _layer);
        }

        public void UnLink(Vector3Int _node, Layer _layer)
        {
            cellData.RemoveLink(_node, _layer);
        }

        public void UnLinkAll(Layer _layer)
        {
            List<Vector3Int> linkedNodes = cellData.GetLinkCoordinates(_layer);
            for (int i = 0; i < linkedNodes.Count; i++)
            {
                gridCtrl.GetCellByCoordinates(linkedNodes[i]).UnLink(GridCoordinates, _layer);
            }
            linkedNodes.Clear();
        }
        #endregion
    }
}
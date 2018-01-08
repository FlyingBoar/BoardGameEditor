using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Grid
{
    [System.Serializable]
    public class Cell
    {
        CellData cellData;

        GridController gridCtrl;
        Vector3Int gridPosition;
        Vector3 distance { get { return cellData.Sector.Diameter + gridCtrl.ResolutionCorrection; } }
        bool isGridEnbedded
        {
            get
            {
                if (gridCtrl != null)
                    return true;
                else
                    return
                        false;
            }
        }

        public Cell(CellData _data, GridController _ctrl, Vector3Int _gridPos)
        {
            cellData = _data;
            gridCtrl = _ctrl;
            gridPosition = _gridPos;
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
                Vector3 centerPos;
                centerPos = new Vector3(gridPosition.x * distance.x, gridPosition.y * distance.y, gridPosition.z * distance.z);
                //centerPos -= cellData.SectorData.Radius;
                centerPos += gridCtrl.Origin;
                return centerPos;
            }
        }

        public Vector3 GetRadius()
        {
            return cellData.Sector.Radius;
        }
        #endregion
        public List<Cell> GetNeighbourgs(Layer _layer)
        {
            return cellData.GetLayeredLink(_layer);
        }

        public void Link(Cell _node, Layer _layer)
        {
                cellData.AddLink(_node, _layer);
        }

        public void UnLink(Cell _node, Layer _layer)
        {
                cellData.RemoveLink(_node, _layer);
        }

        public void UnLinkAll(Layer _layer)
        {
            List<Cell> linkedNodes = cellData.GetLayeredLink(_layer);
            for (int i = 0; i < linkedNodes.Count; i++)
                linkedNodes[i].UnLink(this, _layer);
            for (int i = 0; i < linkedNodes.Count; i++)
                cellData.RemoveLink(linkedNodes[i], _layer);
        }
        #endregion
    }
}
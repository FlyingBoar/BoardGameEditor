using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Grid
{
    public class Cell : ISector, ILayeredLink
    {
        CellData cellData;

        GridController gridCtrl;
        Vector3Int gridPosition;
        Vector3 distance { get { return cellData.SectorData.Diameter + gridCtrl.ResolutionCorrection; } }
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

        public CellData GetCellData()
        {
            return cellData;
        }

        public void SetPosition(Vector3 _position)
        {
            cellData.NodeData.Position = _position;
        }

        public Vector3 GetRadius()
        {
            return cellData.SectorData.Radius;
        }        

        public Cell(CellData _data)
        {
            cellData = _data;
        }

        public Cell(CellData _data, GridController _ctrl, Vector3Int _gridPos)
        {
            cellData = _data;
            gridCtrl = _ctrl;
            gridPosition = _gridPos;
        }

        public Vector3 GetPosition()
        {
            if(!isGridEnbedded)
                return cellData.NodeData.Position;
            else
            {
                Vector3 centerPos;
                centerPos = new Vector3(gridPosition.x * distance.x, gridPosition.y * distance.y, gridPosition.z * distance.z);
                //centerPos -= cellData.SectorData.Radius;
                centerPos += gridCtrl.Origin;
                return centerPos;
            }
        }

        #region ISector

        public Vector3 GetCenter()
        {
            return GetPosition(); // da rivedere
        }

        public bool IsInside()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region ILink

        public List<INode> GetNeighbourgs(Layer _layer)
        {
            return cellData.LinkData.GetLayeredLink(_layer);
        }

        public void Link(INode _node, Layer _layer)
        {
            cellData.LinkData.AddLink(_node, _layer);
        }

        public void UnLink(INode _node, Layer _layer)
        {
            cellData.LinkData.RemoveLink(_node, _layer);
        }

        public void UnLink(ILayeredLink _link, Layer _layer)
        {
            cellData.LinkData.RemoveLink(_link, _layer);
        }

        public void UnLinkAll(Layer _layer)
        {
            if (!_layer.IsEditable)
                return;        
            List<ILayeredLink> linkedNodes = cellData.LinkData.GetLayeredLink(_layer).ConvertAll(l => l as ILayeredLink);
            for (int i = 0; i < linkedNodes.Count; i++)
                linkedNodes[i].UnLink(this, _layer);
            for (int i = 0; i < linkedNodes.Count; i++)
                cellData.LinkData.RemoveLink(linkedNodes[i], _layer);
        }
        #endregion
    }
}
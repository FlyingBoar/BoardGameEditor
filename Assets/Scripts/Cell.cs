using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Grid
{
    public class Cell : ISector, ILink
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
                centerPos += gridCtrl.transform.position;
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

        public List<INode> GetNeighbourgs(string _layer)
        {
            foreach (LayeredLink layeredLink in cellData.LinkData.LayeredLinkedNodes)
                if (layeredLink.Layer == _layer)
                    return layeredLink.LinkedNodes;
            return null;
        }

        public void Link(INode _node, string _layer)
        {
            foreach (LayeredLink layeredLink in cellData.LinkData.LayeredLinkedNodes)
            {
                if (layeredLink.Layer == _layer)
                {
                    layeredLink.LinkedNodes.Add(_node);
                    return;
                }
            }
        }

        public void UnLink(INode _node, string _layer)
        {
            foreach (LayeredLink layeredLink in cellData.LinkData.LayeredLinkedNodes)
            {
                if (layeredLink.Layer == _layer)
                {
                    layeredLink.LinkedNodes.Remove(_node);
                    return;
                }
            }
        }

        public void UnLink(ILink _link, string _layer)
        {
            foreach (LayeredLink layeredLink in cellData.LinkData.LayeredLinkedNodes)
            {
                if (layeredLink.Layer == _layer)
                {
                    layeredLink.LinkedNodes.Remove(_link);
                    return;
                }
            }
        }

        public void UnLinkAll(string _layer)
        {
            foreach (LayeredLink layeredLink in cellData.LinkData.LayeredLinkedNodes)
            {
                if (layeredLink.Layer == _layer)
                {                   
                    List<ILink> linkedNodes = layeredLink.LinkedNodes.ConvertAll(l => l as ILink);
                    for (int i = 0; i < linkedNodes.Count; i++)
                    {
                        linkedNodes[i].UnLink(this, _layer);
                    }
                    layeredLink.LinkedNodes.Clear();
                    return;
                }
            }
        }
        #endregion
    }
}
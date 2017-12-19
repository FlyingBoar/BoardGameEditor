using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class Cell : ISector, ILink
    {
        CellData cellData;

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

        public Vector3 GetPosition()
        {
            return cellData.NodeData.Position;
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

        public List<INode> GetNeighbourgs()
        {
            return cellData.LinkData.LinkedNodes;
        }

        public void Link(INode _node)
        {
            cellData.LinkData.LinkedNodes.Add(_node);
        }

        public void UnLink(INode _node)
        {
            cellData.LinkData.LinkedNodes.Remove(_node);
        }

        public void UnLink(ILink _link)
        {
            cellData.LinkData.LinkedNodes.Remove(_link);
        }

        public void UnLinkAll()
        {
            List<ILink> linkedNodes = cellData.LinkData.LinkedNodes.ConvertAll(l => l as ILink);
            for (int i = 0; i < linkedNodes.Count; i++)
            {
                linkedNodes[i].UnLink(this);
            }
            cellData.LinkData.LinkedNodes.Clear();
        }

        #endregion
    }
}
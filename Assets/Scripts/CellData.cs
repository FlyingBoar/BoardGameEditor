using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [System.Serializable]
    public class CellData
    {
        public Vector3 Position;
        public SectorData Sector;

        [SerializeField]
        List<LayeredLink> LayeredLinkedNodes = new List<LayeredLink>();

        public CellData(SectorData _sector, Vector3 _position, Layer _layer, List<Cell> _nodes = null)
        {
            Sector = _sector;
            Position = _position;

            if (_nodes != null)
                AddLinkList(_nodes, _layer);
            else
                AddLinkList(new List<Cell>(), _layer);
        }

        public CellData(AreaShape _shape, Vector3 _position, Vector3 _radius, Layer _layer, List<Cell> _nodes = null)
        {
            Position = _position;

            Sector = new SectorData();
            Sector.Shape = _shape;
            Sector.Radius = _radius;

            if (_nodes != null)
                AddLinkList(_nodes, _layer);
            else
                AddLinkList(new List<Cell>(), _layer);
        }

        #region API
        public void AddLinkList(List<Cell> _nodes, Layer _layer)
        {
            foreach (LayeredLink layeredLink in LayeredLinkedNodes)
            {
                if (layeredLink.Layer == _layer)
                {
                    foreach (Cell node in _nodes)
                    {
                        if (!layeredLink.LinkedNodes.Contains(node.GetPosition()))
                            layeredLink.LinkedNodes.Add(node.GetPosition());
                    }
                    return;
                }
            }

            LayeredLinkedNodes.Add(new LayeredLink(_layer));
            AddLinkList(_nodes, _layer);
        }

        public void AddLink(Cell _node, Layer _layer)
        {
            foreach (LayeredLink layeredLink in LayeredLinkedNodes)
            {
                if (layeredLink.Layer == _layer)
                {
                    if (!layeredLink.LinkedNodes.Contains(_node.GetPosition()))
                        layeredLink.LinkedNodes.Add(_node.GetPosition());
                    return;
                }
            }

            LayeredLinkedNodes.Add(new LayeredLink(_layer));
            AddLink(_node, _layer);
        }

        public void RemoveLink(Cell _node, Layer _layer)
        {
            foreach (LayeredLink layeredLink in LayeredLinkedNodes)
            {
                if (layeredLink.Layer == _layer)
                {
                    layeredLink.LinkedNodes.Remove(_node.GetPosition());
                    return;
                }
            }
        }

        public List<Vector3> GetLayeredLink(Layer _layer)
        {
            foreach (LayeredLink layeredLink in LayeredLinkedNodes)
                if (layeredLink.Layer == _layer)
                    return layeredLink.LinkedNodes;
            return null;
        }
        #endregion

        internal void RemoveLayeredLink(Layer _layer)
        {
            for (int i = 0; i < LayeredLinkedNodes.Count; i++)
            {
                if (LayeredLinkedNodes[i].Layer == _layer)
                {
                    LayeredLinkedNodes.Remove(LayeredLinkedNodes[i]);
                    return;
                }
            }
        }

        public enum AreaShape
        {
            Circle, Square, Hexagon
        }

        [System.Serializable]
        public struct SectorData
        {
            public AreaShape Shape;
            public Vector3 Radius;
            public Vector3 Diameter { get { return Radius * 2; } }
        }

        [System.Serializable]
        struct LayeredLink
        {
            public Layer Layer;
            public List<Vector3> LinkedNodes;

            public LayeredLink(Layer _layer)
            {
                Layer = _layer;
                LinkedNodes = new List<Vector3>();
            }

            public LayeredLink(List<Vector3> _linkedNodes, Layer _layer)
            {
                Layer = _layer;
                LinkedNodes = _linkedNodes;
            }

            public LayeredLink(Vector3 _linkedNode, Layer _layer)
            {
                Layer = _layer;
                LinkedNodes = new List<Vector3>() { _linkedNode };
            }
        }
    }
}
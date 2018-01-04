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
            Sector.Shape = _shape;
            Position = _position;
            Sector.Radius = _radius;

            if (_nodes != null)
                AddLinkList(_nodes, _layer);
            else
                AddLinkList(new List<Cell>(), _layer);
        }

        public void AddLinkList(List<Cell> _nodes, Layer _layer)
        {
            foreach (LayeredLink layeredLink in LayeredLinkedNodes)
            {
                if (layeredLink.Layer == _layer)
                {
                    foreach (Cell node in _nodes)
                    {
                        if (!layeredLink.LinkedNodes.Contains(node))
                            layeredLink.LinkedNodes.Add(node);
                    }
                    return;
                }
            }

            LayeredLinkedNodes.Add(new LayeredLink(_nodes, _layer));
        }

        public void AddLink(Cell _node, Layer _layer)
        {
            foreach (LayeredLink layeredLink in LayeredLinkedNodes)
            {
                if (layeredLink.Layer == _layer)
                {
                    if (!layeredLink.LinkedNodes.Contains(_node))
                        layeredLink.LinkedNodes.Add(_node);
                    return;
                }
            }

            LayeredLinkedNodes.Add(new LayeredLink(_node, _layer));
        }

        public void RemoveLink(Cell _node, Layer _layer)
        {
            if (!_layer.IsEditable)
                return;

            foreach (LayeredLink layeredLink in LayeredLinkedNodes)
            {
                if (layeredLink.Layer == _layer)
                {
                    layeredLink.LinkedNodes.Remove(_node);
                    return;
                }
            }
        }

        public List<Cell> GetLayeredLink(Layer _layer)
        {
            foreach (LayeredLink layeredLink in LayeredLinkedNodes)
                if (layeredLink.Layer == _layer)
                    return layeredLink.LinkedNodes;
            return null;
        }

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
            public List<Cell> LinkedNodes;
            public Layer Layer;

            public LayeredLink(List<Cell> _linkedNodes, Layer _layer)
            {
                Layer = _layer;
                LinkedNodes = _linkedNodes;
            }

            public LayeredLink(Cell _linkedNode, Layer _layer)
            {
                Layer = _layer;
                LinkedNodes = new List<Cell>() { _linkedNode };
            }
        }
    }



}
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

        public CellData(SectorData _sector, Vector3 _position, Layer _layer, List<Vector3Int> _nodes = null)
        {
            Sector = _sector;
            Position = _position;

            if (_nodes != null)
                AddLinkList(_nodes, _layer);
            else
                AddLinkList(new List<Vector3Int>(), _layer);
        }

        public CellData(AreaShape _shape, Vector3 _position, Vector3 _radius, Layer _layer, List<Vector3Int> _nodes = null)
        {
            Position = _position;

            Sector = new SectorData();
            Sector.Shape = _shape;
            Sector.Radius = _radius;

            if (_nodes != null)
                AddLinkList(_nodes, _layer);
            else
                AddLinkList(new List<Vector3Int>(), _layer);
        }

        #region API
        public void AddLinkList(List<Vector3Int> _nodes, Layer _layer)
        {
            foreach (LayeredLink layeredLink in LayeredLinkedNodes)
            {
                if (layeredLink.Layer == _layer)
                {
                    foreach (Vector3Int node in _nodes)
                    {
                        if (!layeredLink.LinkedCoordinates.Contains(node))
                            layeredLink.LinkedCoordinates.Add(node);
                    }
                    return;
                }
            }

            LayeredLinkedNodes.Add(new LayeredLink(_nodes, _layer));
        }

        public void AddLink(Vector3Int _node, Layer _layer)
        {
            foreach (LayeredLink layeredLink in LayeredLinkedNodes)
            {
                if (layeredLink.Layer == _layer)
                {
                    if (!layeredLink.LinkedCoordinates.Contains(_node))
                        layeredLink.LinkedCoordinates.Add(_node);
                    return;
                }
            }

            LayeredLinkedNodes.Add(new LayeredLink(_node, _layer));
        }

        public void RemoveLink(Vector3Int _node, Layer _layer)
        {
            foreach (LayeredLink layeredLink in LayeredLinkedNodes)
            {
                if (layeredLink.Layer == _layer)
                {
                    layeredLink.LinkedCoordinates.Remove(_node);
                    return;
                }
            }
        }

        public List<Vector3Int> GetLinkCoordinates(Layer _layer)
        {
            foreach (LayeredLink layeredLink in LayeredLinkedNodes)
                if (layeredLink.Layer == _layer)
                    return layeredLink.LinkedCoordinates;
            return null;
        }

        public List<LayeredLink> GetLayeredLinks()
        {
            return LayeredLinkedNodes;
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
            Square/*, Circle, Hexagon*/
        }

        [System.Serializable]
        public struct SectorData
        {
            public AreaShape Shape;
            public Vector3 Radius;
            public Vector3 Diameter { get { return Radius * 2; } }
        }

        [System.Serializable]
        public struct LayeredLink
        {
            public List<Vector3Int> LinkedCoordinates;
            public Layer Layer;

            public LayeredLink(List<Vector3Int> _linkedCoordinates, Layer _layer)
            {
                Layer = _layer;
                LinkedCoordinates = _linkedCoordinates;
            }

            public LayeredLink(Vector3Int _linkedCoordinate, Layer _layer)
            {
                Layer = _layer;
                LinkedCoordinates = new List<Vector3Int>() { _linkedCoordinate };
            }
        }
    }
}
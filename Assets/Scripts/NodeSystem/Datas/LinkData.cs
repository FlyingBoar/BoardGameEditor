using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [System.Serializable]
    public class LinkData 
    {
        List<LayeredLink> LayeredLinkedNodes = new List<LayeredLink>();

        public LinkData() { }

        public LinkData(Layer _layer)
        {
            AddLinkList(new List<INode>(), _layer);
        }

        public LinkData(List<INode> _nodes, Layer _layer)
        {
            AddLinkList(_nodes, _layer);
        }

        public void AddLinkList(List<INode> _nodes, Layer _layer)
        {
            foreach (LayeredLink layeredLink in LayeredLinkedNodes)
            {
                if(layeredLink.Layer == _layer)
                {
                    foreach (INode node in _nodes)
                    {
                        AddLink(node, _layer);
                    } 
                    return;
                }
            }

            LayeredLinkedNodes.Add(new LayeredLink(_nodes, _layer));
        }

        public void AddLink(INode _node, Layer _layer)
        {
            foreach (LayeredLink layeredLink in LayeredLinkedNodes)
            {
                if (layeredLink.Layer == _layer)
                {
                    if(!layeredLink.LinkedNodes.Contains(_node))
                        layeredLink.LinkedNodes.Add(_node);
                    return;
                }
            }

            LayeredLinkedNodes.Add(new LayeredLink(_node, _layer));
        }

        public void RemoveLink(INode _node, Layer _layer)
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

        public List<INode> GetLayeredLink(Layer _layer)
        {
            foreach (LayeredLink layeredLink in LayeredLinkedNodes)
                if (layeredLink.Layer == _layer)
                    return layeredLink.LinkedNodes;
            return null;
        }
    }

    [System.Serializable]
    public struct LayeredLink
    {
        public List<INode> LinkedNodes;
        public Layer Layer;

        public LayeredLink(List<INode> _linkedNodes, Layer _layer)
        {
            Layer = _layer;
            LinkedNodes = _linkedNodes;
        }

        public LayeredLink(INode _linkedNode, Layer _layer)
        {
            Layer = _layer;
            LinkedNodes = new List<INode>() { _linkedNode };
        }
    }
}
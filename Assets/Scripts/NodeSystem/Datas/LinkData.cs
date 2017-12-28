using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [System.Serializable]
    public class LinkData 
    {
        public List<LayeredLink> LayeredLinkedNodes = new List<LayeredLink>();

        public LinkData(string _layer)
        {
            AddLink(new List<INode>(), _layer);
        }

        public LinkData(List<INode> _nodes, string _layer)
        {
            AddLink(_nodes, _layer);
        }

        void AddLink(List<INode> _nodes, string _layer)
        {
            foreach (LayeredLink layeredLink in LayeredLinkedNodes)
            {
                if(layeredLink.Layer == _layer)
                {
                    layeredLink.LinkedNodes.AddRange(_nodes);
                    return;
                }
            }

            LayeredLinkedNodes.Add(new LayeredLink(_nodes, _layer));
        }
    }

    [System.Serializable]
    public struct LayeredLink
    {
        public List<INode> LinkedNodes;
        public string Layer;

        public LayeredLink(List<INode> _linkedNodes, string _layer)
        {
            LinkedNodes = _linkedNodes;
            Layer = _layer;
        }
    }
}
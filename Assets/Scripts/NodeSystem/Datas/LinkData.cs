using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BGEditor.NodeSystem
{
    [System.Serializable]
    public class LinkData 
    {
        public List<INode> LinkedNodes = new List<INode>();

        public LinkData() { }

        public LinkData(List<INode> _nodes)
        {
            LinkedNodes = _nodes;
        }
    }
}
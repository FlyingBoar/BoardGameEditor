using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public interface ILink : INode
    {
        List<INode> GetNeighbourgs(string _layer);

        void Link(INode _node, string _layer);
        void UnLink(INode _node, string _layer);
        void UnLink(ILink _link, string _layer);
        void UnLinkAll(string _layer);
    }
}

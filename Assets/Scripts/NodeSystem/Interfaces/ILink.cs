using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public interface ILink : INode
    {
        List<INode> GetNeighbourgs(Layer _layer);

        void Link(INode _node, Layer _layer);
        void UnLink(INode _node, Layer _layer);
        void UnLink(ILink _link, Layer _layer);
        void UnLinkAll(Layer _layer);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public interface ILayeredLink : INode
    {
        List<INode> GetNeighbourgs(Layer _layer);

        void Link(INode _node, Layer _layer);
        void UnLink(INode _node, Layer _layer);
        void UnLink(ILayeredLink _link, Layer _layer);
        void UnLinkAll(Layer _layer);
    }
}

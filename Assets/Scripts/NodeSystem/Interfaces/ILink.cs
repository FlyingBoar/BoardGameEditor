using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BGEditor.NodeSystem
{
    public interface ILink : INode
    {

        List<INode> GetNeighbourgs();

        void Link(INode _node);

        void UnLink(INode _node);
        void UnLink(ILink _link);

    }
}

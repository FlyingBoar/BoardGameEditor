using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BGEditor.NodeSystem
{
    public interface ISector : INode
    {

        bool IsInside();

        Vector3 GetCenter();

    }
}
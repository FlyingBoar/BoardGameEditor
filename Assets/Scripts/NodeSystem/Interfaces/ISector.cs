using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public interface ISector : INode
    {
        bool IsInside();
        Vector3 GetCenter();
    }
}
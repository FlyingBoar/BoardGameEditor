using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BGEditor.NodeSystem
{
    [System.Serializable]
    public class NodeData
    {
        public Vector3 Position;

        public NodeData() { }

        public NodeData(Vector3 _position)
        {
            Position = _position;
        }
    }
}
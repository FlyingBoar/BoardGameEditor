using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [System.Serializable]
    public class LinkNetworkType
    {
        public string ID;
        public Color Color;

        public LinkNetworkType(string _id, Color _color)
        {
            ID = _id;
            Color = _color;
        }

        public override string ToString()
        {
            return ID;
        }
    }
}


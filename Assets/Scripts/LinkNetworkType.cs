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

        public LinkNetworkType()
        {
            Color.a = 100;
        }

        public LinkNetworkType(string _id)
        {
            ID = _id;
            Color.a = 100;
        }

        public LinkNetworkType(string _id, Color _color)
        {
            ID = _id;
            Color = _color;
        }
    }
}


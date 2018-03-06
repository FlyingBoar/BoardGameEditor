using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [System.Serializable]
    public class LayerData
    {
        public string ID;
        public Color Color;
        public List<LayerItemData> ItemsInLayer = new List<LayerItemData>();

        public LayerData()
        {
            Color.a = 100;
        }

        public LayerData(string _id)
        {
            ID = _id;
            Color.a = 100;
        }

        public LayerData(string _id, Color _color)
        {
            ID = _id;
            Color = _color;
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [System.Serializable]
    public class LayerData
    {
        public string Name;
        public Color Color;
        public List<LayerItemData> ItemsInLayer;

        public LayerData()
        {
            Color.a = 100;
        }

        public LayerData(string _name)
        {
            Name = _name;
            Color.a = 100;
        }

        public LayerData(string _name, Color _color)
        {
            Name = _name;
            Color = _color;
        }
    }
}


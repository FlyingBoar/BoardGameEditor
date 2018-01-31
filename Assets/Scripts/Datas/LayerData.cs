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
        public List<LayerItemData> LayerItems;
    }
}


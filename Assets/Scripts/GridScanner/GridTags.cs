using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class GridTags : MonoBehaviour
    {
        public List<ScannerLayer> ScannerLayers = new List<ScannerLayer>();
    }

    public class ScannerLayer
    {
        public Layer Layer;
        public bool Active;

        public ScannerLayer(Layer _layer, bool _active)
        {
            Layer = _layer;
            Active = _active;
        }
    }
}
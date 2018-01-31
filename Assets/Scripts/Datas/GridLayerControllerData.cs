using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [System.Serializable]
    public class GridLayerControllerData
    {
        public List<Layer> Layers = new List<Layer>();

        public GridLayerControllerData() { }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class LayerManager : MonoBehaviour
    {
        public List<string> Layers = new List<string> { "Base" };

        public bool DoesTheLayerExist(string _layer)
        {
            foreach (string layer in Layers)
                if (layer == _layer)
                    return true;
            return false;
        }
    }
}


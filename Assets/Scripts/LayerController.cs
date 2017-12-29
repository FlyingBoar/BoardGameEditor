using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class LayerController : MonoBehaviour
    {
        public List<Layer> Layers = new List<Layer> { new Layer("Base", true) };
    }
}


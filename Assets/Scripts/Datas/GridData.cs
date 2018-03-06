using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [System.Serializable]
    public class GridData
    {
        public Vector2 Radius = Vector2.one;
        public Vector2 Diameter { get { return Radius * 2; } }

        public Vector3 Normal = Vector3.forward;
        public Vector3 Origin;
        public Vector2 ResolutionCorrection;
        
        public List<Layer> Layers;
    }
}

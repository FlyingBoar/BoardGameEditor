using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{ 
    [System.Serializable]
    public class SectorData
    {
        public AreaShape Shape;
        public Vector3 Radius;
        public Vector3 Diameter { get { return Radius * 2; } }

        public SectorData() { }

        public SectorData(AreaShape _shape, Vector3 _radius)
        {
            Shape = _shape;
            Radius = _radius;
        }

        public enum AreaShape
        {
            Circle, Square, Hexagon
        }
    }
}
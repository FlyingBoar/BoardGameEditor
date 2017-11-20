using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BGEditor.NodeSystem
{ 
    [System.Serializable]
    public class SectorData
    {
        public AreaShape Shape;
        public float Radius;

        public SectorData() { }

        public SectorData(AreaShape _shape, float _radius)
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
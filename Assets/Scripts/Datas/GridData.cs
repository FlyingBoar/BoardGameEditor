using System.Collections.Generic;
using UnityEngine;


namespace Grid
{
    [System.Serializable]
    public class GridData
    {
        public SectorData SectorData = new SectorData() { Radius = new Vector2(1, 1) };

        public Vector3 Normal = Vector3.forward;
        public Vector3 Origin;
        public Vector2 ResolutionCorrection;
        
        public List<Layer> Layers;
    }

    [System.Serializable]
    public struct SectorData
    {
        public Vector2 Radius;
        public Vector2 Diameter { get { return Radius * 2; } }
    }

    [System.Serializable]
    public struct LayeredLink
    {
        public List<Vector2Int> LinkedCoordinates;
        public Layer Layer;

        public LayeredLink(List<Vector2Int> _linkedCoordinates, Layer _layer)
        {
            Layer = _layer;
            LinkedCoordinates = _linkedCoordinates;
        }

        public LayeredLink(Vector2Int _linkedCoordinate, Layer _layer)
        {
            Layer = _layer;
            LinkedCoordinates = new List<Vector2Int>() { _linkedCoordinate };
        }
    }
}

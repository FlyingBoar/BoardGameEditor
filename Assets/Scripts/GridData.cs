using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [System.Serializable]
    public class GridData : ScriptableObject
    {
        public CellData.SectorData SectorData;

        public Vector3 Origin;
        public Vector3Int Size;
        public Vector3 ResolutionCorrection;

        public List<CellData> CellsData;

        public List<Layer> Layers;
    }
}

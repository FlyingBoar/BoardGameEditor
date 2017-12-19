using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [System.Serializable]
    public class CellData
    {
        public NodeData NodeData;
        public LinkData LinkData;
        public SectorData SectorData;
        
        public CellData() { }

        public CellData(NodeData _nodeData, LinkData _linkData, SectorData _sectorData)
        {
            NodeData = _nodeData;
            LinkData = _linkData;
            SectorData = _sectorData;
        }
    }
}
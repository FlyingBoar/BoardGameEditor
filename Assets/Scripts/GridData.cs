using System.Collections.Generic;
using UnityEngine;


namespace Grid
{
    [System.Serializable]
    public class GridData
    {
        public CellData.SectorData SectorData;

        public Vector3 Origin;
        public Vector3Int Size;
        public Vector3 ResolutionCorrection;

        Cell[,,] _cellsMatrix;
        public Cell[,,] CellsMatrix {
            set { _cellsMatrix = value;
                foreach (Cell cell in _cellsMatrix)
                {
                    if(cell != null)
                    {
                        Cells.Add(cell.GetCellData());
                    }
                }
            }
        }
        [SerializeField]
        List<CellData> Cells = new List<CellData>();

        public List<Layer> Layers;

        public List<CellData> GetCellDatas()
        {
            return Cells;
        }
    }
}

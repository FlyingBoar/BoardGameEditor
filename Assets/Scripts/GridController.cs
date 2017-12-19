using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Grid
{
    [ExecuteInEditMode]
    public class GridController : MonoBehaviour
    {
        public NodeNetworkData NetworkData;
        public SectorData SectorData;

        public bool ShowGrid;
        public bool ShowLink;

        public Vector3Int Size;
        protected Vector3 SizeInt;

        Cell[,,] CellsMatrix;

        Vector3 offSet;

        #region API
        public void CreateNewGrid(bool autoLinkCells = true)
        {
            //Reset operation
            ClearGrid();
            if (SectorData.Radius.x == 0)
                SectorData.Radius.x = float.Epsilon;
            if (SectorData.Radius.y == 0)
                SectorData.Radius.y = float.Epsilon;
            if (SectorData.Radius.z == 0)
                SectorData.Radius.z = float.Epsilon;

            SizeInt.x = (uint)(Size.x / SectorData.Diameter.x);
            SizeInt.y = (uint)(Size.y / SectorData.Diameter.y);
            SizeInt.z = (uint)(Size.z / SectorData.Diameter.z);

            offSet = CalculateOffset();
            //New grid creation
            CreateGrid();
            //Linking process
            if (autoLinkCells)
                LinkCells();
        }

        public void ClearGrid()
        {
            CellsMatrix = null;
        }

        public void Load()
        {
            if(NetworkData != null)
                LoadFromNetworkData(NetworkData);
        }

        public void SaveCurrent()
        {
            Save(GetListOfCells());
        }

        #region Getter
        public Cell GetCentralCell()
        {
            return this.GetCellFromPosition(transform.position);
        }

        public List<Cell> GetGridCorners()
        {
            List<Cell> tempList = new List<Cell>();
            Vector3 offset = CalculateOffset();
            tempList.Add(this.GetCellFromPosition(new Vector3(-offset.x, 0, -offset.z)));
            tempList.Add(this.GetCellFromPosition(new Vector3(-offset.x, 0, offset.z)));
            tempList.Add(this.GetCellFromPosition(new Vector3(offset.x, 0, offset.z)));
            tempList.Add(this.GetCellFromPosition(new Vector3(offset.x, 0, -offset.z)));

            return tempList;
        }

        public List<Cell> GetListOfCells()
        {
            List<Cell> cellsList = new List<Cell>();

            if(CellsMatrix != null)
            {
                for (int i = 0; i < CellsMatrix.GetLength(0); i++)
                    for (int j = 0; j < CellsMatrix.GetLength(1); j++)
                        for (int k = 0; k < CellsMatrix.GetLength(2); k++)
                            if (CellsMatrix[i, j, k] != null)
                                cellsList.Add(CellsMatrix[i, j, k]);
            }

            return cellsList;
        }

        public Vector3 GetOffset()
        {
            return offSet;
        }

        public Cell[,,] GetCellsMatrix()
        {
            return CellsMatrix;
        }
        #endregion
        #endregion

        #region GridData Management
        void LoadFromNetworkData(NodeNetworkData _networkData)
        {
            if (_networkData == null)
            {
                Debug.LogWarning("GridController -- No data to load !");
                return;
            }

            ClearGrid();

            CellsMatrix = _networkData.CellsMatrix;

            Size = _networkData.Size;
            SectorData = GetListOfCells().Where(c => c != null).First().GetCellData().SectorData;
        }

        NodeNetworkData Save(List<Cell> _cells)
        {
            NodeNetworkData asset = null;

            List<CellData> cellsData = new List<CellData>();
            foreach (Cell cell in _cells)
            {
                cellsData.Add(cell.GetCellData());
            }

            asset = ScriptableObject.CreateInstance<NodeNetworkData>();
            asset.CellsMatrix = CellsMatrix;
            asset.Size = Size;

            string assetName = "NodeNetworkData.asset";
            asset.name = assetName;
            string completePath = AssetDatabase.GenerateUniqueAssetPath(CheckFolder() + assetName);

            AssetDatabase.CreateAsset(asset, completePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return asset;
        }

        string CheckFolder()
        {
#if UNITY_EDITOR
            if (!AssetDatabase.IsValidFolder("Assets/GridData"))
                AssetDatabase.CreateFolder("Assets", "GridData");
#endif

            return "Assets/GridData/";
        }
        #endregion

        #region Grid Creation
        void CreateGrid()
        {
            int maxSize = SizeInt.x >= SizeInt.y ? (int)SizeInt.x : (int)SizeInt.y;
            maxSize = maxSize >= SizeInt.z ? maxSize : (int)SizeInt.z;

            //Be very carefull about the Matrix initialization.
            CellsMatrix = new Cell[maxSize, maxSize, maxSize];
            for (int i = 0; i < maxSize; i++)
            {
                for (int j = 0; j < maxSize; j++)
                {
                    for (int k = 0; k < maxSize; k++)
                    {                  
                        CreateCell(i, j, k);
                    }
                }
            }
        }

        void CreateCell(int _i, int _j, int _k)
        {
            int i = _i < SizeInt.x ? _i : 0;
            int j = _j < SizeInt.y ? _j : 0;
            int k = _k < SizeInt.z ? _k : 0;

            Vector3 nodePos = this.GetPositionByCoordinates(i, j, k);

            NodeData nodeD = new NodeData(nodePos);
            LinkData linkD = new LinkData();
            SectorData sectorD = SectorData;

            CellsMatrix[i, j, k] = new Cell(new CellData(nodeD, linkD, sectorD));
        }

        /// <summary>
        /// Crea i collegamenti alle celle
        /// </summary>
        void LinkCells()
        {
            for (int i = 0; i < CellsMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < CellsMatrix.GetLength(1); j++)
                {
                    for (int k = 0; k < CellsMatrix.GetLength(2); k++)
                    {
                        if (CellsMatrix[i, j, k] == null)
                            continue;

                        //Link of the next and previus cell along all directions
                        CellsMatrix[i,j,k].Link(CellsMatrix[i != 0 ? i - 1 : 0, j, k]);
                        if (i < (int)SizeInt.x - 1)
                            CellsMatrix[i,j,k].Link(CellsMatrix[i + 1, j, k]);

                        CellsMatrix[i,j,k].Link(CellsMatrix[i, j != 0 ? j - 1 : 0, k]);
                        if(j < (int)SizeInt.y - 1)
                            CellsMatrix[i,j,k].Link(CellsMatrix[i, j + 1 , k]);

                        CellsMatrix[i,j,k].Link(CellsMatrix[i, j, k != 0 ? k - 1 : 0]);
                        if(k < (int)SizeInt.z - 1)
                            CellsMatrix[i,j,k].Link(CellsMatrix[i, j, k + 1]);
                    }
                }
            }
        }
        #endregion

        Vector3 CalculateOffset()
        {
            Vector3 offset = new Vector3(Size.x * SectorData.Diameter.x, Size.y * SectorData.Diameter.y, Size.z * SectorData.Diameter.z);

            offset /= 2;
            offset -= SectorData.Radius;
            return offset;
        }

        private void OnDrawGizmos()
        {
            List<Cell> cellList = GetListOfCells();
            if (cellList.Count <= 0)
                return;

            foreach (Cell cell in cellList)
            {
                if (ShowGrid)
                {
                    Gizmos.color = Color.cyan;
                    
                    Gizmos.DrawWireCube(cell.GetCenter(), cell.GetRadius() * 2);
                    Gizmos.DrawWireCube(cell.GetCenter(), (cell.GetRadius() / 25f));
                    
                }
                if (ShowLink)
                {
                    Gizmos.color = Color.black;
                    foreach (ILink link in cell.GetNeighbourgs())
                    {
                        Vector3 line = link.GetPosition() - cell.GetCenter();
                        Gizmos.DrawLine(cell.GetCenter() + line * 0.25f, cell.GetCenter() + line * .75f);
                    }
                }
            }
        }
    }
}

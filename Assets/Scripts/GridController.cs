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

        public Vector3 Size;
        protected Vector3 SizeInt;

        public List<Cell> CellsList = new List<Cell>();
        public Cell[][][] CellsMatrix;

        public Vector3 offSet;

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
            SizeInt.x = (int)(Size.x / SectorData.Diameter.x);
            SizeInt.y = (int)(Size.y / SectorData.Diameter.y);
            SizeInt.z = (int)(Size.z / SectorData.Diameter.z);

            offSet = CalculateOffset();
            //New grid creation
            CreateGrid();
            //Linking process
            //if(autoLinkCells)
               // LinkCells();
        }

        public Cell GetCentralCell()
        {
            return this.GetCellFromPosition(transform.position);
        }

        public void ClearGrid()
        {
            CellsList.Clear();
        }

        public void Load()
        {
            if(NetworkData != null)
                LoadFromNetworkData(NetworkData);
        }

        public void SaveCurrent()
        {
            Save(CellsList);
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

        // TODO : per Test (per il momento)
        public List<INode> GetListOfCells()
        {
            return CellsList.ConvertAll(c => c as INode);
        }

        #region GridData Management
        public void LoadFromNetworkData(NodeNetworkData _networkData)
        {
            if (_networkData == null)
            {
                Debug.LogWarning("GridController -- No data to load !");
                return;
            }

            CellsList.Clear();

            Size = _networkData.Size;
            SectorData = _networkData.Cells[0].SectorData;

            foreach (CellData cellData in _networkData.Cells)
            {
                CellsList.Add(new Cell(cellData));
            }
        }
        public NodeNetworkData Save(List<Cell> _cells)
        {
            NodeNetworkData asset = null;

            List<CellData> cellsData = new List<CellData>();
            foreach (Cell cell in _cells)
            {
                cellsData.Add(cell.GetCellData());
            }

            asset = ScriptableObject.CreateInstance<NodeNetworkData>();
            asset.Cells = cellsData;
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
        #endregion

        /// <summary>
        /// Crea i collegamenti alle celle
        /// </summary>
        void LinkCells()
        {
            for (int i = 0; i < SizeInt.x; i++)
            {
                for (int j = 0; j < SizeInt.y; j++)
                {
                    for (int k = 0; k < SizeInt.z; k++)
                    {
                        //Link of the next and previus cell along all directions
                        CellsMatrix[i][j][k].Link(CellsMatrix[i != 0 ? i - 1 : 0][j][k]);
                        CellsMatrix[i][j][k].Link(CellsMatrix[i != (int)SizeInt.x ? i + 1 : (int)SizeInt.x][j][k]);
                        CellsMatrix[i][j][k].Link(CellsMatrix[i][j != 0 ? j - 1 : 0][k]);
                        CellsMatrix[i][j][k].Link(CellsMatrix[i][j != (int)SizeInt.y ? j + 1 : (int)SizeInt.y][k]);
                        CellsMatrix[i][j][k].Link(CellsMatrix[i][j][k != 0 ? k - 1 : 0]);
                        CellsMatrix[i][j][k].Link(CellsMatrix[i][j][k != (int)SizeInt.z ? k + 1 : (int)SizeInt.z]);
                    }
                }
            }
        }

        #region Grid Creation
        void CreateGrid()
        {
            Vector2 size2D = new Vector2();
            if (SizeInt.x > 0 && SizeInt.y > 0 && SizeInt.z > 0)
            {
                CreateGrid3D(SizeInt);
            }
            else if (SizeInt.x == 0 && SizeInt.y > 0 && SizeInt.z > 0)
            {
                size2D = new Vector2(SizeInt.y, SizeInt.z);
                CreateGrid2D(size2D, "x");
            }
            else if (SizeInt.x > 0 && SizeInt.y == 0 && SizeInt.z > 0)
            {
                size2D = new Vector2(SizeInt.x, SizeInt.z);
                CreateGrid2D(size2D, "y");
            }
            else if (SizeInt.x > 0 && SizeInt.y > 0 && SizeInt.z == 0)
            {
                size2D = new Vector2(SizeInt.x, SizeInt.y);
                CreateGrid2D(size2D, "z");
            }
            else if (SizeInt.x > 0 && SizeInt.y == 0 && SizeInt.z == 0)
            {
                CreateGrid1D(SizeInt.x, new string[] { "y", "z" });
            }
            else if (SizeInt.x == 0 && SizeInt.y > 0 && SizeInt.z == 0)
            {
                CreateGrid1D(SizeInt.y, new string[] { "x", "z" });
            }
            else if (SizeInt.x == 0 && SizeInt.y == 0 && SizeInt.z > 0)
            {
                CreateGrid1D(SizeInt.z, new string[] { "x", "y" });
            }
        }

        void CreateGrid3D(Vector3 _sizeInt3D)
        {
            //Be very carefull about the Matrix initialization.
            CellsMatrix = new Cell[(int)_sizeInt3D.x][][];
            for (int i = 0; i < _sizeInt3D.x; i++)
            {
                CellsMatrix[i] = new Cell[(int)_sizeInt3D.y][];
                for (int j = 0; j < _sizeInt3D.y; j++)
                {
                    CellsMatrix[i][j] = new Cell[(int)_sizeInt3D.z];
                    for (int k = 0; k < _sizeInt3D.z; k++)
                    {
                        CreateCell(i, j, k);
                    }
                }
            }
        }

        void CreateGrid2D(Vector2 _sizeInt2D, string _nullAxis)
        {
            //Be very carefull about the Matrix initialization.
            CellsMatrix = new Cell[(int)_sizeInt2D.x][][];
            for (int i = 0; i < _sizeInt2D.x; i++)
            {
                CellsMatrix[i] = new Cell[(int)_sizeInt2D.y][];
                for (int j = 0; j < _sizeInt2D.y; j++)
                {
                    if(_nullAxis == "x")
                        CreateCell(0, i, j);
                    else if (_nullAxis == "y")
                        CreateCell(i, 0, j);
                    else if (_nullAxis == "z")
                        CreateCell(i, j, 0);
                }
            }
        }

        void CreateGrid1D(float _sizeInt1D, string[] _nullAxes)
        {
            //Be very carefull about the Matrix initialization.
            CellsMatrix = new Cell[(int)_sizeInt1D][][];
            for (int i = 0; i < _sizeInt1D; i++)
            {
                if (_nullAxes[0] == "y" && _nullAxes[0] == "z")
                    CreateCell(i, 0, 0);
                else if (_nullAxes[0] == "x" && _nullAxes[0] == "z")
                    CreateCell(0, i, 0);
                else if (_nullAxes[0] == "x" && _nullAxes[0] == "y")
                    CreateCell(0, 0, i);                
            }
        }

        void CreateCell(int _i, int _j, int _k)
        {
            Vector3 nodePos = this.GetPositionByCoordinates(_i, _j, _k);

            NodeData nodeD = new NodeData(nodePos);
            LinkData linkD = new LinkData();
            SectorData sectorD = SectorData;

            CellsMatrix[_i][_j][_k] = new Cell(new CellData(nodeD, linkD, sectorD));
            CellsList.Add(CellsMatrix[_i][_j][_k]);
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
            if (CellsList.Count <= 0)
                return;

            foreach (Cell cell in CellsList)
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

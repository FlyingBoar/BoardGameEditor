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

        static List<Cell> CellsList = new List<Cell>();
        static Cell[][][] CellsMatrix;

        Vector3 offSet;

        #region API
        public void CreateNewGrid(bool autoLinkCells = true)
        {
            //Reset operation
            ClearGrid();
            SizeInt.x = (int)(Size.x / SectorData.Diameter.x);
            SizeInt.x = SizeInt.x == 0 ? 1 : SizeInt.x;
            SizeInt.y = (int)(Size.y / SectorData.Diameter.y);
            SizeInt.y = SizeInt.y == 0 ? 1 : SizeInt.y;
            SizeInt.z = (int)(Size.z / SectorData.Diameter.z);
            SizeInt.z = SizeInt.z == 0 ? 1 : SizeInt.z;

            offSet = CalculateOffset();
            //New grid creation
            CreateGrid();
            //Linking process
            if(autoLinkCells)
                LinkCells();
        }

        /// <summary>
        /// data una posizione restituisce la cella corrispondente
        /// </summary>
        /// <param name="_position">la posizione da controllare</param>
        /// <returns>la cella che si trova in quella posizione</returns>
        public Cell GetCellFromPosition(Vector3 _position)
        {
            Cell resultant;
            int[] indexes = GetCoordinatesByPosition(_position);
            resultant = CellsMatrix[indexes[0]][indexes[1]][indexes[2]];
            return resultant;
        }

        public Vector3 GetPositionByCoordinates(int x, int y, int z) {

            Vector3 spacePos = new Vector3((transform.position.x + x*SectorData.Diameter.x), (transform.position.y + y * SectorData.Diameter.y), (transform.position.z + z * SectorData.Diameter.z));
            spacePos += SectorData.Radius;
            spacePos -= offSet;

            return spacePos;
        }

        //Associate the coordinate of clostest Cell center (or cell array index)
        //to the position vector
        //By Math: this is actually the metric conversion of the Grid Sub Vectorial Space, but
        //the int cast on the normalized position
        public int[] GetCoordinatesByPosition(Vector3 _position)
        {
            Vector3 spacePos = _position + offSet;

            int[] coordinates = new int[]
            {
                //Radius == 0 is indefined. 0 as default
                //Postion/Radius is the normalized space position
                //i = V/(2R) + 1
                SectorData.Radius.x != 0 ?(int)(spacePos.x/SectorData.Radius.x)/2 +1 : 0,
                SectorData.Radius.y != 0 ?(int)(spacePos.y/SectorData.Radius.y)/2 +1 : 0,
                SectorData.Radius.z != 0 ?(int)(spacePos.x/SectorData.Radius.z)/2 +1 : 0,
            };

            return coordinates;
        }

        public Cell GetCentralCell()
        {
            return GetCellFromPosition(transform.position);
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
            tempList.Add(GetCellFromPosition(new Vector3(-offset.x, 0, -offset.z)));
            tempList.Add(GetCellFromPosition(new Vector3(-offset.x, 0, offset.z)));
            tempList.Add(GetCellFromPosition(new Vector3(offset.x, 0, offset.z)));
            tempList.Add(GetCellFromPosition(new Vector3(offset.x, 0, -offset.z)));

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

        void CreateGrid()
        {
            //Be very carefull about the Matrix initialization.
            CellsMatrix = new Cell[(int)SizeInt.x][][];
            for (int i = 0; i < SizeInt.x; i++)
            {
                CellsMatrix[i] = new Cell[(int)SizeInt.y][];
                for (int j = 0; j < SizeInt.y; j++)
                {
                    CellsMatrix[i][j] = new Cell[(int)SizeInt.z];
                    for (int k = 0; k < SizeInt.z; k++)
                    {
                        Vector3 nodePos = GetPositionByCoordinates(i, j, k);

                        NodeData nodeD = new NodeData(nodePos);
                        LinkData linkD = new LinkData();
                        SectorData sectorD = SectorData;

                        CellsMatrix[i][j][k] = new Cell(new CellData(nodeD, linkD, sectorD));
                        CellsList.Add(CellsMatrix[i][j][k]);
                    }
                }
            }
        }

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

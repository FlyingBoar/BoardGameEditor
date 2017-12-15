using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

namespace BGEditor.NodeSystem
{
    [ExecuteInEditMode]
    public class GridController : MonoBehaviour
    {
        public NodeNetworkData NetworkData;
        public SectorData SectorData;

        public bool ShowGrid;
        public bool ShowLink;

        public Vector3 Size;

        static List<Cell> cells = new List<Cell>();

        #region API
        public void CreateNewGrid()
        {
            ClearGrid();
            Vector3 offset = CalculateOffset();

            if (Size.x != 0 && Size.y != 0 && Size.z != 0)
            {
                CreateGrid3D(offset);
            }
            else
            {
                Vector2 size2D = new Vector2();

                if (Size.x == 0 && Size.y != 0 && Size.z != 0)
                    size2D = new Vector2(Size.y, Size.z);
                else if (Size.x != 0 && Size.y == 0 && Size.z != 0)
                    size2D = new Vector2(Size.x, Size.z);
                else if (Size.x != 0 && Size.y != 0 && Size.z == 0)
                    size2D = new Vector2(Size.x, Size.y);
                else
                {
                    Debug.LogWarning("GridController -- The minimum axis number for building a grid is 2 !");
                    return;
                }

                CreateGrid2D(size2D, offset);
            }

            LinkCells();
        }

        void CreateGrid3D(Vector3 _offset)
        {
            for (int i = 0; i < Size.x; i++)
            {
                for (int j = 0; j < Size.y; j++)
                {
                    for (int k = 0; k < Size.z; k++)
                    {
                        Vector3 nodePos = new Vector3((transform.position.x + i * SectorData.Radius * 2), (transform.position.y + j * SectorData.Radius * 2), (transform.position.z + k * SectorData.Radius * 2));
                        nodePos -= _offset;
                        NodeData nodeD = new NodeData(nodePos);
                        LinkData linkD = new LinkData();
                        SectorData sectorD = SectorData;
                        cells.Add(new Cell(new CellData(nodeD, linkD, sectorD)));
                    }
                }
            }
        }

        void CreateGrid2D(Vector2 size2D, Vector3 _offset)
        {
            for (int i = 0; i < size2D.x; i++)
            {
                for (int j = 0; j < size2D.y; j++)
                {
                    Vector3 nodePos = new Vector3();

                    if (Size.x == 0 && Size.y != 0 && Size.z != 0)
                        nodePos = new Vector3(0f, (i * SectorData.Radius * 2), (j * SectorData.Radius * 2));

                    else if (Size.x != 0 && Size.y == 0 && Size.z != 0)
                        nodePos = new Vector3((i * SectorData.Radius * 2), 0f, (j * SectorData.Radius * 2));

                    else if (Size.x != 0 && Size.y != 0 && Size.z == 0)
                        nodePos = new Vector3((i * SectorData.Radius * 2), (j * SectorData.Radius * 2), 0f);

                    nodePos -= _offset;
                    NodeData nodeD = new NodeData(nodePos);
                    LinkData linkD = new LinkData();
                    SectorData sectorD = SectorData;
                    cells.Add(new Cell(new CellData(nodeD, linkD, sectorD)));
                }
            }
        }

        /// <summary>
        /// data una posizione restituisce la cella corrispondente
        /// </summary>
        /// <param name="_position">la posizione da controllare</param>
        /// <returns>la cella che si trova in quella posizione</returns>
        public static Cell ReturnCellFromPosition(Vector3 _position)
        {
            foreach (Cell cell in cells)
            {
                if (Vector3.Distance(cell.GetPosition(), _position) < cell.GetRadius())
                {
                    return cell;
                }
            }
            return null;
        }

        public Cell GetCentralCell()
        {
            return ReturnCellFromPosition(transform.position);
        }

        public void ClearGrid()
        {
            cells.Clear();
        }

        public void Load()
        {
            if(NetworkData != null)
                LoadFromNetworkData(NetworkData);
        }

        public void SaveCurrent()
        {
            Save(cells);
        }

        public List<Cell> GetGridCorners()
        {
            List<Cell> tempList = new List<Cell>();
            Vector3 offset = CalculateOffset();
            tempList.Add(ReturnCellFromPosition(new Vector3(-offset.x,0, -offset.z)));
            tempList.Add(ReturnCellFromPosition(new Vector3(-offset.x,0, offset.z)));
            tempList.Add(ReturnCellFromPosition(new Vector3(offset.x, 0, offset.z)));
            tempList.Add(ReturnCellFromPosition(new Vector3(offset.x, 0, -offset.z)));

            return tempList;
        }

        #region GridData Management
        public void LoadFromNetworkData(NodeNetworkData _networkData)
        {
            if (_networkData == null)
            {
                Debug.LogWarning("GridController -- No data to load !");
                return;
            }

            cells.Clear();

            Size = _networkData.Size;
            SectorData = _networkData.Cells[0].SectorData;

            foreach (CellData cellData in _networkData.Cells)
            {
                cells.Add(new Cell(cellData));
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
            for (int i = 0; i < cells.Count; i++)
            {
                for (int j = 0; j < cells.Count; j++)
                {
                    if (Vector3.Distance(cells[i].GetPosition(), cells[j].GetPosition()) <= SectorData.Radius * 2 && i != j)
                    {
                        cells[i].Link(cells[j]);
                    }
                }
            }
        }

        Vector3 CalculateOffset()
        {
            Vector3 offset= new Vector3((Size.x * SectorData.Radius) * 2, (Size.y * SectorData.Radius) * 2, (Size.z * SectorData.Radius) * 2);
            offset -= Vector3.one * SectorData.Radius * 2;
            offset.y *= 0;
            offset /= 2;
            return offset;
        }

        private void OnDrawGizmos()
        {
            if (cells.Count <= 0)
                return;

            foreach (Cell cell in cells)
            {
                if (ShowGrid)
                {
                    Gizmos.color = Color.cyan;
                    if (Size.x != 0 && Size.y != 0 && Size.z != 0)
                    {
                        Gizmos.DrawWireCube(cell.GetCenter(), new Vector3(1f, 1f, 1f) * cell.GetRadius() * 2);
                        Gizmos.DrawWireCube(cell.GetCenter(), new Vector3(1f, 1f, 1f) * (cell.GetRadius() / 25f));
                    }
                    else
                    {
                        if (Size.x == 0 && Size.y != 0 && Size.z != 0)
                        {
                            Gizmos.DrawWireCube(cell.GetCenter(), new Vector3(0f, 1f, 1f) * cell.GetRadius() * 2);
                            Gizmos.DrawWireCube(cell.GetCenter(), new Vector3(0f, 1f, 1f) * (cell.GetRadius() / 25f));
                        }
                        else if (Size.x != 0 && Size.y == 0 && Size.z != 0)
                        {
                            Gizmos.DrawWireCube(cell.GetCenter(), new Vector3(1f, 0f, 1f) * cell.GetRadius() * 2);
                            Gizmos.DrawWireCube(cell.GetCenter(), new Vector3(1f, 0f, 1f) * (cell.GetRadius() / 25f));
                        }
                        else if (Size.x != 0 && Size.y != 0 && Size.z == 0)
                        {
                            Gizmos.DrawWireCube(cell.GetCenter(), new Vector3(1f, 1f, 0f) * cell.GetRadius() * 2);
                            Gizmos.DrawWireCube(cell.GetCenter(), new Vector3(1f, 1f, 0f) * (cell.GetRadius() / 25f));
                        } 
                    }
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

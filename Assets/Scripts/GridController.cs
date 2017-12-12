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

            for (int i = 0; i < Size.x; i++)
            {
                for (int j = 0; j < Size.y; j++)
                {
                    for (int k = 0; k < Size.z; k++)
                    {
                        Vector3 nodePos = new Vector3((transform.position.x + i * SectorData.Radius * 2), (transform.position.y + j * SectorData.Radius * 2), (transform.position.z + k * SectorData.Radius * 2));
                        nodePos -= offset;
                        NodeData nodeD = new NodeData(nodePos);
                        LinkData linkD = new LinkData();
                        SectorData sectorD = SectorData;
                        cells.Add(new Cell(new CellData(nodeD, linkD, sectorD)));
                    }
                }
            }

            LinkCells();
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

        #region GridData Management
        public void LoadFromNetworkData(NodeNetworkData _networkData)
        {
            if (_networkData == null)
            {
                Debug.LogWarning("No data to load !");
                return;
            }

            cells.Clear();
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
            return new Vector3((Size.x * SectorData.Radius) / 2, (Size.y * SectorData.Radius) / 2, (Size.z * SectorData.Radius) / 2);
        }

        private void OnDrawGizmos()
        {
            if (cells.Count <= 0)
                return;

            foreach (Cell item in cells)
            {
                if (ShowGrid)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawWireCube(item.GetCenter(), new Vector3(1f, 1f, 1f) * item.GetRadius() * 2);
                    Gizmos.DrawWireCube(item.GetCenter(), new Vector3(1f, 1f, 1f) * (item.GetRadius() / 25f));
                }
                if (ShowLink)
                {
                    Gizmos.color = Color.black;
                    foreach (ILink link in item.GetNeighbourgs())
                    {
                        Vector3 line = link.GetPosition() - item.GetCenter();
                        Gizmos.DrawLine(item.GetCenter() + line * 0.25f, item.GetCenter() + line * .75f);
                    }
                }
            }
        }
    }
}

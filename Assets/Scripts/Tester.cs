using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BGEditor.NodeSystem;
using UnityEditor;

[ExecuteInEditMode]
public class Tester : MonoBehaviour
{
    public NodeNetworkData NetworkData;
    public SectorData SectorData;

    public bool ShowGrid;
    public bool ShowLink;

    public Vector2 Size;

    List<Cell> cells = new List<Cell>();

	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateNewGrid();
            //------
            LinkCells();
            //------
        }
        if (Input.GetKeyDown(KeyCode.D))
            ClearGrid();
        if (Input.GetKeyDown(KeyCode.S))
            SaveGrid(cells);
        if (Input.GetKeyDown(KeyCode.L))
            LoadGrid(NetworkData);

    }

    public void CreateNewGrid()
    {
        Vector2 offset = CalculateOffset();
        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                NodeData nodeD = new NodeData(new Vector3((transform.position.x + i * SectorData.Radius) - offset.x, 0f, (transform.position.z + j * SectorData.Radius) - offset.y));
                LinkData linkD = new LinkData();
                SectorData sectorD = SectorData;
                Cell tempCell = new Cell(new CellData(nodeD, linkD, sectorD));
                cells.Add(tempCell);
            }
        }
    }

    //---------------------------------

    public List<Cell> GetCells()
    {
        return cells;
    }

    /// <summary>
    /// Crea i collegamenti alle celle
    /// </summary>
    void LinkCells()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            for (int j = 0; j < cells.Count; j++)
            {
                if (Vector3.Distance(cells[i].GetPosition(), cells[j].GetPosition()) <= SectorData.Radius && i != j)
                {
                    cells[i].Link(cells[j]); 
                }
            }
        }
    }

    /// <summary>
    /// data una posizione restituisce la cella corrispondente
    /// </summary>
    /// <param name="_position">la posizione da controllare</param>
    /// <returns>la cella che si trova in quella posizione</returns>
    public Cell WorldToGridPosition(Vector3 _position)
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

    //---------------------------------
    public void LoadGrid(NodeNetworkData _networkData)
    {
        if(_networkData == null)
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

    public NodeNetworkData SaveGrid(List<Cell> _cells)
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

    public void ClearGrid()
    {
        cells.Clear();
    }

    public string CheckFolder()
    {
        #if UNITY_EDITOR
        if (!AssetDatabase.IsValidFolder("Assets/GridData"))
            AssetDatabase.CreateFolder("Assets", "GridData");
        #endif

        return "Assets/GridData/";
    }

    Vector2 CalculateOffset()
    {
        return new Vector2((Size.x * SectorData.Radius) / 2, (Size.y * SectorData.Radius) / 2);
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
                Gizmos.DrawWireCube(item.GetCenter(), new Vector3(1f, 0f, 1f) * item.GetRadius());
                Gizmos.DrawWireCube(item.GetCenter(), new Vector3(1f, 0f, 1f) * (item.GetRadius() / 50f)); 
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

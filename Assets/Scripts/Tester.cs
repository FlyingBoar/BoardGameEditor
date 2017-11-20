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

    public Vector2 Size;

    List<Cell> cells = new List<Cell>();

	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            CreateNewGrid();
        if (Input.GetKeyDown(KeyCode.A))
            ClearGrid();
        if (Input.GetKeyDown(KeyCode.S))
            SaveGrid(cells);
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
                cells.Add( new Cell( new CellData(nodeD, linkD, sectorD)));
            }
        }
    }

    public void LoadGrid(NodeNetworkData _networkData)
    {
        NetworkData = _networkData;
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

        Gizmos.color = Color.cyan;
        foreach (Cell item in cells)
        {
            Gizmos.DrawWireCube(item.GetCenter(), new Vector3(1f, 0f, 1f) * item.GetRadius());
            Gizmos.DrawWireCube(item.GetCenter(), new Vector3(1f, 0f, 1f) * (item.GetRadius()/50f) );
        }
    }
}

using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

namespace Grid
{
    public class GridController
    {
        public GridData GridData;
        public CellData.SectorData SectorData { get { return GridData.SectorData; } }

        public Vector3 Origin = Vector3.zero;
        public Vector3Int Size;
        public Vector3 ResolutionCorrection;

        public GridVisualizer GridVisualizer;
        public GridLayerController LayerCtrl;

        Cell[,,] CellsMatrix;

        public GridController() { }

        public void Init(GridVisualizer _gridVisualizer, GridLayerController _layerCtrl)
        {
            GridVisualizer = _gridVisualizer;
            LayerCtrl = _layerCtrl;
            GridData = new GridData();
        }

        #region API
        public bool DoesGridExist()
        {
            if (CellsMatrix != null)
                return true;
            else
                return false;
        }

        public void CreateNewGrid(bool autoLinkCells = true)
        {
            //Reset operation
            ClearGrid();
            //New grid creation
            CreateGrid();
            //Linking process
            if (autoLinkCells)
            {
                for (int i = 0; i < LayerCtrl.GetNumberOfLayers(); i++)
                {
                    LayerCtrl.LinkAllCells(LayerCtrl.GetLayerAtIndex(i));
                }
            }
        }

        public void ClearGrid()
        {
            CellsMatrix = null;
        }

        public void Load(string _gridDataPath)
        {
            LoadFromJSON(_gridDataPath);
        }

        public void Save(string _name)
        {
            SaveCurrent(_name);
        }

        #region Getter
        public Cell GetCentralCell()
        {
            return this.GetCellFromPosition(Origin);
        }

        /// <summary>
        /// Return the list of not null cell in Grid
        /// </summary>
        /// <returns></returns>
        public List<Cell> GetListOfCells()
        {
            List<Cell> cellsList = new List<Cell>();

            if (CellsMatrix != null)
            {
                for (int i = 0; i < CellsMatrix.GetLength(0); i++)
                    for (int j = 0; j < CellsMatrix.GetLength(1); j++)
                        for (int k = 0; k < CellsMatrix.GetLength(2); k++)
                            if (CellsMatrix[i, j, k] != null)
                                cellsList.Add(CellsMatrix[i, j, k]);
            }

            return cellsList;
        }

        /// <summary>
        /// Return the Matrix of the grid
        /// </summary>
        /// <returns></returns>
        public Cell[,,] GetCellsMatrix()
        {
            return CellsMatrix;
        }
        #endregion
        #endregion

        #region GridData Management
        void LoadFromJSON(string _jsonGridDataPath)
        {
            string _jsonGridData = File.ReadAllText(_jsonGridDataPath);
            GridData _newGridData = JsonUtility.FromJson<GridData>(_jsonGridData);

            if (_jsonGridData == null)
            {
                Debug.LogWarning("GridController -- No data to load !");
                return;
            }

            GridData = _newGridData;
            Size = GridData.Size;
            Origin = GridData.Origin;
            ResolutionCorrection = GridData.ResolutionCorrection;

            
            LayerCtrl.LoadFromData(GridData);
            CreateNewGrid(false);

            foreach (CellData _data in GridData.GetCellDatas())
            {
                Vector3Int _matrixPosition = this.GetCoordinatesByPosition(_data.Position);
                Cell _newCell = CellsMatrix[_matrixPosition.x, _matrixPosition.y, _matrixPosition.z] = new Cell(_data, this);
                foreach (var item in _data.GetLayeredLinks())
                {
                    LayerCtrl.LinkCells(_newCell, _data.GetLinkCoordinates(item.Layer), item.Layer);
                }
            }
        }

        GridData SaveCurrent(string _name = null)
        {
            GridData newGridData = new GridData();

            newGridData.SectorData = SectorData;
            newGridData.Origin = Origin;
            newGridData.Size = Size;
            newGridData.ResolutionCorrection = ResolutionCorrection;
            newGridData.CellsMatrix = CellsMatrix;
            newGridData.Layers = LayerCtrl.Layers;

            string assetName;
            if (_name == null)
                assetName = "NewGridData.json";
            else
                assetName = _name + ".json";

            //newGridData.name = assetName;
            string completePath = AssetDatabase.GenerateUniqueAssetPath(CheckFolder() + assetName);

            string jasonData = JsonUtility.ToJson(newGridData);

            File.WriteAllText(completePath, jasonData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return newGridData;
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
            int maxSize = Size.x >= Size.y ? Size.x : Size.y;
            maxSize = maxSize >= Size.z ? maxSize : Size.z;

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
            int i = _i < Size.x ? _i : 0;
            int j = _j < Size.y ? _j : 0;
            int k = _k < Size.z ? _k : 0;

            Vector3 nodePos = this.GetPositionByCoordinates(new Vector3Int(i, j, k));

            CellsMatrix[i, j, k] = new Cell(new CellData(SectorData, nodePos, LayerCtrl.GetLayerAtIndex(0)), this);
        }

        
        #endregion
    }
}

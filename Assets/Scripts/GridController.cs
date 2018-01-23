using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

namespace Grid
{
    public class GridController
    {
        public GridData GridData
        {
            get { return DataManager.GridDataInstance; }
            set { DataManager.GridDataInstance = value; }
        }
        public CellData.SectorData SectorData { get { return GridData.SectorData; } }

        public Vector3 Origin {
            get { return GridData.Origin; }
            set { GridData.Origin = value; }
        }
        public Vector3Int Size {
            get { return GridData.Size; }
            set { GridData.Size = value; }
        }
        public Vector3 ResolutionCorrection {
            get { return GridData.ResolutionCorrection; }
            set { GridData.ResolutionCorrection = value; }
        }

        public GridLayerController LayerCtrl;

        Cell[,,] CellsMatrix;

        public GridController() { }

        public GridController(GridLayerController _layerCtrl) {
            Init(_layerCtrl);
        }

        public void Init(GridLayerController _layerCtrl)
        {
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

        public void ReInitVariables()
        {
            LayerCtrl.LoadFromData(GridData);
            CreateNewGrid(false);

            foreach (CellData _cellData in GridData.GetCellDatas())
            {
                Vector3Int _matrixPosition = this.GetCoordinatesByPosition(_cellData.Position);
                Cell _newCell = CellsMatrix[_matrixPosition.x, _matrixPosition.y, _matrixPosition.z] = new Cell(_cellData, this);
                foreach (var item in _cellData.GetLayeredLinks())
                {
                    LayerCtrl.LinkCells(_newCell, _cellData.GetLinkCoordinates(item.Layer), item.Layer);
                }
            }
        }

        public void SaveCellMatrixInData()
        {
            GridData.CellsMatrix = CellsMatrix;
            GridData.Layers = LayerCtrl.Layers;
        }

        #region Getter
        /// <summary>
        /// Return the list of not null cell in Grid
        /// </summary>
        /// <returns></returns>
        public List<Cell> GetCellsList()
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
        //void LoadFromJSON(string _jsonGridDataPath)
        //{
        //    string _jsonGridData = File.ReadAllText(_jsonGridDataPath);
        //    GridData _newGridData = JsonUtility.FromJson<GridData>(_jsonGridData);

        //    if (_jsonGridData == null)
        //    {
        //        Debug.LogWarning("GridController -- No data to load !");
        //        return;
        //    }

        //    GridData = _newGridData;
        //    Size = GridData.Size;
        //    Origin = GridData.Origin;
        //    ResolutionCorrection = GridData.ResolutionCorrection;

            
        //    LayerCtrl.LoadFromData(GridData);
        //    CreateNewGrid(false);

        //    foreach (CellData _data in GridData.GetCellDatas())
        //    {
        //        Vector3Int _matrixPosition = this.GetCoordinatesByPosition(_data.Position);
        //        Cell _newCell = CellsMatrix[_matrixPosition.x, _matrixPosition.y, _matrixPosition.z] = new Cell(_data, this);
        //        foreach (var item in _data.GetLayeredLinks())
        //        {
        //            LayerCtrl.LinkCells(_newCell, _data.GetLinkCoordinates(item.Layer), item.Layer);
        //        }
        //    }
        //}

        //void OverwriteFile(string _jsonGridDataPath)
        //{
        //    string _jsonGridData = File.ReadAllText(_jsonGridDataPath);

        //    GridData newGridData = new GridData();

        //    newGridData.SectorData = SectorData;
        //    newGridData.Origin = Origin;
        //    newGridData.Size = Size;
        //    newGridData.ResolutionCorrection = ResolutionCorrection;
        //    newGridData.CellsMatrix = CellsMatrix;
        //    newGridData.Layers = LayerCtrl.Layers;

        //    string newJsonData = JsonUtility.ToJson(newGridData);

        //    // controllo che siano state fatte delle modifiche rispetto al file caricato, altrimenti è inutile salvare lo stesso contenuto nel file
        //    if(_jsonGridData != newJsonData)
        //    {
        //        File.WriteAllText(_jsonGridDataPath, newJsonData);
        //        AssetDatabase.SaveAssets();
        //        AssetDatabase.Refresh();
        //    }
        //}

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

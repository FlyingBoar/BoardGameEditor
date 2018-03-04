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
        public SectorData SectorData { get { return GridData.SectorData; } }

        public Vector3 Normal
        {
            get { return GridData.Normal; }
            set
            {
                GridData.Normal = value;
                RotationToGridSpace = Quaternion.FromToRotation(Vector3.forward, Normal.normalized);
            }
        }
        Quaternion _rotToGridSpace;
        public Quaternion RotationToGridSpace
        {
            get
            {
                if (_rotToGridSpace == null)
                {
                    if (GridData != null)
                        _rotToGridSpace = Quaternion.identity;
                    else
                        _rotToGridSpace = Quaternion.FromToRotation(Vector3.forward, Normal.normalized);
                }

                return _rotToGridSpace;
            }
            private set { _rotToGridSpace = value; }
        }
        public Vector3 Origin {
            get { return GridData.Origin; }
            set { GridData.Origin = value; }
        }

        public Vector2 ResolutionCorrection {
            get { return GridData.ResolutionCorrection; }
            set { GridData.ResolutionCorrection = value; }
        }

        public GridLayerController LayerCtrl;

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
        //TODO: check con Luca/Fulvio l'uso di questa funzione
        //public void CreateNewGrid(bool autoLinkCells = true)
        //{
        //    //Linking process
        //    if (autoLinkCells)
        //    {
        //        for (int i = 0; i < LayerCtrl.GetNumberOfLayers(); i++)
        //        {
        //            LayerCtrl.LinkAllCells(LayerCtrl.GetLayerAtIndex(i));
        //        }
        //    }
        //}

        public void ReInitVariables()
        {
            LayerCtrl.LoadFromData(GridData);

            //TODO: check con Luca/Fulvio sull'uso di questa parte di codice
            //CreateNewGrid(false);
            //foreach (CellData _cellData in GridData.GetCellDatas())
            //{
            //    Vector3Int _matrixPosition = MasterGrid.GetCoordinatesByPosition(_cellData.Position);
            //    Cell _newCell = CellsMatrix[_matrixPosition.x, _matrixPosition.y, _matrixPosition.z] = new Cell(_cellData, this);
            //    foreach (var item in _cellData.GetLayeredLinks())
            //    {
            //        LayerCtrl.LinkCells(_newCell, _cellData.GetLinkCoordinates(item.Layer), item.Layer);
            //    }
            //}
        }

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
    }
}

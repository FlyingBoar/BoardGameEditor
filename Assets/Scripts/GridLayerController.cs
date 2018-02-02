using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    /// <summary>
    /// Controller che contiene qualsiasi funzione legata ai layer.
    /// </summary>
    public class GridLayerController
    {
        List<Layer> Layers = new List<Layer> { new Layer("Base") };

        List<LinkNetworkType> NetworkTypes = new List<LinkNetworkType>();

        int _selectedLayer = -1;
        public int SelectedLayer
        {
            get { return _selectedLayer; }
            set { _selectedLayer = value; }
        }

        GridController gridCtrl;

        public GridLayerController( GridController _gridCtrl)
        {
            gridCtrl = _gridCtrl;
        }

        #region API
        public void Save()
        {
            string layerSaved = string.Empty;
            foreach (Layer layer in Layers)
            {
                layerSaved += layer.SaveToJson(gridCtrl);
            }
            Debug.Log(layerSaved);
        }

        public void LoadFromData(GridData _gridData)
        {
            Layers = _gridData.Layers;
        }

        #region LinkNetwork
        public void AddLinkNetwork(string _id, Color _color)
        {
            NetworkTypes.Add(new LinkNetworkType(_id, _color));
        }

        public void RemoveLinkNetwork(LinkNetworkType _layerToRemove)
        {
            NetworkTypes.Remove(_layerToRemove);
        }

        public int GetNumberOfLinkNetworks()
        {
            return NetworkTypes.Count;
        }

        public LinkNetworkType GetLinkNetworkAtIndex(int _index)
        {
            if (_index >= NetworkTypes.Count || _index < 0)
                return null;
            else
                return NetworkTypes[_index];
        }
        #endregion

        #region LayerItem
        public void NewItemInScene(LayerItem _item)
        {
            _item.SetCoordinates(MasterGrid.gridCtrl.GetCoordinatesByPosition(_item.transform.position));
            Layers[0].Data.ItemsInLayer.Add(_item.GetData());
        }

        public void RemoveItemFromScene(LayerItem _item)
        {
            Layers[0].Data.ItemsInLayer.Remove(_item.GetData());
        }
        #endregion

        #region Layer
        public List<Layer> GetLayers()
        {
            return Layers;
        }

        public int GetNumberOfLayers()
        {
            return Layers.Count;
        }

        public Layer GetLayerAtIndex(int _index)
        {
            if (_index >= Layers.Count || _index < 0)
                return null;
            else
                return Layers[_index];
        }

        public void AddLayer(Layer _layer)
        {
            if(!Layers.Contains(_layer))
                Layers.Add(_layer);

            if(gridCtrl.DoesGridExist())
                LinkAllCells(_layer);
        }

        public void AddLayer(string _name, Color _gizmoColor)
        {
            Layer newLayer = new Layer(_name, _gizmoColor);
            if (!Layers.Contains(newLayer))
                Layers.Add(newLayer);

            if (gridCtrl.DoesGridExist())
                LinkAllCells(newLayer);
        }

        public void RemoveLayer(Layer _layer)
        {
            Layers.Remove(_layer);

            if (gridCtrl.DoesGridExist())
                RemoveAllLinks(_layer);
        }
        #endregion
        #endregion

        #region OLD LINKS
        public void LinkCells(Cell _cellOrigin, List<Vector3Int> _cellsCoordinates, Layer _layer, bool _mutualLink = false)
        {
            foreach (Vector3Int _coordinate in _cellsCoordinates)
            {
                _cellOrigin.Link(_coordinate, _layer);
                if (_mutualLink)
                    gridCtrl.GetCellByCoordinates(_coordinate).Link(_cellOrigin.GridCoordinates, _layer);
            }
        }

        /// <summary>
        ///Chiama la funzione Link alla cella selezionata passando la cella su cui si trova il cursore
        /// </summary>
        public void LinkCells(Cell startingCell, Cell endingCell, Layer _layer, bool mutualLink = false)
        {
            //startingCell.Link(this.GetCellFromPosition(InputAdapter_Tester.PointerPosition), LayerCtrl.GetLayerAtIndex(0));
            startingCell.Link(endingCell.GridCoordinates, _layer);

            if (mutualLink)
                endingCell.Link(startingCell.GridCoordinates, _layer);
        }

        public void UnlinkCells(Cell startingCell, Cell endingCell)
        {
            startingCell.UnLink(endingCell.GridCoordinates, GetLayerAtIndex(0));
            endingCell.UnLink(startingCell.GridCoordinates, GetLayerAtIndex(0));
        }

        /// <summary>
        /// Crea i collegamenti alle celle
        /// </summary>
        internal void LinkAllCells(Layer _layer)
        {
            Cell[,,] cellsMatrix = gridCtrl.GetCellsMatrix();

            for (int i = 0; i < cellsMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < cellsMatrix.GetLength(1); j++)
                {
                    for (int k = 0; k < cellsMatrix.GetLength(2); k++)
                    {
                        if (cellsMatrix[i, j, k] == null)
                            continue;

                        //Link of the next and previus cell along all directions
                        cellsMatrix[i, j, k].Link(cellsMatrix[i != 0 ? i - 1 : 0, j, k].GridCoordinates, _layer);
                        if (i < gridCtrl.Size.x - 1)
                            cellsMatrix[i, j, k].Link(cellsMatrix[i + 1, j, k].GridCoordinates, _layer);

                        cellsMatrix[i, j, k].Link(cellsMatrix[i, j != 0 ? j - 1 : 0, k].GridCoordinates, _layer);
                        if (j < gridCtrl.Size.y - 1)
                            cellsMatrix[i, j, k].Link(cellsMatrix[i, j + 1, k].GridCoordinates, _layer);

                        cellsMatrix[i, j, k].Link(cellsMatrix[i, j, k != 0 ? k - 1 : 0].GridCoordinates, _layer);
                        if (k < gridCtrl.Size.z - 1)
                            cellsMatrix[i, j, k].Link(cellsMatrix[i, j, k + 1].GridCoordinates, _layer);
                    }
                }
            }
        }

        internal void RemoveAllLinks(Layer _layer)
        {
            Cell[,,] cellsMatrix = gridCtrl.GetCellsMatrix();

            for (int i = 0; i < cellsMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < cellsMatrix.GetLength(1); j++)
                {
                    for (int k = 0; k < cellsMatrix.GetLength(2); k++)
                    {
                        if (cellsMatrix[i, j, k] == null)
                            continue;
                        cellsMatrix[i, j, k].UnLinkAll(_layer);
                        cellsMatrix[i, j, k].GetCellData().RemoveLayeredLink(_layer);
                    }
                }
            }
        }
        #endregion
    }
}


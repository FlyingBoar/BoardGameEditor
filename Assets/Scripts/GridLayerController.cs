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

        int _selectedLayer = 0;
        public int SelectedLayer
        {
            get { return _selectedLayer; }
            set { _selectedLayer = value; }
        }

        GridController gridCtrl;

        public GridLayerController(GridController _gridCtrl)
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

        public LinkNetworkType GetLinkNetworkByIndex(int _index)
        {
            if (_index >= NetworkTypes.Count || _index < 0)
                return null;
            else
                return NetworkTypes[_index];
        }

        public LinkNetworkType GetLinkNetworkByID(string _id)
        {
            for (int i = 0; i < NetworkTypes.Count; i++)
                if (NetworkTypes[i].ID == _id)
                    return NetworkTypes[i];

            return null;
        }
        #endregion

        #region LayerItem
        public void NewItemInScene(LayerItem _item)
        {
            _item.SetCoordinates(MasterGrid.GetCoordinatesByPosition(_item.transform.position));
            _item.MembershipLayer = Layers[SelectedLayer];
            Layers[SelectedLayer].Data.ItemsInLayer.Add(_item.GetData());
            Layers[SelectedLayer].LayerItemInstances.Add(_item);
        }

        public void RemoveItemFromScene(LayerItem _item)
        {
            Layers[SelectedLayer].Data.ItemsInLayer.Remove(_item.GetData());
            Layers[SelectedLayer].LayerItemInstances.Remove(_item);
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

        public Layer GetSelectedLayer()
        {
            return Layers[SelectedLayer];
        }

        public void AddLayer(Layer _layer)
        {
            if(!Layers.Contains(_layer))
                Layers.Add(_layer);
        }

        public void AddLayer(string _name, Color _gizmoColor)
        {
            Layer newLayer = new Layer(_name, _gizmoColor);
            if (!Layers.Contains(newLayer))
                Layers.Add(newLayer);
        }

        public void RemoveLayer(Layer _layer)
        {
            Layers.Remove(_layer);
        }
        #endregion
        #endregion
    }
}


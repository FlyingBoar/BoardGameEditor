using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class LayerController
    {
        List<Layer> Layers = new List<Layer> { new Layer("Base", false) };

        GridController gridCtrl;

        public LayerController(GridController _gridCtrl)
        {
            gridCtrl = _gridCtrl;
        }

        #region API
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
                gridCtrl.LinkCells(_layer);
        }

        public void AddLayer(string _name, bool _isEditable, Color _gizmoColor)
        {
            Layer newLayer = new Layer(_name, _isEditable, _gizmoColor);
            if (!Layers.Contains(newLayer))
                Layers.Add(newLayer);

            if (gridCtrl.DoesGridExist())
                gridCtrl.LinkCells(newLayer);
        }

        public void RemoveLayer(Layer _layer)
        {
            Layers.Remove(_layer);

            if (gridCtrl.DoesGridExist())
                gridCtrl.RemoveLinks(_layer);
        }
        #endregion
    }
}


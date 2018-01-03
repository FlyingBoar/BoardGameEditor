using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(GridController))]
    public class LayerController : MonoBehaviour
    {
        List<Layer> Layers = new List<Layer> { new Layer("Base", true) };

        GridController _gridCtrl;
        GridController gridCtrl
        {
            get
            {
                if (!_gridCtrl)
                    _gridCtrl = GetComponent<GridController>();

                return _gridCtrl;
            }
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
        }

        public void AddLayer(string _name, bool _isEditable, Color _gizmoColor)
        {
            Layer newLayer = new Layer(_name, _isEditable, _gizmoColor);
            if (!Layers.Contains(newLayer))
                Layers.Add(newLayer);

            gridCtrl.LinkCells(newLayer);
        }

        public void RemoveLayer(Layer _layer)
        {
            Layers.Remove(_layer);
            gridCtrl.RemoveLinks(_layer);
        }
        #endregion
    }
}


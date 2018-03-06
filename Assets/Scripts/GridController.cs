using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

namespace Grid
{
    public class GridController
    {
        public GridData GridData
        {
            get { return DataManager.GridDataInstance; }
            set { DataManager.GridDataInstance = value; }
        }

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
    }
}
